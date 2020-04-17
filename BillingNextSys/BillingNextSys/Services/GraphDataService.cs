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
    public class GraphDataService 
    {
        
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        
        private readonly IHubContext<GraphDataHub> _hubContext;
        public GraphDataService(BillingNextSys.Models.BillingNextSysContext context, IHubContext<GraphDataHub> hubContext)
        {
            _context = context;  
            _hubContext=hubContext;
        }


       private async Task<List<GraphDataModel>> GenerateData()
        {
           // return await _context.Received.GroupBy(x=>x.ReceivedDate).Select(g=> new GraphDataCashFlows{
               // Datelables=g.Key,
             //  Amount= g.Sum(x=>x.ReceivedAmount)
         //   }).ToListAsync();  
         List<GraphDataModel> graphDataCashFlows= new List<GraphDataModel>();   
         List<TwoDGraphData> graphDataInCashes = new List<TwoDGraphData>();
         graphDataInCashes.Add(new TwoDGraphData{ Label= DateTime.Now.Date.ToString(),Data= Convert.ToString(2000.2)});
         graphDataInCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,12,2).Date.ToString(),Data=Convert.ToString(2000.2)});
         graphDataInCashes.Add(new TwoDGraphData{ Label= new DateTime(2020,1,2).Date.ToString(),Data= Convert.ToString(3100)});
         graphDataInCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,12,2).Date.ToString(),Data=Convert.ToString(4200.2)});
         graphDataInCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,12,5).Date.ToString(),Data=Convert.ToString(3500.2)});

         graphDataCashFlows.Add(new GraphDataModel{Count=5, Type="Inward Cash Flows", GraphContent=graphDataInCashes});

         List<TwoDGraphData> graphDataOutCashes = new List<TwoDGraphData>();
         graphDataOutCashes.Add(new TwoDGraphData{ Label= DateTime.Now.Date.ToString(),Data= Convert.ToString(2000.2)});
         graphDataOutCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,12,12).Date.ToString(),Data=Convert.ToString(4000.2)});
         graphDataOutCashes.Add(new TwoDGraphData{ Label= new DateTime(2020,1,12).Date.ToString(),Data= Convert.ToString(2100)});
         graphDataOutCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,12,4).Date.ToString(),Data=Convert.ToString(3200.2)});
         graphDataOutCashes.Add(new TwoDGraphData{ Label= new DateTime(2019,1,3).Date.ToString(),Data=Convert.ToString(1500.2)});

         graphDataCashFlows.Add(new GraphDataModel{Count=5, Type="Outward Cash Flows", GraphContent=graphDataOutCashes});
         return graphDataCashFlows;
        }

        public async void UpdateDataOnClients()
        {
            List<GraphDataModel> graphDataAmountReceived= await GenerateData();
            await _hubContext.Clients.All.SendAsync("sendToUser",JsonConvert.SerializeObject(graphDataAmountReceived));
        }
    }

}