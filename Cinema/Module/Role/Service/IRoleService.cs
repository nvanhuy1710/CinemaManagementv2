using Cinema.Model;
using Cinema.Module.Role.DTO;

namespace Cinema.Module.Role.Service
{
    public interface IRoleService
    {
        RoleDTO getRole(int id);

        RoleDTO getRoleByName(string name);

        List<RoleDTO> getAllRoles();

        RoleDTO AddRole(RoleDTO role);

        void DeleteRole(int id);

        List<RoleDTO> GetRoles(List<int> ids);
    }
}
