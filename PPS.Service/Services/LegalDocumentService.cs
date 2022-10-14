using PPS.API.Shared.ViewModel.Document;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.Services
{
    public class LegalDocumentService:ILegalDocumentInterface
    {
        private ILegalDocumentRepository _legalDocumentRepository;
        private  PPSDbContext _ppsDbContext;
        

        public LegalDocumentService()
        {
            _legalDocumentRepository = new LegalDocumentRepository();
            _ppsDbContext = new PPSDbContext();
        }

        public List<LegalDocumentVm> GetLegalDocumentList()
        {
            return _legalDocumentRepository.GetLegalDocumentList();
        }
        public List<DocumentType> GetLegalDocumentType()
        {
            return _legalDocumentRepository.GetLegalDocumentType();
        }
        public List<DocumentRenewalCategory> GetRenewalCategory()
        {
            return _legalDocumentRepository.GetRenewalCategory();
        }
        public List<DocumentStatus>GetLeglDocumentStatus()
        {
            return _legalDocumentRepository.GetLeglDocumentStatus();
        }
        public LegalDocument SaveLegalDocument(LegalDocumentVm legalDocument)
        {
            LegalDocument legDocModel = new LegalDocument
            {
                CompanyId = legalDocument.CompanyId,
                DocumentTypeId = legalDocument.DocumentTypeId,
                IssueDate = legalDocument.IssueDate,
                DocumentNumber = legalDocument.DocumentNumber,
                ExpireDate = legalDocument.ExpireDate,
                DocumentRenewCategoryId = legalDocument.DocumentRenewCategoryId,
                DocumentStatusId = legalDocument.DocumentStatusId,
                IsActive = legalDocument.IsActive,
                OrganizationName = legalDocument.OrganizationName,
                OrganizationAddress = legalDocument.OrganizationAddress,
                OrganizationContactEmail = legalDocument.OrganizationContactEmail,
                OrganizationContactName = legalDocument.OrganizationContactName,
                OrganizationPhoneNumber = legalDocument.OrganizationPhoneNumber,
                CreatedBy = Convert.ToInt32(legalDocument.CreatedBy),
                CreatedOn = legalDocument.CreatedOn
            };
            var legalDoc = _legalDocumentRepository.SaveLegalDocument(legDocModel);
            return legalDoc;
        }

        public LegalDocumentVm GetLegalDocument(int id)
        {
            var legalDoc = _legalDocumentRepository.GetLegalDocument(id);
            return legalDoc;
        }
        public LegalDocument UpdateLegalDocument(LegalDocumentVm legalDocumentVm)
        {
            
            var ldUpdate = _ppsDbContext.LegalDocument.FirstOrDefault(m => m.Id==legalDocumentVm.Id);
            if (ldUpdate == null)
            {
                throw new KeyNotFoundException($"Legal Document Id: {legalDocumentVm.Id} does not exist.");
            }           
            var legalDoc = new LegalDocument
            {
                Id=ldUpdate.Id,
                CompanyId = legalDocumentVm.CompanyId,
                DocumentTypeId = legalDocumentVm.DocumentTypeId,
                IssueDate = legalDocumentVm.IssueDate,
                DocumentNumber = legalDocumentVm.DocumentNumber,
                ExpireDate = legalDocumentVm.ExpireDate,
                DocumentRenewCategoryId = legalDocumentVm.DocumentRenewCategoryId,
                DocumentStatusId = legalDocumentVm.DocumentStatusId,
                IsActive = legalDocumentVm.IsActive,
                OrganizationName = legalDocumentVm.OrganizationName,
                OrganizationAddress = legalDocumentVm.OrganizationAddress,
                OrganizationContactEmail = legalDocumentVm.OrganizationContactEmail,
                OrganizationContactName = legalDocumentVm.OrganizationContactName,
                OrganizationPhoneNumber = legalDocumentVm.OrganizationPhoneNumber,
                CreatedBy = ldUpdate.CreatedBy,
                CreatedOn=ldUpdate.CreatedOn,
                UpdatedBy =Convert.ToInt32(legalDocumentVm.UpdatedBy),
                UpdatedOn = legalDocumentVm.UpdatedOn
            };
            LegalDocumentHistory legDocHistory = new LegalDocumentHistory
            {
                LegalDocumentId=ldUpdate.Id,
                CompanyId = ldUpdate.CompanyId,
                DocumentTypeId = ldUpdate.DocumentTypeId,
                IssueDate = ldUpdate.IssueDate,
                DocumentNumber = ldUpdate.DocumentNumber,
                ExpireDate = ldUpdate.ExpireDate,
                DocumentRenewCategoryId = ldUpdate.DocumentRenewCategoryId,
                DocumentStatusId = ldUpdate.DocumentStatusId,
                IsActive = ldUpdate.IsActive,
                OrganizationName = ldUpdate.OrganizationName,
                OrganizationAddress = ldUpdate.OrganizationAddress,
                OrganizationContactEmail = ldUpdate.OrganizationContactEmail,
                OrganizationContactName = ldUpdate.OrganizationContactName,
                OrganizationPhoneNumber = ldUpdate.OrganizationPhoneNumber,
                CreatedBy = ldUpdate.CreatedBy,
                CreatedOn = ldUpdate.CreatedOn,
                UpdatedBy = ldUpdate.UpdatedBy,
                UpdatedOn = ldUpdate.UpdatedOn
            };
           
            var legalDocUpdate = _legalDocumentRepository.UpdateLegalDocument(legalDoc, legDocHistory);
            return legalDocUpdate;
           
        }

        public List<LegalDocumentVm> LegalDocumentListPrint()
        {
            return _legalDocumentRepository.LegalDocumentListPrint();
        }

        public LegalDocumentVm LegalDocumentSinglePrint(int Id)
        {
            return  _legalDocumentRepository.LegalDocumentSinglePrint(Id);
        }
        public List<LegalDocumentVm> LegalDocumentHistory(int Id)
        {
            return _legalDocumentRepository.LegalDocumentHistory(Id);
        }
        public LegalDocumentVm GetLegalDocById(int Id)
        {
            return _legalDocumentRepository.GetLegalDocById(Id);
        }

    }
}