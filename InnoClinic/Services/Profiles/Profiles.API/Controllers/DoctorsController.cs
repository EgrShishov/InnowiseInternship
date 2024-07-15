﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class DoctorsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public DoctorsController(IMediator mediator, IMapper mapper, ILogger logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("create-profile")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateDoctorProfile(CreateDoctorRequest request, int AccountId)
    {
        var command = _mapper.Map<CreateDoctorCommand>((AccountId, request));
        var result = await _mediator.Send(command);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var command = new DeleteDoctorCommand(id);
        var result = await _mediator.Send(command);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Receptionist, Doctor")]
    public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorRequest request)
    {
        var command = _mapper.Map<UpdateDoctorCommand>((id, request));
        var result = await _mediator.Send(command);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpGet("by-office")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorsByOffice(int officeId, int pageNumber = 1, int pageSize = 10)
    {
        var query = new FilterByOfficeQuery(officeId, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpGet("by-specialization")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorsBySpecialization(int specializationId, int pageNumber = 1, int pageSize = 10)
    {
        var query = new FilterBySpecializationQuery(specializationId, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpGet("search-by-name")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> SearchDoctorsByName(SearchByNameRequest request, int pageNumber = 1, int pageSize = 10)
    {
        var query = _mapper.Map<SearchByNameQuery>((request, pageNumber, pageSize));
        var result = await _mediator.Send(query);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var query = new ViewByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetAllDoctors(int pageNumber = 1, int pageSize = 10)
    {
        var query = new ViewDoctorsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        return Ok(result.Value);
    }
    }
