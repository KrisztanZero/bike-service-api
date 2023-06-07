using BikeServiceAPI.Models.DTOs;
using BikeServiceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ServiceEventController : Controller
{
    private readonly IServiceEventService _serviceEventService;

    public ServiceEventController(IServiceEventService serviceEventService)
    {
        _serviceEventService = serviceEventService;
    }

    [HttpGet]
    public async Task<List<ServiceEventDto>> GetAll()
    {
        return await _serviceEventService.GetAllServiceEvent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventServiceById(long id)
    {
        try
        {
            return Ok(await _serviceEventService.GetServiceEventById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.StackTrace);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddServiceEvent([FromBody] ServiceEventDto serviceEventDto)
    {
        return Ok(await _serviceEventService.AddServiceEvent(serviceEventDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateServiceEvent([FromBody] ServiceEventDto serviceEventDto)
    {
        try
        {
            return Ok(await _serviceEventService.UpdateServiceEvent(serviceEventDto));
        }
        catch (Exception e)
        {
            return BadRequest(e.StackTrace);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceEvent(long id)
    {
        try
        {
            return Ok(await _serviceEventService.DeleteServiceEvent(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.StackTrace);
        }
    }
}