using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PPS.API.Shared.ViewModel.Transaction;
using PPS.Data.Dtos.Transaction;
using PPS.Data.Edmx;
using PPS.API.Shared.Extensions;
using System.Data.Entity;
using PPS.API.Shared.Enums;

namespace PPS.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PPSDbContext _ppsDbContext;

        public TransactionRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public List<TransactionDto> GetTransactionList(int fiscalYear, int companyId, int tranTypeId)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.FiscalYear == fiscalYear
                         && entry.CompanyId == companyId
                         && entry.TransactionTypeId == tranTypeId
                         && entry.Active == true
                         orderby entry.TransactionDate descending
                         select entry).ToList();

            var tranDtos = new List<TransactionDto>();
            foreach (var tran in trans)
            {
                var hasHistory =
                    _ppsDbContext.TransactionEntryHistory.Where(x => x.TransactionEntryId == tran.Id).ToList().Count > 0;
                var tranDto = new TransactionDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    UpdateReason = tran.UpdateReason,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    VerifiedById = tran.VerifiedById,
                    VerifiedDate = tran.VerifiedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Status = tran.Accepted == false ? tran.VerifiedById == null ? "Pending" : "Verified" : "Approved",
                    HasHistory = hasHistory
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var verifiedId = tran.VerifiedById;
                if (verifiedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == verifiedId);
                    if (user != null)
                    {
                        tranDto.VerifiedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                if (user != null)
                {
                    tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                tranDto.TransactionDetail = new List<TransactionDetailDto>();
                var totalAmount = 0.0;
                if (tran.TransactionDetail.Count > 0)
                {
                    foreach (var d in tran.TransactionDetail)
                    {
                        tranDto.TransactionDetail.Add(new TransactionDetailDto
                        {
                            TranHead = d.AccountHead?.AccountHeadName,
                            TranHeadId = d.AccountHeadId,
                            DrAmount = d.DrAmount,
                            CrAmount = d.CrAmount,
                            Note = d.Note
                        });
                        totalAmount += d.DrAmount;
                    }
                }
                tranDto.TranAmount = totalAmount;

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        public List<TransactionRejectReasonTypeDto> GetTransactionRejectReasonType()
        {
            var rejectReasonType = _ppsDbContext.TransactionRejectReasonType.ToList();
            var types = new List<TransactionRejectReasonTypeDto>();
            if (rejectReasonType.Count > 0)
            {
                rejectReasonType.ForEach(reason =>
                {
                    types.Add(new TransactionRejectReasonTypeDto { Id = reason.Id, ReasonText = reason.ReasonText });
                });
            }
            return types;
        }

        public List<TransactionListDto> GetUnapprovedAccountsTransaction(int fiscalYear, int companyId)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.FiscalYear == fiscalYear
                               && entry.CompanyId == companyId
                               && entry.Active == true
                               && entry.Accepted == false
                               && entry.RejectedById == null
                               && (entry.TransactionTypeId == (int)TransactionTypeEnum.Payment
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Receipt
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Journal
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Contra)
                         orderby entry.TransactionDate descending, entry.TransactionNumber descending
                         select entry).
                         ToList();

            var tranDtos = new List<TransactionListDto>();
            foreach (var tran in trans)
            {
                var hasHistory =
                    _ppsDbContext.TransactionEntryHistory.Where(x => x.TransactionEntryId == tran.Id).ToList().Count > 0;
                var tranDto = new TransactionListDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    TranType = tran.TransactionType.Type,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    VerifiedById = tran.VerifiedById,
                    VerifiedByName = null,
                    VerifiedDate = tran.VerifiedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Status = tran.VerifiedById == null ? "Pending" : "Verified",
                    HasHistory = hasHistory
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var verifiedById = tran.VerifiedById;
                if (verifiedById != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == verifiedById);
                    if (user != null)
                    {
                        tranDto.VerifiedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                if (acceptedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                    if (user != null)
                    {
                        tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }
                if (tran.TransactionDetail.Count > 0)
                {
                    tranDto.TranAmount = tran.TransactionDetail.Sum(x => x.DrAmount);
                }

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        public List<TransactionListDto> GetUnapprovedSalesTransaction(int fiscalYear, int companyId)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.FiscalYear == fiscalYear
                               && entry.CompanyId == companyId
                               && entry.Active == true
                               && entry.Accepted == false
                               && entry.RejectedById == null
                               && entry.TransactionTypeId == (int)TransactionTypeEnum.Sales
                         orderby entry.TransactionDate descending, entry.TransactionNumber descending
                         select entry).ToList();

            var tranDtos = new List<TransactionListDto>();
            foreach (var tran in trans)
            {
                var tranDto = new TransactionListDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    TranType = tran.TransactionType.Type,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                if (acceptedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                    if (user != null)
                    {
                        tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }
                if (tran.TransactionDetail.Count > 0)
                {
                    tranDto.TranAmount = tran.TransactionDetail.Sum(x => x.DrAmount);
                }

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        public List<TransactionListDto> GetUnapprovedPurchaseTransaction(int fiscalYear, int companyId)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.FiscalYear == fiscalYear
                               && entry.CompanyId == companyId
                               && entry.Active == true
                               && entry.Accepted == false
                               && entry.RejectedById == null
                               && entry.TransactionTypeId == (int)TransactionTypeEnum.Purchase
                         orderby entry.TransactionDate descending, entry.TransactionNumber descending
                         select entry).ToList();

            var tranDtos = new List<TransactionListDto>();
            foreach (var tran in trans)
            {
                var tranDto = new TransactionListDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    TranType = tran.TransactionType.Type,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                if (acceptedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                    if (user != null)
                    {
                        tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }
                if (tran.TransactionDetail.Count > 0)
                {
                    tranDto.TranAmount = tran.TransactionDetail.Sum(x => x.DrAmount);
                }

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        public TransactionModel SaveTransaction(TransactionModel transactionEntry)
        {
            if (transactionEntry.TranTypeId == (int)TransactionTypeEnum.Payment ||
                transactionEntry.TranTypeId == (int)TransactionTypeEnum.Receipt)
            {
                var bankAccountSubHeadId = -1;
                var bankAccountSubHead = _ppsDbContext.ReferenceTable.FirstOrDefault(x =>
                    x.ReferenceName == ReferenceTableEnum.BankAccountSubHeadId.ToString())?.ReferenceValue;
                if (bankAccountSubHead != null)
                {
                    bankAccountSubHeadId = int.Parse(bankAccountSubHead);
                }
                var cashAccountSubHeadId = -1;
                var cashAccountSubHead = _ppsDbContext.ReferenceTable.FirstOrDefault(x =>
                    x.ReferenceName == ReferenceTableEnum.CashAccountSubHeadId.ToString())?.ReferenceValue;
                if (cashAccountSubHead != null)
                {
                    cashAccountSubHeadId = int.Parse(cashAccountSubHead);
                }
                var tranAccountHeadIdList = transactionEntry.TransactionDetail.Select(x => x.TranHeadId).ToList();

                var checker = _ppsDbContext.AccountHead.FirstOrDefault(x =>
                    (x.AccountSubHeadId == bankAccountSubHeadId || x.AccountSubHeadId == cashAccountSubHeadId) &&
                    tranAccountHeadIdList.Contains(x.Id));
                if (checker == null)
                {
                    throw new Exception("Please add cash or bank account head to your transaction.");
                }
            }

            var tranEntry = new TransactionEntry
            {
                TransactionNumber = transactionEntry.TranNo,
                TransactionDate = transactionEntry.TranDate,
                FiscalYear = transactionEntry.FiscalYear,
                TransactionTypeId = transactionEntry.TranTypeId,
                CompanyId = transactionEntry.CompanyId,
                PostingDate = transactionEntry.PostingDate,
                Particulars = string.IsNullOrEmpty(transactionEntry.Particulars) ? "" : transactionEntry.Particulars.Trim(),
                Accepted = transactionEntry.Accepted,
                AcceptedById = transactionEntry.AcceptedById,
                AcceptedDate = transactionEntry.AcceptedDate,
                Deleted = false,
                UpdateReason = null,
                Active = true,
                CreatedById = transactionEntry.CreatedById,
                CreatedDate = transactionEntry.CreatedDate,
                UpdatedById = transactionEntry.UpdatedById,
                UpdatedDate = transactionEntry.UpdatedDate,
                TransactionDetail = new List<TransactionDetail>()
            };
            foreach (var tran in transactionEntry.TransactionDetail)
            {
                tranEntry.TransactionDetail.Add(
                    new TransactionDetail
                    {
                        AccountHeadId = tran.TranHeadId,
                        CrAmount = tran.CrAmount,
                        DrAmount = tran.DrAmount,
                        Note = string.IsNullOrEmpty(tran.Note) ? "" : tran.Note.Trim()
                    });
            }
            var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == transactionEntry.TranTypeId
                            && x.FiscalYear == transactionEntry.FiscalYear
                            && x.CompanyId == transactionEntry.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

            var lastNumber = 0;
            if (lastTran != null)
            {
                lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
            }
            tranEntry.TransactionNumber = CreateTransactionNumber(transactionEntry.TranTypeId, tranEntry.TransactionDate, lastNumber + 1);

            _ppsDbContext.TransactionEntry.Add(tranEntry);
            _ppsDbContext.SaveChanges();

            transactionEntry.TranNo = tranEntry.TransactionNumber;

            return transactionEntry;
        }

        public TransactionModel UpdateTransaction(TransactionModel transactionModel)
        {
            var tranEntry = _ppsDbContext.TransactionEntry.FirstOrDefault(x => x.Id == transactionModel.TranId);
            if (tranEntry == null)
            {
                throw new KeyNotFoundException($"Transaction Id {transactionModel.TranId} does not exist.");
            }

            if (tranEntry.VerifiedById != null)
            {
                throw new Exception($"This Transaction Id: {transactionModel.TranId} has already been verified.");
            }

            if (tranEntry.Accepted == true)
            {
                throw new Exception($"This Transaction Id: {transactionModel.TranId} has already been approved.");
            }

            // Add to history entry and detail
            var tranHistory = new TransactionEntryHistory
            {
                TransactionEntryId = tranEntry.Id,
                TransactionNumber = tranEntry.TransactionNumber,
                TransactionTypeId = tranEntry.TransactionTypeId,
                CompanyId = tranEntry.CompanyId,
                PostingDate = tranEntry.PostingDate,
                Accepted = tranEntry.Accepted,
                AcceptedById = tranEntry.AcceptedById,
                AcceptedDate = tranEntry.AcceptedDate,
                Deleted = tranEntry.Deleted,
                Active = tranEntry.Active,
                CreatedById = tranEntry.CreatedById,
                CreatedDate = tranEntry.CreatedDate,
                IsSystemGenerated = tranEntry.IsSystemGenerated,
                RejectedById = tranEntry.RejectedById,
                RejectedDate = tranEntry.RejectedDate,
                RejectedReasonTypeId = tranEntry.RejectedReasonTypeId,
                FiscalYear = tranEntry.FiscalYear,
                Particulars = string.IsNullOrEmpty(tranEntry.Particulars) ? "" : tranEntry.Particulars.Trim(),
                TransactionDate = tranEntry.TransactionDate,
                UpdatedById = tranEntry.UpdatedById,
                UpdatedDate = tranEntry.UpdatedDate,
                UpdateReason = tranEntry.UpdateReason,
                TransactionDetailHistory = new List<TransactionDetailHistory>()
            };
            foreach (var td in tranEntry.TransactionDetail)
            {
                tranHistory.TransactionDetailHistory.Add(
                    new TransactionDetailHistory
                    {
                        TransactionDetailId = td.Id,
                        AccountHeadId = td.AccountHeadId,
                        CrAmount = td.CrAmount,
                        DrAmount = td.DrAmount,
                        Note = string.IsNullOrEmpty(td.Note) ? "" : td.Note.Trim()
                    });
            }
            _ppsDbContext.TransactionEntryHistory.Add(tranHistory);

            // Update entry and detail
            tranEntry.TransactionDate = transactionModel.TranDate;
            tranEntry.Particulars = string.IsNullOrEmpty(transactionModel.Particulars) ? "" : transactionModel.Particulars.Trim();
            tranEntry.UpdateReason = transactionModel.UpdateReason;
            tranEntry.UpdatedById = transactionModel.UpdatedById;
            tranEntry.UpdatedDate = transactionModel.UpdatedDate;

            _ppsDbContext.TransactionDetail.RemoveRange(tranEntry.TransactionDetail);
            tranEntry.TransactionDetail = new List<TransactionDetail>();
            foreach (var t in transactionModel.TransactionDetail)
            {
                tranEntry.TransactionDetail.Add(
                    new TransactionDetail
                    {
                        AccountHeadId = t.TranHeadId,
                        CrAmount = t.CrAmount,
                        DrAmount = t.DrAmount,
                        Note = t.Note
                    });
            }

            _ppsDbContext.TransactionEntry.Attach(tranEntry);
            _ppsDbContext.Entry(tranEntry).State = EntityState.Modified;

            _ppsDbContext.SaveChanges();

            return transactionModel;
        }

        public string CreateTransactionNumber(int tranType, DateTime tranDate, int number)
        {
            var tranNumber = tranDate.Year.ToString("0000")
                                + tranDate.Month.ToString("00")
                                + tranDate.Day.ToString("00");
            switch (tranType)
            {
                case 1:
                    tranNumber += "P" + number.ToString("0000");
                    break;
                case 2:
                    tranNumber += "R" + number.ToString("0000");
                    break;
                case 3:
                    tranNumber += "J" + number.ToString("0000");
                    break;
                case 4:
                    tranNumber += "C" + number.ToString("0000");
                    break;
                case 5:
                    tranNumber += "S" + number.ToString("0000");
                    break;
                case 6:
                    tranNumber += "A" + number.ToString("0000");
                    break;
                case 7:
                    tranNumber += "B" + number.ToString("0000");//B for sales return
                    break;
                default:
                    tranNumber += "X" + number.ToString("0000");
                    break;
            }
            return tranNumber;
        }

        public List<TransactionListDto> GetTransactionAccountsRejectedList(int fiscalYear, int companyId)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.FiscalYear == fiscalYear
                               && entry.CompanyId == companyId
                               && entry.Active == false
                               && entry.Accepted == false
                               && entry.RejectedById != null
                               && (entry.TransactionTypeId == (int)TransactionTypeEnum.Payment
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Receipt
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Journal
                               || entry.TransactionTypeId == (int)TransactionTypeEnum.Contra)
                         orderby entry.TransactionDate descending, entry.TransactionNumber descending
                         select entry).
                         ToList();

            var tranDtos = new List<TransactionListDto>();
            foreach (var tran in trans)
            {
                var tranDto = new TransactionListDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    TranType = tran.TransactionType.Type,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    VerifiedById = tran.VerifiedById,
                    VerifiedByName = null,
                    VerifiedDate = tran.VerifiedDate,
                    RejectedById = tran.RejectedById,
                    RejectedDate = tran.RejectedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    RejectedReasonTypeName = tran.RejectedReasonType.RejectedReasonTypeName,
                    Status = tran.VerifiedById == null ? "Pending" : "Verified"
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var verifiedById = tran.VerifiedById;
                if (verifiedById != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == verifiedById);
                    if (user != null)
                    {
                        tranDto.VerifiedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var rejectedById = tran.RejectedById;
                if (rejectedById != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == rejectedById);
                    if (user != null)
                    {
                        tranDto.RejectedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                if (acceptedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                    if (user != null)
                    {
                        tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }
                if (tran.TransactionDetail.Count > 0)
                {
                    tranDto.TranAmount = tran.TransactionDetail.Sum(x => x.DrAmount);
                }

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        public List<TransactionListDto> GetTransactionByTransactionNo(int fiscalYear, int companyId, string transactionNo)
        {
            var trans = (from entry in _ppsDbContext.TransactionEntry
                         where entry.CompanyId == companyId
                               && entry.TransactionNumber.Contains(transactionNo)
                         select entry).
                         ToList();

            var tranDtos = new List<TransactionListDto>();
            foreach (var tran in trans)
            {
                var tranDto = new TransactionListDto
                {
                    TranId = tran.Id,
                    TranNo = tran.TransactionNumber,
                    TranTypeId = tran.TransactionTypeId,
                    CompanyId = tran.CompanyId,
                    FiscalYear = tran.FiscalYear,
                    TranDate = tran.TransactionDate,
                    TranType = tran.TransactionType.Type,
                    PostingDate = tran.PostingDate,
                    Active = tran.Active,
                    Particulars = tran.Particulars,
                    CreatedById = tran.CreatedById,
                    CreatedDate = tran.CreatedDate,
                    UpdatedById = tran.UpdatedById,
                    UpdatedDate = tran.UpdatedDate,
                    VerifiedById = tran.VerifiedById,
                    VerifiedByName = null,
                    VerifiedDate = tran.VerifiedDate,
                    RejectedById = tran.RejectedById,
                    RejectedDate = tran.RejectedDate,
                    Accepted = tran.Accepted,
                    AcceptedById = tran.AcceptedById,
                    AcceptedDate = tran.AcceptedDate,
                    Status = GetTransactionStatus(tran)
                };

                var createdId = tran.CreatedById;
                var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == createdId);
                if (user != null)
                {
                    tranDto.CreatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                }

                var updatedId = tran.UpdatedById;
                if (updatedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == updatedId);
                    if (user != null)
                    {
                        tranDto.UpdatedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var verifiedById = tran.VerifiedById;
                if (verifiedById != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == verifiedById);
                    if (user != null)
                    {
                        tranDto.VerifiedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var rejectedById = tran.RejectedById;
                if (rejectedById != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == rejectedById);
                    if (user != null)
                    {
                        tranDto.RejectedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }

                var acceptedId = tran.AcceptedById;
                if (acceptedId != null)
                {
                    user = _ppsDbContext.User.FirstOrDefault(x => x.Id == acceptedId);
                    if (user != null)
                    {
                        tranDto.AcceptedByName = StringExtension.ToFullName(user.FirstName, user.LastName);
                    }
                }
                if (tran.TransactionDetail.Count > 0)
                {
                    tranDto.TranAmount = tran.TransactionDetail.Sum(x => x.DrAmount);
                }

                tranDtos.Add(tranDto);
            }
            return tranDtos;
        }

        private string GetTransactionStatus(TransactionEntry transactionEntry)
        {
            var status = "Pending";

            if (transactionEntry.Accepted == true)
            {
                status = "Approved";
            }
            else if (transactionEntry.RejectedById != null)
            {
                status = "Rejected";
            }
            else if (transactionEntry.VerifiedById != null)
            {
                status = "Verified";
            }

            return status;
        }

        public IQueryable<TransactionEntry> GetTransactionEntry()
        {
            return _ppsDbContext.TransactionEntry.Where(x => x.Accepted == true);
        }

        public IQueryable<AccountSubHead> GetAccountSubHead()
        {
            return _ppsDbContext.AccountSubHead;
        }
        public IQueryable<AccountHead> GetAccountHead()
        {
            return _ppsDbContext.AccountHead;
        }
    }
}