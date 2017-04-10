using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HubEx.Service.ES.Api.RouteConstraints
{
    public class EnumConstraint : IRouteConstraint
    {
        public const string ROUTE_LABEL = "enum";

        private static readonly ConcurrentDictionary<string, string[]> _cache = new ConcurrentDictionary<string, string[]>();
        private readonly string[] _validOptions;
        /// <summary>
        /// Create new <see cref="EnumConstraint"/>
        /// </summary>
        /// <param name="enumType"></param>
        public EnumConstraint(string enumType)
        {
            _validOptions = _cache.GetOrAdd(enumType, key =>
            {
                var type = Type.GetType(key);
                return type != null ? Enum.GetNames(type) : new string[0];
            });
        }
        /// <inheritdoc />
        public Boolean Match(HttpContext httpContext, IRouter route, String routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;
            if (values.TryGetValue(routeKey, out value) && value != null)
            {
                return _validOptions.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
