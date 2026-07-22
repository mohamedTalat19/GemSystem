using AutoMapper;
using GemSystem.DAL.Models;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MembersMappings();
            SessionsMappings();
            MapPlan();
            MapTrainer();





        }


        private void SessionsMappings()
        {
            CreateMap<Session, SessionViewModel>()
               .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(s => s.Trainer.Name))
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(c => c.Category.Name));

            CreateMap<CreateSessionViewModel, Session>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(s => s.StartDate));

            CreateMap<Category, CategorySelectViewModel>();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }


        private void MembersMappings()
        {
            CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(m =>
            $"{m.Address.BuildingNumber}-{m.Address.Street}-{m.Address.Street}"));

            CreateMap<Member, MemberViewModel>().ReverseMap();

            CreateMap<HealtRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }));

               

            CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });


        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
            CreateMap<Trainer, TrainerViewModel>()
                            .ForMember(dest => dest.Address,
                            opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }
    }
}
