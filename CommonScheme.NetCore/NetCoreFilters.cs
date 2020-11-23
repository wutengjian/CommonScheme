using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.RegularExpressions;

namespace CommonScheme.NetCore
{
    /// <summary>
    /// 优先级1：权限过滤器：它在Filter Pipleline中首先运行，并用于决定当前用户是否有请求权限。如果没有请求权限直接返回。
    /// </summary>
    public class MyAuthorization : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

        }
    }

    /// <summary>
    /// 优先级2：资源过滤器： 它在Authorzation后面运行，同时在后面的其它过滤器完成后还会执行。Resource filters 实现缓存或其它性能原因返回。因为它运行在模型绑定前，所以这里的操作都会影响模型绑定。
    /// </summary>
    public class MyResourceFilterAttribute : IResourceFilter
    {
        //这个ResourceFiltersAttribute是最适合做缓存了,在这里做缓存有什么好处?因为这个OnResourceExecuting是在控制器实例化之前运营，如果能再这里获取ViewReuslt就不必实例化控制器，在走一次视图了，提升性能
        private static readonly Dictionary<string, object> _Cache = new Dictionary<string, object>();
        private string _cacheKey;
        /// <summary>
        /// 这个方法会在控制器实例化之前之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();//这个是请求地址，它肯定是指向的视图
            if (_Cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _Cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue;
                }
            }
        }
        /// <summary>
        /// 这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的（每次执行完Action之后得到就是一个ViewResult）
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();//这个是请求地址
            if (!string.IsNullOrEmpty(_cacheKey) && !_Cache.ContainsKey(_cacheKey))
            {
                //因为这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的，所以context.Result必然有值了，这个值就是Action方法执行后得到的ViewResult
                var result = context.Result as ViewResult;
                if (result != null)
                {
                    _Cache.Add(_cacheKey, result);
                }
            }
        }
    }

    /// <summary>
    /// 优先级3：方法过滤器：它会在执行Action方法前后被调用。这个可以在方法中用来处理传递参数和处理方法返回结果。
    /// </summary>
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        private readonly string securitykey = "0123456789abcdef";
        public override void OnActionExecuting(ActionExecutingContext context) { }
        public override void OnActionExecuted(ActionExecutedContext context) { }
        public override void OnResultExecuting(ResultExecutingContext context) { }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //context.HttpContext.Response.Body = AESDecrypt(context.HttpContext.Response.Body);
        }
        private Stream AESDecrypt(Stream response)
        {
            using (var reader = new StreamReader(response))
            {
                response.Position = 0;
                string a = reader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(a))
                {
                    a = AESEncrypt(a, securitykey);
                }
                using (var writer = new StreamWriter(response))
                {
                    writer.Write(a);
                    writer.Flush();
                    return response;
                }
            }
        }
        private string AESEncrypt(string toEncrypt, string key)
        {
            if (string.IsNullOrWhiteSpace(toEncrypt)) return "";

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }

    /// <summary>
    /// 优先级4：异常过滤器：被应用全局策略处理未处理的异常发生前异常被写入响应体
    /// </summary>
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _moprovider;
        public MyExceptionFilterAttribute(IModelMetadataProvider moprovider)
        {
            this._moprovider = moprovider;
        }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (!context.ExceptionHandled)//如果异常没有被处理过
            {
                string controllerName = (string)context.RouteData.Values["controller"];
                string actionName = (string)context.RouteData.Values["action"];
                //string msgTemplate =string.Format( "在执行controller[{0}的{1}]方法时产生异常",controllerName,actionName);//写入日志

                if (this.IsAjaxRequest(context.HttpContext.Request))
                {

                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        PromptMsg = "系统出现异常，请联系管理员",
                        DebugMessage = context.Exception.Message
                    });
                }
                else
                {
                    var result = new ViewResult { ViewName = "~Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_moprovider, context.ModelState);
                    result.ViewData.Add("Execption", context.Exception);
                    context.Result = result;
                }

;
            }
        }
        //判断是否为ajax请求
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }

    /// <summary>
    /// 优先级5：结果过滤器：它可以在执行Action结果之前执行，且执行Action成功后执行，使用逻辑必须围绕view或格式化执行结果。
    /// </summary>
    public class MyResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
