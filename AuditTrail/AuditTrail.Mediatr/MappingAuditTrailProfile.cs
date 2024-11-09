using AuditTrail.Domain.Models;
using AutoMapper;
using CommonModule.Shared.Responses.AuditTrail;

namespace AuditTrail.Mediatr;

public class MappingAuditTrailProfile : Profile
{
    public MappingAuditTrailProfile()
    {
        this.CreateMap<AuditTrailEntity, AuditTrailResponse>();
    }
}