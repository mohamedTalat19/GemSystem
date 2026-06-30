using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class HealtRecord : BaseEntity
    {
        public int Height { get; set; }
        public float Wieght { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Notes { get; set; } = default!;
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
