using AutoMapper;
using GemSystem.DAL.Models;
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
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            if (await _trainerRepo.AnyAsync(t => t.Email == model.Email, ct))
                return Result.Fail("A trainer with this email already exists.");
            if (await _trainerRepo.AnyAsync(t => t.Phone == model.Phone, ct))
                return Result.Fail("A trainer with this phone number already exists.");

            var entity = _mapper.Map<Trainer>(model);
            _trainerRepo.Add(entity);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Trainer");
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken  ct= default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainers = await _trainerRepo.GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?> GetTrainersDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainers = await _trainerRepo.GetByIdAsync(trainerId ,  ct);
            return trainers is null ? null : _mapper.Map<TrainerViewModel>(trainers);
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            return trainer is null ? null : _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public async Task<Result> RemioveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer not found."); ;

            var _sessionRepo = _unitOfWork.GetRepository<Session>();

            var hasFutureSessions = await _sessionRepo.AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now , ct);
            if (hasFutureSessions)
                return Result.Fail("Cannot delete a trainer with upcoming sessions."); 

             _trainerRepo.Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok(); 
        }

        public async Task<Result> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);

            if (trainer is null) return Result.NotFound("Trainer not found.");

            // BUG FIX from original: original code checked against MemberEntity (wrong table).
            if (await _trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct))
                return Result.Fail("Another trainer is already using this email.");
            if (await _trainerRepo.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct))
                return Result.Fail("Another trainer is already using this phone number.");

            _mapper.Map(model, trainer);

            _trainerRepo.Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Trainer");




        }

        }
    }

