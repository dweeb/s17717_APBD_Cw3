using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if(context.Request != null)
            {
                string path = context.Request.Path.ToString();
                string query = context.Request.QueryString.ToString();
                string method = context.Request.Method.ToString();
                string body;
                using(StreamReader reader = new StreamReader(context.Request.Body, System.Text.Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }
                using (StreamWriter writer = new StreamWriter(@"logfile.txt", true))
                {
                    await writer.WriteLineAsync(DateTime.Now.ToString("yyyyMMddHHmmss ") + "REQUEST");
                    await writer.WriteLineAsync(DateTime.Now.ToString("yyyyMMddHHmmss ") + "PATH=" + path);
                    await writer.WriteLineAsync(DateTime.Now.ToString("yyyyMMddHHmmss ") + "QUERY=" + query);
                    await writer.WriteLineAsync(DateTime.Now.ToString("yyyyMMddHHmmss ") + "METHOD=" + method);
                    await writer.WriteLineAsync(DateTime.Now.ToString("yyyyMMddHHmmss ") + "BODY=" + body);
                }
            }
            await _next(context);
        }
    }
}
