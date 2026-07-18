using GymSystem.BLL.ViewModels.AnalyticsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetDataAsync(CancellationToken ct = default);
    }
}
