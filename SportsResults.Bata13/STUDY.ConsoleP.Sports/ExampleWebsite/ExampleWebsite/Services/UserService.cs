using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Interfaces;

namespace WebApplication5.Services
{
    public class UserService : IUserService
    {
        private int amountPerPage = 5;
        public List<User> GetUsers(int paginationNumber)
        {
            var index = amountPerPage * paginationNumber;
            var users = GetUsers();

            if (index >= users.Count)
                return new List<User>();

            return users.GetRange(index, Math.Min(users.Count-index, amountPerPage));
        }

        private List<User> GetUsers()
        {
            return new List<User>
            {
                { GetUser("Avery", "Forrest",39) },
                { GetUser("Hadassah", "Beard",15) },
                { GetUser("Madelyn", "Stokes",16) },
                { GetUser("Sacha", "Hutton",29) },
                { GetUser("Adrian", "Chapman",44) },
                { GetUser("Kanye", "Weber",63) },
                { GetUser("Nathanael", "Avalos",18) },
                { GetUser("Raymond", "Humphrey",56) },
                { GetUser("Sophie", "Marie",36) },
                { GetUser("Nial", "Chaney",34) },
                { GetUser("Caelan", "Sherman",72) },
            };
        }

        private User GetUser(string firstName, string lastName, int age)
        {
            return new User
            {
                Firstname = firstName,
                Lastname = lastName,
                Age = age
            };
        }

        public int MaxPaginationNumber()
        {
            return GetUsers().Count / amountPerPage;
        }
    }
}