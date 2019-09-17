using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Models;
using ProductRatingApi.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserService _UserService;

        public AuthenticateController(UserService userService)
        {
            _UserService = userService;
        }



        [HttpPost]
        public ActionResult<string> Authenticate(Users userdata)
        {
            if (string.IsNullOrEmpty(userdata.Email) || string.IsNullOrEmpty(userdata.Password))
                return NotFound();

            var user = _UserService.Get().SingleOrDefault(x => x.Email == userdata.Email);

            if (user == null)
            {
                return NotFound("No user exist. Please register");
            }

            if (userdata.Password == null)
                return null;

            // check if password is correct
            if (VerifyPasswordHash(userdata.Password, user.Hash, user.Salt))
            {
                //Authentication successful, Issue Token with user credentials
                //Provide the security key which was given in the JWToken configuration in Startup.cs
                var key = Encoding.ASCII.GetBytes
                          ("WQsmfqc2CLrnjwc-gbgqD2HQEU5JcytWdqfmw0lmmofFD_hvhZ-hlpLRfEkVCN-8GR-6BkXnEQehHFhqeVxOrA");
                //Generate Token for user 
                List<Claim> claimList = new List<Claim>();
                var Email = new Claim("UserEmail", user.Email);
                var UserName = new Claim("UserName", user.UserName);
                var UserID = new Claim("_id", user.Id);
                var isAdmin = new Claim("isAdmin",user.Admin.ToString());
                claimList.Add(Email);
                claimList.Add(UserName);
                claimList.Add(UserID);
                claimList.Add(isAdmin);
                //claimList.Add(user.Admin)
                var JWToken = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claimList,

                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddHours(5)).DateTime,
                    //Using HS512 Algorithm to encrypt Token
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha512)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

                HttpContext.Session.SetString("JWToken", token);
                //Response.Headers.Add("x-authToken", token);
                return token;
            }

            return NotFound("User Name or Password is invalid.");
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            byte[] saltBytes = storedSalt;
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8); ;
            return hashed.SequenceEqual(storedHash);
        }
    }
}