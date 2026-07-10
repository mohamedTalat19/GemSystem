using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public int Capacity { get; set; }
        public string Description { get; set; } = default!;
        public DateTime EndDate { get; set; }
        //rename CreatedAt to StartDate
        public ICollection<Booking> Bookings { get; set; }
        public Trainer Trainer { get; set; }
        public int TrainerId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
