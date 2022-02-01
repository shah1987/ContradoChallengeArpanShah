using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContradoCommon
{
    public class CommonResponse
    {
        public int  StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public CommonResponse(Common.CustomCodes customCode, dynamic result, string errorMessage)
        {
            StatusCode = (int)customCode;
            Data = result;
            Message = string.IsNullOrWhiteSpace(errorMessage) ? Common.Response[customCode].ToString() : errorMessage;
        }

        public static HttpResponseMessageResult CommonAPIResponse(object result = null, string errorMessage = "", Common.CustomCodes customCodes = Common.CustomCodes.Success)
        {
            var customeCode = !string.IsNullOrWhiteSpace(errorMessage) ? Common.CustomCodes.InternalError : customCodes;
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new CommonResponse(customeCode, result, errorMessage)))
            };

            return new HttpResponseMessageResult(response);
        }
    }

    public class HttpResponseMessageResult : Microsoft.AspNetCore.Mvc.IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_responseMessage.StatusCode;

            foreach (var header in _responseMessage.Headers)
            {
                context.HttpContext.Response.Headers.TryAdd(header.Key, new StringValues(header.Value.ToArray()));
            }

            context.HttpContext.Response.Headers.TryAdd("Content-Type", "application/json");

            using (var stream = await _responseMessage.Content.ReadAsStreamAsync())
            {
                await stream.CopyToAsync(context.HttpContext.Response.Body);
                await context.HttpContext.Response.Body.FlushAsync();
            }
        }
    }
}
