using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Results
{
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        Validation,
        Forbiden
    }
}
