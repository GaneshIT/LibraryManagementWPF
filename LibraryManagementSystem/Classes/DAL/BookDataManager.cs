using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Interfaces.DAL;

namespace LibraryManagementSystem.Classes.DAL
{
    public class BookDataManager : IBookDataManager
    {
        public static List<Book> bookStore = new List<Book>();
        public static List<BookTransaction> bookTransactions = new List<BookTransaction>();
        public static List<Customer> customers = new List<Customer>();
        public async Task<bool> CreateBook(Book book)
        {
            bool success = true;
            try
            {
                await Task.Run(() => bookStore.Add(book));
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<bool> UpdateBook(Book book)
        {
            bool success = true;
            
            try
            {
                int index = await Task.Run(()=> bookStore.FindIndex(b => b.BookId == book.BookId));
                bookStore[index].ISBN = book.ISBN;
                bookStore[index].PlainISBN = book.PlainISBN;
                bookStore[index].Title = book.Title;
                bookStore[index].Authors = book.Authors;
                bookStore[index].PublishingYear = book.PublishingYear;
                bookStore[index].DateAdded = book.DateAdded;
                bookStore[index].TotalCopies = book.TotalCopies;
                bookStore[index].AvailableCopies = book.AvailableCopies;
                bookStore[index].TotalCheckOuts = book.TotalCheckOuts;
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<bool> DeleteBook(uint bookId)
        {
            bool success = true;
            
            try
            {
                int index = bookStore.FindIndex(b => b.BookId == bookId);

                await Task.Run(() => bookStore.RemoveAt(index));
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<Book> GetBook(uint bookId)
        {
            Book result = null;

            try
            {
               result = bookStore.Find(b => b.BookId == bookId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<Book>> SearchBooks(string search)
        {
            IEnumerable<Book> result = null;
            
            try
            {
                result = bookStore.Where(b => b.Title.Contains(search));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public async Task<bool> CheckOutBook(BookTransaction transaction)
        {
            bool success = true;
           
            try
            {
                BookTransaction bookTransaction =(BookTransaction) bookTransactions.Where(b => b.Customer.CustomerId == transaction.Customer.CustomerId && b.Book.BookId == transaction.Book.BookId);

                if (bookTransactions.Count > 0)
                    bookTransactions.Add(bookTransaction);
                else
                    bookTransactions.Add(transaction);
                
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<bool> CheckInBook(BookTransaction transaction)
        {
            bool success = true;         
            try
            {
                int index = -1;
                index = bookTransactions.FindIndex(b => b.Customer.CustomerId == transaction.Customer.CustomerId && b.Book.BookId == transaction.Book.BookId);
                bookTransactions[index].CheckIn = transaction.CheckIn;
                if (index == -1)
                    success = false;
                else
                    bookStore.Where(b => b.BookId == transaction.Book.BookId).SetValue(b => b.AvailableCopies = b.AvailableCopies + 1);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public async Task<IEnumerable<BookTransaction>> GetBookTransactionHistory(uint bookId)
        {
            IEnumerable<BookTransaction> result = null;            

            try
            {
                var output = (from bt in bookTransactions join b in bookStore on bt.Book.BookId equals b.BookId
                             where bt.Book.BookId==bookId join c in customers 
                             on bt.Customer.CustomerId equals c.CustomerId 
                             select new
                             {
                                 bt.TransactionId,
                                 bt.CheckOut,
                                 bt.CheckIn,
                                 b.BookId,
                                 b.ISBN,
                                 b.PlainISBN,
                                 b.Title,
                                 b.Authors,
                                 b.PublishingYear,
                                 b.DateAdded,
                                 b.TotalCopies,
                                 b.AvailableCopies,
                                 c.CustomerId,
                                 c.FirstName,
                                 c.LastName,
                                 c.Email,
                                 c.DateOfBirth,
                                 c.AccountCreatedOn

                             }).ToList();
                List<BookTransaction> bookTransactionList = new List<BookTransaction>();             
                foreach (var item in output)
                {
                    BookTransaction bookTransactionObj = new BookTransaction();
                    bookTransactionObj.TransactionId = item.TransactionId;
                    bookTransactionObj.Customer.CustomerId = item.CustomerId;
                    bookTransactionObj.Book.BookId = item.BookId;
                    bookTransactionObj.CheckIn = item.CheckIn;
                    bookTransactionObj.CheckOut = item.CheckOut;
                }
                result = bookTransactionList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
    public static class SetValueExtension
    {
        public static IEnumerable<T> SetValue<T>(this IEnumerable<T> items, Action<T>
             updateMethod)
        {
            foreach (T item in items)
            {
                updateMethod(item);
            }
            return items;
        }
    }

}