﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.ViewModels;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailservice;
        private readonly IDutchRepository repository;

        public AppController(IMailService mailservice, IDutchRepository repository)
        {
            this.mailservice = mailservice;
            this.repository = repository;
        }
        public IActionResult Index()
        {
            //var results = this.context.Products.ToList();
            return View();
        }
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";

            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Send the email
                this.mailservice.SendMessage("shawn@wildermuth.com", model.Subject, $"From : {model.Name},{model.Email}, Message: {model.Message} ");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            else
            {
                //Show the error
            }

            return View();
        }
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        [Authorize]
        public IActionResult Shop()
        {
            var results = this.repository.GetAllProducts();
            return View(results);
        }

    }
}
