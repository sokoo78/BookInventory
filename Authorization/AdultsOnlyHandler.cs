using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BookInventory.Models;
using BookInventory.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookInventory.Authorization
{

    public class AdultsOnlyHandler : AuthorizationHandler<AdultsOnlyRequirement>
    {
        private readonly IDataService _dataService;

        public AdultsOnlyHandler(IDataService dataService)
        {
            _dataService = dataService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AdultsOnlyRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
                                            c.Issuer == "https://localhost:5001/"))
            {
                return Task.FromResult(0);
            }

            if (!context.TryGetParamValue<int>("Id", out var Id))
            {
                return Task.FromResult(0);
            }

            Book book = _dataService.GetBook(Id).Result;
            var adultsOnlyEvent = book.AgeLimit > requirement.RequiredMinimumAge;

            var dateOfBirth = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth &&
                                            c.Issuer == "https://localhost:5001/").Value);

            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (!adultsOnlyEvent || calculatedAge >= requirement.RequiredMinimumAge)
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }

}