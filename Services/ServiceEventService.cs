using BikeServiceAPI.Models;
using BikeServiceAPI.Models.DTOs;
using BikeServiceAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeServiceAPI.Services;

public class ServiceEventService : IServiceEventService
{
    private readonly BikeServiceContext _context;

    public ServiceEventService(BikeServiceContext context)
    {
        _context = context;
    }

    public async Task<int> AddServiceEvent(ServiceEventDto serviceEventDto)
    {
        var serviceEvent = new ServiceEvent(serviceEventDto);
        _context.ServiceEvents.Add(serviceEvent);
        return await _context.SaveChangesAsync();
    }

    public async Task<ServiceEventDto> GetServiceEventById(long id)
    {
        var serviceEvent = await GetServiceEventEntityById(id);
        return new ServiceEventDto(serviceEvent);
    }

    public async Task<int> UpdateServiceEvent(ServiceEventDto serviceEventDto)
    {
        var serviceEvent = await GetServiceEventEntityById(serviceEventDto.Id);
        var updateServiceEvent = new ServiceEventDto(serviceEvent);
        _context.Entry(serviceEvent).CurrentValues.SetValues(updateServiceEvent);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteServiceEvent(long id)
    {
        var serviceEvent = await GetServiceEventEntityById(id);
        _context.Remove(serviceEvent);
        return await _context.SaveChangesAsync();
    }

    public async Task<List<ServiceEventDto>> GetAllServiceEvent()
    {
        var serviceEventList = await _context.ServiceEvents.ToListAsync();
        return serviceEventList.Select(@event => new ServiceEventDto(@event)).ToList();
    }

    private async Task<ServiceEvent> GetServiceEventEntityById(long id)
    {
        return await _context.ServiceEvents.FirstOrDefaultAsync(service => service.Id == id) ??
               throw new InvalidOperationException($"Service-event with {id} id not exist.");
    }
}