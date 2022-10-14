using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Company;
using PPS.Data.Edmx;
using System.Collections.Concurrent;

namespace PPS.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private PPSDbContext _ppsDbContext;
        public CompanyRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public CompanyVm GetCompanyById(int companyId)
        {
            var company = _ppsDbContext.Company.FirstOrDefault(x => x.Id == companyId);
            if (company == null)
            {
                return null;
            }
            var companyVm = new CompanyVm
            {
                Id = company.Id,
                Name = company.Name,
                FullName = company.FullName,
                ContactPerson = company.ContactPerson,
                ContactNumber = company.ContactNumber,
                Address = company.Address,
                Phone = company.Phone,
                Fax = company.Fax,
                LogoPath = company.LogoPath,
                Email = company.Email,
                GroupId = company.GroupId,
                AllowedInvalid = company.AllowedInvalid
            };
            return companyVm;
        }

        public List<CompanyVm> GetCompanyList()
        {
            var companyList = new ConcurrentBag<CompanyVm>();
            _ppsDbContext.Company.ToList()
                .ForEach(x =>
                {
                    companyList.Add(new CompanyVm { Id = x.Id, Name = x.FullName });
                });
            return companyList.ToList();
        }
    }
}