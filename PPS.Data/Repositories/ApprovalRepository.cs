using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PPS.API.Shared.ViewModel.Account;
using PPS.Data.Edmx;
using System.Data.Entity;

namespace PPS.Data.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public ApprovalRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }
        public bool VerifyTransaction(AcceptRejectTransactionVm vm)
        {
            using (var db = new PPSDbContext())
            {
                var tran = db.TransactionEntry.FirstOrDefault(x => x.TransactionNumber == vm.TransactionNo);
                if (tran == null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} doesn't exist.");
                }
                if (tran.VerifiedById != null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has already been approved.");
                }
                if (tran.Accepted == true)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has already been approved.");
                }

                var tranHistory = new TransactionEntryHistory
                {
                    TransactionEntryId = tran.Id,
                    TransactionNumber = tran.TransactionNumber,
                    TransactionTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    PostingDate = tran.PostingDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Deleted = tran.Deleted,
                    Active = tran.Active,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    IsSystemGenerated = tran.IsSystemGenerated,
                    RejectedById = tran.RejectedById,
                    RejectedDate = tran.RejectedDate,
                    RejectedReasonTypeId = tran.RejectedReasonTypeId,
                    FiscalYear = tran.FiscalYear,
                    Particulars = tran.Particulars,
                    TransactionDate = tran.TransactionDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    UpdateReason = tran.UpdateReason,
                    VerifiedById = tran.VerifiedById,
                    VerifiedDate = tran.VerifiedDate,
                    TransactionDetailHistory = new List<TransactionDetailHistory>()
                };
                foreach (var td in tran.TransactionDetail)
                {
                    tranHistory.TransactionDetailHistory.Add(
                        new TransactionDetailHistory
                        {
                            TransactionDetailId = td.Id,
                            AccountHeadId = td.AccountHeadId,
                            CrAmount = td.CrAmount,
                            DrAmount = td.DrAmount
                        });
                }
                db.TransactionEntryHistory.Add(tranHistory);

                tran.VerifiedDate = vm.DatedOn;
                tran.VerifiedById = vm.UserId;
                db.TransactionEntry.Attach(tran);
                db.Entry(tran).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }
        public bool AcceptTransaction(AcceptRejectTransactionVm vm)
        {
            using (var db = new PPSDbContext())
            {
                var tran = db.TransactionEntry.FirstOrDefault(x => x.TransactionNumber == vm.TransactionNo);
                if (tran == null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} doesn't exist.");
                }

                if (tran.VerifiedById == null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has not been verified yet.");
                }

                if (tran.Accepted == true)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has already been approved.");
                }

                var tranHistory = new TransactionEntryHistory
                {
                    TransactionEntryId = tran.Id,
                    TransactionNumber = tran.TransactionNumber,
                    TransactionTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    PostingDate = tran.PostingDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Deleted = tran.Deleted,
                    Active = tran.Active,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    IsSystemGenerated = tran.IsSystemGenerated,
                    RejectedById = tran.RejectedById,
                    RejectedDate = tran.RejectedDate,
                    RejectedReasonTypeId = tran.RejectedReasonTypeId,
                    FiscalYear = tran.FiscalYear,
                    Particulars = tran.Particulars,
                    TransactionDate = tran.TransactionDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    UpdateReason = tran.UpdateReason,
                    VerifiedById = tran.VerifiedById,
                    VerifiedDate = tran.VerifiedDate,
                    TransactionDetailHistory = new List<TransactionDetailHistory>()
                };
                foreach (var td in tran.TransactionDetail)
                {
                    tranHistory.TransactionDetailHistory.Add(
                        new TransactionDetailHistory
                        {
                            TransactionDetailId = td.Id,
                            AccountHeadId = td.AccountHeadId,
                            CrAmount = td.CrAmount,
                            DrAmount = td.DrAmount
                        });
                }
                db.TransactionEntryHistory.Add(tranHistory);

                tran.Accepted = true;
                tran.AcceptedDate = vm.DatedOn;
                tran.AcceptedById = vm.UserId;
                db.TransactionEntry.Attach(tran);
                db.Entry(tran).State = EntityState.Modified;

                var demandOrderDiscountTransaction = db.DemandOrderDiscountTransaction.FirstOrDefault(x => x.TransactionEntryId == tran.Id);
                if (demandOrderDiscountTransaction != null)
                {
                    demandOrderDiscountTransaction.ApprovedBy = vm.UserId;
                    demandOrderDiscountTransaction.ApprovedOn = vm.DatedOn;
                    demandOrderDiscountTransaction.IsApproved = true;

                    db.DemandOrderDiscountTransaction.Attach(demandOrderDiscountTransaction);
                    db.Entry(demandOrderDiscountTransaction).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool RejectTransaction(AcceptRejectTransactionVm vm)
        {
            using ( var db = new PPSDbContext())
            {
                var tran = db.TransactionEntry.FirstOrDefault(x => x.TransactionNumber == vm.TransactionNo);
                if (tran == null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} doesn't exist.");
                }

                if (tran.Accepted == true)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has already been approved.");
                }

                if (tran.VerifiedById == null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has not been verified yet.");
                }

                if (tran.RejectedById != null)
                {
                    throw new Exception($"This transaction no. {vm.TransactionNo} has already been rejected.");
                }

                var tranHistory = new TransactionEntryHistory
                {
                    TransactionEntryId = tran.Id,
                    TransactionNumber = tran.TransactionNumber,
                    TransactionTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    PostingDate = tran.PostingDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Deleted = tran.Deleted,
                    Active = tran.Active,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    IsSystemGenerated = tran.IsSystemGenerated,
                    RejectedById = tran.RejectedById,
                    RejectedDate = tran.RejectedDate,
                    RejectedReasonTypeId = tran.RejectedReasonTypeId,
                    FiscalYear = tran.FiscalYear,
                    Particulars = tran.Particulars,
                    TransactionDate = tran.TransactionDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    UpdateReason = tran.UpdateReason,
                    VerifiedById = tran.VerifiedById,
                    VerifiedDate = tran.VerifiedDate,
                    TransactionDetailHistory = new List<TransactionDetailHistory>()
                };

                foreach (var td in tran.TransactionDetail)
                {
                    tranHistory.TransactionDetailHistory.Add(
                        new TransactionDetailHistory
                        {
                            TransactionDetailId = td.Id,
                            AccountHeadId = td.AccountHeadId,
                            CrAmount = td.CrAmount,
                            DrAmount = td.DrAmount
                        });
                }
                db.TransactionEntryHistory.Add(tranHistory);

                tran.Active = false;
                tran.Accepted = false;
                tran.RejectedDate = vm.DatedOn;
                tran.RejectedById = vm.UserId;
                tran.RejectedReasonTypeId = vm.RejectReasonTypeId;
                db.TransactionEntry.Attach(tran);
                db.Entry(tran).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }
    }
}