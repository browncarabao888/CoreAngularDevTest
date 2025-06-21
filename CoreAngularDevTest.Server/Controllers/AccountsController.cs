using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.Accounts;
using CoreAngularDevTest.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreAngularDevTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUser _userService;

        #region Constructor
        public AccountsController(IUser userService )
        {
            _userService = userService;
        }
        #endregion


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AccountDTO newAccount, CancellationToken token)
        {
            
            if ( await _userService.IsEmailConsumed(newAccount?.Emailaddress, token))
                    return BadRequest("Email address is already registered");


            var result = await _userService.AddAccountsAsync(newAccount, token);

            if (result == -1)  
                return Conflict(ApiResponse<string>.Fail("Email is already in use.", 1001));

            if (result == 0)  
                return BadRequest(ApiResponse<string>.Fail("Account creation failed.", 1002));

            return Ok(ApiResponse<int>.Ok((int)result));  
        }


        [HttpGet("passkey")]
        public async Task<IActionResult> Reset(string passkey, CancellationToken token)
        {
            var result = await _userService.ResetKeyAsync(passkey, token);

            if (result <= 0)
                return BadRequest("failed");

            return Ok(result);
        }

        [HttpPost("Validate")]
        public async Task<IActionResult> Validate([FromBody] LoginDTO loginDTO, CancellationToken token)
        {
            var result = await _userService.ValidateAsync(loginDTO, token);

            if (result <= 0)
                return BadRequest("failed");

            return Ok(result);
        }


    }
}
