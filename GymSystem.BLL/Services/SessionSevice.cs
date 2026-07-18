using AutoMapper;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.Results;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Models.Enums;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services
{
    public class SessionSevice : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionSevice(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if(model.StartDate > model.EndDate) return Result.Validation("StartDate Must Be Before EndDate");
            if(model.StartDate < DateTime.Now) return Result.Validation("StartDate Must Be In the future"); ;
            if (model.Capacity > 25 || model.Capacity < 1) return Result.Validation("Capacity Must Be Between 1 and 25"); 

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId , ct);
            if (trainer == null) return Result.Validation("Trainer was not Found"); 

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category == null) return Result.Validation("Category was not Found"); 

            var isValid = Enum.TryParse<Specialties>(category.Name, true, out var specialties);
            if (!isValid || trainer.Specialties != specialties) return Result.Validation("Trainer Doesn't match the speciality"); 

            var session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Session Creation Failed");

        }

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct)
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = await sessionRepo.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (sessions == null) return Result<IEnumerable<SessionViewModel>>.NotFound("Sessions Not Found"); 

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions); 

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await sessionRepo.GetCountBookedSlotsAsync(session.Id ,ct);
            }
            return Result<IEnumerable<SessionViewModel>>.Ok(mappedSessions);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownListAsync(CancellationToken ct)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync( int sessionId,CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);
            if (session is null)
                return Result<SessionViewModel>.NotFound("Session Not Found");
            else
            {
                var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
                mappedSession.AvailableSlots = 
                    mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountBookedSlotsAsync(sessionId, ct);
                return Result<SessionViewModel>.Ok(mappedSession);
            }
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Can Not Update Session That Has Already Started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountBookedSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result<UpdateSessionViewModel>.Fail("Can Not Update Session That Has Already Bookings");

            var mappedSession = _mapper.Map<Session , UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownListAsync(CancellationToken ct)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session == null) return Result.NotFound("Session is Not Found");
            if (session.EndDate >=  DateTime.Now)
                return Result.Fail("Can Not Delete Session That Has bookings");

            var BookCount = await _unitOfWork.SessionRepository.GetCountBookedSlotsAsync(sessionId, ct);
            if (BookCount > 0)
                return Result.Fail("Can Not Delet Session That has Bookings");

            _unitOfWork.SessionRepository.Delete(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
        }

        public async Task<Result> UpateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);
            if (session == null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can Not Edit Session That Has Already Started");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must be after Start Date");

            var BookCount = await _unitOfWork.SessionRepository.GetCountBookedSlotsAsync(id , ct);
            if (BookCount > 0)
                return Result.Fail("Can Not Update Session that Has Already Bookings");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId,ct);
            if (trainer == null) return Result.NotFound("Trainer was not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);

            var isValid = Enum.TryParse<Specialties>(category.Name, true, out var specialties);
            if (!isValid || trainer.Specialties != specialties) return Result.Validation("Trainer Doesn't match the speciality");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionRepository.Update(session);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");


        }
    }
}
