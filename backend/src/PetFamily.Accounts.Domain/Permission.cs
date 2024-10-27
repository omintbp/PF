namespace PetFamily.Accounts.Domain;

public class Permission
{
    public List<RolePermission> RolesPermissions { get; set; }
    
    public Guid Id { get; set; }
    
    public string Code { get; set; }

    public Permission(string code)
    {
        Code = code;
    }
}