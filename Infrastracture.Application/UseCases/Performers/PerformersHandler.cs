using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public class PerformersHandler: IRequestHandler<PerformersRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    private readonly IRegion _region;
    private readonly IMapper _mapper;
    private readonly IOffice _office;
    private readonly IPlaceOfWork _placeOfWork;

    public PerformersHandler(IUser user, IRole role, IMapper mapper, IRegion region, IOffice office, IPlaceOfWork placeOfWork)
    {
        _user = user;
        _role = role;
        _mapper = mapper;
        _region = region;
        _office = office;
        _placeOfWork = placeOfWork;
    }
    
    public async Task<Response> Handle(PerformersRequest request, CancellationToken cancellationToken)
    {
        var user = await _user.GetUserByUserId((Guid)request.UserId);
        var role = await _role.GetRoleByUser(user);
        
        if (role.Name != "admin")
            return new Response("", 404);
        
        var regionId = _region.GetRegionIdByUserId((Guid)request.UserId);
        
        var performers = _user.GetUsersByRegionId(await regionId).Result.Where(u => _role.GetRoleByUser(u).Result.Name == "performer").ToList();
        if (request.Fullname != null)
            performers = performers.Where(p => $"{p.Name} {p.Surname} {p.Patronymic}".Contains(request.Fullname)).ToList();
        if (request.OfficeId != null)
        {
            var placeOfWorks = _placeOfWork.GetByOfficeId((Guid)request.OfficeId).Result;
            performers = performers.Where(p => _placeOfWork.GetByUserId(p.Id).Result.Any(item => placeOfWorks.Contains(item))).ToList();
        }
            
        
        var paginationDTO = new PaginationDTO();
        paginationDTO.PageIndex = request.Page;
        paginationDTO.TotalPages = performers.Count / 20 + 1;
        paginationDTO.TotalRecords = performers.Count;

        var contents = new List<ContentDTO>();
        foreach (var performer in performers)
        {
            var placeOfWork = _placeOfWork.GetByUserId(performer.Id).Result;
            var content = new ContentDTO();
            content.Id = performer.Id;
            content.Name = performer.Name;
            content.Surname = performer.Surname;
            content.Patronymic = performer.Patronymic;
            // var content = _mapper.Map(perforver, new ContentDTO());
            var offices = new List<string>();
            foreach (var e in placeOfWork)
            {
                var office = _office.GetOfficeById(e.OfficeId).Result;
                offices.Add($"{office.City}, {office.Address}");
            }
            content.Office = offices;
            content.Category = ["1", "2", "3"];
            contents.Add(content);
        }

        var result = new PerformersDTO();
        result.Pagination = paginationDTO;
        var length = 20;
        if (request.Page * length > performers.Count)
            length = performers.Count - (request.Page - 1) * length;
        result.Content = contents.Slice(20 * (request.Page - 1), length);
        return new Response("Performers", 200, result);
    }
}