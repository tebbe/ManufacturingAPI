using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Account;
using PPS.API.Shared.ViewModel.Sales;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.User;
using PPS.API.Shared.ViewModel.Filter;
using PPS.Data.Edmx;
using PPS.API.Shared.Enums;

namespace PPS.Service.Services
{
    public class CustomerService : ICustomerInterface
    {
        private ICustomerRepository _customerRepository;
        private PPSDbContext _ppsDbContext;
        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _ppsDbContext = new PPSDbContext();
        }

        public List<CustomerModel> GetCustomerList()
        {
            var customerList = _customerRepository.GetCustomerList();
            if (customerList == null)
                return null;
            var custlist = new List<CustomerModel>();
            foreach (var customer in customerList)
            {
                custlist.Add(customer);
            }
            return custlist;
        }

        public async Task<double> GetAvailableBalanceByCustomerId(int customerId)
        {
            return await _customerRepository.GetAvailableAmountByCustomer(customerId);
        }

        public CustomerModel GetCustomerById(int customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }

        public CustomerModel SaveCustomer(CustomerModel customerModel)
        {
            var customerDto = _customerRepository.SaveCustomer(customerModel);
            return customerDto;
        }

        public async Task<bool> SaveCustomerTransaction(int userId, CustomerTransactionVm customerTransactionVm)
        {
            return await _customerRepository.SaveCustomerTransaction(userId, customerTransactionVm);
        }

        public List<CustomerTransactionVm> GetUnapprovedCustomerTransaction()
        {
            return _customerRepository.GetUnapprovedCustomerTransaction();
        }

        public bool ApproveCustomerTransaction(UserVm userVm, int fiscalYear, CustomerTransactionVm customerTransactionVm)
        {
            return _customerRepository.ApproveCustomerTransaction(userVm, fiscalYear, customerTransactionVm);
        }

        public async Task<bool> VerifyCustomerAsync(CustomerModel customerModel)
        {
            return await _customerRepository.VerifyCustomer(customerModel);
        }

        public async Task<bool> DeactivateCustomerAsync(CustomerModel customerModel)
        {
            return await _customerRepository.DeactivateCustomerAsync(customerModel);
        }

        public async Task<bool> ActivateCustomerAsync(CustomerModel customerModel)
        {
            return await _customerRepository.ActivateCustomerAsync(customerModel);
        }

        public List<CustomerModel> GetPendingDeactivatedCustomerList()
        {
            return _customerRepository.GetPendingDeactivatedCustomer();
        }
        public IList<CustomerTypeModel> GetCustomerType()
        {
            return _customerRepository.GetCustomerType();
        }

        public IList<AttachmentTypeVm> GetAttachmentType()
        {
            return _customerRepository.GetAttachmentType();
        }

        public CustomerModel UpdateCustomer(CustomerModel customerModel)
        {
            return _customerRepository.UpdateCustomer(customerModel);
        }

        public List<CustomerTransactionVm> GetCustomerTransactionList()
        {
            return _customerRepository.GetCustomerTransactionList();
        }

        public List<CustomerTransactionVm> GetCustomerTransactionListForFiltering(FilterVm filterVm)
        {
            return _customerRepository.GetCustomerTransactionListForFiltering(filterVm);
        }

        public List<CustomerTransactionVm> GetUnapprovedCustomerTransactionForFiltering(FilterVm filterVm)
        {
            return _customerRepository.GetUnapprovedCustomerTransactionForFiltering(filterVm);
        }

