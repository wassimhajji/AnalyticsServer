﻿using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersConnectionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(Cache.UsersConnectionCache.GetAllUsersAndConnections());
        }
    }
}