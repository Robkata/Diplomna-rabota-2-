using ExpressTaxi.Abstractions;
using ExpressTaxi.Data;
using ExpressTaxi.Models.Statistic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressTaxi.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaxiService _taxiService;

        public AdminController(ApplicationDbContext context, ITaxiService taxiService)
        {
            this._context = context;
            this._taxiService = taxiService;
        }

        public IActionResult Index()
        {
            StatisticVM statistic = new StatisticVM();

            statistic.countTaxies = _taxiService.countTaxies();
            statistic.countUsers = _taxiService.countUsers();
            statistic.countReservations = _taxiService.countReservations();
            return View(statistic);
        }
    }
}