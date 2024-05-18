using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Interfaces;

namespace WebApplication5.Models
{
    public class UsersModel
    {
        public int Index { get; set; }
        public List<User> Users { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}