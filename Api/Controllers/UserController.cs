using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken ct)
    {
        var users = await userService.GetAllUsersAsync(ct);
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
    {
        var user = await userService.GetUserByIdAsync(id, ct);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreaterUserDto dto, CancellationToken ct)
    {
        var createdUser = await userService.CreateUserAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreaterUserDto dto, CancellationToken ct)
    {
        await userService.UpdateUserAsync(id, dto, ct);
        return NoContent();
    }

    [HttpPut("deactivate/{id:int}")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        await userService.DeactivateUserAsync(id, ct);
        return NoContent();
    }

    [HttpPut("activate/{id:int}")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        await userService.ActivateUserAsync(id, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await userService.DeleteUserAsync(id, ct);
        return NoContent();
    }

    [HttpPost("import-external")]
    public async Task<IActionResult> ImportExternal(CancellationToken ct)
    {
        await userService.ImportExternalUsersAsync(ct);
        return Ok(new { message = "Users imported successfully." });
    }
}