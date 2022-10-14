using AutoMapper;
using PPS.API.Shared.Enums;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PPS.Operations.Service
{
    public class SystemWarningService : ISystemWarningService
    {
        private readonly ISystemWarningRepository _systemWarningRepository;
        private ITransactionRepository _transactionRepository;
        public SystemWarningService()
        {
            _systemWarningRepository = new SystemWarningRepository();
            _transactionRepository = new TransactionRepository();
        }
        // Check Warning
        public bool CheckSystemWarning(int fiscalYear, int companyId, int userId)
        {
            var warningTypeList = _systemWarningRepository.GetSystemWarningType();
            foreach (var w in warningTypeList)
            {
                var transactions = _transactionRepository.GetTransactionEntry();
                var systemWarningTypeName = w.SystemWarningTypeName;
                if(systemWarningTypeName == SystemWarningTypeEnum.CashInHand.ToString() 
                    || systemWarningTypeName == SystemWarningTypeEnum.CashAtBank.ToString())
                { 
                    var accountSubHeadList = _transactionRepository.GetAccountHead().Where(x => x.AccountSubHeadId == w.EntityId);
                    foreach (var head in accountSubHeadList)
                    {
                        var tDetail = transactions.Where(x => x.FiscalYear == fiscalYear
                            && x.CompanyId == companyId)
                            .SelectMany(d => d.TransactionDetail.Where(td => td.AccountHeadId == head.Id));
                        GenerateWarning(tDetail, userId, w, true);                  
                        
                    }
                }
            }
            RemoveResolvedWarning();
            SentOutEmailForDangerWarning();
            return true;
        }

        private void GenerateWarning(IQueryable<TransactionDetail> query, int userId, SystemWarningType systemWarningType, bool drAmountShouldGreaterThenCrAmount)
        {
            var tDetail = query.ToList();
            var drAmount = tDetail.Sum(x => x.DrAmount);
            var crAmount = tDetail.Sum(x => x.CrAmount);
            if ((drAmountShouldGreaterThenCrAmount == true && drAmount < crAmount)
                || (drAmountShouldGreaterThenCrAmount == false && drAmount > crAmount))
            {
                var unresolvedWarning = _systemWarningRepository.GetSystemWarning().FirstOrDefault(x => x.EntityId == systemWarningType.EntityId && x.Active == true);
                if(unresolvedWarning != null)
                {
                    // Create SystemWarningHistory
                    var systemWarningHistory = Mapper.Map<SystemWarningHistory>(unresolvedWarning);
                    systemWarningHistory.Id = 0;
                    systemWarningHistory.SystemWarningId = unresolvedWarning.Id;
                    _systemWarningRepository.SaveSystemWarningHistory(systemWarningHistory);

                    // Update warning
                    unresolvedWarning.WarningDays = unresolvedWarning.WarningDays + 1;
                    unresolvedWarning.UpdatedBy = userId;
                    unresolvedWarning.UpdatedOn = DateTime.Now;
                    _systemWarningRepository.UpdateSystemWarning(unresolvedWarning);
                }
                else
                {
                    // Create warning 
                    var newSystemWarning = new SystemWarning
                    {
                        Active = true,
                        EntityId = systemWarningType.EntityId,
                        SystemWarningTypeId = systemWarningType.Id,
                        WarningDays = 1,
                        Message = tDetail.FirstOrDefault()?.AccountHead?.AccountHeadName?.ToString() + " has negative balance",
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    };
                    _systemWarningRepository.SaveSystemWarning(newSystemWarning);
                }                
            }            
        }

        // Remove resolved warnings
        private void RemoveResolvedWarning()
        {
            var resolvedWarning = _systemWarningRepository.GetSystemWarning()
                .Where(x => x.Active == true && DbFunctions.TruncateTime(x.UpdatedOn) != DateTime.Now.Date)
                .ToList();
            resolvedWarning.Select(x => x.Active == false);
            _systemWarningRepository.UpdateBulkSystemWarning(resolvedWarning);
        }

        // Sent out email
        private void SentOutEmailForDangerWarning()
        {
            var dangerWarning = _systemWarningRepository.GetSystemWarning()
                .Where(x => x.Active == true && x.WarningDays >= x.SystemWarningType.WarningPeriod)
                .ToList();
            var darngerMessageList = new List<string>();
            dangerWarning.ForEach(x => {
                darngerMessageList.Add("");
            });
        }
    }
}
