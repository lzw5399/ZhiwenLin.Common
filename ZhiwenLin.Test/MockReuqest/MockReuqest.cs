using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ZhiwenLin.Test.MockReuqest
{
    public class FakeUserPasswordController : UserPasswordController
    {
        public FakeUserPasswordController()
            : base()
        {
            var context = new HttpActionContext();
            var headerValue = new AuthenticationHeaderValue("Bearer", "mockedValue");
            var request = new HttpRequestMessage();
            request.Headers.Authorization = headerValue;
            var controllerContext = new HttpControllerContext();
            controllerContext.Request = request;
            context.ControllerContext = controllerContext;

            base.ActionContext = context;
        }
    }

    #region MockedData
    internal class HttpControllerContext
    {
        public HttpControllerContext()
        {
        }

        public HttpRequestMessage Request { get; internal set; }
    }

    internal class HttpActionContext
    {
        public HttpActionContext()
        {
        }

        public HttpControllerContext ControllerContext { get; internal set; }
    }

    public class UserPasswordController
    {
        internal HttpActionContext ActionContext { get; set; }
    } 
    #endregion
}
