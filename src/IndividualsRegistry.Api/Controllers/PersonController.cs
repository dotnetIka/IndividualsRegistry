using IndividualsRegistry.Application.Features.Person.Commands.AddRelatedPerson;
using IndividualsRegistry.Application.Features.Person.Commands.CreatePerson;
using IndividualsRegistry.Application.Features.Person.Commands.DeletePerson;
using IndividualsRegistry.Application.Features.Person.Commands.RemoveRelatedPerson;
using IndividualsRegistry.Application.Features.Person.Commands.UpdatePerson;
using IndividualsRegistry.Application.Features.Person.Queries.GetPerson;
using IndividualsRegistry.Application.Features.Person.Queries.GetPersonList;
using IndividualsRegistry.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IndividualsRegistry.Api.Controllers;

[ApiController]
[Route("api/v1/Person")]
public class PersonController : ApiController
{

    public PersonController(ISender sender)
           : base(sender)
    {
    }

    [HttpGet("{id}", Name = "GetPerson")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetPersonQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetPersonListQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { Id = id }, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new DeletePersonCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("relations")]
    public async Task<IActionResult> AddRelation(AddRelatedPersonCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("relations/{relationshipId}")]
    public async Task<IActionResult> RemoveRelation(int relationshipId, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new RemoveRelatedPersonCommand(relationshipId), cancellationToken);
        return HandleResult(result);
    }
}