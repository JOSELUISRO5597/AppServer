using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppServer.Helpers;
using AppServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppServer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        DALUserHelper userHelper = new DALUserHelper();
        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }


        // GET: api/<controller>
        [HttpGet, Authorize]
        public IActionResult GetAll()
        {
            return Ok(userHelper.getUsers());
            
        }

        // GET api/<controller>/5
        [HttpGet("{id}"), Authorize]
        public IActionResult Get(int id)
        {
            return Ok(userHelper.getUser(id));
        }

        // POST api/<controller>/Login
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginPost([FromBody]User user)
        {
            var userDB = userHelper.LoginUser(user.Email, user.Password);

            if (userDB != null)
            {
                userDB.Token = TokenController.CreateToken(_config, userDB);

                return Ok(userDB);
            }
            else
                return BadRequest("Email or pass incorrectas");

            
        }

        // POST api/<controller>/Register
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody]User user)
        {
            return Ok(userHelper.RegisterUser(user));
        }

        [HttpPost]
        [Route("CheckUserEmail")]
        public IActionResult CheckUserEmail([FromBody]User user)
        {
            return Ok(userHelper.CheckUserEmail(user.Email));
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromBody]User user)
        {
            return Ok(userHelper.ResetPassword(user.Email));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
