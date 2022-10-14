using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Company;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;

namespace PPS.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService()
        {
            _companyRepository = new CompanyRepository();
        }

        public CompanyVm GetCompanyById(int companyId)
        {
            return _companyRepository.GetCompanyById(companyId);
        }

        public List<CompanyVm> GetCompanyList()
        {
            return _companyRepository.GetCompanyList();
        }
    }
}