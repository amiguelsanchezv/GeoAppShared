using Geo.Application;
using Geo.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeoreferencePointController(IOperations operations, IMediator mediator) : ControllerBase
    {
        private readonly IOperations _operations = operations;
        private readonly IMediator _mediator = mediator;

        [HttpPost(Name = "Add GeoreferencePoint", Order = 1)]
        [EnableCors("AllowOrigin")]
        [ActionName("CreateGeoreferencePoint")]
        public async Task<IActionResult> RequestGeoreferencePoint([FromBody] GeoreferencePointApi georeferencePoint)
        {
            try
            {
                return Ok(await _operations.AddGeoreferencePoint(_mediator, georeferencePoint.GetGeoreferencePoint()));
            }
            catch (Exception e)
            {
                return GetException(e);
            }
        }

        [HttpGet(Name = "Get GeoreferencePoint", Order = 2)]
        [EnableCors("AllowOrigin")]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetGeoferencePoint()
        {
            try
            {
                return Ok(await _operations.GetAll(_mediator));
            }
            catch (Exception e)
            {
                return GetException(e);
            }
        }

        [HttpPost(Name = "Get GeoreferencePoint By Ratio", Order = 3)]
        [EnableCors("AllowOrigin")]
        [ActionName("GetByRatio")]
        public async Task<IActionResult> GetGeoferencePoint([FromBody] GeoreferencePointRatio georeferencePointRatio)
        {
            try
            {
                var response = await _operations.GetByRatio(_mediator, georeferencePointRatio.GetCoordinates(), georeferencePointRatio.Meters);
                response = response.Where(a => a.Distance <= georeferencePointRatio.Meters).ToList();
                if (georeferencePointRatio.ActivityId != null)
                {
                    await _operations.UpdateActivityLocations(_mediator, georeferencePointRatio.ActivityId ?? 0, [.. response], georeferencePointRatio.Location);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return GetException(e);
            }
        }

        [HttpPost(Name = "Get GeoreferencePoint By Two Ratio", Order = 4)]
        [EnableCors("AllowOrigin")]
        [ActionName("GetByTwoRatio")]
        public async Task<IActionResult> GetGeoferencePoint([FromBody] GeoreferencePointTwoRatio georeferencePointTwoRatio)
        {
            try
            {
                var response = await _operations.GetByTwoRatio(_mediator,
                    georeferencePointTwoRatio.GetCoordinates(), georeferencePointTwoRatio.InnerRadius, georeferencePointTwoRatio.OuterRadius);
                response = response.Where(a => a.Distance >= georeferencePointTwoRatio.InnerRadius && a.Distance <= georeferencePointTwoRatio.OuterRadius).ToList();
                if (georeferencePointTwoRatio.ActivityId != null)
                {
                    await _operations.UpdateActivityLocations(_mediator, georeferencePointTwoRatio.ActivityId ?? 0, [.. response], georeferencePointTwoRatio.Location);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return GetException(e);
            }
        }

        private ObjectResult GetException(Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
}
