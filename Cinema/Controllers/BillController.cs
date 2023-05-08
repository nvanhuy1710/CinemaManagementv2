using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        private readonly IUserService _userService;

        public BillController(IBillService billService, IUserService userService)
        {
            _billService = billService;
            _userService = userService;
        }

        [HttpPost("bill")]
        public IActionResult AddBill([FromBody] BillDTO billDTO)
        {
            UserDTO userDTO = GetCurrentUser();
            if (userDTO != null)
            {
                billDTO.UserId = userDTO.Id;
                BillDTO result = _billService.AddBill(billDTO);
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet("bill")]
        public IActionResult GetCurrentUserBills()
        {
            UserDTO userDTO = GetCurrentUser();
            if(userDTO != null)
            {
                List<BillDTO> billDTOs = _billService.GetBillByUserId(userDTO.Id);
                return Ok(billDTOs);
            }
            return Unauthorized();
        }

        [NonAction]
        private UserDTO GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return _userService.GetUserByEmail(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            return null;
        }
    }
}
