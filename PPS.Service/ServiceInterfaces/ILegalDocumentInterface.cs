using PPS.API.Shared.ViewModel.Document;
using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.ServiceInterfaces
{
    public interface ILegalDocumentInterface
    {
        List<LegalDocumentVm>GetLegalDocumentList();
        List<DocumentType> GetLegalDocumentType();
        List<DocumentRenewalCategory> GetRenewalCategory();
        List<DocumentStatus> GetLeglDocumentStatus();
        LegalDocument SaveLegalDocument(LegalDocumentVm legalDocument);
        LegalDocumentVm GetLegalDocument(int id);
        LegalDocument UpdateLegalDocument(LegalDocumentVm legalDocumentVm);
        LegalDocumentVm LegalDocumentSinglePrint(int id);
        List<LegalDocumentVm> LegalDocumentListPrint();
        List<LegalDocumentVm> LegalDocumentHistory(int id);
        LegalDocumentVm GetLegalDocById(int id);


    }
}