using PPS.API.Shared.ViewModel.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface ICompanyService
    {
        List<CompanyVm> GetCompanyList();
        CompanyVm GetCompanyById(int companyId);
    }
}
