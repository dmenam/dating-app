using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        [HttpGet] //Asynchronous method to not block the thread wile making the query, useful for scalability
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await context.Users.ToListAsync();
        }

        
        [Authorize]
        [HttpGet("{ID}")]//routes to api/users/n  where n is the ID of the user you want yo get from the database
        public async Task<ActionResult<AppUser>> GetUser(int ID){
            return await context.Users.FindAsync(ID);
        }
    }
}