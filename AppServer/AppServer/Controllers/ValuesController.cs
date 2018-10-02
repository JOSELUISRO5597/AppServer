using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet,Authorize]
        public IEnumerable<string> Get()
        {
            var currentUser = HttpContext.User;
            int userId = 0;

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var idString= currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                userId = Int32.Parse(idString);
            }

            Conexion conexion = new Conexion();
            try
            {
                conexion.openConnection();
                return new string[] { "Connection", "ok" };
            }
            catch
            {
                Console.WriteLine("ERROR");
                
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }

            return new string[] { "Connection", "error" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value " + id;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
