using PPS.API.Shared.ViewModel.Document;
using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ILegalDocumentRepository
    {
        List<LegalDocumentVm>GetLegalDocumentList();
        List<DocumentType> GetLegalDocumentType();
        List<DocumentRenewalCategory> GetRenewalCategory();
        List<DocumentStatus> GetLeglDocumentStatus();
        LegalDocument SaveLegalDocument(LegalDocument legalDocument); 
        LegalDocumentVm GetLegalDocument(int id);
        LegalDocument UpdateLegalDocument(LegalDocument legDocUpdate, LegalDocumentHistory legDocHistory);
        List<LegalDocumentVm> LegalDocumentListPrint();
        LegalDocumentVm LegalDocumentSinglePrint(int id);
        List<LegalDocumentVm>LegalDocumentHistory(int id);
        LegalDocumentVm GetLegalDocById(int id);


    }
}