using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Services;
using Easy_Http;
using Easy_Http.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoreLinq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using BillingNextSys.Hubs;
using BillingNextSys.DataModels;

namespace BillingNextSys.Pages.Bill.Format1
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IWhatsappSender _smsSender;

        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IGraphDataService _graphDataService;

        

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor, IOptions<Whatsappoptions> optionsAccessor,  IGraphDataService graphDataService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            Options = optionsAccessor.Value;
            _graphDataService = graphDataService;
        }
        public Whatsappoptions Options { get; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetAdvancePaySettleAsync(int billdetid, double advancepayamt)
        {
            double billamtout = _context.BillDetails.Where(bd => bd.BillDetailsID.Equals(billdetid)).Select(a => a.BillAmountOutstanding).FirstOrDefault();

            Models.AdvancePayDeduct adpd = new Models.AdvancePayDeduct();
            adpd.CompanyID= (int)_session.GetInt32("Cid");
            adpd.BranchID = (int)_session.GetInt32("Bid"); 
            adpd.DebtorGroupID = _context.BillDetails.Where(a => a.BillDetailsID.Equals(billdetid)).Select(a => a.DebtorGroupID).FirstOrDefault();
            adpd.DeductDate = DateTime.Now;
            adpd.BillDetailsID = billdetid;
            if(advancepayamt<=billamtout)
            {
                adpd.AdvanceAmountDeducted =  advancepayamt;
            }
            else
            {
                adpd.AdvanceAmountDeducted = billamtout;
            }
            _context.AdvancePayDeduct.Add(adpd);
            _context.SaveChanges();

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var billout = billamtout- adpd.AdvanceAmountDeducted;
            var billdet = new Models.BillDetails { BillDetailsID = billdetid, BillAmountOutstanding = billout };
            _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
            _context.SaveChanges();

            var dgout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(billdetid)).Join(
                _context.DebtorGroup,
                bd=>bd.DebtorGroupID,
                dg=>dg.DebtorGroupID,
                (bd,dg)=>new { 
                dg.DebtorGroupID,
                dg.DebtorOutstanding,
                dg.AdvancePayAmount
                }).FirstOrDefault();

            var debtorgroup = _context.DebtorGroup.Find(dgout.DebtorGroupID);
           // debtorgroup.DebtorOutstanding = dgout.DebtorOutstanding + adpd.AdvanceAmountDeducted;
            debtorgroup.AdvancePayAmount = dgout.AdvancePayAmount - adpd.AdvanceAmountDeducted;

            _context.Entry(debtorgroup).State = EntityState.Modified;
            _context.SaveChanges();

            return new JsonResult("Successful!");
        }

        public async Task<IActionResult> OnGetSelectAllAsync(string id)
        {
        //     return new JsonResult((from b in _context.BillDetails   
        //                           join d in _context.DebtorGroup 
        //                           on b.DebtorGroupID equals d.DebtorGroupID
        //                           join ap in _context.AdvancePay 
        //                           on d.DebtorGroupID equals ap.DebtorGroupID
        //                           where b.BillNumber == id
        //                           select new
        //                           {
        //                               ap.AdvanceAmount,
        //                               b.ParticularsName,
        //                               b.Amount,
        //                               b.BillDetailsID,
        //                               b.BillAmountOutstanding
                                      
        //                           }).Distinct());

           

            return new JsonResult(_context.BillDetails.Where(x=>x.BillNumber == id).Join
            (_context.DebtorGroup, 
            bd=>bd.DebtorGroupID,
            dg => dg.DebtorGroupID,
           (bd,dg)=>new
           {
               dg.AdvancePayAmount,
               bd.ParticularsName,
               bd.Amount,
               bd.BillDetailsID,
               bd.BillAmountOutstanding
           }).Distinct());

        }

        public async Task<IActionResult> OnPostInsertReceivedAsync(int dgid, [FromBody] Models.Received obj)
        {
            obj.CompanyID = (int)_session.GetInt32("Cid");
            obj.DebtorGroupID = _context.BillDetails.Where(a => a.BillDetailsID.Equals(obj.BillDetailsID)).Select(a => a.DebtorGroupID).FirstOrDefault();
            _context.Received.Add(obj);
            _context.SaveChanges();

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var amtout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(obj.BillDetailsID)).FirstOrDefault().BillAmountOutstanding;
            // var amtout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;
            var billout = amtout - obj.ReceivedAmount;
            var billdet = new Models.BillDetails { BillDetailsID = obj.BillDetailsID, BillAmountOutstanding = billout };
            _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
            _context.SaveChanges();

            var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;

            var debout = dgout - obj.ReceivedAmount;
            var dgdet = new Models.DebtorGroup { DebtorGroupID = dgid, DebtorOutstanding = debout };
            _context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
            _context.SaveChanges();

            await _graphDataService.UpdateCashFlowsGraph();
            return new JsonResult("Successful!");
        }

        public IActionResult OnPost()
        {
            string amount = Request.Form["recamt"].ToString();
            string chequepayment = Request.Form["chqpaymt"].ToString();
            string receivedate = Request.Form["redate"].ToString();


           var paymode = "";
            if (string.IsNullOrEmpty(chequepayment))
            {
                paymode = "Cash";
            }
            else
            {
                paymode = "Cheque";
            }
            var DebtorGPhone = Request.Form["dphone"].ToString();
            var DebtorName = Request.Form["dgname"].ToString();
            var CompanyName = Request.Form["compname"].ToString();
            SendSmsAsync(DebtorGPhone.Substring(DebtorGPhone.Length - 10), DebtorName, amount, paymode, receivedate, CompanyName);

            return Page();
        }

        public async Task SendSmsAsync(string number, string name, string amount, string mode, string recdate, string CompanyName)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            //var accountSid = Options.WhatsappAccountIdentification;
            //// Your Auth Token from twilio.com/console
            //var authToken = Options.WhatsappAccountPassword;

            //TwilioClient.Init(accountSid, authToken);

            //return MessageResource.CreateAsync(
            //to: new PhoneNumber(number),
            //from: new PhoneNumber(Options.WhatsappAccountFrom),
            var msg = $"Dear {name}, Thank you for the payment of Rs.{amount} by {mode} towards Receipt on {recdate}. \nThank you for showing interest in {CompanyName.Replace("&", "And")}";
            await new RequestBuilder<string>()
            .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message={msg}&country=91")
            .SetContentType(ContentType.Application_Json)
            .SetType(RequestType.Get)
            .Build()
            .Execute();

        }

        public async Task<IActionResult> OnPostAmtAdvanceAsync( [FromBody] Models.AdvancePay obj)
        {
            try
            {
                obj.CompanyID= (int)_session.GetInt32("Cid");
                obj.BranchID = (int)_session.GetInt32("Bid");
                _context.AdvancePay.Add(obj);
                await _context.SaveChangesAsync();
                
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).Select(x => new { x.DebtorOutstanding, x.AdvancePayAmount }).FirstOrDefault();
                var debtorgroup = _context.DebtorGroup.Find(obj.DebtorGroupID);
                debtorgroup.DebtorOutstanding = dgout.DebtorOutstanding - obj.AdvanceAmount;
                debtorgroup.AdvancePayAmount = dgout.AdvancePayAmount + obj.AdvanceAmount;
                _context.Entry(debtorgroup).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).FirstOrDefault().DebtorOutstanding;
                //var debout = dgout - obj.AdvanceAmount;
                //var dgdet = new Models.DebtorGroup { DebtorGroupID = obj.DebtorGroupID, DebtorOutstanding = debout };
                //_context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
                //_context.SaveChanges();
                
                return new JsonResult(1);
            }
            catch(Exception e)
            {
                return new JsonResult(0);
            }
            
        }

        public async Task<IActionResult> OnPostAllAdvanceSettleAsync([FromBody] object obj)
        {
            
            dynamic stuff = JsonConvert.DeserializeObject(obj.ToString());
            string billid = stuff.billid;
            double advancePayAmount = stuff.amt;

           List<Models.BillDetails> listbill= _context.BillDetails.Where(a => a.BillNumber.Equals(billid)).Where(b=>b.BillAmountOutstanding>0).AsNoTracking().ToList();
            foreach (var item in listbill)
            {
                double billamtout = _context.BillDetails.Where(bd => bd.BillDetailsID.Equals(item.BillDetailsID)).Select(a => a.BillAmountOutstanding).FirstOrDefault();
                //if (advancePayAmount >= billamtout)
                //{
                    Models.AdvancePayDeduct adpd = new Models.AdvancePayDeduct();
                    adpd.CompanyID = (int)_session.GetInt32("Cid");
                    adpd.BranchID = (int)_session.GetInt32("Bid");
                    adpd.DebtorGroupID = _context.BillDetails.Where(a => a.BillDetailsID.Equals(item.BillDetailsID)).Select(a => a.DebtorGroupID).FirstOrDefault();
                    adpd.DeductDate = DateTime.Now;
                    adpd.BillDetailsID = item.BillDetailsID;
                    if (advancePayAmount <= billamtout)
                    {
                       adpd.AdvanceAmountDeducted = advancePayAmount;
                    }
                    else
                    {
                       adpd.AdvanceAmountDeducted = billamtout;
                    }
                    _context.AdvancePayDeduct.Add(adpd);
                    _context.SaveChanges();

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var billout = billamtout - adpd.AdvanceAmountDeducted;
                    var billdet = new Models.BillDetails { BillDetailsID = item.BillDetailsID, BillAmountOutstanding = billout };
                    _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
                    _context.SaveChanges();

                    var dgout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(item.BillDetailsID)).Join(
                        _context.DebtorGroup,
                        bd => bd.DebtorGroupID,
                        dg => dg.DebtorGroupID,
                        (bd, dg) => new
                        {
                            dg.DebtorGroupID,
                            dg.DebtorOutstanding,
                            dg.AdvancePayAmount
                        }).FirstOrDefault();

                    var debtorgroup = _context.DebtorGroup.Find(dgout.DebtorGroupID);
                    //debtorgroup.DebtorOutstanding = dgout.DebtorOutstanding + adpd.AdvanceAmountDeducted;
                    debtorgroup.AdvancePayAmount = dgout.AdvancePayAmount - adpd.AdvanceAmountDeducted;

                    _context.Entry(debtorgroup).State = EntityState.Modified;
                    _context.SaveChanges();

                    advancePayAmount -= billamtout;
                //}
                //else
                //{
                //    return new JsonResult("Missed out from: " + item.BillDetailsID);
                //}
            }
            return new JsonResult(1);
        }
    }


        [Authorize]
        public class IndexGridModel : PageModel
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private ISession _session => _httpContextAccessor.HttpContext.Session;
            private readonly BillingNextSys.Models.BillingNextSysContext _context;

            public IndexGridModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
        }
            public IQueryable<Models.Bill> Bills { get; set; }

            public IActionResult OnGet()
            {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");
                Bills = _context.Bill.Where(a => a.BillActNum.HasValue).Where(a=>a.CompanyID.Equals(cid)).Where(a=>a.BranchID.Equals(bid)).Include(b => b.Company).Include(b => b.DebtorGroup).AsQueryable();
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
        }

    }