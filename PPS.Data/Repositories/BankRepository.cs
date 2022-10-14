using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Sales;
using System.Collections.Concurrent;
using PPS.API.Shared.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Bank;

namespace PPS.Data.Repositories
{
    public class BankRepository : IBankRepository
    {
        private PPSDbContext _ppsDbContext;

        public BankRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public List<BankVm> GetBankList()
        {
            var banks = _ppsDbContext.BankInfo.Where(x => x.Active == true).ToList();
            var bankListVm = banks.Select(x =>
                new BankVm
                {
                    Id = x.Id,
                    BankName = x.BankName,
                    BankBranch = x.BranchName
                }
            );
            return bankListVm.ToList();
        }



        #region Private members
        private string GetUserByOn(int userId, DateTime on)
        {
            var text = string.Empty;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            text += " - " + on.ToString("dd/MM/yyyy");
            return text;
        }
        private string GetUserName(int userId)
        {
            var text = string.Empty;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            return text;
        }        
        #endregion
    }
}
