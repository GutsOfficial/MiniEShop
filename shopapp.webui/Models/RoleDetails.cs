using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.webui.Identity;
using Microsoft.AspNetCore.Identity;

namespace shopapp.webui.Models
{
    public class RoleDetails
    {
            public IdentityRole Role { get; set; }
            public IEnumerable<User> Members { get; set; }
            public IEnumerable<User> NonMembers { get; set; }
    }
}