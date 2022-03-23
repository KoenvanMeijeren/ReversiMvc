namespace ReversiMvc.Data;

public static class ApplicationRoleTypes
{
    public const string Admin = "Admin";
    public const string Mediator = "Mediator";
    
    public static readonly List<string> All = new List<string> {
        ApplicationRoleTypes.Admin,
        ApplicationRoleTypes.Mediator,
    };

}
