using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Data
{
    public interface IRepo
    {
        public Task<IEnumerable<Staff>> GetStaffsAsync();

        public Task<Staff> GetStaffAsync(int id);

        public Task<IEnumerable<Product>> GetProductsAsync();

        public Task<Product> GetProductAsync(int id);

        public Task AddCommentAsync(TheComment c);

        public Task<IEnumerable<TheComment>> GetTheCommentsAsync();

        public Task<bool> ValidLoginAsync(string a, string b);

        public Task<User> GetUserAsync(string a);

        public Task AddUserAsync(User a);

        public Task<bool> GetProductByIDAsync(int a);

        public Task AddOrderAsync(Order a);
    }
}
