using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Dto;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]

    public class AccountController :ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _context;

        private readonly SignInManager<User> _signInManager;

        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager , ApplicationDBContext context , ITokenService tokenService , SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
         try
         {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            var appUser = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            if (string.IsNullOrEmpty(registerDto.Password))
            {
   
           throw new ArgumentException("Password cannot be null or empty.", nameof(registerDto.Password));
            }

            var createUser = await _userManager.CreateAsync(appUser , registerDto.Password);
            if(createUser.Succeeded) {
               var roleResult = await _userManager.AddToRoleAsync(appUser , "User");
               if(roleResult.Succeeded)
               {
                return Ok(
                     new NewUserDto 
                     {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.createToken(appUser)
                     }
                );
               } else {
                return StatusCode(500 , "Failed to Set Role");
               }
            }
           return StatusCode(500, "Failed to create user: " + string.Join(", ", createUser.Errors.Select(e => e.Description)));
            
         }
         catch (Exception e)
         
         {
          return StatusCode(500 , e);
         }
        }

        [HttpPost("login")]
public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
{
    try
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (result.Succeeded)
        {
            return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.createToken(user)
                }
            );
        }

        return Unauthorized("Invalid username or password.");
    }
    catch (Exception e)
    {
        return StatusCode(500, e.Message);
    }
}


    }
}