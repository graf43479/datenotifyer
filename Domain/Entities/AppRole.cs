using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name)
        {
        }
    }
}
