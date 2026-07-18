using GymSystem.BLL.Results;
using GymSystem.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainersDetailsAsync(int trainerId , CancellationToken ct = default);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<Result> UpdateTrainerDetailsAsync( int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> RemioveTrainerAsync(int trainerId , CancellationToken ct = default);


    }
}
