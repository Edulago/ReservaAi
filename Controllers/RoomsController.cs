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
        [HttpGet("v1/rooms")]
        public async Task<IActionResult> GetAsync([FromServices] ReservaDataContext context)
        {
            try
            {
                var rooms = await context.Rooms.ToListAsync();
                var result = rooms.Select(RoomViewModel.FromRoom).ToList();
                return Ok(new ResultViewModel<List<RoomViewModel>>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("v1/rooms/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            try
            {
                var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
                if (room == null)
                {
                    return NotFound(new ResultViewModel<string>("Room not found"));
                }
                return Ok(new ResultViewModel<RoomViewModel>(RoomViewModel.FromRoom(room)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost("v1/rooms")]
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

                return Created($"/v1/rooms/{room.Id}", new ResultViewModel<RoomViewModel>(RoomViewModel.FromRoom(room)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPut("v1/rooms/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] ReservaDataContext context, [FromBody] EditorRoomViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>("Invalid model"));
            }
            var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if(room == null)
                {
                    return NotFound(new ResultViewModel<RoomViewModel>("Sala não encontrada."));
                }
                room.Name = model.Name;
                room.Capacity = model.Capacity;
                room.Description = model.Description;
                room.Status = model.Status;

                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<RoomViewModel>(RoomViewModel.FromRoom(room)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }
        [HttpDelete("v1/rooms/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] ReservaDataContext context, [FromRoute] int id)
        {
            var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if(room == null)
                {
                    return NotFound(new ResultViewModel<RoomViewModel>("Sala não encontrada."));
                }
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<RoomViewModel>(RoomViewModel.FromRoom(room)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("v1/rooms/available")]
        public async Task<IActionResult> GetAvailableRoomsAsync([FromServices] ReservaDataContext context, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var availableRooms = await context.Rooms
                    .Where(r => !context.Reservations.Any(res => res.RoomId == r.Id && res.StartTime < end && res.FinishTime > start))
                    .ToListAsync();
                var result = availableRooms.Select(RoomViewModel.FromRoom).ToList();
                return Ok(new ResultViewModel<List<RoomViewModel>>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<string>(ex.Message));
            }
        }   
    }
}
