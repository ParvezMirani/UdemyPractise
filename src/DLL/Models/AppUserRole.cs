using System;
using DLL.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DLL.Models
{
    public class AppUserRole : IdentityUserRole<int>, ITrackable, ISoftDeletable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
