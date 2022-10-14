using PPS.API.Shared.ViewModel.Document;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace PPS.Data.Repositories
{
    public class LegalDocumentRepository : ILegalDocumentRepository
    {
        private PPSDbContext _ppsDbContext;
        public LegalDocumentRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public List<LegalDocumentVm> GetLegalDocumentList()
        {

            var lgList = _ppsDbContext.LegalDocument
                 .Select(d => new LegalDocumentVm
                 {
                     Id = d.Id,
                     CompanyName = d.Company.Name,
                     DocumentRenewalCategoryName = d.DocumentRenewalCategory.Name,
                     DocumentStatusName = d.DocumentStatus.Name,
                     DocumentTypeName = d.DocumentType.DocumentTypeName,
                     IssueDate = d.IssueDate,
                     ExpireDate = d.ExpireDate,
                     DocumentNumber = d.DocumentNumber,
                     OrganizationName = d.OrganizationName,
                     OrganizationContactName = d.OrganizationContactName,
                     OrganizationPhoneNumber = d.OrganizationPhoneNumber,
                     OrganizationContactEmail = d.OrganizationContactEmail,
                     OrganizationAddress = d.OrganizationAddress,
                     IsActive = d.IsActive,
                     CreatedBy = d.User.FirstName + " " + d.User.LastName,
                     UpdatedBy = d.UpdatedBy != null ? d.User1.FirstName + " " + d.User1.LastName : string.Empty,
                     CreatedOn = d.CreatedOn,
                     UpdatedOn = d.UpdatedOn
                 }).ToList();

            return lgList;
        }
        public List<DocumentType> GetLegalDocumentType()
        {
            var lgList = new ConcurrentBag<DocumentType>();
            _ppsDbContext.DocumentType.ToList()
                .ForEach(x =>
                {
                    lgList.Add(new DocumentType { Id = x.Id, DocumentTypeName = x.DocumentTypeName });
                });
            return lgList.ToList();

        }
        public List<DocumentRenewalCategory> GetRenewalCategory()
        {
            var lgList = new ConcurrentBag<DocumentRenewalCategory>();
            _ppsDbContext.DocumentRenewalCategory.ToList()
                .ForEach(x =>
                {
                    lgList.Add(new DocumentRenewalCategory { Id = x.Id, Name = x.Name });
                });
            return lgList.ToList();
        }
        public List<DocumentStatus> GetLeglDocumentStatus()
        {
            var lgList = new ConcurrentBag<DocumentStatus>();
            _ppsDbContext.DocumentStatus.ToList().ForEach(x =>
            {
                lgList.Add(new DocumentStatus { Id = x.Id, Name = x.Name });
            });
            return lgList.ToList();
        }
        public LegalDocument SaveLegalDocument(LegalDocument legalDocument)
        {
            using(var db=new PPSDbContext())
            {
                db.LegalDocument.Add(legalDocument);
                db.SaveChanges();
                return legalDocument;
            }
            
        }

        public LegalDocumentVm GetLegalDocument(int id)
        {
            var legalDoc = _ppsDbContext.LegalDocument.Where(m => m.Id == id).FirstOrDefault();
            LegalDocumentVm vM = new LegalDocumentVm
            {
                Id = legalDoc.Id,
                DocumentNumber = legalDoc.DocumentNumber,
                CompanyId = legalDoc.CompanyId,
                CompanyName = legalDoc.Company.Name,
                DocumentRenewCategoryId = legalDoc.DocumentRenewCategoryId,
                DocumentRenewalCategoryName = legalDoc.DocumentRenewalCategory.Name,
                DocumentTypeId = legalDoc.DocumentTypeId,
                DocumentTypeName = legalDoc.DocumentType.DocumentTypeName,
                DocumentStatusName = legalDoc.DocumentStatus.Name,
                DocumentStatusId = legalDoc.DocumentStatusId,
                IssueDate = legalDoc.IssueDate,
                ExpireDate = legalDoc.ExpireDate,
                IsActive = legalDoc.IsActive,
                OrganizationName = legalDoc.OrganizationName,
                OrganizationAddress = legalDoc.OrganizationAddress,
                OrganizationContactEmail = legalDoc.OrganizationContactEmail,
                OrganizationContactName = legalDoc.OrganizationContactName,
                OrganizationPhoneNumber = legalDoc.OrganizationPhoneNumber,
                CreatedBy = legalDoc.CreatedBy > 0 ? legalDoc.User.FirstName + " " + legalDoc.User.LastName : string.Empty,
                CreatedOn = legalDoc.CreatedOn,
                UpdatedBy = legalDoc.UpdatedBy != null ? legalDoc.User1.FirstName + " " + legalDoc.User1.LastName : string.Empty,
                UpdatedOn = legalDoc.UpdatedOn
            };
            return vM;
        }
        public LegalDocument UpdateLegalDocument(LegalDocument legalDocument, LegalDocumentHistory legDocHistory)
        {
            using (var db = new PPSDbContext())
            {
                db.LegalDocumentHistory.Add(legDocHistory);
                db.LegalDocument.Attach(legalDocument);
                db.Entry(legalDocument).State = EntityState.Modified;
                db.SaveChanges();
                return legalDocument;
            }

        }

        public List<LegalDocumentVm> LegalDocumentListPrint()
        {
            var lgList = _ppsDbContext.LegalDocument
                .Select(d => new LegalDocumentVm
                {
                    Id = d.Id,
                    CompanyName = d.Company.Name,
                    DocumentRenewalCategoryName = d.DocumentRenewalCategory.Name,
                    DocumentStatusName = d.DocumentStatus.Name,
                    DocumentTypeName = d.DocumentType.DocumentTypeName,
                    IssueDate = d.IssueDate,
                    ExpireDate = d.ExpireDate,
                    DocumentNumber = d.DocumentNumber,
                    OrganizationName = d.OrganizationName,
                    OrganizationContactName = d.OrganizationContactName,
                    OrganizationPhoneNumber = d.OrganizationPhoneNumber,
                    OrganizationContactEmail = d.OrganizationContactEmail,
                    OrganizationAddress = d.OrganizationAddress,
                    IsActive = d.IsActive,
                    CreatedBy = d.User.FirstName + " " + d.User.LastName,
                    UpdatedBy = d.UpdatedBy != null ? d.User1.FirstName + " " + d.User1.LastName : string.Empty,
                    CreatedOn = d.CreatedOn,
                    UpdatedOn = d.UpdatedOn
                }).ToList();
            return lgList;
        }
        public LegalDocumentVm LegalDocumentSinglePrint(int Id)
        {
            var legalDoc = _ppsDbContext.LegalDocument.FirstOrDefault(m => m.Id == Id);

            if (legalDoc == null)
            {
                throw new KeyNotFoundException($"Document Id: {legalDoc.Id} does not exist.");
            }
            LegalDocumentVm vModel = new LegalDocumentVm
            {
                Id = legalDoc.Id,
                CompanyName = legalDoc.Company.Name,
                DocumentRenewalCategoryName = legalDoc.DocumentRenewalCategory.Name,
                DocumentTypeName = legalDoc.DocumentType.DocumentTypeName,
                DocumentStatusName = legalDoc.DocumentStatus.Name,
                DocumentNumber = legalDoc.DocumentNumber,
                IssueDate = legalDoc.IssueDate,
                ExpireDate = legalDoc.ExpireDate,
                OrganizationName = legalDoc.OrganizationName,
                OrganizationAddress = legalDoc.OrganizationAddress,
                OrganizationContactName = legalDoc.OrganizationContactName,
                OrganizationContactEmail = legalDoc.OrganizationContactEmail,
                OrganizationPhoneNumber = legalDoc.OrganizationPhoneNumber,
                CreatedBy = legalDoc.User.FirstName + " " + legalDoc.User.LastName,
                UpdatedBy = legalDoc.User1.FirstName + " " + legalDoc.User1.LastName,
                CreatedOn = legalDoc.CreatedOn,
                UpdatedOn = legalDoc.UpdatedOn
            };

            return vModel;
        }

        public LegalDocumentVm GetLegalDocById(int id)
        {

            var legalDoc = _ppsDbContext.LegalDocument.Where(m => m.Id == id).FirstOrDefault();
            if (legalDoc == null)
            {
                throw new KeyNotFoundException($"Document Id: {legalDoc.Id} does not exist.");
            }
            LegalDocumentVm vM = new LegalDocumentVm
            {
                Id = legalDoc.Id,
                DocumentNumber = legalDoc.DocumentNumber,
                CompanyName = legalDoc.Company.Name,
                DocumentRenewalCategoryName = legalDoc.DocumentRenewalCategory.Name,
                DocumentTypeName = legalDoc.DocumentType.DocumentTypeName,
                DocumentStatusName = legalDoc.DocumentStatus.Name,
                IssueDate = legalDoc.IssueDate,
                ExpireDate = legalDoc.ExpireDate,
                IsActive = legalDoc.IsActive,
                OrganizationName = legalDoc.OrganizationName,
                OrganizationAddress = legalDoc.OrganizationAddress,
                OrganizationContactEmail = legalDoc.OrganizationContactEmail,
                OrganizationContactName = legalDoc.OrganizationContactName,
                OrganizationPhoneNumber = legalDoc.OrganizationPhoneNumber,
                CreatedBy = legalDoc.CreatedBy > 0 ? legalDoc.User.FirstName + " " + legalDoc.User.LastName : string.Empty,
                CreatedOn = legalDoc.CreatedOn,
                UpdatedBy = legalDoc.UpdatedBy != null ? legalDoc.User1.FirstName + " " + legalDoc.User1.LastName : string.Empty,
                UpdatedOn = legalDoc.UpdatedOn
            };
            return vM;
        }
        public List<LegalDocumentVm> LegalDocumentHistory(int Id)
        {
            var historyList = new List<LegalDocumentVm>();
            var history = _ppsDbContext.LegalDocumentHistory.Where(m => m.LegalDocumentId == Id).OrderByDescending(m => m.Id).ToList();
            history.ForEach(d =>
            {
                historyList.Add(new LegalDocumentVm
                {
                    LegalDocumentId = d.LegalDocumentId,
                    DocumentNumber = d.DocumentNumber,
                    CompanyName = d.Company.Name,
                    DocumentRenewalCategoryName = d.DocumentRenewalCategory.Name,
                    DocumentTypeName = d.DocumentType.DocumentTypeName,
                    DocumentStatusName = d.DocumentStatus.Name,
                    IssueDate = d.IssueDate,
                    ExpireDate = d.ExpireDate,
                    OrganizationName = d.OrganizationName,
                    OrganizationAddress = d.OrganizationAddress,
                    OrganizationContactEmail = d.OrganizationContactEmail,
                    OrganizationContactName = d.OrganizationContactName,
                    OrganizationPhoneNumber = d.OrganizationPhoneNumber,
                    CreatedBy = d.CreatedBy > 0 ? d.User.FirstName + " " + d.User.LastName : string.Empty,
                    CreatedOn = d.CreatedOn,
                    UpdatedBy = d.UpdatedBy != null ? d.User1.FirstName + " " + history.FirstOrDefault().User1.LastName : string.Empty,
                    UpdatedOn = d.UpdatedOn
                });

            });
            return historyList;
        }


    }
}