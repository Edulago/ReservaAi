using ReservaAi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ReservaAi.Data;
using ReservaAi.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservaAi.Controllers
{
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [HttpGet("api/rooms")]
        public async Task<IActionResult> GetAsync([FromServices] ReservaDataContext context)
        {
            try
            {
                var rooms = await context.Rooms.ToListAsync();
                return Ok(new ResultViewModel<List<Room>>(rooms));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("api/rooms/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            try
            {
                var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
                if (room == null)
                {
                    return NotFound(new ResultViewModel<string>("Room not found"));
                }
                return Ok(new ResultViewModel<Room>(room));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost("api/rooms")]
        public async Task<IActionResult> PostAsync([FromServices] ReservaDataContext context, [FromBody] EditorRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>("Invalid model"));
            }
            try
            {
                var room = new Room
                {
                    Name = model.Name,
                    Capacity = model.Capacity,
                    Description = model.Description,
                    Status = model.Status,
                    Created_at = DateTime.UtcNow
                };

                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();

                return Created($"api/rooms/{room.Id}", new ResultViewModel<Room>(room));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPut("api/rooms/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] ReservaDataContext context, [FromBody] EditorRoomViewModel model, [FromRoute] int id)
        {
            var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if(room == null)
                {
                    return NotFound(new ResultViewModel<Room>("Sala não encontrada."));
                }
                room.Name = model.Name;
                room.Capacity = model.Capacity;
                room.Description = model.Description;
                room.Status = model.Status;

                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Room>(room));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }
        [HttpDelete("api/rooms/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if(room == null)
                {
                    return NotFound(new ResultViewModel<Room>("Sala não encontrada."));
                }
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Room>(room));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("/api/rooms/available?start=&end=")]
        public async Task<IActionResult> GetAvailableRoomsAsync([FromServices] ReservaDataContext context, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var availableRooms = await context.Rooms
                    .Where(r => !context.Reservations.Any(res => res.RoomId == r.Id && res.StartTime < end && res.FinishTime > start))
                    .ToListAsync();
                return Ok(new ResultViewModel<List<Room>>(availableRooms));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }   
    }
}
