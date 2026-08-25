using ReservaAi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservaAi.Data;
using ReservaAi.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservaAi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("v1/users")]
        public async Task<IActionResult> GetAsync([FromServices] ReservaDataContext context)
        {
            try
            {
                var users = await context.Users.ToListAsync();
                var result = users.Select(UserViewModel.FromUser).ToList();
                return Ok(new ResultViewModel<List<UserViewModel>>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("v1/users/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return NotFound(new ResultViewModel<UserViewModel>("User not found"));
                }
                return Ok(new ResultViewModel<UserViewModel>(UserViewModel.FromUser(user)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost("v1/users")]
        public async Task<IActionResult> CreateAsync([FromServices] ReservaDataContext context, [FromServices] IPasswordHasher<User> passwordHasher, [FromBody] EditorUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>("Invalid data"));
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new ResultViewModel<string>("A senha é obrigatória"));
            }
            try
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    FullName = model.FullName,
                    Birthdate = model.Birthdate,
                    Created_at = DateTime.UtcNow
                };
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Created($"/v1/users/{user.Id}", new ResultViewModel<UserViewModel>(UserViewModel.FromUser(user)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPut("v1/users/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] ReservaDataContext context, [FromServices] IPasswordHasher<User> passwordHasher, [FromRoute] int id, [FromBody] EditorUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>("Invalid data"));
            }
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new ResultViewModel<UserViewModel>("User not found"));
            }
            try
            {
                user.Username = model.Username;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.Birthdate = model.Birthdate;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
                }

                context.Users.Update(user);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<UserViewModel>(UserViewModel.FromUser(user)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpDelete("v1/users/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new ResultViewModel<UserViewModel>("User not found"));
            }
            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<UserViewModel>(UserViewModel.FromUser(user)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }
    }
}
