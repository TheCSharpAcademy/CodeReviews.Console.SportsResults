using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Interfaces
{
    public interface IUserService
    {
        public int MaxPaginationNumber();
        public List<User> GetUsers(int paginationNumber);
    }

    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
    }
}