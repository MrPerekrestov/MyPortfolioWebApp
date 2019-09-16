using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)=>        
                request?.Headers != null ?
                request.Headers["X-Requested-With"] == "XMLHttpRequest" :
                false;       
    }
}
