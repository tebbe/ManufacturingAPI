using PPS.API.Shared.Enums;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.Product;
using PPS.API.Shared.ViewModel.Report;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PPS.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public ReportRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        //Ledger Report
        public LedgerModel GetLedger(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            var fiscalYearDetail = _ppsDbContext.FiscalYear.FirstOrDefault(x => x.Year == fiscalYear);

            var trans = _ppsDbContext.TransactionEntry
                .Where(x => x.FiscalYear == fiscalYear
                            && x.CompanyId == companyId
                            && DbFunctions.TruncateTime(x.TransactionDate) >= DbFunctions.TruncateTime(fiscalYearDetail.OpenDate)
                            && DbFunctions.TruncateTime(x.TransactionDate) <= DbFunctions.TruncateTime(fiscalYearDetail.CloseDate))
                            //&& x.VerifiedById != null)
                            .SelectMany(x => x.TransactionDetail.Select(y => new
                            {
                                AccountHeadId = y.AccountHeadId,
                                AccountHeadName = y.AccountHead.AccountHeadName,
                                DrAmount = y.DrAmount,
                                CrAmount = y.CrAmount,
                                TransactionDate = x.TransactionDate
                            })).ToList();

            var openingBalance = _ppsDbContext.AccountHeadOpening
                            .Where(x => x.FiscalYear == fiscalYear
                              && x.CompanyId == companyId)
                              .Select(t => new TransactionDetailVm
                              {
                                  AccountHeadName = t.AccountHead.AccountHeadName,
                                  AccountHeadId = t.AccountHeadId,
                                  DrAmount = t.DrAmount,
                                  CrAmount = t.CrAmount
                              })
                              .ToList();

            var openingBalanceBeforeDate = trans.Where(x => x.TransactionDate.Date >= fiscalYearDetail?.OpenDate.Date && x.TransactionDate.Date < startDate.Date)
                .GroupBy(g => g.AccountHeadId).Select(y => new TransactionDetailVm
                {
                    AccountHeadId = y.Key,
                    AccountHeadName = y.FirstOrDefault().AccountHeadName,
                    DrAmount = y.Sum(k => k.DrAmount),
                    CrAmount = y.Sum(k => k.CrAmount)
                }).ToList();


            var openingBalJoin = openingBalance.Union(openingBalanceBeforeDate).ToList();

            var newOpeningBal = openingBalJoin.GroupBy(x => new { x.AccountHeadId, x.AccountHeadName })
                .Select(t => new TransactionDetailVm
                {
                    AccountHeadName = t.Key.AccountHeadName,
                    AccountHeadId = t.Key.AccountHeadId,
                    DrAmount = t.Sum(x => x.DrAmount),
                    CrAmount = t.Sum(x => x.CrAmount)
                })
                .ToList();

            var currentTrans = trans.Where(x => x.TransactionDate.Date >= startDate.Date && x.TransactionDate.Date <= endDate.Date)
                .GroupBy(g => g.AccountHeadId).Select(y => new TransactionDetailVm
                {
                    AccountHeadId = y.Key,
                    AccountHeadName = y.FirstOrDefault().AccountHeadName,
                    DrAmount = y.Sum(k => k.DrAmount),
                    CrAmount = y.Sum(k => k.CrAmount)
                }).ToList();

            var newJoinWithCurr = newOpeningBal.Union(currentTrans).ToList();

            var newOpeningBalWithCurrent = newJoinWithCurr.GroupBy(x => new { x.AccountHeadId, x.AccountHeadName })
                .Select(t => new TransactionDetailVm
                {
                    AccountHeadName = t.Key.AccountHeadName,
                    AccountHeadId = t.Key.AccountHeadId,
                    DrAmount = t.Sum(x => x.DrAmount),
                    CrAmount = t.Sum(x => x.CrAmount)
                })
                .ToList();

            var finalLedger = (from op in newOpeningBal
                               join opwithCurrent in newOpeningBalWithCurrent
                               on op.AccountHeadId equals opwithCurrent.AccountHeadId
                               select new LedgerDetailModel
                               {
                                   AccountHeadId = op.AccountHeadId,
                                   AccountHead = op.AccountHeadName,
                                   OpenDrAmount = op.DrAmount > op.CrAmount ? op.DrAmount - op.CrAmount : 0,
                                   OpenCrAmount = op.DrAmount <= op.CrAmount ? op.CrAmount - op.DrAmount : 0,
                                   DrAmount = opwithCurrent.DrAmount > opwithCurrent.CrAmount
                                       ? opwithCurrent.DrAmount - opwithCurrent.CrAmount : 0,
                                   CrAmount = opwithCurrent.DrAmount <= opwithCurrent.CrAmount
                                       ? opwithCurrent.CrAmount - opwithCurrent.DrAmount : 0
                               }).ToList();

            var ledgerModel = new LedgerModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Detail = new List<LedgerDetailModel>()
            };
            ledgerModel.Detail = finalLedger;

            return ledgerModel;
        }
        //Day Book Report
        public JournalModel GetJournal(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            var journalModel = new JournalModel
            {
                StartDate = startDate,
                EndDate = endDate,
                JournalEntry = new List<JournalEntryModel>()
            };

            var journalEntry = (from entry in _ppsDbContext.TransactionEntry
                                join detail in _ppsDbContext.TransactionDetail
                                on entry.Id equals detail.TransactionEntryId
                                where entry.FiscalYear == fiscalYear
                                && entry.CompanyId == companyId
                                && (DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate))
                                //&& entry.VerifiedById != null
                                orderby entry.TransactionDate, entry.TransactionNumber
                                select new JournalEntryModel
                                {
                                    TransactionNo = entry.TransactionNumber,
                                    TransactionDate = entry.TransactionDate,
                                    TransactionType = entry.TransactionType.Type,
                                    AccountHead = detail.AccountHead.AccountHeadName,
                                    Debit = detail.DrAmount,
                                    Credit = detail.CrAmount
                                }).ToList();

            journalModel.DebitTotal = journalEntry.Sum(x => x.Debit);
            journalModel.CreditTotal = journalEntry.Sum(x => x.Credit);

            journalModel.JournalEntry = journalEntry;

            return journalModel;
        }
        //Individual Ledger Details Report
        public IndividualLedgerModel GetIndividualLedger(int fiscalYear, int companyId, int headId, DateTime startDate, DateTime endDate)
        {
            var individualLedgerModel = new IndividualLedgerModel
            {
                StartDate = startDate,
                EndDate = endDate,
                AccountHeadId = headId,
                OpeningBalanceDr = 0,
                OpeningBalanceCr = 0,
                CurrTransTotalDr = 0,
                CurrTransTotalCr = 0,
                CurrentBalanceDr = 0,
                CurrentBalanceCr = 0,
                ClosingBalanceDr = 0,
                ClosingBalanceCr = 0,
                Detail = new List<IndividualLedgerDetailModel>()
            };

            var firstDayofCurrentFiscalYear = new DateTime(fiscalYear, 1, 1);

            var openingBalance = (from head in _ppsDbContext.AccountHeadOpening
                                  where head.FiscalYear == fiscalYear
                                  && head.CompanyId == companyId
                                  && head.AccountHeadId == headId
                                  select new IndividualLedgerDetailModel
                                  {
                                      DrAmount = head.DrAmount,
                                      CrAmount = head.CrAmount
                                  }).ToList();

            var openingBalanceBeforeDate = (from entry in _ppsDbContext.TransactionEntry
                                            join detail in _ppsDbContext.TransactionDetail
                                            on entry.Id equals detail.TransactionEntryId
                                            where detail.AccountHeadId == headId
                                                  && entry.FiscalYear == fiscalYear
                                                  && entry.CompanyId == companyId
                                                  && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
                                                  && DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                            //&& entry.VerifiedById != null
                                            group detail by detail.AccountHeadId into gDetailHead
                                            select new IndividualLedgerDetailModel
                                            {
                                                DrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                CrAmount = gDetailHead.Sum(x => x.CrAmount)
                                            }).ToList();
            var transactionEntryList = _ppsDbContext.TransactionDetail.Where(m => m.AccountHeadId == headId&&(m.TransactionEntry.TransactionDate>=startDate&&m.TransactionEntry.TransactionDate<=endDate)).Select(m => m.TransactionEntryId).ToList();

            var currentTrans = (from entry in _ppsDbContext.TransactionEntry.Where(m=>transactionEntryList.Contains(m.Id))
                                join detail in _ppsDbContext.TransactionDetail
                                on entry.Id equals detail.TransactionEntryId
                                where entry.Id == detail.TransactionEntryId
                                      && entry.FiscalYear == fiscalYear
                                      && entry.CompanyId == companyId
                                      && (DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                      && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate))
                                //&& entry.VerifiedById != null
                                orderby entry.TransactionDate, entry.TransactionNumber
                                select new IndividualLedgerDetailModel
                                {
                                    TransactionNo = entry.TransactionNumber,
                                    TransactionDate = entry.TransactionDate,
                                    TransactionType = entry.TransactionType.Type,
                                    Particular = entry.Particulars,
                                    AccountHeadName = detail.AccountHead.AccountHeadName,
                                    DrAmount = detail.DrAmount,
                                    CrAmount = detail.CrAmount
                                }).ToList();


            //var currentTrans = (from entry in _ppsDbContext.TransactionEntry
            //                    join detail in _ppsDbContext.TransactionDetail
            //                    on entry.Id equals detail.TransactionEntryId
            //                    where detail.TransactionEntryId == entry.Id
            //                          && entry.FiscalYear == fiscalYear
            //                          && entry.CompanyId == companyId
            //                          && (DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
            //                          && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate))
            //                    //&& entry.VerifiedById != null
            //                    orderby entry.TransactionDate, entry.TransactionNumber
            //                    select new IndividualLedgerDetailModel
            //                    {
            //                        TransactionNo = entry.TransactionNumber,
            //                        TransactionDate = entry.TransactionDate,
            //                        TransactionType = entry.TransactionType.Type,
            //                        Particular=entry.Particulars,
            //                        AccountHeadName=detail.AccountHead.AccountHeadName,
            //                        DrAmount = detail.DrAmount,
            //                        CrAmount = detail.CrAmount
            //                    }).ToList();

            var currTransBalance = (from entry in _ppsDbContext.TransactionEntry
                                    join detail in _ppsDbContext.TransactionDetail
                                    on entry.Id equals detail.TransactionEntryId
                                    where detail.AccountHeadId == headId
                                          && entry.FiscalYear == fiscalYear
                                          && entry.CompanyId == companyId
                                          && (DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                              && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate))
                                    //&& entry.VerifiedById != null
                                    group detail by detail.AccountHeadId into gDetailHead
                                    select new IndividualLedgerDetailModel
                                    {
                                        DrAmount = gDetailHead.Sum(x => x.DrAmount),
                                        CrAmount = gDetailHead.Sum(x => x.CrAmount)
                                    }).ToList();

            //Opening Balance

            double openingBalanceDr = 0, openingBalanceCr = 0, openingBalanceDrFinal = 0, openingBalanceCrFinal = 0;
            double openingBalanceDrBeforeDate = 0, openingBalanceCrBeforeDate = 0;

            double finalOpeningBalance = 0;

            if (openingBalance.Count > 0)
            {
                openingBalanceDr += openingBalance.FirstOrDefault().DrAmount;
                openingBalanceCr += openingBalance.FirstOrDefault().CrAmount;
            }

            if (openingBalanceBeforeDate.Count > 0)
            {
                openingBalanceDrBeforeDate += openingBalanceBeforeDate.FirstOrDefault().DrAmount;
                openingBalanceCrBeforeDate += openingBalanceBeforeDate.FirstOrDefault().CrAmount;
            }

            openingBalanceDrFinal = openingBalanceDr + openingBalanceDrBeforeDate;
            openingBalanceCrFinal = openingBalanceCr + openingBalanceCrBeforeDate;

            finalOpeningBalance = openingBalanceDrFinal - openingBalanceCrFinal;

            individualLedgerModel.OpeningBalanceDr = finalOpeningBalance > 0 ? finalOpeningBalance : 0;
            individualLedgerModel.OpeningBalanceCr = finalOpeningBalance > 0 ? 0 : -finalOpeningBalance;

            var finalLedger = new List<IndividualLedgerDetailModel>();
            individualLedgerModel.Detail = finalLedger;

            if (currentTrans.Count > 0)
            {
                foreach (var currentTran in currentTrans)
                {
                    finalLedger.Add(new IndividualLedgerDetailModel
                    {
                        TransactionDate = currentTran.TransactionDate,
                        TransactionNo = currentTran.TransactionNo,
                        TransactionType = currentTran.TransactionType,
                        AccountHeadName=currentTran.AccountHeadName,
                        Particular=currentTran.Particular,
                        DrAmount = currentTran.DrAmount,
                        CrAmount = currentTran.CrAmount
                    });
                }
            }

            double transTotalDr = 0, transTotalCr = 0, currentBalanceDr = 0, currentBalanceCr = 0;
            double finalCurrentBalance = 0, finalClosingBalanceDr = 0, finalClosingBalanceCr = 0, finalClosingBalance = 0;

            //Current Transection Total and Balance

            if (currTransBalance.Count > 0)
            {
                transTotalDr += currTransBalance.FirstOrDefault().DrAmount;
                transTotalCr += currTransBalance.FirstOrDefault().CrAmount;
            }

            currentBalanceDr = transTotalDr;
            currentBalanceCr = transTotalCr;

            individualLedgerModel.CurrTransTotalDr = transTotalDr;
            individualLedgerModel.CurrTransTotalCr = transTotalCr;


            finalCurrentBalance = currentBalanceDr - currentBalanceCr;

            individualLedgerModel.CurrentBalanceDr = finalCurrentBalance > 0 ? finalCurrentBalance : 0;
            individualLedgerModel.CurrentBalanceCr = finalCurrentBalance > 0 ? 0 : -finalCurrentBalance;

            //Final Closing Balance
            finalClosingBalanceDr = currentBalanceDr + openingBalanceDrFinal;
            finalClosingBalanceCr = currentBalanceCr + openingBalanceCrFinal;

            finalClosingBalance = finalClosingBalanceDr - finalClosingBalanceCr;

            individualLedgerModel.ClosingBalanceDr = finalClosingBalance > 0 ? finalClosingBalance : 0;
            individualLedgerModel.ClosingBalanceCr = finalClosingBalance > 0 ? 0 : -finalClosingBalance;

            return individualLedgerModel;
        }
        //Trail Balance Report
        public TrailBalanceModel GetTrailBalance(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            var ledgerModel = new TrailBalanceModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Detail = new List<TrailBalanceDetailModel>()
            };

            var firstDayofCurrentFiscalYear = new DateTime(fiscalYear, 1, 1);

            //Opening Balance Details
            var openingBalance = (from opening in _ppsDbContext.AccountHeadOpening
                                  where opening.FiscalYear == fiscalYear
                                        && opening.CompanyId == companyId
                                  orderby opening.AccountHeadId
                                  select new TrailBalanceDetailModel
                                  {
                                      AccountHeadId = opening.AccountHeadId,
                                      AccountHead = opening.AccountHead.AccountHeadName,
                                      OpenDrAmount = opening.DrAmount,
                                      OpenCrAmount = opening.CrAmount
                                  }).ToList();

            //Current Transection Before Details
            var currTransBeforeDetails = (from entry in _ppsDbContext.TransactionEntry
                                          join detail in _ppsDbContext.TransactionDetail
                                          on entry.Id equals detail.TransactionEntryId
                                          where entry.FiscalYear == fiscalYear
                                                && entry.CompanyId == companyId
                                                && DbFunctions.TruncateTime(detail.TransactionEntry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
                                                && DbFunctions.TruncateTime(detail.TransactionEntry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                          //&& entry.VerifiedById != null
                                          group detail by detail.AccountHeadId into gDetailHead
                                          orderby gDetailHead.FirstOrDefault().AccountHeadId
                                          select new TrailBalanceDetailModel()
                                          {
                                              AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                              AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                              OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                              OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                          }).ToList();

            var currTransBeforeSummary = new List<TrailBalanceDetailModel>();

            // Current Transection Before Summary
            if (currTransBeforeDetails.Count > 0)
            {
                foreach (var currentTranBefore in currTransBeforeDetails)
                {
                    double finalAmount = currentTranBefore.OpenDrAmount - currentTranBefore.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : 0;
                    var currCrAmount = finalAmount > -1 ? 0 : -finalAmount;
                    currTransBeforeSummary.Add(new TrailBalanceDetailModel
                    {
                        AccountHeadId = currentTranBefore.AccountHeadId,
                        OpenDrAmount = currDrAmount,
                        OpenCrAmount = currCrAmount
                    });
                }
            }

            //Current Transection Detials
            var currTransDetails = (from entry in _ppsDbContext.TransactionEntry
                                    join detail in _ppsDbContext.TransactionDetail
                                    on entry.Id equals detail.TransactionEntryId
                                    where entry.FiscalYear == fiscalYear
                                          && entry.CompanyId == companyId
                                          && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                          && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                    //&& entry.VerifiedById != null
                                    group detail by detail.AccountHeadId into gDetailHead
                                    orderby gDetailHead.FirstOrDefault().AccountHeadId
                                    select new TrailBalanceDetailModel()
                                    {
                                        AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                        AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                        OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                        OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                    }).ToList();

            var currTransSummary = new List<TrailBalanceDetailModel>();

            // Current Transection Summary
            if (currTransDetails.Count > 0)
            {
                foreach (var currentTran in currTransDetails)
                {
                    double finalAmount = currentTran.OpenDrAmount - currentTran.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : 0;
                    var currCrAmount = finalAmount > -1 ? 0 : -finalAmount;
                    currTransSummary.Add(new TrailBalanceDetailModel
                    {
                        AccountHeadId = currentTran.AccountHeadId,
                        OpenDrAmount = currDrAmount,
                        OpenCrAmount = currCrAmount
                    });
                }
            }

            var openingAndCurrBefore = openingBalance.Union(currTransBeforeSummary);

            var openingAndCurrBeforeSummary = (from openAndcurrBefore in openingAndCurrBefore
                                               group openAndcurrBefore by openAndcurrBefore.AccountHeadId into gDetailHead
                                               orderby gDetailHead.FirstOrDefault().AccountHeadId
                                               select new TrailBalanceDetailModel()
                                               {
                                                   AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                   OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount),
                                                   OpenCrAmount = gDetailHead.Sum(x => x.OpenCrAmount)
                                               }).ToList();
            var openingFinal = openingAndCurrBeforeSummary;

            var openAndCurrDetails = openingFinal.Union(currTransSummary);
            var closingDetails = (from openAndCurr in openAndCurrDetails
                                  group openAndCurr by openAndCurr.AccountHeadId into gDetailHead
                                  orderby gDetailHead.FirstOrDefault().AccountHeadId
                                  select new TrailBalanceDetailModel()
                                  {
                                      AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                      OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount),
                                      OpenCrAmount = gDetailHead.Sum(x => x.OpenCrAmount)
                                  }).ToList();

            var closingBalanceSummary = new List<TrailBalanceDetailModel>();

            // Opening Balance Summary
            if (closingDetails.Count > 0)
            {
                foreach (var closingBal in closingDetails)
                {
                    double finalAmount = closingBal.OpenDrAmount - closingBal.OpenCrAmount;
                    var openDrAmount = finalAmount > -1 ? finalAmount : 0;
                    var openCrAmount = finalAmount > -1 ? 0 : -finalAmount;
                    closingBalanceSummary.Add(new TrailBalanceDetailModel
                    {
                        AccountHeadId = closingBal.AccountHeadId,
                        OpenDrAmount = openDrAmount,
                        OpenCrAmount = openCrAmount
                    });
                }
            }

            var accountHeadFromClosing = (from closing in closingBalanceSummary
                                          orderby closing.AccountHeadId
                                          select new TrailBalanceDetailModel()
                                          {
                                              AccountHeadId = closing.AccountHeadId,
                                          }).ToList();

            var openingWithHead = (from openingHead in accountHeadFromClosing
                                   join opening in openingFinal
                                   on openingHead.AccountHeadId equals opening.AccountHeadId
                                   orderby openingHead.AccountHeadId
                                   select new TrailBalanceDetailModel()
                                   {
                                       AccountHeadId = openingHead.AccountHeadId,
                                       OpenDrAmount = opening.OpenDrAmount,
                                       OpenCrAmount = opening.OpenCrAmount
                                   }).ToList();

            var currentWithHead = (from openingHead in accountHeadFromClosing
                                   join current in currTransSummary
                                   on openingHead.AccountHeadId equals current.AccountHeadId
                                   orderby openingHead.AccountHeadId
                                   select new TrailBalanceDetailModel()
                                   {
                                       AccountHeadId = openingHead.AccountHeadId,
                                       OpenDrAmount = current.OpenDrAmount,
                                       OpenCrAmount = current.OpenCrAmount,
                                   }).ToList();

            if (openingWithHead.Count > currentWithHead.Count)
            {
                var headMatching = openingWithHead.Union(currentWithHead);
                var finalHead = (from hm in headMatching
                                 orderby hm.AccountHeadId
                                 group hm by hm.AccountHeadId into hdGroup
                                 select new TrailBalanceDetailModel()
                                 {
                                     AccountHeadId = hdGroup.FirstOrDefault().AccountHeadId,
                                     OpenDrAmount = 0,
                                     OpenCrAmount = 0
                                 }).ToList();

                var temp = currentWithHead.Union(finalHead);
                currentWithHead = (from t in temp
                                   group t by t.AccountHeadId
                                  into headGroup
                                   orderby headGroup.FirstOrDefault().AccountHeadId
                                   select new TrailBalanceDetailModel()
                                   {
                                       AccountHeadId = headGroup.FirstOrDefault().AccountHeadId,
                                       OpenDrAmount = headGroup.Sum(x => x.OpenDrAmount),
                                       OpenCrAmount = headGroup.Sum(x => x.OpenCrAmount)
                                   }).ToList();

            }

            if (openingWithHead.Count < currentWithHead.Count)
            {
                var headMatching = currentWithHead.Union(openingWithHead);
                var finalHead = (from hm in headMatching
                                 orderby hm.AccountHeadId
                                 group hm by hm.AccountHeadId into hdGroup
                                 select new TrailBalanceDetailModel()
                                 {
                                     AccountHeadId = hdGroup.FirstOrDefault().AccountHeadId,
                                     OpenDrAmount = 0,
                                     OpenCrAmount = 0
                                 }).Distinct().ToList();
                var temp = openingWithHead.Union(finalHead);
                openingWithHead = (from t in temp
                                   group t by t.AccountHeadId
                                  into headGroup
                                   orderby headGroup.FirstOrDefault().AccountHeadId
                                   select new TrailBalanceDetailModel()
                                   {
                                       AccountHeadId = headGroup.FirstOrDefault().AccountHeadId,
                                       OpenDrAmount = headGroup.Sum(x => x.OpenDrAmount),
                                       OpenCrAmount = headGroup.Sum(x => x.OpenCrAmount)
                                   }).ToList();
            }


            //HeadName
            var accountHeadName = (from accHead in accountHeadFromClosing
                                   join AH in _ppsDbContext.AccountHead
                                   on accHead.AccountHeadId equals AH.Id
                                   orderby AH.Id
                                   select new TrailBalanceDetailModel
                                   {
                                       AccountHeadId = AH.Id,
                                       AccountHead = AH.AccountHeadName
                                   }).ToList();

            int limit = accountHeadName.Count;

            var finalTrailBalance = new List<TrailBalanceDetailModel>();
            ledgerModel.Detail = finalTrailBalance;

            if (limit > 0)
            {
                for (int index = 0; index < limit; index++)
                {
                    finalTrailBalance.Add(new TrailBalanceDetailModel
                    {
                        AccountHead = accountHeadName[index].AccountHead,
                        OpenDrAmount = openingWithHead[index].OpenDrAmount,
                        OpenCrAmount = openingWithHead[index].OpenCrAmount,
                        CurrDrAmount = currentWithHead[index].OpenDrAmount,
                        CurrCrAmount = currentWithHead[index].OpenCrAmount,
                        CloseDrAmount = closingBalanceSummary[index].OpenDrAmount,
                        CloseCrAmount = closingBalanceSummary[index].OpenCrAmount
                    });
                }
            }
            return ledgerModel;
        }
        //Profit and loss account report
        public ProfitAndLossAccountModel GetProfitAndLossAccount(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            var netIncomeModel = new ProfitAndLossAccountModel
            {
                StartDate = startDate,
                EndDate = endDate,
                RevenueDetail = new List<ProfitAndLossAccountRevenueDetailModel>(),
                DirectExpenseDetail = new List<ProfitAndLossAccountExpenseDetailModel>(),
                IndirectExpenseDetail = new List<ProfitAndLossAccountExpenseDetailModel>(),
                ExpenseDetail = new List<ProfitAndLossAccountExpenseDetailModel>(),
                TotalRevenue = new TotalRevenue(),
                TotalExpense = new TotalExpense(),
                NetIncome = new NetIncome()
            };

            var firstDayofCurrentFiscalYear = new DateTime(fiscalYear, 1, 1);

            //Current Transection Detials Revenue

            var currTransDetailsRevenue = (from entry in _ppsDbContext.TransactionEntry
                                           join detail in _ppsDbContext.TransactionDetail
                                           on entry.Id equals detail.TransactionEntryId
                                           join accountHead in _ppsDbContext.AccountHead
                                           on detail.AccountHeadId equals accountHead.Id
                                           join ash in _ppsDbContext.AccountSubHead
                                           on accountHead.AccountSubHeadId equals ash.Id
                                           join aph in _ppsDbContext.AccountPrimaryHead
                                           on ash.AccountPrimaryHeadId equals aph.Id
                                           join at in _ppsDbContext.AccountType
                                           on aph.AccountTypeId equals at.Id
                                           join an in _ppsDbContext.AccountNature
                                           on at.AccountNatureId equals an.Id
                                           where entry.FiscalYear == fiscalYear
                                                 && entry.CompanyId == companyId
                                                 && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                                 && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                                 //&& entry.VerifiedById != null
                                                 && an.Id == 3
                                           group detail by detail.AccountHeadId into gDetailHead
                                           orderby gDetailHead.FirstOrDefault().AccountHeadId
                                           select new ProfitAndLossAccountDetailModel()
                                           {
                                               AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                               AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                               OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                               OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                           }).ToList();

            var currTransSummaryRevenue = new List<ProfitAndLossAccountDetailModel>();

            // Current Transection Summary Revenue
            if (currTransDetailsRevenue.Count > 0)
            {
                foreach (var currentTranRevenue in currTransDetailsRevenue)
                {
                    double finalAmount = currentTranRevenue.OpenDrAmount - currentTranRevenue.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : 0;
                    var currCrAmount = finalAmount > -1 ? 0 : -finalAmount;
                    currTransSummaryRevenue.Add(new ProfitAndLossAccountDetailModel
                    {
                        AccountHeadId = currentTranRevenue.AccountHeadId,
                        AccountHead = currentTranRevenue.AccountHead,
                        OpenDrAmount = currDrAmount,
                        OpenCrAmount = currCrAmount
                    });
                }
            }

            //Current Transection Detials Expenses
            var currTransDetailsExpenses = (from entry in _ppsDbContext.TransactionEntry
                                            join detail in _ppsDbContext.TransactionDetail
                                            on entry.Id equals detail.TransactionEntryId
                                            join accountHead in _ppsDbContext.AccountHead
                                            on detail.AccountHeadId equals accountHead.Id
                                            join ash in _ppsDbContext.AccountSubHead
                                            on accountHead.AccountSubHeadId equals ash.Id
                                            join aph in _ppsDbContext.AccountPrimaryHead
                                            on ash.AccountPrimaryHeadId equals aph.Id
                                            join at in _ppsDbContext.AccountType
                                            on aph.AccountTypeId equals at.Id
                                            join an in _ppsDbContext.AccountNature
                                            on at.AccountNatureId equals an.Id
                                            where entry.FiscalYear == fiscalYear
                                                  && entry.CompanyId == companyId
                                                  && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                                  && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                                  //&& entry.VerifiedById != null
                                                  && (an.Id == 4
                                                  || an.Id == 5
                                                  || an.Id == 6)
                                            group detail by detail.AccountHeadId into gDetailHead
                                            orderby gDetailHead.FirstOrDefault().AccountHeadId
                                            select new ProfitAndLossAccountDetailModel()
                                            {
                                                AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                                OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                            }).ToList();

            var currTransSummaryExpenses = new List<ProfitAndLossAccountDetailModel>();

            // Current Transection Summary Revenue
            if (currTransDetailsExpenses.Count > 0)
            {
                foreach (var currentTranExpense in currTransDetailsExpenses)
                {
                    double finalAmount = currentTranExpense.OpenDrAmount - currentTranExpense.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : 0;
                    var currCrAmount = finalAmount > -1 ? 0 : -finalAmount;
                    currTransSummaryExpenses.Add(new ProfitAndLossAccountDetailModel
                    {
                        AccountHeadId = currentTranExpense.AccountHeadId,
                        AccountHead = currentTranExpense.AccountHead,
                        OpenDrAmount = currDrAmount,
                        OpenCrAmount = currCrAmount
                    });
                }
            }

            int limitForRevenue = currTransSummaryRevenue.Count;

            var profitAndLossAccountRevenue = new List<ProfitAndLossAccountRevenueDetailModel>();
            netIncomeModel.RevenueDetail = profitAndLossAccountRevenue;

            if (limitForRevenue > 0)
            {
                for (int index = 0; index < limitForRevenue; index++)
                {
                    profitAndLossAccountRevenue.Add(new ProfitAndLossAccountRevenueDetailModel
                    {
                        AccountHead = currTransSummaryRevenue[index].AccountHead,
                        OpenDrAmount = currTransSummaryRevenue[index].OpenDrAmount,
                        OpenCrAmount = currTransSummaryRevenue[index].OpenCrAmount
                    });
                }
            }

            int limitForExpenses = currTransSummaryExpenses.Count;

            var profitAndLossAccountExpenses = new List<ProfitAndLossAccountExpenseDetailModel>();
            netIncomeModel.ExpenseDetail = profitAndLossAccountExpenses;

            if (limitForExpenses > 0)
            {
                for (int index = 0; index < limitForExpenses; index++)
                {
                    profitAndLossAccountExpenses.Add(new ProfitAndLossAccountExpenseDetailModel
                    {
                        AccountHead = currTransSummaryExpenses[index].AccountHead,
                        OpenDrAmount = currTransSummaryExpenses[index].OpenDrAmount,
                        OpenCrAmount = currTransSummaryExpenses[index].OpenCrAmount
                    });
                }
            }

            var totalClosingRevenue = (from CBSR in currTransSummaryRevenue
                                       group CBSR by 1 into gClosingDetail
                                       select new ProfitAndLossAccountDetailModel
                                       {
                                           OpenDrAmount = gClosingDetail.Sum(x => x.OpenDrAmount),
                                           OpenCrAmount = gClosingDetail.Sum(x => x.OpenCrAmount)
                                       }).ToList();

            var totalClosingExpense = (from CBSE in currTransSummaryExpenses
                                       group CBSE by 1 into gClosingDetail
                                       select new ProfitAndLossAccountDetailModel
                                       {
                                           OpenDrAmount = gClosingDetail.Sum(x => x.OpenDrAmount),
                                           OpenCrAmount = gClosingDetail.Sum(x => x.OpenCrAmount)
                                       }).ToList();

            var DrAmount = 0.0;
            var CrAmount = 0.0;

            if (totalClosingRevenue.Count > 0)
            {
                var totalRevenue = new TotalRevenue();
                netIncomeModel.TotalRevenue = totalRevenue;
                CrAmount = totalClosingRevenue.FirstOrDefault().OpenCrAmount;
                totalRevenue.OpenCrAmount = CrAmount;
            }

            if (totalClosingExpense.Count > 0)
            {
                var totalExpense = new TotalExpense();
                netIncomeModel.TotalExpense = totalExpense;
                DrAmount = totalClosingExpense.FirstOrDefault().OpenDrAmount;
                totalExpense.OpenDrAmount = DrAmount;
            }

            var netIncomeAmount = CrAmount - DrAmount;

            var netIncome = new NetIncome();
            netIncomeModel.NetIncome = netIncome;
            netIncome.OpenDrAmount = netIncomeAmount > -1 ? netIncomeAmount : netIncomeAmount;


            return netIncomeModel;
        }
        //Balance sheet report
        public BalanceSheetModel GetBalanceSheet(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            var ledgerModel = new BalanceSheetModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Detail = new List<BalanceSheetDetailModel>(),
                AssetsDetail = new List<BalanceSheetDetailAssetModel>(),
                TotalAssets = new BalanceSheetDetailAssetModel(),
                LiabilitiesDetail = new List<BalanceSheetDetailLiabilitiesModel>(),
                TotalLiabilities = new BalanceSheetDetailLiabilitiesModel(),
                OwnersEquityCapital = new BalanceSheetDetailOwnersEquityModel(),
                OwnersEquityDrawing = new BalanceSheetDetailOwnersEquityModel(),
                NetIncome = new BalanceSheetDetailOwnersEquityModel(),
                TotalOwnersEquity = new BalanceSheetDetailOwnersEquityModel(),
                TotalLiabilitiesAndOwnersEquity = new BalanceSheetDetailOwnersEquityModel()
            };

            var firstDayofCurrentFiscalYear = new DateTime(fiscalYear, 1, 1);

            //Opening Balance for Asset Details
            var openingBalanceAsset = (from opening in _ppsDbContext.AccountHeadOpening
                                       join accountHead in _ppsDbContext.AccountHead
                                       on opening.AccountHeadId equals accountHead.Id
                                       join ash in _ppsDbContext.AccountSubHead
                                       on accountHead.AccountSubHeadId equals ash.Id
                                       join aph in _ppsDbContext.AccountPrimaryHead
                                       on ash.AccountPrimaryHeadId equals aph.Id
                                       join at in _ppsDbContext.AccountType
                                       on aph.AccountTypeId equals at.Id
                                       join an in _ppsDbContext.AccountNature
                                       on at.AccountNatureId equals an.Id
                                       where opening.FiscalYear == fiscalYear
                                             && opening.CompanyId == companyId
                                             && an.Id == 1
                                       orderby opening.AccountHeadId
                                       select new BalanceSheetDetailModel
                                       {
                                           AccountHeadId = opening.AccountHeadId,
                                           AccountHead = accountHead.AccountHeadName,
                                           OpenDrAmount = opening.DrAmount,
                                           OpenCrAmount = opening.CrAmount
                                       }).ToList();


            var openingBalanceSummaryAssets = new List<BalanceSheetDetailModel>();

            // Opening Balance Summary Asset
            if (openingBalanceAsset.Count > 0)
            {
                foreach (var openingBalance in openingBalanceAsset)
                {
                    double finalAmount = openingBalance.OpenDrAmount - openingBalance.OpenCrAmount;
                    var openDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    openingBalanceSummaryAssets.Add(new BalanceSheetDetailModel
                    {
                        AccountHeadId = openingBalance.AccountHeadId,
                        AccountHead = openingBalance.AccountHead,
                        OpenDrAmount = openDrAmount
                    });
                }
            }

            //Current Transection Detials Asset
            var currTransBeforeDetailsAssets = (from entry in _ppsDbContext.TransactionEntry
                                                join detail in _ppsDbContext.TransactionDetail
                                                on entry.Id equals detail.TransactionEntryId
                                                join accountHead in _ppsDbContext.AccountHead
                                                on detail.AccountHeadId equals accountHead.Id
                                                join ash in _ppsDbContext.AccountSubHead
                                                on accountHead.AccountSubHeadId equals ash.Id
                                                join aph in _ppsDbContext.AccountPrimaryHead
                                                on ash.AccountPrimaryHeadId equals aph.Id
                                                join at in _ppsDbContext.AccountType
                                                on aph.AccountTypeId equals at.Id
                                                join an in _ppsDbContext.AccountNature
                                                on at.AccountNatureId equals an.Id
                                                where entry.FiscalYear == fiscalYear
                                                      && entry.CompanyId == companyId
                                                      && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
                                                      && DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                                      //&& entry.VerifiedById != null
                                                      && an.Id == 1
                                                group detail by detail.AccountHeadId into gDetailHead
                                                orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                select new BalanceSheetDetailAssetModel()
                                                {
                                                    AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                    AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                                    OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                    OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                                }).ToList();

            var currTransBeforeSummaryAssets = new List<BalanceSheetDetailAssetModel>();

            // Current Transection Summary Asset
            if (currTransBeforeDetailsAssets.Count > 0)
            {
                foreach (var currentTranBeforeAssets in currTransBeforeDetailsAssets)
                {
                    double finalAmount = currentTranBeforeAssets.OpenDrAmount - currentTranBeforeAssets.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    currTransBeforeSummaryAssets.Add(new BalanceSheetDetailAssetModel
                    {
                        AccountHeadId = currentTranBeforeAssets.AccountHeadId,
                        AccountHead = currentTranBeforeAssets.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var openingBalanceAndCurrTransBeforeSummaryAssets = openingBalanceSummaryAssets.Union(currTransBeforeSummaryAssets);


            var openingAndCurrBeforeSummaryAssets = (from openAndcurrBefore in openingBalanceAndCurrTransBeforeSummaryAssets
                                                     group openAndcurrBefore by openAndcurrBefore.AccountHeadId into gDetailHead
                                                     orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                     select new BalanceSheetDetailAssetModel()
                                                     {
                                                         AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                         AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                         OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount),
                                                         OpenCrAmount = gDetailHead.Sum(x => x.OpenCrAmount)
                                                     }).ToList();

            var openingFinal = openingAndCurrBeforeSummaryAssets;


            //Current Transection Detials Asset
            var currTransDetailsAssets = (from entry in _ppsDbContext.TransactionEntry
                                          join detail in _ppsDbContext.TransactionDetail
                                          on entry.Id equals detail.TransactionEntryId
                                          join accountHead in _ppsDbContext.AccountHead
                                          on detail.AccountHeadId equals accountHead.Id
                                          join ash in _ppsDbContext.AccountSubHead
                                          on accountHead.AccountSubHeadId equals ash.Id
                                          join aph in _ppsDbContext.AccountPrimaryHead
                                          on ash.AccountPrimaryHeadId equals aph.Id
                                          join at in _ppsDbContext.AccountType
                                          on aph.AccountTypeId equals at.Id
                                          join an in _ppsDbContext.AccountNature
                                          on at.AccountNatureId equals an.Id
                                          where entry.FiscalYear == fiscalYear
                                                && entry.CompanyId == companyId
                                                && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                                && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                                //&& entry.VerifiedById != null
                                                && an.Id == 1
                                          group detail by detail.AccountHeadId into gDetailHead
                                          orderby gDetailHead.FirstOrDefault().AccountHeadId
                                          select new BalanceSheetDetailModel()
                                          {
                                              AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                              AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                              OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                              OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                          }).ToList();

            var currTransSummaryAssets = new List<BalanceSheetDetailModel>();

            // Current Transection Summary Asset
            if (currTransDetailsAssets.Count > 0)
            {
                foreach (var currentTranAssets in currTransDetailsAssets)
                {
                    double finalAmount = currentTranAssets.OpenDrAmount - currentTranAssets.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    currTransSummaryAssets.Add(new BalanceSheetDetailModel
                    {
                        AccountHeadId = currentTranAssets.AccountHeadId,
                        AccountHead = currentTranAssets.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var closingAssets = openingFinal.Union(currTransSummaryAssets);


            var closingAssetFinal = (from closingAsset in closingAssets
                                     group closingAsset by closingAsset.AccountHeadId into gDetailHead
                                     orderby gDetailHead.FirstOrDefault().AccountHeadId
                                     select new BalanceSheetDetailAssetModel()
                                     {
                                         AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                         AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                         OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                     }).ToList();

            var closingHeadForMatching = (from clsAssetFinal in closingAssetFinal
                                          orderby clsAssetFinal.AccountHeadId
                                          select new BalanceSheetDetailAssetModel()
                                          {
                                              AccountHeadId = clsAssetFinal.AccountHeadId,
                                              AccountHead = clsAssetFinal.AccountHead,
                                              OpenDrAmount = 0
                                          }).ToList();

            var openingFinalAndClosingWithHeadMatching = closingHeadForMatching.Union(openingBalanceSummaryAssets);

            var openingFinalAndClosingWithHeadMatchingFinal = (from headFinal in openingFinalAndClosingWithHeadMatching
                                                               group headFinal by headFinal.AccountHeadId into gDetailHead
                                                               orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                               select new BalanceSheetDetailAssetModel()
                                                               {
                                                                   AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                                   AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                                   OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                                               }).ToList();

            var currentAndClosingWithHeadMatching = closingHeadForMatching.Union(currTransSummaryAssets);

            var currentAndClosingWithHeadMatchingFinal = (from headFinal in currentAndClosingWithHeadMatching
                                                          group headFinal by headFinal.AccountHeadId into gDetailHead
                                                          orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                          select new BalanceSheetDetailAssetModel()
                                                          {
                                                              AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                              AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                              OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                                          }).ToList();

            //Final Asset Data

            var openingAssetTotal = 0.0;
            var currentAssetTotal = 0.0;
            var closingAssetTotal = 0.0;

            int limitForClosingBalanceAsset = closingAssetFinal.Count;

            var closingBalAsset = new List<BalanceSheetDetailAssetModel>();
            ledgerModel.AssetsDetail = closingBalAsset;

            if (limitForClosingBalanceAsset > 0)
            {
                for (int index = 0; index < limitForClosingBalanceAsset; index++)
                {
                    openingAssetTotal += openingFinal[index].OpenDrAmount;
                    currentAssetTotal += currentAndClosingWithHeadMatchingFinal[index].OpenDrAmount;
                    closingAssetTotal += closingAssetFinal[index].OpenDrAmount;

                    closingBalAsset.Add(new BalanceSheetDetailAssetModel
                    {
                        AccountHead = closingAssetFinal[index].AccountHead,
                        OpenDrAmount = openingFinal[index].OpenDrAmount,
                        CurrDrAmount = currentAndClosingWithHeadMatchingFinal[index].OpenDrAmount,
                        CloseDrAmount = closingAssetFinal[index].OpenDrAmount
                    });
                }
            }

            var totalAsset = new BalanceSheetDetailAssetModel();
            ledgerModel.TotalAssets = totalAsset;

            totalAsset.OpenDrAmount = openingAssetTotal;
            totalAsset.CurrDrAmount = currentAssetTotal;
            totalAsset.CloseDrAmount = closingAssetTotal;

            //Asset End

            //Liabilities Start

            //Opening Balance for Liabilities Details
            var openingBalanceLiabilities = (from opening in _ppsDbContext.AccountHeadOpening
                                             join accountHead in _ppsDbContext.AccountHead
                                             on opening.AccountHeadId equals accountHead.Id
                                             join ash in _ppsDbContext.AccountSubHead
                                             on accountHead.AccountSubHeadId equals ash.Id
                                             join aph in _ppsDbContext.AccountPrimaryHead
                                             on ash.AccountPrimaryHeadId equals aph.Id
                                             join at in _ppsDbContext.AccountType
                                             on aph.AccountTypeId equals at.Id
                                             join an in _ppsDbContext.AccountNature
                                             on at.AccountNatureId equals an.Id
                                             where opening.FiscalYear == fiscalYear
                                                   && opening.CompanyId == companyId
                                                   && an.Id == 2 && at.Id == 4
                                             orderby opening.AccountHeadId
                                             select new BalanceSheetDetailLiabilitiesModel
                                             {
                                                 AccountHeadId = opening.AccountHeadId,
                                                 AccountHead = accountHead.AccountHeadName,
                                                 OpenDrAmount = opening.DrAmount,
                                                 OpenCrAmount = opening.CrAmount
                                             }).ToList();


            var openingBalanceSummaryLiabilities = new List<BalanceSheetDetailLiabilitiesModel>();

            // Opening Balance Summary Liabilities
            if (openingBalanceLiabilities.Count > 0)
            {
                foreach (var openingBalance in openingBalanceLiabilities)
                {
                    double finalAmount = openingBalance.OpenDrAmount - openingBalance.OpenCrAmount;
                    var openDrAmount = finalAmount > -1 ? -finalAmount : finalAmount;
                    openingBalanceSummaryLiabilities.Add(new BalanceSheetDetailLiabilitiesModel
                    {
                        AccountHeadId = openingBalance.AccountHeadId,
                        AccountHead = openingBalance.AccountHead,
                        OpenDrAmount = openDrAmount
                    });
                }
            }

            //Current Transection Detials Liabilities
            var currTransBeforeDetailsLiabilities = (from entry in _ppsDbContext.TransactionEntry
                                                     join detail in _ppsDbContext.TransactionDetail
                                                     on entry.Id equals detail.TransactionEntryId
                                                     join accountHead in _ppsDbContext.AccountHead
                                                     on detail.AccountHeadId equals accountHead.Id
                                                     join ash in _ppsDbContext.AccountSubHead
                                                     on accountHead.AccountSubHeadId equals ash.Id
                                                     join aph in _ppsDbContext.AccountPrimaryHead
                                                     on ash.AccountPrimaryHeadId equals aph.Id
                                                     join at in _ppsDbContext.AccountType
                                                     on aph.AccountTypeId equals at.Id
                                                     join an in _ppsDbContext.AccountNature
                                                     on at.AccountNatureId equals an.Id
                                                     where entry.FiscalYear == fiscalYear
                                                           && entry.CompanyId == companyId
                                                           && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
                                                           && DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                                           //&& entry.VerifiedById != null
                                                           && an.Id == 2
                                                     group detail by detail.AccountHeadId into gDetailHead
                                                     orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                     select new BalanceSheetDetailLiabilitiesModel()
                                                     {
                                                         AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                         AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                                         OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                         OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                                     }).ToList();

            var currTransBeforeSummaryLiabilities = new List<BalanceSheetDetailLiabilitiesModel>();

            // Current Transection Summary Liabilities
            if (currTransBeforeDetailsLiabilities.Count > 0)
            {
                foreach (var currentTranBeforeLiabilities in currTransBeforeDetailsLiabilities)
                {
                    double finalAmount = currentTranBeforeLiabilities.OpenDrAmount - currentTranBeforeLiabilities.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : -finalAmount;
                    currTransBeforeSummaryLiabilities.Add(new BalanceSheetDetailLiabilitiesModel
                    {
                        AccountHeadId = currentTranBeforeLiabilities.AccountHeadId,
                        AccountHead = currentTranBeforeLiabilities.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var openingBalanceAndCurrTransBeforeSummaryLiabilities = openingBalanceSummaryLiabilities.Union(currTransBeforeSummaryLiabilities);


            var openingAndCurrBeforeSummaryLiabilities = (from openAndcurrBefore in openingBalanceAndCurrTransBeforeSummaryLiabilities
                                                          group openAndcurrBefore by openAndcurrBefore.AccountHeadId into gDetailHead
                                                          orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                          select new BalanceSheetDetailLiabilitiesModel()
                                                          {
                                                              AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                              AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                              OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount),
                                                              OpenCrAmount = gDetailHead.Sum(x => x.OpenCrAmount)
                                                          }).ToList();

            var openingLiabilitiesFinal = openingAndCurrBeforeSummaryLiabilities;


            //Current Transection Detials Liabilities
            var currTransDetailsLiabilities = (from entry in _ppsDbContext.TransactionEntry
                                               join detail in _ppsDbContext.TransactionDetail
                                               on entry.Id equals detail.TransactionEntryId
                                               join accountHead in _ppsDbContext.AccountHead
                                               on detail.AccountHeadId equals accountHead.Id
                                               join ash in _ppsDbContext.AccountSubHead
                                               on accountHead.AccountSubHeadId equals ash.Id
                                               join aph in _ppsDbContext.AccountPrimaryHead
                                               on ash.AccountPrimaryHeadId equals aph.Id
                                               join at in _ppsDbContext.AccountType
                                               on aph.AccountTypeId equals at.Id
                                               join an in _ppsDbContext.AccountNature
                                               on at.AccountNatureId equals an.Id
                                               where entry.FiscalYear == fiscalYear
                                                     && entry.CompanyId == companyId
                                                     && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                                     && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                                     //&& entry.VerifiedById != null
                                                     && an.Id == 2
                                               group detail by detail.AccountHeadId into gDetailHead
                                               orderby gDetailHead.FirstOrDefault().AccountHeadId
                                               select new BalanceSheetDetailLiabilitiesModel()
                                               {
                                                   AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                   AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                                   OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                   OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                               }).ToList();

            var currTransSummaryLiabilities = new List<BalanceSheetDetailLiabilitiesModel>();

            // Current Transection Summary Asset
            if (currTransDetailsLiabilities.Count > 0)
            {
                foreach (var currentTranLiabilities in currTransDetailsLiabilities)
                {
                    double finalAmount = currentTranLiabilities.OpenDrAmount - currentTranLiabilities.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : -finalAmount;
                    currTransSummaryLiabilities.Add(new BalanceSheetDetailLiabilitiesModel
                    {
                        AccountHeadId = currentTranLiabilities.AccountHeadId,
                        AccountHead = currentTranLiabilities.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var closingLiabilities = openingLiabilitiesFinal.Union(currTransSummaryLiabilities);


            var closingLiabilitiesFinal = (from closingLiability in closingLiabilities
                                           group closingLiability by closingLiability.AccountHeadId into gDetailHead
                                           orderby gDetailHead.FirstOrDefault().AccountHeadId
                                           select new BalanceSheetDetailLiabilitiesModel()
                                           {
                                               AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                               AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                               OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                           }).ToList();

            var closingHeadForMatchingLiabilities = (from clsLiabilitiesFinal in closingLiabilitiesFinal
                                                     orderby clsLiabilitiesFinal.AccountHeadId
                                                     select new BalanceSheetDetailLiabilitiesModel()
                                                     {
                                                         AccountHeadId = clsLiabilitiesFinal.AccountHeadId,
                                                         AccountHead = clsLiabilitiesFinal.AccountHead,
                                                         OpenDrAmount = 0
                                                     }).ToList();

            var openingFinalAndClosingWithHeadMatchingLiabilities = closingHeadForMatchingLiabilities.Union(openingBalanceSummaryLiabilities);

            var openingFinalAndClosingWithHeadMatchingFinalLiabilities = (from headFinal in openingFinalAndClosingWithHeadMatchingLiabilities
                                                                          group headFinal by headFinal.AccountHeadId into gDetailHead
                                                                          orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                                          select new BalanceSheetDetailLiabilitiesModel()
                                                                          {
                                                                              AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                                              AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                                              OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                                                          }).ToList();

            var currentAndClosingWithHeadMatchingLiabilities = closingHeadForMatchingLiabilities.Union(currTransSummaryLiabilities);

            var currentAndClosingWithHeadMatchingFinalLiabilities = (from headFinal in currentAndClosingWithHeadMatchingLiabilities
                                                                     group headFinal by headFinal.AccountHeadId into gDetailHead
                                                                     orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                                     select new BalanceSheetDetailAssetModel()
                                                                     {
                                                                         AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                                         AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                                         OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                                                     }).ToList();

            //Final Liabilities Data

            var openingLiabilitiesTotal = 0.0;
            var currentLiabilitiesTotal = 0.0;
            var closingLiabilitiesTotal = 0.0;

            int limitForClosingBalanceLiabilities = closingLiabilitiesFinal.Count;

            var closingBalLiabilities = new List<BalanceSheetDetailLiabilitiesModel>();
            ledgerModel.LiabilitiesDetail = closingBalLiabilities;

            if (limitForClosingBalanceLiabilities > 0)
            {
                for (int index = 0; index < limitForClosingBalanceLiabilities; index++)
                {
                    openingLiabilitiesTotal += openingLiabilitiesFinal[index].OpenDrAmount;
                    currentLiabilitiesTotal += currentAndClosingWithHeadMatchingFinalLiabilities[index].OpenDrAmount;
                    closingLiabilitiesTotal += closingLiabilitiesFinal[index].OpenDrAmount;

                    closingBalLiabilities.Add(new BalanceSheetDetailLiabilitiesModel
                    {
                        AccountHead = closingLiabilitiesFinal[index].AccountHead,
                        OpenDrAmount = openingLiabilitiesFinal[index].OpenDrAmount,
                        CurrDrAmount = currentAndClosingWithHeadMatchingFinalLiabilities[index].OpenDrAmount,
                        CloseDrAmount = closingLiabilitiesFinal[index].OpenDrAmount
                    });
                }
            }

            var totalLiabilities = new BalanceSheetDetailLiabilitiesModel();
            ledgerModel.TotalLiabilities = totalLiabilities;

            totalLiabilities.OpenDrAmount = openingLiabilitiesTotal;
            totalLiabilities.CurrDrAmount = currentLiabilitiesTotal;
            totalLiabilities.CloseDrAmount = closingLiabilitiesTotal;


            //Liabilities End


            //Owners Equity Start

            //Capital Start
            //Opening Balance for Owners Equity Details
            var openingBalanceCapital = (from opening in _ppsDbContext.AccountHeadOpening
                                         join accountHead in _ppsDbContext.AccountHead
                                         on opening.AccountHeadId equals accountHead.Id
                                         join ash in _ppsDbContext.AccountSubHead
                                         on accountHead.AccountSubHeadId equals ash.Id
                                         join aph in _ppsDbContext.AccountPrimaryHead
                                         on ash.AccountPrimaryHeadId equals aph.Id
                                         join at in _ppsDbContext.AccountType
                                         on aph.AccountTypeId equals at.Id
                                         join an in _ppsDbContext.AccountNature
                                         on at.AccountNatureId equals an.Id
                                         where opening.FiscalYear == fiscalYear
                                               && opening.CompanyId == companyId
                                               && an.Id == 2
                                               && at.Id == 3
                                         orderby opening.AccountHeadId
                                         select new BalanceSheetDetailOwnersEquityModel
                                         {
                                             AccountHeadId = opening.AccountHeadId,
                                             AccountHead = accountHead.AccountHeadName,
                                             OpenDrAmount = opening.DrAmount,
                                             OpenCrAmount = opening.CrAmount
                                         }).ToList();


            var openingBalanceSummaryCapital = new List<BalanceSheetDetailOwnersEquityModel>();

            // Opening Balance Summary Capital
            if (openingBalanceCapital.Count > 0)
            {
                foreach (var openingBalance in openingBalanceCapital)
                {
                    double finalAmount = openingBalance.OpenDrAmount - openingBalance.OpenCrAmount;
                    var openDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    openingBalanceSummaryCapital.Add(new BalanceSheetDetailOwnersEquityModel
                    {
                        AccountHeadId = openingBalance.AccountHeadId,
                        AccountHead = openingBalance.AccountHead,
                        OpenDrAmount = openDrAmount
                    });
                }
            }

            //Current Transection Detials Capital
            var currTransBeforeDetailsCapital = (from entry in _ppsDbContext.TransactionEntry
                                                 join detail in _ppsDbContext.TransactionDetail
                                                 on entry.Id equals detail.TransactionEntryId
                                                 join accountHead in _ppsDbContext.AccountHead
                                                 on detail.AccountHeadId equals accountHead.Id
                                                 join ash in _ppsDbContext.AccountSubHead
                                                 on accountHead.AccountSubHeadId equals ash.Id
                                                 join aph in _ppsDbContext.AccountPrimaryHead
                                                 on ash.AccountPrimaryHeadId equals aph.Id
                                                 join at in _ppsDbContext.AccountType
                                                 on aph.AccountTypeId equals at.Id
                                                 join an in _ppsDbContext.AccountNature
                                                 on at.AccountNatureId equals an.Id
                                                 where entry.FiscalYear == fiscalYear
                                                       && entry.CompanyId == companyId
                                                       && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
                                                       && DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                                       //&& entry.VerifiedById != null
                                                       && an.Id == 2
                                               && at.Id == 3
                                                 group detail by detail.AccountHeadId into gDetailHead
                                                 orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                 select new BalanceSheetDetailOwnersEquityModel()
                                                 {
                                                     AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                     AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                                     OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                                     OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                                 }).ToList();

            var currTransBeforeSummaryCapital = new List<BalanceSheetDetailOwnersEquityModel>();

            // Current Transection Summary Capital
            if (currTransBeforeDetailsCapital.Count > 0)
            {
                foreach (var currentTranBeforeCapital in currTransBeforeSummaryCapital)
                {
                    double finalAmount = currentTranBeforeCapital.OpenDrAmount - currentTranBeforeCapital.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    currTransBeforeSummaryCapital.Add(new BalanceSheetDetailOwnersEquityModel
                    {
                        AccountHeadId = currentTranBeforeCapital.AccountHeadId,
                        AccountHead = currentTranBeforeCapital.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var openingBalanceAndCurrTransBeforeSummaryCapital = openingBalanceSummaryCapital.Union(currTransBeforeSummaryCapital);


            var openingAndCurrBeforeSummaryCapital = (from openAndcurrBefore in openingBalanceAndCurrTransBeforeSummaryCapital
                                                      group openAndcurrBefore by openAndcurrBefore.AccountHeadId into gDetailHead
                                                      orderby gDetailHead.FirstOrDefault().AccountHeadId
                                                      select new BalanceSheetDetailOwnersEquityModel()
                                                      {
                                                          AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                                          AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                                          OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount),
                                                          OpenCrAmount = gDetailHead.Sum(x => x.OpenCrAmount)
                                                      }).ToList();

            var openingCapitalFinal = openingAndCurrBeforeSummaryCapital;


            //Current Transection Detials Capital
            var currTransDetailsCapital = (from entry in _ppsDbContext.TransactionEntry
                                           join detail in _ppsDbContext.TransactionDetail
                                           on entry.Id equals detail.TransactionEntryId
                                           join accountHead in _ppsDbContext.AccountHead
                                           on detail.AccountHeadId equals accountHead.Id
                                           join ash in _ppsDbContext.AccountSubHead
                                           on accountHead.AccountSubHeadId equals ash.Id
                                           join aph in _ppsDbContext.AccountPrimaryHead
                                           on ash.AccountPrimaryHeadId equals aph.Id
                                           join at in _ppsDbContext.AccountType
                                           on aph.AccountTypeId equals at.Id
                                           join an in _ppsDbContext.AccountNature
                                           on at.AccountNatureId equals an.Id
                                           where entry.FiscalYear == fiscalYear
                                                 && entry.CompanyId == companyId
                                                 && DbFunctions.TruncateTime(entry.TransactionDate) >= (startDate)
                                                 && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                                 //&& entry.VerifiedById != null
                                                 && an.Id == 2
                                                 && at.Id == 3
                                           group detail by detail.AccountHeadId into gDetailHead
                                           orderby gDetailHead.FirstOrDefault().AccountHeadId
                                           select new BalanceSheetDetailOwnersEquityModel()
                                           {
                                               AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                               AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
                                               OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
                                               OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
                                           }).ToList();

            var currTransSummaryCapital = new List<BalanceSheetDetailOwnersEquityModel>();

            // Current Transection Summary Capital
            if (currTransDetailsCapital.Count > 0)
            {
                foreach (var currentTransCapital in currTransDetailsCapital)
                {
                    double finalAmount = currentTransCapital.OpenDrAmount - currentTransCapital.OpenCrAmount;
                    var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
                    currTransSummaryCapital.Add(new BalanceSheetDetailOwnersEquityModel
                    {
                        AccountHeadId = currentTransCapital.AccountHeadId,
                        AccountHead = currentTransCapital.AccountHead,
                        OpenDrAmount = currDrAmount
                    });
                }
            }


            var closingCapital = openingCapitalFinal.Union(currTransSummaryCapital);


            var closingCapitalFinal = (from clsCapital in closingCapital
                                       group clsCapital by clsCapital.AccountHeadId into gDetailHead
                                       orderby gDetailHead.FirstOrDefault().AccountHeadId
                                       select new BalanceSheetDetailOwnersEquityModel()
                                       {
                                           AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
                                           AccountHead = gDetailHead.FirstOrDefault().AccountHead,
                                           OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
                                       }).ToList();

            //Final Capital Data

            var openingCapitalTotal = 0.0;
            var currentCapitalTotal = 0.0;
            var closingCapitalTotal = 0.0;

            int limitForClosingBalanceCapital = closingCapitalFinal.Count;

            var closingBalCapital = new BalanceSheetDetailOwnersEquityModel();
            ledgerModel.OwnersEquityCapital = closingBalCapital;

            if (limitForClosingBalanceCapital > 0)
            {
                for (int index = 0; index < limitForClosingBalanceCapital; index++)
                {
                    var tempOpening = openingCapitalFinal.Count > 0 ? openingCapitalFinal[index].OpenDrAmount : 0;
                    openingCapitalTotal += tempOpening;
                    var tempCurrent = currTransSummaryCapital.Count > 0 ? currTransSummaryCapital[index].OpenDrAmount : 0;
                    currentCapitalTotal += tempCurrent;
                    var tempClosing = closingCapitalFinal[index].OpenDrAmount;
                    closingCapitalTotal += tempClosing;

                    closingBalCapital.AccountHead = closingCapitalFinal[index].AccountHead;
                    closingBalCapital.OpenDrAmount = tempOpening;
                    closingBalCapital.CurrDrAmount = tempCurrent;
                    closingBalCapital.CloseDrAmount = tempClosing;

                }
            }

            var totalCapital = new BalanceSheetDetailOwnersEquityModel();
            ledgerModel.TotalOwnersEquity = totalCapital;

            totalCapital.OpenDrAmount = openingCapitalTotal;
            totalCapital.CurrDrAmount = currentCapitalTotal;
            totalCapital.CloseDrAmount = closingCapitalTotal;


            //Capital End

            ////Drawing Start

            ////Current Transection Detials Drawing
            //var currTransBeforeDetailsDrawing = (from entry in _ppsDbContext.TransactionEntry
            //                                     join detail in _ppsDbContext.TransactionDetail
            //                                     on entry.Id equals detail.TransactionEntryId
            //                                     join accountHead in _ppsDbContext.AccountHead
            //                                     on detail.AccountHeadId equals accountHead.Id
            //                                     join ash in _ppsDbContext.AccountSubHead
            //                                     on accountHead.AccountSubHeadId equals ash.Id
            //                                     join aph in _ppsDbContext.AccountPrimaryHead
            //                                     on ash.AccountPrimaryHeadId equals aph.Id
            //                                     join at in _ppsDbContext.AccountType
            //                                     on aph.AccountTypeId equals at.Id
            //                                     join an in _ppsDbContext.AccountNature
            //                                     on at.AccountNatureId equals an.Id
            //                                     where entry.FiscalYear == fiscalYear
            //                                           && entry.CompanyId == companyId
            //                                           && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(firstDayofCurrentFiscalYear)
            //                                           && DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
            //                                           && entry.VerifiedById != null
            //                                           && an.Id == 5
            //                                           && at.Id == 10
            //                                     group detail by detail.AccountHeadId into gDetailHead
            //                                     orderby gDetailHead.FirstOrDefault().AccountHeadId
            //                                     select new BalanceSheetDetailOwnersEquityModel()
            //                                     {
            //                                         AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
            //                                         AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
            //                                         OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
            //                                         OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
            //                                     }).ToList();

            //var currTransBeforeSummaryDrawing = new List<BalanceSheetDetailOwnersEquityModel>();

            //// Current Transection Summary Drawing
            //if (currTransBeforeDetailsDrawing.Count > 0)
            //{
            //    foreach (var currentTranBeforeDrawing in currTransBeforeDetailsDrawing)
            //    {
            //        double finalAmount = currentTranBeforeDrawing.OpenDrAmount - currentTranBeforeDrawing.OpenCrAmount;
            //        var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
            //        currTransBeforeSummaryDrawing.Add(new BalanceSheetDetailOwnersEquityModel
            //        {
            //            AccountHeadId = currentTranBeforeDrawing.AccountHeadId,
            //            AccountHead = currentTranBeforeDrawing.AccountHead,
            //            OpenDrAmount = currDrAmount
            //        });
            //    }
            //}

            //var openingDrawingFinal = currTransBeforeSummaryDrawing;


            ////Current Transection Detials Drawing
            //var currTransDetailsDrawing = (from entry in _ppsDbContext.TransactionEntry
            //                               join detail in _ppsDbContext.TransactionDetail
            //                               on entry.Id equals detail.TransactionEntryId
            //                               join accountHead in _ppsDbContext.AccountHead
            //                               on detail.AccountHeadId equals accountHead.Id
            //                               join ash in _ppsDbContext.AccountSubHead
            //                               on accountHead.AccountSubHeadId equals ash.Id
            //                               join aph in _ppsDbContext.AccountPrimaryHead
            //                               on ash.AccountPrimaryHeadId equals aph.Id
            //                               join at in _ppsDbContext.AccountType
            //                               on aph.AccountTypeId equals at.Id
            //                               join an in _ppsDbContext.AccountNature
            //                               on at.AccountNatureId equals an.Id
            //                               where entry.FiscalYear == fiscalYear
            //                                     && entry.CompanyId == companyId
            //                                     && DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
            //                                     && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
            //                                     && entry.VerifiedById != null
            //                                     && an.Id == 5
            //                                     && at.Id == 10
            //                               group detail by detail.AccountHeadId into gDetailHead
            //                               orderby gDetailHead.FirstOrDefault().AccountHeadId
            //                               select new BalanceSheetDetailOwnersEquityModel()
            //                               {
            //                                   AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
            //                                   AccountHead = gDetailHead.FirstOrDefault().AccountHead.AccountHeadName,
            //                                   OpenDrAmount = gDetailHead.Sum(x => x.DrAmount),
            //                                   OpenCrAmount = gDetailHead.Sum(x => x.CrAmount)
            //                               }).ToList();

            //var currTransSummaryDrawing = new List<BalanceSheetDetailOwnersEquityModel>();

            //// Current Transection Summary Drawing
            //if (currTransDetailsDrawing.Count > 0)
            //{
            //    foreach (var currentTransDrawing in currTransDetailsDrawing)
            //    {
            //        double finalAmount = currentTransDrawing.OpenDrAmount - currentTransDrawing.OpenCrAmount;
            //        var currDrAmount = finalAmount > -1 ? finalAmount : finalAmount;
            //        currTransSummaryDrawing.Add(new BalanceSheetDetailOwnersEquityModel
            //        {
            //            AccountHeadId = currentTransDrawing.AccountHeadId,
            //            AccountHead = currentTransDrawing.AccountHead,
            //            OpenDrAmount = currDrAmount
            //        });
            //    }
            //}


            //var closingDrawing = openingDrawingFinal.Union(currTransSummaryDrawing);


            //var closingDrawingFinal = (from clsDrawing in closingDrawing
            //                           group clsDrawing by clsDrawing.AccountHeadId into gDetailHead
            //                           orderby gDetailHead.FirstOrDefault().AccountHeadId
            //                           select new BalanceSheetDetailOwnersEquityModel()
            //                           {
            //                               AccountHeadId = gDetailHead.FirstOrDefault().AccountHeadId,
            //                               AccountHead = gDetailHead.FirstOrDefault().AccountHead,
            //                               OpenDrAmount = gDetailHead.Sum(x => x.OpenDrAmount)
            //                           }).ToList();

            ////Final Drawing Data

            //var openingDrawingTotal = 0.0;
            //var currentDrawingTotal = 0.0;
            //var closingDrawingTotal = 0.0;

            //int limitForClosingBalanceDrawing = closingDrawingFinal.Count;

            //var closingBalDrawing = new BalanceSheetDetailOwnersEquityModel();
            //ledgerModel.OwnersEquityDrawing = closingBalDrawing;

            //if (limitForClosingBalanceDrawing > 0)
            //{
            //    for (int index = 0; index < limitForClosingBalanceDrawing; index++)
            //    {
            //        var tempOpening = openingDrawingFinal.Count > 0 ? openingDrawingFinal[index].OpenDrAmount : 0;
            //        openingDrawingTotal += tempOpening;
            //        var tempCurrent = currTransSummaryDrawing.Count > 0 ? currTransSummaryDrawing[index].OpenDrAmount : 0;
            //        currentDrawingTotal += tempCurrent;
            //        var tempClosing = closingDrawingFinal.Count > 0 ? closingDrawingFinal[index].OpenDrAmount : 0;
            //        closingDrawingTotal += tempClosing;

            //        closingBalDrawing.AccountHead = closingDrawingFinal[index].AccountHead;
            //        closingBalDrawing.OpenDrAmount = tempOpening;
            //        closingBalDrawing.CurrDrAmount = tempCurrent;
            //        closingBalDrawing.CloseDrAmount = tempClosing;

            //    }
            //}

            //var totalDrawing = new BalanceSheetDetailOwnersEquityModel();
            //ledgerModel.OwnersEquityDrawing = totalDrawing;
            //totalDrawing.OpenDrAmount = openingDrawingTotal;
            //totalDrawing.CurrDrAmount = currentDrawingTotal;
            //totalDrawing.CloseDrAmount = closingDrawingTotal;


            ////Drawing End

            //Net Income Start
            var openingNetIncome = 0.0;
            var currentNetIncome = 0.0;
            var closingNetIncome = 0.0;

            var netIncome = new BalanceSheetDetailOwnersEquityModel();
            ledgerModel.NetIncome = netIncome;

            var newOpeningNetIncome = GetProfitAndLossAccount(fiscalYear, companyId, firstDayofCurrentFiscalYear, startDate.AddDays(-1));
            var openNetIncome = newOpeningNetIncome.NetIncome.OpenDrAmount;

            var newCurrentNetIncome = GetProfitAndLossAccount(fiscalYear, companyId, startDate, endDate);
            var currNetIncome = newCurrentNetIncome.NetIncome.OpenDrAmount;

            var closeNetIncome = openNetIncome + currNetIncome;


            var tempOpeningNetIncome = netIncome.OpenDrAmount = openNetIncome;
            openingNetIncome = tempOpeningNetIncome;
            var tempCurrNetIncome = netIncome.CurrDrAmount = currNetIncome;
            currentNetIncome = tempCurrNetIncome;
            var tempClosingNetIncome = netIncome.CloseDrAmount = closeNetIncome;
            closingNetIncome = tempClosingNetIncome;

            //Net Income End

            //Total Owners Equity Start
            var totalOpeningOwnersEquity = 0.0;
            var totalCurrentOwnersEquity = 0.0;
            var totalClosingOwnersEquity = 0.0;

            //var totalOwnersEquity = new BalanceSheetDetailOwnersEquityModel();
            //ledgerModel.TotalOwnersEquity = totalOwnersEquity;
            //totalOwnersEquity.OpenDrAmount = openingCapitalTotal + openingNetIncome - openingDrawingTotal;
            //totalOpeningOwnersEquity = totalOwnersEquity.OpenDrAmount;
            //totalOwnersEquity.CurrDrAmount = currentCapitalTotal + currentNetIncome - currentDrawingTotal;
            //totalCurrentOwnersEquity = totalOwnersEquity.CurrDrAmount;
            //totalOwnersEquity.CloseDrAmount = closingCapitalTotal + closingNetIncome - closingDrawingTotal;
            //totalClosingOwnersEquity = totalOwnersEquity.CloseDrAmount;

            var totalOwnersEquity = new BalanceSheetDetailOwnersEquityModel();
            ledgerModel.TotalOwnersEquity = totalOwnersEquity;
            totalOwnersEquity.OpenDrAmount = openingCapitalTotal + openingNetIncome;
            totalOpeningOwnersEquity = totalOwnersEquity.OpenDrAmount;
            totalOwnersEquity.CurrDrAmount = currentCapitalTotal + currentNetIncome;
            totalCurrentOwnersEquity = totalOwnersEquity.CurrDrAmount;
            totalOwnersEquity.CloseDrAmount = closingCapitalTotal + closingNetIncome;
            totalClosingOwnersEquity = totalOwnersEquity.CloseDrAmount;
            //Total Owners Equity End

            //Total Liabilities and Owners Equity

            var totalOpeningLiabilitiesAndOwnersEquity = 0.0;
            var totalCurrentLiabilitiesAndOwnersEquity = 0.0;
            var totalClosingLiabilitiesAndOwnersEquity = 0.0;

            var totalLiabilitiesAndOwnersEquity = new BalanceSheetDetailOwnersEquityModel();
            ledgerModel.TotalLiabilitiesAndOwnersEquity = totalLiabilitiesAndOwnersEquity;
            totalLiabilitiesAndOwnersEquity.OpenDrAmount = openingLiabilitiesTotal + totalOpeningOwnersEquity;
            totalOpeningLiabilitiesAndOwnersEquity = totalLiabilitiesAndOwnersEquity.OpenDrAmount;
            totalLiabilitiesAndOwnersEquity.CurrDrAmount = currentLiabilitiesTotal + totalCurrentOwnersEquity;
            totalCurrentLiabilitiesAndOwnersEquity = totalLiabilitiesAndOwnersEquity.CurrDrAmount;
            totalLiabilitiesAndOwnersEquity.CloseDrAmount = closingLiabilitiesTotal + totalClosingOwnersEquity;
            totalClosingLiabilitiesAndOwnersEquity = totalLiabilitiesAndOwnersEquity.CloseDrAmount;

            return ledgerModel;
        }

        public CustomerStatementVm GetCustomerStatement(int companyId, int customerId, DateTime startDate, DateTime endDate)
        {
            var customer = _ppsDbContext.Customer.FirstOrDefault(x => x.Id == customerId);
            if (customer == null && customer?.AccountHeadId == null)
            {
                throw new Exception($"This customer id: {customerId} doesn't exist.");
            }
            var previousCustomerTransaction = (from entry in _ppsDbContext.TransactionEntry
                                               join detail in _ppsDbContext.TransactionDetail
                                                   on entry.Id equals detail.TransactionEntryId
                                               where entry.CompanyId == companyId &&
                                                    DbFunctions.TruncateTime(entry.TransactionDate) < DbFunctions.TruncateTime(startDate)
                                                     // && entry.VerifiedById != null
                                                     && detail.AccountHeadId == customer.AccountHeadId
                                               group detail by detail.AccountHeadId into gDetail
                                               select new
                                               {
                                                   DrAmount = gDetail.Sum(x => x.DrAmount),
                                                   CrAmount = gDetail.Sum(x => x.CrAmount),
                                               }).ToList();
            var previousBalance = 0d;
            var asOfDateBalance = 0d;
            if (previousCustomerTransaction.Count > 0)
            {
                previousBalance = -(previousCustomerTransaction.FirstOrDefault().DrAmount -
                                  previousCustomerTransaction.FirstOrDefault().CrAmount);

            }

            var customerTransaction = (from entry in _ppsDbContext.TransactionEntry
                                       join detail in _ppsDbContext.TransactionDetail
                                           on entry.Id equals detail.TransactionEntryId
                                       where entry.CompanyId == companyId &&
                                             DbFunctions.TruncateTime(entry.TransactionDate) >= DbFunctions.TruncateTime(startDate)
                                             && DbFunctions.TruncateTime(entry.TransactionDate) <= DbFunctions.TruncateTime(endDate)
                                             //&& entry.VerifiedById != null 
                                             && detail.AccountHeadId == customer.AccountHeadId
                                       orderby entry.TransactionDate
                                       select new
                                       {
                                           entry.TransactionDate,
                                           entry.TransactionTypeId,
                                           detail.AccountHead.AccountHeadName,
                                           detail.DrAmount,
                                           detail.CrAmount
                                       }).ToList();


            var customerStatementDetailVm = new List<CustomerStatementDetailVm>();

            if (customerTransaction.Count > 0)
            {
                var currentBalance = previousBalance;
                foreach (var ct in customerTransaction)
                {
                    var balance = ct.DrAmount > ct.CrAmount ? ct.DrAmount : ct.CrAmount;
                    currentBalance += ct.TransactionTypeId == (int)TransactionTypeEnum.Receipt
                        ? balance
                        : -balance;
                    customerStatementDetailVm.Add(new CustomerStatementDetailVm
                    {
                        AccountHead = ct.AccountHeadName,
                        TransactionDate = ct.TransactionDate,
                        TransactionAmount = balance,
                        TransactionType = ct.TransactionTypeId == (int)TransactionTypeEnum.Receipt ? "Deposit" : "Invoice",
                        TransactionBalance = currentBalance
                    });
                }
                asOfDateBalance = currentBalance;
            }

            var customerStatementVm = new CustomerStatementVm
            {
                CustomerName = customer.CustomerName,
                CustomerCode = customer.CustomerCode.ToString(),
                CustomerAddress = customer.CustomerAddress,
                CustomerMobile = customer.CustomerMobile,
                CustomerSeCode = customer.EmployeeId?.ToString() ?? "",
                StatementStartDate = startDate,
                StatementEndDate = endDate,
                PreviousBalance = previousBalance,
                AsOfDateBalance = asOfDateBalance,
                CustomerStatementDetail = customerStatementDetailVm
            };

            return customerStatementVm;
        }

        public VoucherVm GetVoucherDetail(string tranNo)
        {
            var transactionEntry = _ppsDbContext.TransactionEntry.FirstOrDefault(x => x.TransactionNumber == tranNo);
            if (transactionEntry == null)
            {
                throw new Exception($"This transaction no: {tranNo} doesn't exist.");
            }

            var transactionDetail = transactionEntry.TransactionDetail.
                Select(x => new VoucherDetailVm
                {
                    AccountHeadName = x.AccountHead.AccountHeadName,
                    DrAmount = x.DrAmount,
                    CrAmount = x.CrAmount,
                    Note = x.Note
                }).
                ToList();

            var totalDrAmount = transactionDetail.Sum(x => x.DrAmount);
            var totalCrAmount = transactionDetail.Sum(x => x.CrAmount);

            var vocherVm = new VoucherVm
            {
                TransactionNo = transactionEntry.TransactionNumber,
                TransactionDate = transactionEntry.TransactionDate,
                UpdateDate = transactionEntry.UpdatedDate,
                UpdateReason = transactionEntry.UpdateReason,
                TransactionType = transactionEntry.TransactionType.Type,
                TransactionAmount = totalDrAmount,
                Particulars = transactionEntry.Particulars,
                CreatedBy = transactionEntry.CreatedById,
                CreatedByName = GetUserNameById(transactionEntry.CreatedById).Item1,
                CreatedByDesignation = GetUserNameById(transactionEntry.CreatedById).Item2,
                CreatedDate = transactionEntry.CreatedDate,
                VerifiedBy = transactionEntry.VerifiedById,
                VerifiedByName = transactionEntry.VerifiedById != null ? GetUserNameById(transactionEntry.VerifiedById).Item1 : "",
                VerifiedByDesignation = transactionEntry.VerifiedById != null ? GetUserNameById(transactionEntry.VerifiedById).Item2 : "",
                VerifiedDate = transactionEntry.VerifiedDate,
                ApprovedBy = transactionEntry.AcceptedById,
                ApprovedDate = transactionEntry.AcceptedDate,
                VoucherDetail = transactionDetail
            };

            return vocherVm;
        }

        public List<VoucherVm> GetTransactionHistoryByTransactionNo(int companyId, string tranNo)
        {
            var transactionHistoryList = new List<VoucherVm>();

            var currenttransaction = GetVoucherDetail(tranNo);
            transactionHistoryList.Add(currenttransaction);

            var transactionEntryHistory = _ppsDbContext.TransactionEntryHistory.Where(x => x.TransactionNumber == tranNo).OrderByDescending(o => o.UpdatedDate).ToList();
            if (transactionEntryHistory.Count == 0)
            {
                throw new Exception($"This transaction no: {tranNo} doesn't exist.");
            }

            transactionEntryHistory.ToList().ForEach(tran =>
            {
                var transactionHistoryDetail = tran.TransactionDetailHistory.
                    Select(x => new VoucherDetailVm
                    {
                        AccountHeadName = x.AccountHead.AccountHeadName,
                        DrAmount = x.DrAmount,
                        CrAmount = x.CrAmount,
                        Note = x.Note
                    }).
                    ToList();

                var totalDrAmount = transactionHistoryDetail.Sum(x => x.DrAmount);
                var totalCrAmount = transactionHistoryDetail.Sum(x => x.CrAmount);

                var vocherVm = new VoucherVm
                {
                    TransactionNo = tran.TransactionNumber,
                    TransactionDate = tran.TransactionDate,
                    UpdateDate = tran.UpdatedDate,
                    UpdateReason = tran.UpdateReason,
                    TransactionType = tran.TransactionType.Type,
                    TransactionAmount = totalDrAmount,
                    Particulars = tran.Particulars,
                    CreatedByName = GetUserNameById(tran.CreatedById).Item1,
                    CreatedByDesignation = GetUserNameById(tran.CreatedById).Item2,
                    CreatedDate = tran.CreatedDate,
                    VerifiedByName = GetUserNameById(tran.VerifiedById).Item1,
                    VerifiedByDesignation = GetUserNameById(tran.VerifiedById).Item2,
                    VerifiedDate = tran.VerifiedDate,
                    ApprovedDate = tran.AcceptedDate,
                    VoucherDetail = transactionHistoryDetail
                };

                transactionHistoryList.Add(vocherVm);
            });

            return transactionHistoryList;
        }
        public List<ProductDeliveryListVm> GetProductReportList(DatePickerVm datePickerVm)
        {
            var data = new List<DemandOrderDetail>();
            var productDeliverydetails = new List<ProductDeliveryListVm>();

            if (datePickerVm.StartDate != null && datePickerVm.EndDate != null)
            {
                data = _ppsDbContext.DemandOrderDetail.Where(m => m.DemandOrder.CreatedOn >= datePickerVm.StartDate && m.DemandOrder.CreatedOn <= datePickerVm.EndDate).ToList();
            }
            foreach (var item in data.GroupBy(m => m.ProductId).ToList())
            {

                ProductDeliveryListVm model = new ProductDeliveryListVm
                {
                    ProductId = item.FirstOrDefault().ProductId,
                    Name = item.FirstOrDefault().Product.Name,
                    Color=item.FirstOrDefault().Product.Color,
                    Length=item.FirstOrDefault().Product.Length,
                    Thickness= item.FirstOrDefault().Product.Thickness,
                    Code = item.FirstOrDefault().Product.Code,
                    DemandOrderQuantity = item.Sum(m => m.Quantity)

                };

                var demandOrderList = item.Select(m => m.DemandOrderId).ToList();
                var invoiceList = demandOrderList.Count()>0? _ppsDbContext.Invoice.Where(m => demandOrderList.Contains(m.DemandOrderId)).Select(m => m.Id).ToList(): demandOrderList;
                var invoiceDetailList =_ppsDbContext.InvoiceDetail.Where(n => invoiceList.Contains(n.InvoiceId)).ToList();
                var productAmount = invoiceDetailList.Where(n => n.ProductId == item.Key).Any() ? invoiceDetailList.Where(n => n.ProductId == item.Key).ToList().Sum(m => m.TotalAmount) : 0;
                model.InvoiceQuantity = invoiceDetailList.Where(n => n.ProductId == item.Key).Any()? invoiceDetailList.Where(n => n.ProductId == item.Key).ToList().Sum(m => m.Quantity):0;
                model.DeliveryQuantity = invoiceDetailList.Where(n => n.ProductId == item.Key).Any() ? invoiceDetailList.Where(n => n.ProductId == item.Key).ToList().Sum(m => m.Quantity) : 0;
                model.Ammount =(double) productAmount;
                productDeliverydetails.Add(model);
            }

            return productDeliverydetails;
        }

        public IQueryable<DemandOrder> DealerAuditReport(DatePickerVm datePickerVm)
        {
            return _ppsDbContext.DemandOrder.Where(m => m.CreatedOn >= datePickerVm.StartDate && m.CreatedOn <= datePickerVm.EndDate);
        }
        private Tuple<string, string> GetUserNameById(int? userId)
        {
            var userName = "";
            var designation = "";
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                userName = StringExtension.ToFullName(user.FirstName, user.LastName);
                designation = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId)?.Designation?
                    .DesignationName;
            }
            var tuple = Tuple.Create(userName, designation);

            return tuple;
        }
    }
}