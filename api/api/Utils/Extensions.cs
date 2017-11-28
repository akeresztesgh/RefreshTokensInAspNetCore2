using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utils
{
    public static class Extensions
    {
        public const string AdminClaim = "admin";
        public const string UserClaim = "user";
        public const string ManageUserClaim = "manage_user";
        public const string AdminRole = "admin";
        public const string UserRole = "user";

        public const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public static string Error(this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count > 0)
                    return modelState[key].Errors[0].ErrorMessage;
            }
            return string.Empty;
        }
    }
}
