using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Member : GymUser
    {
        //Rename Created At To JoinDate
        public string? Photo { get; set; }
        public HealtRecord HealtRecord { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
