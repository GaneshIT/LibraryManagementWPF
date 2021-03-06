using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Interfaces.DAL
{
    public interface ICustomerDataManager
    {
        Task<int> CreateCustomer(Customer customer);

        Task<Customer> GetCustomerDetails(uint customerId);

        Task<bool> UpdateCustomer(Customer customer);

        Task<bool> DeleteCustomer(uint customerId);
    }
}
