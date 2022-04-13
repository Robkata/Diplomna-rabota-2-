using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressTaxi.Models.Reservation
{
    public class ReservationListingViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Destination { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Passengers { get; set; }
        public int TaxiId { get; set; }

        [Display(Name = "Taxi")]
        public int Taxi { get; set; }
       
        public string TaxiUserId { get; set; }

        [Display(Name = "TaxiUser")]
        public string TaxiUser { get; set; }
        public string Status { get; set; }
        public int OptionId { get; set; }
        [Display(Name = "Option")]
        public string OptionName { get; set; }
    }
}
