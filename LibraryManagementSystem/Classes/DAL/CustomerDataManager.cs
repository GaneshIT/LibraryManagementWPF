using LibraryManagementSystem.Interfaces.DAL;
using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Classes.DAL
{
    public class CustomerDataManager : ICustomerDataManager
    {
        Random RandomCusomterId = new Random();
        public static List<Customer> customerDataList = new List<Customer>();
        public async Task<int> CreateCustomer(Customer customer)
        {
            int custId = 0;
           
            try
            {
                customer.CustomerId = RandomCusomterId.Next();
                await Task.Run(()=> customerDataList.Add(customer));
                custId = customer.CustomerId;
            }
            catch (Exception e)
            {
                custId = 0;
            }

            return custId;
        }

        public async Task<Customer> GetCustomerDetails(uint customerId)
        {
            Customer result = null;
            
            try
            {
                result = customerDataList.Find(c => c.CustomerId == customerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            bool success = true;
            
            try
            {
                int index = customerDataList.FindIndex(c => c.CustomerId == customer.CustomerId);
                customerDataList[index].FirstName = customer.FirstName;
                customerDataList[index].LastName = customer.LastName;
                customerDataList[index].Email = customer.Email;
                customerDataList[index].DateOfBirth = customer.DateOfBirth;
                customerDataList[index].AccountCreatedOn = customer.AccountCreatedOn;
                
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<bool> DeleteCustomer(uint customerId)
        {
            bool success = true;
            

            try
            {
                int index = -1;
                index = customerDataList.FindIndex(c => c.CustomerId == customerId);
                customerDataList.RemoveAt(index);
                if (index == -1)
                    success = false;

            }
            catch (Exception e)
            {
                success = false;
                Console.WriteLine(e.Message);
            }

            return success;
        }

        
    }
}