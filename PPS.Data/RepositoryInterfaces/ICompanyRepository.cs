using PPS.API.Shared.ViewModel.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ICompanyRepository
    {
        List<CompanyVm> GetCompanyList();
        CompanyVm GetCompanyById(int companyId);
    }
}
