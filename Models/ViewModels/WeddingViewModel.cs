using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Models.CustomValidations;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Name of bride/groom required")]
        [MinLength(2, ErrorMessage = "Name must be longer than 2 characters")]
        [Display(Name = "Wedder One: ")]
        public string WedderOne { get; set; }
        [Required(ErrorMessage = "Name of bride/groom required")]
        [MinLength(2, ErrorMessage = "Name must be longer than 2 characters")]
        [Display(Name = "Wedder Two: ")]

        public string WedderTwo { get; set; }
        
        [Display(Name = "Date of Wedding: ")]
        [Required(ErrorMessage = "Date required")]
        [DataType(DataType.Date)]
        [InTheFuture(ErrorMessage = "Date of event must be in the future.")]
        public DateTime? Date { get; set; }
        
        
        [Display(Name = "Address: ")]
        [Required(ErrorMessage = "Address required")]
        public string Address { get; set; }
    }
}