using Cinema.Data;
using Cinema.Model;

namespace Cinema.Module.Role.Repository.Impl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;
        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public RoleModel AddRole(RoleModel role)
        {
            RoleModel roleModel = _context.Roles.Add(role).Entity;
            _context.SaveChanges();
            return roleModel;
        }

        public void DeleteRole(int id)
        {
            _context.Roles.Remove(_context.Roles.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public RoleModel GetRole(int id)
        {
            return _context.Roles.Where(p => p.Id == id).Single();
        }

        public List<RoleModel> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public List<RoleModel> GetRoles(List<int> ids)
        {
            return _context.Roles.Where(p => ids.Contains(p.Id)).ToList();
        }

        public RoleModel GetRoleByName(string name)
        {
            return _context.Roles.Where(p => p.Name == name).Single();
        }
    }
}
