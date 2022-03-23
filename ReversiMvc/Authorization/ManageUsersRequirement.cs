using Microsoft.AspNetCore.Authorization;
using ReversiMvc.Authorization.Contracts;

namespace ReversiMvc.Authorization;

public class IsAllowedToManageUsersRequirement : IAdminAuthorizationRequirement
{
}
