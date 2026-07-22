using AutoMapper;
using GemSystem.DAL.Models;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.Results;
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

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService=attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var _membersRepo = _unitOfWork.GetRepository<Member>();

            if (await _membersRepo.AnyAsync(m => m.Email == model.Email, ct))
                return Result.Fail("A member with this email already exists.");
            if (await _membersRepo.AnyAsync(m => m.Phone == model.Phone, ct))
                return Result.Fail("A member with this phone number already exists.");

            var member = _mapper.Map<Member>(model);

            var fileName = await _attachmentService
                .UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MemberPhotos", ct);
            if (fileName == null) return Result.Fail("Failed to upload member photo");
            member.Photo = fileName;


            _membersRepo.Add(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result == 0)
                _attachmentService.Delete(fileName, "MemberPhotos");
            
            return Result.Ok();

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();

            var members = await _memberRepo.GetAllAsync(ct);
            
            if (!members.Any()) return [];

            var memberViewModels = _mapper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>>(members);

            return memberViewModels;
            
        }

        public async Task<MemberDetailsViewModel?> GetMemberDetailsByIdAsync(int id, CancellationToken ct)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();
            //Get member from Db
            var member = await _memberRepo.GetByIdAsync(id , ct);

            //if null return null
            if (member == null) return null;

            //map member Entity To MemberDetails View Model
           
            var memberDetailsVM = _mapper.Map<MemberDetailsViewModel>(member);

            //Active Membership
            var _membershipRepo = _unitOfWork.GetRepository<Membership>();

            var activeMembership = await _membershipRepo.FirstOrDefaultAsync(x => x.MemberId == member.Id && x.EndDate > DateTime.Now, ct);

            if(activeMembership != null)
            {
                //Plan name
                var _planRepo = _unitOfWork.GetRepository<Plan>();

                var plan = await _planRepo.GetByIdAsync(activeMembership.PlanId, ct);
                memberDetailsVM.PlanName = plan.Name;

                //assign membershipStartDate & membershipEndDate
                memberDetailsVM.MembershipStartDate = activeMembership.StartDate.ToString();
                memberDetailsVM.MembershipEndDate = activeMembership.EndDate.ToString();

            }

           //return MemberDetailsViewModel
            return memberDetailsVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            var _healtRecordRepo = _unitOfWork.GetRepository<HealtRecord>();

            var healthRecord = await _healtRecordRepo.FirstOrDefaultAsync(x => x.MemberId == memberId, ct);
            if (healthRecord == null) return null;
            
            return _mapper.Map<HealthRecordViewModel>(healthRecord);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();

            var member = await _memberRepo.GetByIdAsync(memberId , ct);
            if (member == null) return null;
            
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<Result> RemveMemberAsync(int id, CancellationToken ct)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();

            var member = await _memberRepo.GetByIdAsync(id, ct);
            if (member == null) return Result.NotFound("Member Not Found");

            var _bookingRepo = _unitOfWork.GetRepository<Booking>();

            var hasFutureBookings = await _bookingRepo.AnyAsync(x => x.MemberId == id && x.BookingDate > DateTime.Now , ct);
            if (hasFutureBookings) return Result.NotFound("Cannot Delete member because have future Bookings");

            _attachmentService.Delete(member?.Photo ?? "", "MemberPhotos");

            _memberRepo.Delete(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To delete member");
              

            
        }

        public async Task<Result> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var _memberRepo = _unitOfWork.GetRepository<Member>();

            var member = await _memberRepo.GetByIdAsync(id, ct);
            if (member == null) return Result.NotFound("Member not Found");


            if (await _memberRepo.AnyAsync(m => m.Email == model.Email, ct))
                return Result.Fail("A member with this email already exists.");
            if (await _memberRepo.AnyAsync(m => m.Phone == model.Phone, ct))
                return Result.Fail("A member with this phone number already exists.");

            _mapper.Map(model, member);

            member.UpdatedAt = DateTime.Now;
            _memberRepo.Update(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To update Member");
        }


    }
    }

