using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Interfaces;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class StaticController : Controller
    {
        private IUserService userService;

        public StaticController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index(int index)
        {
            var maxIndex = userService.MaxPaginationNumber();

            if (index < 0)
                index = 0;

            if (index > maxIndex)
                index = maxIndex;

            var model = new UsersModel
            {
                Index = index,
                Users = userService.GetUsers(index),
                HasNext = index < maxIndex,
                HasPrevious = index > 0
            };

            return View(model);
        }
    }
}