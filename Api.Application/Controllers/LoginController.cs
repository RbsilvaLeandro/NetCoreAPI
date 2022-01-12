using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.DTO;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LoginController : ControllerBase
    {
        public readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (user == null)
                return BadRequest(ModelState);

            try
            {
                var result = await _loginService.FindByLogin(user);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
