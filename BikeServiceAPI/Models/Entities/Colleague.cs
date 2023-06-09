﻿using BikeServiceAPI.Enums;
using BikeServiceAPI.Models.DTOs;

namespace BikeServiceAPI.Models.Entities;

public class Colleague : Person
{
    public SkillLevel SkillLevel { get; set; }

    public List<ServiceEvent>? ServiceEvents { get; set; } = new List<ServiceEvent>();

    public Colleague(string name, string email, string password, string phone,
        string? introduction = null) : base(name, email, password, phone, introduction)
    {
    }

    public Colleague(ColleagueDto dto) : base(dto.Name, dto.Email, dto.Password, dto.Phone, dto.Introduction)
    {
        Id = dto.Id;
        SkillLevel = Enum.Parse<SkillLevel>(dto.SkillLevel);
        ServiceEvents = dto.ServiceEvents.Select(eventDto => new ServiceEvent(eventDto)).ToList();
        Roles = dto.Roles.Select(r => Enum.Parse<Role>(r)).ToList();
    }
}