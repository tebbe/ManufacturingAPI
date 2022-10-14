using System;
using PPS.API.Shared.ViewModel.Sales;
using PPS.API.Shared.ViewModel.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.Filter;
using PPS.Data.Edmx;

namespace PPS.Service.ServiceInterfaces
{
    public interface ICustomerInterface
    {
        List<CustomerModel> GetCustomerList();
        CustomerModel GetCustomerById(int customerId);
        Task<double> GetAvailableBalanceByCustomerId(int customerId);
        CustomerModel SaveCustomer(CustomerModel customerModel);
        Task<bool> SaveCustomerTransaction(int userId, CustomerTransactionVm customerTransactionVm);
        List<CustomerTransactionVm> GetUnapprovedCustomerTransaction();
        bool ApproveCustomerTransaction(UserVm userVm, int fiscalYear, CustomerTransactionVm customerTransactionVm);
        Task<bool> VerifyCustomerAsync(CustomerModel customerModel);
        Task<bool> DeactivateCustomerAsync(CustomerModel customerModel);
        Task<bool> ActivateCustomerAsync(CustomerModel customerModel);
        List<CustomerModel> GetPendingDeactivatedCustomerList();
        IList<CustomerTypeModel> GetCustomerType();
        IList<AttachmentTypeVm> GetAttachmentType();
        CustomerModel UpdateCustomer(CustomerModel customerModel);
        List<CustomerTransactionVm> GetCustomerTransactionList();
        List<CustomerTransactionVm> GetCustomerTransactionListForFiltering(FilterVm filterVm);
        List<CustomerTransactionVm> GetUnapprovedCustomerTransactionForFiltering(FilterVm filterVm);
        List<CustomerTransactionVm> CustomerTransactionSearch(DateTime startDate, DateTime endDate);
        CustomerTransactionVm GetCustomerTransactionByTransactionId(int id);
        CustomerTransaction UpdateCustomerTransaction(int id, CustomerTransactionVm customerTransactionVm);
    }
}