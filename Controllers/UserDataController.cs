using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers;

[ApiController]
[Route("user")]
public class UserDataController : ControllerBase
{
    [HttpPost("register")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> Create(
        [FromBody]UserDataRegister user,
        [FromServices]IUserDataService service)
    {
        var errors = new List<string>();
        if(user is null || user.UserName is null)
            errors.Add("É necessário informar um nome");
        
        if(errors.Count > 0)
            return BadRequest(errors);
        
        await service.Create(user);
        return Ok();
    }

    [HttpPost("login")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> Login(
        [FromBody]UserDataLogin user,
        [FromServices]IUserDataService service,
        [FromServices]ISecurityService security)
    {
        var loggedUser = await service.GetByLogin(user.UserName);

        if(loggedUser is null)
            return Unauthorized("Usuário não existe");

        var password = await security.HashPassword(loggedUser.UserName, loggedUser.Salt);
        System.Console.WriteLine("Senha input: ", password.ToString());
        var realPassword = loggedUser.UserPassword;
        System.Console.WriteLine("Senha banco: ", realPassword.ToString());

        if(password != realPassword)
            return BadRequest("Senha incorreta");
        
        return Ok(user);
    }
}
