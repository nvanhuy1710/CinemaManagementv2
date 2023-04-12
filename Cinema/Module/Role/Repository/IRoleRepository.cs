using Cinema.Model;

namespace Cinema.Module.Role.Repository
{
    public interface IRoleRepository
    {
        RoleModel GetRole(int id);

        RoleModel GetRoleByName(string name);

        List<RoleModel> GetAllRoles();

        RoleModel AddRole(RoleModel role);

        void DeleteRole(int id);

        List<RoleModel> GetRoles(List<int> ids);
    }
}
