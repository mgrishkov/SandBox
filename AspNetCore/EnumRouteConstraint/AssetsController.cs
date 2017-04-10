using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HubEx.DataProvider.Abstractions.Enums;
using HubEx.DataProvider.Abstractions.Interfaces.Core;
using HubEx.DataProvider.Abstractions.Interfaces.Helpers;
using HubEx.DataProvider.Exceptions;
using HubEx.Exceptions.Handling.Models;
using HubEx.Service.Common.AspNetCore.Attributes;
using HubEx.Service.Common.AspNetCore.Controllers;
using HubEx.Service.ES.Api.Enums;
using HubEx.Service.ES.Api.Results.Assets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SmartService.Common.AspNetCore.Constants;
using SmartService.Common.Swagger.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HubEx.Service.ES.Api.Controllers
{
    [Route("[controller]")]
    public partial class AssetsController : HubExBaseController
    {
        private readonly IAssetHelper _assetHelper;

        public AssetsController(ILogger<AssetsController> logger, IHelperResolver helperResolver) : base(logger, helperResolver)
        {
            _assetHelper = helperResolver.CreateInstance<IAssetHelper>();
        }

        [HttpGet("root/assets")]
        [RequiresPermission(PermissionsApi.AssetsList)]
        #region Swagger
        [SwaggerHeader(HeaderNames.Authorization                , AuthenticationSchemeNames.BEARER)]
        [SwaggerResponse((int)HttpStatusCode.OK                 , Type = typeof(Dictionary<int, AssetResult>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(List<ErrorModel>))]
        #endregion
        public async Task<IActionResult> ListRoots()
        {
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                (var assets, var taskActualities) = await _assetHelper.ListRootAsync(TenantID.Value, TenantMemberID.Value);
                // ReSharper restore PossibleInvalidOperationException

                var result = BuildResut(assets, taskActualities);

                if (result == null)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorObject.StatusCode = (int)HttpStatusCode.InternalServerError;
                ErrorObject.Add(ex);
                return ErrorObject;
            }
        }
        

        [HttpGet("{parentAssetID:int}/assets/{extension:enum(HubEx.Service.ES.Api.Enums.RouteExtension)?}")]
        [RequiresPermission(PermissionsApi.AssetsList)]
        #region Swagger
        [SwaggerHeader(HeaderNames.Authorization, AuthenticationSchemeNames.BEARER)]
        [SwaggerResponse((int)HttpStatusCode.OK                 , Type = typeof(Dictionary<int, AssetResult>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest         , Type = typeof(List<ErrorModel>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(List<ErrorModel>))]
        #endregion
        public async Task<IActionResult> ListDescendants([FromRoute] int parentAssetID, [FromRoute] RouteExtension extension)
        {
            if (parentAssetID <= 0)
            {
                ErrorObject.Add(new ArgumentNullException(nameof(parentAssetID)));
                return ErrorObject;
            }

            try
            {
                // ReSharper disable PossibleInvalidOperationException
                (var assets, var taskActualities) = extension == RouteExtension.All ? await _assetHelper.ListDescendantsAsync(TenantID.Value, TenantMemberID.Value, parentAssetID)
                                                  : await _assetHelper.ListChildrenAsync(TenantID.Value, TenantMemberID.Value, parentAssetID);
                // ReSharper restore PossibleInvalidOperationException

                var result = BuildResut(assets, taskActualities);

                if (result == null || !result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (AssetDeletedException ex)
            {
                ErrorObject.Add(ex);
                return ErrorObject;
            }
            catch (AssetArchivedException ex)
            {
                ErrorObject.Add(ex);
                return ErrorObject;
            }
            catch (Exception ex)
            {
                ErrorObject.StatusCode = (int) HttpStatusCode.InternalServerError;
                ErrorObject.Add(ex);
                return ErrorObject;
            }
        }
    }
}
