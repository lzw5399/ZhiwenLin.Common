using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ZhiwenLin.Common.Handlers
{
    public class AntiXssHttpMessageHandler : DelegatingHandler
    {
        private readonly IProfileDirService profileDirService;

        public AntiXssHttpMessageHandler(IProfileDirService profileDirService)
        {
            this.profileDirService = profileDirService;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, System.Threading.CancellationToken cancellationToken)
        {
            if (Request.Method == HttpMethod.Options)
            {
                return base.SendAsync(Request, cancellationToken);
            }

            var queryStrings = Request.RequestUri.ParseQueryString();
            foreach (var key in queryStrings.AllKeys)
            {
                var value = Sanitizer.GetSafeHtmlFragment(queryStrings[key]);
                if (value != queryStrings[key])
                {
                    return SendError($"input ${key} can not contain sensitive character", HttpStatusCode.BadRequest);
                }
            }

            if (Request.Content.IsFormData())
            {
                var results = Request.Content.ReadAsFormDataAsync().Result;

                foreach (var key in results.AllKeys)
                {
                    var value = results.GetValues(key).FirstOrDefault();
                    var safeValue = Sanitizer.GetSafeHtmlFragment(value);
                    if (value != safeValue)
                    {
                        return SendError($"input ${key} can not contain sensitive character", HttpStatusCode.BadRequest);
                    }
                }
            }
            else if (Request.Content.IsMimeMultipartContent())
            {
                string tempDir = profileDirService.GetTempDir();

                var provider = new MultipartFormDataStreamProvider(tempDir);
                var results = Request.Content.ReadAsMultipartAsync(provider).Result;

                foreach (var key in results.FormData.AllKeys)
                {
                    var value = results.FormData.GetValues(key).FirstOrDefault();
                    var safeValue = Sanitizer.GetSafeHtmlFragment(value);
                    if (value != safeValue)
                    {
                        return SendError($"input ${key} can not contain sensitive character", HttpStatusCode.BadRequest);
                    }
                }
            }
            else
            {
                try
                {
                    var result = Request.Content.ReadAsStringAsync().Result;
                    var obj = JObject.Parse(result);

                    foreach (var item in obj)
                    {
                        var saleValue = Sanitizer.GetSafeHtmlFragment(item.Value.ToString());
                        if (item.Value.ToString() != saleValue)
                        {
                            return SendError($"input ${item.Key} can not contain sensitive character", HttpStatusCode.BadRequest);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return base.SendAsync(Request, cancellationToken);
        }

        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(error);
            response.StatusCode = code;

            return Task.Run(() => response);
        }
    }
}
