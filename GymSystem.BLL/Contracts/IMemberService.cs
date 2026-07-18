using GymSystem.BLL.Results;
using GymSystem.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct);

        Task<Result> CreateMemberAsync(CreateMemberViewModel model,  CancellationToken ct);

        Task<MemberDetailsViewModel?> GetMemberDetailsByIdAsync(int id , CancellationToken ct);

        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId , CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId , CancellationToken ct);
        Task<Result> UpdateMemberAsync(int id,MemberToUpdateViewModel model, CancellationToken ct);
        Task<Result> RemveMemberAsync(int id , CancellationToken ct);
    }

    
}
