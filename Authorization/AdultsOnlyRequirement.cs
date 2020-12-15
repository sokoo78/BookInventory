using Microsoft.AspNetCore.Authorization;

namespace BookInventory.Authorization
{
    public class AdultsOnlyRequirement : IAuthorizationRequirement
    {
        public int RequiredMinimumAge { get; }

        public AdultsOnlyRequirement(int requiredMinimumAge)
        {
            RequiredMinimumAge = requiredMinimumAge;
        }
    }
}