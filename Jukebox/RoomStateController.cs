using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jukebox.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomStateController : ControllerBase
    {

        RoomState roomState = new RoomState();
        bool songPlaying = false;

        [HttpGet("/getRoomState")]
        public async Task<ActionResult> Get()
        {
            return Ok(roomState);
        }

        [HttpPost("/postRoomState")]

        public async Task<ActionResult> Send(RoomState room)
        {
            roomState = room;
            return Ok();
        }

    }
}