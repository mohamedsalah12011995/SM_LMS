using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.GateWay.Controllers
{
    public interface IGateWayController
    {
        public Task<OperationOutput> GenerateTokenWinAuthAsync();
    }
}
