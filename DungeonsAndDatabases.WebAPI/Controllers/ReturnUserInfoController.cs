﻿using DungeonsAndDatabases.Data;
using DungeonsAndDatabases.Models.UserInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DungeonsAndDatabases.WebAPI.Controllers
{
    public class ReturnUserInfoController : ApiController
    {
        //This endpoint is for debugging and testing only, it should be
        //Disabled if deploying this API in a public facing manner
 
        /// <summary>
        /// Get User Info
        /// </summary>
        /// <returns>Returns the GUID,UserName and PlayerName associated with the Token passed in the header</returns>
        public async Task<IHttpActionResult> GetUserInfoAsync()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            string username = User.Identity.GetUserName();
            var ctx = new ApplicationDbContext();
            var player = await ctx.Players
                .FirstOrDefaultAsync(e => e.PlayerID == userId);
            var info = new UserInfo
            {
                PlayerName = player.PlayerName,
                UserName = username,
                UserId = userId
            };
            return Ok(info);
        }
    }
}
