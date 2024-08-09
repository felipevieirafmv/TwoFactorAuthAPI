using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using Google.Authenticator;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers;

[ApiController]
[Route("user")]
public class UserDataController : ControllerBase
{
    private readonly TwoFactorAuthenticator _tfa;

    public UserDataController() {
        this._tfa = new TwoFactorAuthenticator();
    }

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
        var realPassword = loggedUser.UserPassword;

        if(password != realPassword)
            return BadRequest("Senha incorreta");
               
        return Ok(GenerateQR(loggedUser.Email));
    }

    private ActionResult<string> GenerateQR(string email) 
    {
        string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        Console.WriteLine(key);
        SetupCode setupInfo = _tfa.GenerateSetupCode("Two Factor Auth", email, key, false, 3);

        return setupInfo.QrCodeSetupImageUrl;
    }

    [HttpPost("validatecode")] 
    public ActionResult<bool> ValidateCode(
        [FromBody]ValidateCode validateJson)
    {
        if(!_tfa.ValidateTwoFactorPIN(validateJson.Key, validateJson.Code))
            return BadRequest("Code is not valid");

        return Ok("Code is valid");
    }
}
