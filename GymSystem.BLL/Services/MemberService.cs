using GymSystem.BLL.Contracts;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services
{
    public class MemberService : IMemberService
    {

        private readonly IGenericRepository<Member> _membersRepo;

        public MemberService(IGenericRepository<Member> memberRepo)
        {
            _membersRepo = memberRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //Validate Email Doesn't Exist
            var emailExits = await _membersRepo.AnyAsync(m => m.Email == model.Email, ct);
            //Validate Phone Doesn't Exist
            var phoneExits = await _membersRepo.AnyAsync(m => m.Phone == model.Phone, ct);

            if(emailExits ||  phoneExits) return false;

            var member = new Member
            {
                Email = model.Email,
                Phone = model.Phone,
                Name = model.Name,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address
                {
                    City = model.City,
                    Street = model.Street,
                    BuildingNumber = model.BuildingNumber,
                },
                HealtRecord = new HealtRecord
                {
                    Height = model.HealthRecordViewModel.Height,
                    Wieght = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Notes = model.HealthRecordViewModel.Notes,
                }
            };

            var result = await _membersRepo.AddAsync(member, ct);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct)
        {
            var members = await _membersRepo.GetAllAsync(ct);
            
            if (!members.Any()) return [];

            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Gender = m.Gender,
                Phone = m.Phone,
                Photo = m.Photo,
            });


            return memberViewModels;
            
        }
    }
}
