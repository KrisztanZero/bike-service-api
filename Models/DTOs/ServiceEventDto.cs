using BikeServiceAPI.Models.Entities;

namespace BikeServiceAPI.Models.DTOs;

public class ServiceEventDto
{
    public long Id { get; set; }

    public string Type { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public double Price { get; set; }
    public BikeDto Bike { get; set; }
    public ColleagueDto Colleague { get; set; }

    public ServiceEventDto(ServiceEvent serviceEvent)
    {
        Id = serviceEvent.Id;
        Type = serviceEvent.Type.ToString();
        Start = serviceEvent.Start;
        End = serviceEvent.End;
        Price = serviceEvent.Price;
        Bike = new BikeDto(serviceEvent.Bike);
        Colleague = new ColleagueDto(serviceEvent.Colleague);
    }
    
}