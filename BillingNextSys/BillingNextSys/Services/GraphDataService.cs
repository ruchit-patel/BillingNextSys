using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.DataModels;
using Microsoft.AspNetCore.SignalR;
using BillingNextSys.Hubs;
using Newtonsoft.Json;

namespace BillingNextSys.Services
{
    public class GraphDataService : IGraphDataService
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        private readonly IHubContext<GraphDataHub> _hubContext;
        public GraphDataService(BillingNextSys.Models.BillingNextSysContext context, IHubContext<GraphDataHub> hubContext)
        { 
            _hubContext=hubContext;
            _context = context;
        }


        //public async void UpdateDataOnClients(List<GraphDataModel> graphDataModels)
        //{
        //   // var abc= new List<GraphDataModel> { new GraphDataModel { Count = twodGraphdata.Count, Type = "Inward Cash Flows", GraphContent = twodGraphdata } };
        //    //List<GraphDataModel> graphDataAmountReceived= await GenerateData();
        //    await _hubContext.Clients.All.SendAsync("sendToUser",JsonConvert.SerializeObject(graphDataModels));
        //}

        async Task IGraphDataService.UpdateCashFlowsGraph()
        {
            var InflowData = await _context.Received.GroupBy(x => x.ReceivedDate).Select(g => new TwoDGraphData
            {
                Label = g.Key.ToString(),
                Data = g.Sum(x => x.ReceivedAmount).ToString()
            }).ToListAsync();
            var OutFlowData = await _context.Bill.GroupBy(x => x.BillDate.Date).Select(g => new TwoDGraphData
            {
                Label = g.Key.ToString(),
                Data = g.Sum(x => x.BillAmount).ToString()
            }).ToListAsync();
            var graphDataList = new List<GraphDataModel>();
            graphDataList.Add(new GraphDataModel { Count = InflowData.Count, Type = "Cash Inflow", GraphContent = InflowData });
            graphDataList.Add(new GraphDataModel { Count = OutFlowData.Count, Type = "Bills Generated", GraphContent = OutFlowData });
            await _hubContext.Clients.All.SendAsync("sendCashFlowsData", JsonConvert.SerializeObject(graphDataList));
        }
    }

}