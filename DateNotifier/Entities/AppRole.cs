using Microsoft.AspNetCore.Identity;

namespace DateNotifier.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name)
        {
        }
    }
}
