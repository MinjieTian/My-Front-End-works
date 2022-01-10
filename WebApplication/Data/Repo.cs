using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class Repo : IRepo
    {
        private readonly WPDb _Db;

        public Repo(WPDb db)
        {
            _Db = db;
        }

        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return _Db.Staffs;
        }

        public async Task<Staff> GetStaffAsync(int id)
        {
            Staff target =  _Db.Staffs.FirstOrDefault(e => e.Id == id);
            return target;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return _Db.Products;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return _Db.Products.FirstOrDefault(e => e.Id == id);
        }

        public async Task AddCommentAsync(TheComment comment)
        {
            await _Db.Comments.AddAsync(comment);
            await _Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TheComment>> GetTheCommentsAsync()
        {
            return _Db.Comments;
        }

        public async Task AddStaff(Staff staff)
        {
            await _Db.Staffs.AddAsync(staff);
            await _Db.SaveChangesAsync();
        }

        public async Task AddProductAsync(Product c)
        {
            await _Db.Products.AddAsync(c);
            await _Db.SaveChangesAsync();
        }

        public async Task<bool> ValidLoginAsync(string name, string password)
        {
            User res = _Db.Users.FirstOrDefault(u => u.UserName == name);
            if (res != null)
            {
                if (res.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<User> GetUserAsync(string Name)
        {
            User res = _Db.Users.FirstOrDefault(u => u.UserName == Name);
            return res;
        }

        public async Task AddUserAsync(User a)
        {
            await _Db.Users.AddAsync(a);

            await _Db.SaveChangesAsync();
        }

        public async Task<bool> GetProductByIDAsync(int a)
        {
            Product res = _Db.Products.FirstOrDefault(p => p.Id == a);
            if (res != null)
            {
                return true;
            }
            return false;
        }

        public async Task AddOrderAsync(Order a)
        {
            await _Db.Orders.AddAsync(a);
            await _Db.SaveChangesAsync();
        }
    }
}
