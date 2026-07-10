using GemSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Membership : BaseEntity
    {
        public IEnumerable<Member> Members { get; set; } = [];
        public int MemberId { get; set; }
        public IEnumerable<Plan> Plans { get; set; } = [];
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
