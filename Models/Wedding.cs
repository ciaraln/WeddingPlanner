using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        [Key]
        public int WeddingId { get; set; }
        public string WedderOne { get; set; }
        public string WedderTwo { get; set; }

        [FutureDate(ErrorMessage = "Wedding must be in the future!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime Date { get; set; }
        public string Address { get; set; }
      
        [ForeignKey("User")]
        public int userId { get; set; }
        public User User {get; set;}

        public List<GuestList> Guests { get; set; }

        // instantiating the list; guest objects.  //
        public Wedding()
        {
            Guests = new List<GuestList>();
        }


        public class FutureDateAttribute : ValidationAttribute
        {
            public FutureDateAttribute() { }
            public override bool IsValid(object value)
            {
                var date = (DateTime)value;
                if (date > DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }
    }
}