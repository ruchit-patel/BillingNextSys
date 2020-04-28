using BillingNextSys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingNextSys.Services
{
    public interface IGraphDataService
    {
        Task UpdateCashFlowsGraph();
    }
}
