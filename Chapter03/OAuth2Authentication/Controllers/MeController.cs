﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace OAuth2Authentication.Controllers
{
    public class MeController : ApiController
    {

        // GET api/<controller>
        public IEnumerable<object> Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            return identity.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}


