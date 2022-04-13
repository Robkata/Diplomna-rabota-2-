using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ExpressTaxi.Domain;
using ExpressTaxi.Models.Option;

namespace ExpressTaxi.Models.Reservation
{
    public class ReservationCreateBindingModel
    {

        public ReservationCreateBindingModel()
        {
            Options = new List<OptionPairVM>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int TaxiId { get; set; }

        public string Destination { get; set; }

        public int Passangers { get; set; }
        //public string Status { get; set; }
        public int OptionId { get; set; }
        public virtual List<OptionPairVM> Options { get; set; }
    }
}
