using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using cw4.Models;
using Microsoft.AspNetCore.Http;

namespace cw4.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

       
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path;
                string method = context.Request.Method;
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"/Users/karol/Desktop/requestsLog.txt", true))
                {
                    file.WriteLine(method);
                    file.WriteLine(path);
                    file.WriteLine(queryString);
                    file.WriteLine(bodyStr);
                    file.WriteLine("======================");
                }

                if (_next != null)
                {
                    await _next(context);
                }
            }
            
        }
        

    }
}