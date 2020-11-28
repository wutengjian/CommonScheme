using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CommonScheme.NetCore
{
    #region ExceptionHandle
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                // _logger.LogError(ex.Message);
                await context.Response.WriteAsync(context.Request.Path.Value + " @Error: " + ex.Message);
            }
        }
    }
    public static class ExceptionHandleExtensions
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
    #endregion

    #region HostHeader
    public class HostHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public HostHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.Headers.ContainsKey("hostulr") == false)
                context.Response.Headers.Add("hostult", HostDataInfo.Urls);
            await _next(context);
        }
    }
    public static class HostHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseHostHeader(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HostHeaderMiddleware>();
        }
    }
    #endregion

    #region  RealIp
    public class RealIpMiddleware
    {
        private readonly RequestDelegate _next;
        public RealIpMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                context.Connection.RemoteIpAddress = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            GetIpAddress(context);
            return _next(context);
        }
        public string GetIpAddress(HttpContext context)
        {
            Microsoft.Extensions.Primitives.StringValues ip;
            if (context.Request.Headers.ContainsKey("x-forwarded-for"))
            {
                context.Request.Headers.TryGetValue("x-forwarded-for", out ip);
                if (string.IsNullOrEmpty(ip) == false && ip.ToString().ToUpper() != "unknown")
                    return ip;
            }
            if (context.Request.Headers.ContainsKey("Proxy-Client-IP"))
            {
                context.Request.Headers.TryGetValue("Proxy-Client-IP", out ip);
                if (string.IsNullOrEmpty(ip) == false && ip.ToString().ToUpper() != "unknown")
                    return ip;
            }
            if (context.Request.Headers.ContainsKey("WL-Proxy-Client-IP"))
            {
                context.Request.Headers.TryGetValue("WL-Proxy-Client-IP", out ip);
                if (string.IsNullOrEmpty(ip) == false && ip.ToString().ToUpper() != "unknown")
                    return ip;
            }
            return null;
        }
    }
    public static class RealIpExtensions
    {
        public static IApplicationBuilder UseRealIpExtensions(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RealIpMiddleware>();
        }
    }
    #endregion
}
