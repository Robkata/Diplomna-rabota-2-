using ExpressTaxi.Abstractions;
using ExpressTaxi.Data;
using ExpressTaxi.Domain;
using ExpressTaxi.Models.Option;
using ExpressTaxi.Models.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpressTaxi.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IOptionService _optionService;
        private readonly ApplicationDbContext context;


        public ReservationsController(IOptionService optionService, ApplicationDbContext context)
        {
            this._optionService = optionService;
            this.context = context;
        }

       
        public IActionResult Create(ReservationCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.context.Users.SingleOrDefault(u => u.Id == userId);
                var ev = this.context.Taxies.SingleOrDefault(e => e.Id == bindingModel.TaxiId);

                if (user == null || ev == null)
                {

                    return this.RedirectToAction("All", "Taxi");
                }
                Reservation orderForDb = new Reservation
                {
                    Destination = bindingModel.Destination,

                    Start = DateTime.UtcNow,
                    //TO DO
                    End = DateTime.UtcNow,
                    Passengers = bindingModel.Passangers,
                    TaxiId = bindingModel.TaxiId,

                    TaxiUserId = userId,
                    Status = "reserved",
                    OptionId =bindingModel.OptionId ,
                };


                this.context.Taxies.Update(ev);
                this.context.Reservations.Add(orderForDb);
                this.context.SaveChanges();
            }
            return this.RedirectToAction("All", "Taxi");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.Users.SingleOrDefault(u => u.Id == userId);

            List<ReservationListingViewModel> orders = context
                 .Reservations
                 .Select(x => new ReservationListingViewModel
                 {
                     Id = x.Id,
                     TaxiId=x.TaxiId,
                     Taxi=x.Taxi.TaxiId,

                     TaxiUserId=x.TaxiUserId,
                     TaxiUser=x.TaxiUser.FirstName+" "+ x.TaxiUser.LastName,

                     OptionId = x.OptionId,
                     OptionName = x.Option.Name,
                     Start = x.Start.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                     //TO DO
                     End = x.End.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                     Passengers = x.Passengers,
                                          
                     Status = x.Status,
                     Destination = x.Destination
                 }).ToList();

            return View(orders);
        }

        public IActionResult Edit(int? id)
        {
            Reservation item = context.Reservations.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            
            ReservationEditBindingModel reservation = new ReservationEditBindingModel()
            {
                Id = item.Id,
                Status = item.Status
            }; 
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(ReservationEditBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
                {
                    Id=bindingModel.Id,
                    Status = bindingModel.Status
                };
                context.Reservations.Update(reservation);
                context.SaveChanges();
                return this.RedirectToAction("All");
            }
            return View(bindingModel);
        }

        [Authorize]
        public IActionResult My(string searchString)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.context.Users.SingleOrDefault(u => u.Id == currentUserId);
            if (user == null)
            {
                return null;
            }

            List<ReservationListingViewModel> orders = this.context.Reservations
                .Where(x => x.TaxiUserId == user.Id)
            .Select(x => new ReservationListingViewModel
            {
                Id = x.Id,
                TaxiId = x.TaxiId,
                Taxi = x.Taxi.TaxiId,

                TaxiUserId = x.TaxiUserId,
                TaxiUser = x.TaxiUser.FirstName + " " + x.TaxiUser.LastName,

                OptionId = x.OptionId,
                OptionName = x.Option.Name,
                Start = x.Start.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                //TO DO
                End = x.End.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                Passengers = x.Passengers,

                Status = x.Status,
                Destination = x.Destination,


            })
            .ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Start.Contains(searchString)).ToList();
            }
            return this.View(orders);
        }
    }
}
