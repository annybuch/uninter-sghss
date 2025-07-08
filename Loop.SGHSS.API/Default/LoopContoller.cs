using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Default
{
    public class LoopContoller : ControllerBase
    {
        protected async Task<IActionResult> Created(Task callback)
        {
            await callback;
            return StatusCode(StatusCodes.Status201Created);
        }

        protected async Task<IActionResult> Ok(Task callback)
        {
            await callback;
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
