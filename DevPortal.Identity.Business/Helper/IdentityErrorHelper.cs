using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Identity.Business
{
    public static class IdentityErrorHelper
    {
        public static string GetIdentityError(IEnumerable<IdentityError> errors)
        {
            return string.Join(" <br> ", errors.Select(s => s.Description));
        }
    }
}