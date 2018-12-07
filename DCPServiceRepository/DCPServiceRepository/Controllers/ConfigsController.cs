using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading;
using DCPServiceRepository.Common;
using System;
using ConfigServiceReference;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DCPServiceRepository.Common.Enums;
using Microsoft.Extensions.Configuration;

namespace DCPServiceRepository.Controllers
{
    [Route("api/[controller]")]
    public class ConfigsController : Controller
    {
        private readonly DenormRepoDbContext _context;
        private readonly ICallLogRepository _ICallLogRepository;
        private readonly IOrderCodeRepository _IOrderCodeRepository;
        private readonly IConfiguration _IConfiguration;
        private readonly IConfigurationSection _IConfigurationSection;

        public ConfigsController(DenormRepoDbContext context, ICallLogRepository ICallLogRepository, IOrderCodeRepository IOrderCodeRepository, IConfiguration Configuration)
        {
            _context = context;
            _ICallLogRepository = ICallLogRepository;
            _IOrderCodeRepository = IOrderCodeRepository;
            _IConfiguration = Configuration;
            _IConfigurationSection = _IConfiguration.GetSection("AppSettings");
        }

        // GET api/config/GetOrderCodeDetails

        [HttpGet("GetOrderCodeDetails")]
        public async Task<string> GetOrderCodeDetails(string orderCode, string customerSet, string country, string language)
        {
            ConfigServiceClient sc = new ConfigServiceClient(ConfigServiceClient.EndpointConfiguration.BasicHttpService,
                _IConfigurationSection.GetSection("ServiceURL").Value);

            ProcessRequestRequest request = new ProcessRequestRequest
            {
                request = new ConfigRequest()
            };
            request.request.OrderCode = orderCode;
            request.request.CustomerSetId = customerSet;
            request.request.LanguageId = language;
            request.request.CountryId = country;
            request.request.Version = 7.1;
            request.request.AllowChanges = false;
            //request.request.CartId = System.Guid.NewGuid();
            request.request.CatalogId = 0;
            request.request.PostType = PostType.PartialPost;
            request.request.ResponseType = ResponseType.Default;
            request.request.RetrieveCorrectionMode = false;
            request.request.TrackSelectionUpdates = true;
            request.request.CfiOptionalModuleId = -2147483648;
            request.request.CfiRequiredModuleId = -2147483648;

            //TODO : generate KEY
            var jsonRequest = JsonConvert.SerializeObject(request);
            string hashKeyForRequest = HashGenerator.GenerateHashKey(jsonRequest.ToLower());
            request.request.CartId = Guid.NewGuid();

            //TODO : Write to CallLog
            // 1. check if already exists or in progress also check for ExpiryDate
            // 2. Decide if you need to call service or wait (sleep)

            var callLogRow = _context.CallLog
            .Where(b => b.Key == hashKeyForRequest)
            .FirstOrDefault();


            if (callLogRow != null)
            {
                ProcessRequestResponse processRequestResponse = null;
                int responseSnapshoptDurationInMinutes = (DateTime.UtcNow - callLogRow.FinishTime).Minutes;
                int serviceRequestThresholdInSeconds = (DateTime.UtcNow - callLogRow.RequestStartTime).Seconds;
                if (responseSnapshoptDurationInMinutes >= Convert.ToInt16(_IConfigurationSection.GetSection("ResponseSnapshoptDurationInMinutes").Value))
                {
                    if (callLogRow.Status == CallLogStatus.InProgress.ToString() && serviceRequestThresholdInSeconds < Convert.ToInt16(_IConfigurationSection.GetSection("serviceRequestThresholdInSeconds").Value))
                    {   //Open questions are there..
                        Thread.Sleep(5001);
                        callLogRow = _context.CallLog
                            .Where(b => b.Key == hashKeyForRequest)
                            .FirstOrDefault();
                        if (callLogRow.Status == CallLogStatus.InProgress.ToString() && serviceRequestThresholdInSeconds < Convert.ToInt16(_IConfigurationSection.GetSection("serviceRequestThresholdInSeconds").Value))
                        {
                            Thread.Sleep(5001);
                        }
                     
                    }


                    //var task = new[]
                    //{
                    //    Task.Factory.StartNew(() => { }),
                    //    Task.Factory.StartNew(() => { })
                    //};
                    //Task.WaitAll(task);

                    new Thread(delegate ()
                    {
                        _ICallLogRepository.UpdateCallLog(callLogRow, hashKeyForRequest,
                            CallLogStatus.InProgress);
                    }).Start();

                    //_ICallLogRepository.UpdateCallLog(callLogRow,hashKeyForRequest, CallLogStatus.InProgress);
                    try
                    {

                        processRequestResponse = await sc.ProcessRequestAsync(request);
                    }
                    catch (Exception ex)
                    {
                        _ICallLogRepository.DeleteCallLog(callLogRow, hashKeyForRequest);
                        _IOrderCodeRepository.DeleteOrderCode(hashKeyForRequest);
                        return "error";
                    }

                    string jsonResponse = JsonConvert.SerializeObject(processRequestResponse);
                    string compressedJsonResponse = Utility.CompressString(jsonResponse);

                    new Thread(delegate ()
                    {
                        _IOrderCodeRepository.UpdateOrderCode(compressedJsonResponse, hashKeyForRequest,
                            jsonRequest);
                    }).Start();

                    //_IOrderCodeRepository.UpdateOrderCode(compressedJsonResponse, hashKeyForRequest, jsonRequest);
                    _ICallLogRepository.UpdateCallLog(callLogRow, hashKeyForRequest, CallLogStatus.Completed);

                    return jsonResponse;

                }
                else
                {
                    var orderCodeRow = _context.OrderCode
                        .Where(b => b.Key == hashKeyForRequest)
                        .FirstOrDefault();


                    string deCompressedJsonResponse = Utility.DecompressString(orderCodeRow.Response);
                    return deCompressedJsonResponse;
                }
            }
            else
            {
                ProcessRequestResponse processRequestResponse = null;
                _ICallLogRepository.CreateCallLog(hashKeyForRequest, CallLogStatus.InProgress);
                //TODO : if first call, make async call to Config Service
                try
                {
                    processRequestResponse = await sc.ProcessRequestAsync(request);
                }

                catch (Exception ex)
                {
                    // TODO: update call log
                    _ICallLogRepository.DeleteCallLog(null, hashKeyForRequest);
                    return "error";

                }
                string jsonResponse = JsonConvert.SerializeObject(processRequestResponse);

                string compressedJsonResponse = Utility.CompressString(jsonResponse);

                // 1. Exception Handling (do not store in case of failure)
                // 2. Zip the output .
                // 3. Store output in DB (order code model repository)
                _IOrderCodeRepository.CreateOrderCode(compressedJsonResponse, hashKeyForRequest, jsonRequest);

                //TODO : UPdate call log with finish time and status.
                _ICallLogRepository.UpdateCallLog(null, hashKeyForRequest, CallLogStatus.Completed);

                //TODO : convert the output to required format and return

                return jsonResponse;


            }

        }

    }
}
