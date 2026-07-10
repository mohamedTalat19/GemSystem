using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels
{
    public class TrainerDetailsViewModel
    {
        public string Name { get; set; }

        
        public string Specialty { get; set; }

        
        public string Email { get; set; }

        public string Phone { get; set; }

        
        public DateOnly DateOfBirth { get; set; }

        public Address Address { get; set; }
       
    }
}
