using GymSystem.BLL.Contracts;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymSystem.BLL.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepo;
        private readonly IGenericRepository<Session> _sessionRepo;

        public TrainerService(IGenericRepository<Trainer> trainerRepo, IGenericRepository<Session> sessionRepo)
        {
            _trainerRepo=trainerRepo;
            _sessionRepo=sessionRepo;
        }




        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            if (await _trainerRepo.AnyAsync(t => t.Email == model.Email, ct))
                return false;
            if (await _trainerRepo.AnyAsync(t => t.Phone == model.Phone, ct))
                return false;



            var trainer = new Trainer()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Specialties = model.Specialties,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    City = model.City,
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                }
            };

            var result = await _trainerRepo.AddAsync(trainer, ct);
            return result > 0;
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken  ct= default)
        {
            var trainers = await _trainerRepo.GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties
            });
        }

        public async Task<TrainerViewModel?> GetTrainersDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainers = await _trainerRepo.GetByIdAsync(trainerId ,  ct);
            if (trainers == null)
                return null;
            else
                return new TrainerViewModel()
                {
                    Name = trainers.Name,
                    Email = trainers.Email,
                    Phone = trainers.Phone,
                    Specialties= trainers.Specialties,
                    DateOfBirth = trainers.DateOfBirth.ToShortDateString(),
                    Address = $"{trainers.Address.BuildingNumber} - {trainers.Address.Street} - {trainers.Address.City}"
                };
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer == null)
                return null;
            else
                return new TrainerToUpdateViewModel()
                {
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Phone = trainer.Phone,
                    Specialties = trainer.Specialties,
                    BuildingNumber = trainer.Address.BuildingNumber,
                    Street = trainer.Address.Street,
                    City = trainer.Address.City

                    
                };
        }

        public async Task<bool> RemioveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;

            var hasFutureSessions = await _sessionRepo.AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now , ct);
            if (hasFutureSessions)
                return false;

            var result = await _trainerRepo.DeleteAsync(trainer, ct);
            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;
            
            if (await _trainerRepo.AnyAsync(t => t.Email == model.Email  && t.Id != trainerId, ct))
                return false;
            if (await _trainerRepo.AnyAsync(t => t.Phone == model.Phone  && t.Id != trainerId, ct))
                return false;


            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Specialties = model.Specialties;
            trainer.UpdatedAt = DateTime.Now;
            var result = await _trainerRepo.UpdateAsync(trainer, ct);
            return result > 0;

            
        
        
        }

        }
    }

