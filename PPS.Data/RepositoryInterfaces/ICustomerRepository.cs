using PPS.API.Shared.ViewModel.Sales;
using PPS.API.Shared.ViewModel.User;
using PPS.Data.Dtos;
using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        List<CustomerModel> GetCustomerList();
        CustomerModel GetCustomerById(int customerId);
        Task<double> GetAvailableAmountByCustomer(int customerId, Customer customer = null);
        Task<bool> SaveCustomerTransaction(int userId, CustomerTransactionVm customerTransactionVm);
        CustomerModel SaveCustomer(CustomerModel customerModel);
        List<CustomerTransactionVm> GetUnapprovedCustomerTransaction();
        bool ApproveCustomerTransaction(UserVm userVm, int fiscalYear, CustomerTransactionVm customerTransactionVm);
        Task<bool> VerifyCustomer(CustomerModel customerModel);
        Task<bool> DeactivateCustomerAsync(CustomerModel customerModel);
        Task<bool> ActivateCustomerAsync(CustomerModel customerModel);
        List<CustomerModel> GetPendingDeactivatedCustomer();
        IList<CustomerTypeModel> GetCustomerType();
        IList<AttachmentTypeVm> GetAttachmentType();
        CustomerModel UpdateCustomer(CustomerModel customerModel);
        List<CustomerTransactionVm> GetCustomerTransactionList();
        List<CustomerTransactionVm> GetCustomerTransactionListForFiltering(FilterVm filterVm);
        List<CustomerTransactionVm> GetUnapprovedCustomerTransactionForFiltering(FilterVm filterVm);
        List<CustomerTransactionVm> CustomerTransactionSearch(DateTime startDate, DateTime endDate);
        CustomerTransaction GetCustomerTransactionByTransactionId(int id);
        CustomerTransaction UpdateCustomerTransaction(CustomerTransaction customerTransaction);
    }
}