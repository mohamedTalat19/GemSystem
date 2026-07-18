using GymSystem.BLL.Results;
using GymSystem.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
    public interface ISessionService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct);

        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownListAsync(CancellationToken ct);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownListAsync(CancellationToken ct);
        Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId,CancellationToken ct);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId , CancellationToken ct);
        Task<Result> UpateSessionAsync(int id , UpdateSessionViewModel model ,  CancellationToken ct);
        Task<Result> RemoveSessionAsync(int sessionId , CancellationToken ct); 
    }
}
