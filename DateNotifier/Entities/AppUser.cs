using Microsoft.AspNetCore.Identity;

namespace DateNotifier.Entities
{
    public class AppUser : IdentityUser
    {        
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
