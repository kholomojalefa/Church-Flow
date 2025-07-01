using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ChurchFlowAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();
    }
}
