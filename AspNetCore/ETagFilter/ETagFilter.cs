using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SmartService.Common.Extensions;

namespace HubEx.Service.ES.Api.FIlters
{
    public class ETagFilterAttribute : ResultFilterAttribute, IResultFilter
    {

        private static string GenerateETag(string uri, IActionResult result)
        {
            return result == null ? null : $"{uri}@{result.ToJson()}".ToHashSha256();
        }
        
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context?.Result == null || context?.HttpContext == null)
                return;

            var ctx = context.HttpContext;
            if (ctx.Response.StatusCode != (int)HttpStatusCode.OK || context?.Result == null)
                return;

            var eTagHash = GenerateETag(ctx.Request.Path, context.Result);

            if (String.IsNullOrWhiteSpace(eTagHash))
                return;

            ctx.Response.Headers.Add(HeaderNames.ETag, eTagHash);

            if (ctx.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out StringValues requestedETag)
                && requestedETag.Any(x => String.CompareOrdinal(x, eTagHash) == 0))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                return;
            }
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
