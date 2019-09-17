using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductRatingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;


        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<Users>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "Getuser")]
        public ActionResult<Users> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Users> Create(Users user)
        {
            if (_userService.Get().Any(x => x.Email == user.Email))
                return BadRequest("Username \"" + user.Email + "\" is already taken");

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hashed = KeyDerivation.Pbkdf2(
                 password: user.Password,
                 salt: salt,
                 prf: KeyDerivationPrf.HMACSHA1,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8);
            user.Hash = hashed;
            user.Salt = salt;
            byte[] hasedPassword = hashed.Concat(salt).ToArray();

            user.Password = Convert.ToBase64String(hasedPassword);

            _userService.Create(user);

            return CreatedAtRoute("Getuser", new { id = user.Id.ToString() }, user);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Users userIn)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }

        //public static string HashPasswordUsingPBKDF2(string password)
        //{
        //    var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
        //    {
        //        IterationCount = 10000
        //    };

        //    byte[] hash = rfc2898DeriveBytes.GetBytes(20);

        //    byte[] salt = rfc2898DeriveBytes.Salt;


        //}
    }
}