using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class GuestList :  BaseEntity
    {
        [Key]
        public int GuestListId { get; set; }
        [ForeignKey("User")]
        public int userId { get; set; }
        public User User { get; set; }

        [ForeignKey("Wedding")]
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }

    }
}