        public List<CustomerTransactionVm> CustomerTransactionSearch(DateTime startDate, DateTime endDate)
        {
            return _customerRepository.CustomerTransactionSearch(startDate, endDate);
        }
        public CustomerTransactionVm GetCustomerTransactionByTransactionId(int id)
        {
            var customerTransaction = _customerRepository.GetCustomerTransactionByTransactionId(id);

            CustomerTransactionVm transaction = new CustomerTransactionVm
            {
                Id=customerTransaction.Id,
                AccountName = customerTransaction.AccountHead.AccountHeadName,
                CashBankAccountHeadId=customerTransaction.CashBankAccountHeadId,
                TransactionReference = customerTransaction.TransactionReference,
                TransactionAmount = customerTransaction.TransactionAmount,
                BankChargeAmount = customerTransaction.BankChargeAmount,
                TransactionDate=customerTransaction.TransactionDate,
                CreatedByName =_ppsDbContext.User.Where(m=>m.Id==customerTransaction.CreatedBy).Select(m=>m.FirstName +" "+m.LastName).FirstOrDefault(),
                CreatedOn = customerTransaction.CreatedOn,
                UpdatedByName = customerTransaction.UpdatedBy>0? _ppsDbContext.User.Where(m => m.Id == customerTransaction.UpdatedBy).Select(m => m.FirstName+" " + m.LastName).FirstOrDefault():null,
                UpdatedOn = customerTransaction.UpdatedBy > 0 ?customerTransaction.UpdatedOn:null,
                IsApproved=customerTransaction.IsApproved,
                CustomerTransactionDetail = customerTransaction.CustomerTransactionDetail.Select(p => new CustomerTransactionDetailVm
                {
                    CustomerName=p.Customer.CustomerName,
                    CustomerTransactionId=p.CustomerTransactionId,
                    CustomerId=p.CustomerId,
                    TransactionAmount=p.TransactionAmount,
                    BookNo=p.BookNo,
                    BookSerialNo =p.BookSerialNo,
                    
                }).ToList()
            };
            return transaction;
        }

        public CustomerTransaction UpdateCustomerTransaction(int userId, CustomerTransactionVm customerTransactionVm)
        {
            var transaction = _ppsDbContext.CustomerTransaction.Where(m => m.Id == customerTransactionVm.Id && m.IsApproved!=true).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Customer transaction not found");
            }
            //var totalCustomerTranAmount = customerTransactionVm.CustomerTransactionDetail.Sum(x => x.TransactionAmount);
            //if (customerTransactionVm.TransactionAmount + customerTransactionVm.BankChargeAmount - totalCustomerTranAmount != 0)
            //{
            //    throw new Exception("Transaction amount mismatch.");
            //}

            int? bankChargeAccountHeadId = null;
            if (customerTransactionVm.BankChargeAmount > 0)
            {
                var bankChargeAccountHead = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.BankChargeAccountHeadId.ToString()).ReferenceValue;
                if (bankChargeAccountHead == null)
                {
                    throw new Exception("Bank charge account head not found");
                }
                else
                {
                    bankChargeAccountHeadId = int.Parse(bankChargeAccountHead);
                }
            }
          
            var customerTransaction = new CustomerTransaction
            {
                Id=customerTransactionVm.Id,
                CashBankAccountHeadId = customerTransactionVm.CashBankAccountHeadId,
                TransactionReference = customerTransactionVm.TransactionReference,
                BankChargeAccountHeadId = bankChargeAccountHeadId,
                BankChargeAmount = customerTransactionVm.BankChargeAmount,
                TransactionAmount = customerTransactionVm.TransactionAmount,
                TransactionDate = customerTransactionVm.TransactionDate,
                CreatedBy = transaction.CreatedBy,
                CreatedOn = transaction.CreatedOn,
                UpdatedBy= customerTransactionVm.UpdatedBy,
                UpdatedOn= customerTransactionVm.UpdatedOn,
                IsApproved = transaction.IsApproved,
                CustomerTransactionDetail =null
            };
            if (customerTransactionVm.CustomerTransactionDetail.Count() > 0)
            {
                _ppsDbContext.CustomerTransactionDetail.RemoveRange(transaction.CustomerTransactionDetail);
                foreach (var data in customerTransactionVm.CustomerTransactionDetail)
                {
                    CustomerTransactionDetail transactionDetail=
                        new CustomerTransactionDetail
                        {
                            CustomerTransactionId = transaction.Id,
                            CustomerId = data.CustomerId,
                            BookNo = data.BookNo,
                            BookSerialNo = data.BookSerialNo,
                            TransactionAmount = data.TransactionAmount
                        };
                    _ppsDbContext.CustomerTransactionDetail.Add(transactionDetail);
                }
                    _ppsDbContext.SaveChanges();
            }
           

            var editCustomerTransaction = _customerRepository.UpdateCustomerTransaction(customerTransaction);
            return editCustomerTransaction;
        }
    }

}