using AutoMapper;
using Cinema.Model;
using Cinema.Module.Role.DTO;
using Cinema.Module.Role.Repository;
using System.Transactions;

namespace Cinema.Module.Role.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public RoleDTO AddRole(RoleDTO role)
        {
            return _mapper.Map<RoleDTO>(_roleRepository.AddRole(_mapper.Map<RoleModel>(role)));
        }

        public void DeleteRole(int id)
        {
            _roleRepository.DeleteRole(id);
        }

        public List<RoleDTO> getAllRoles()
        {
            return _roleRepository.GetAllRoles().Select(role => _mapper.Map<RoleDTO>(role)).ToList();
        }

        public RoleDTO getRole(int id)
        {
            return _mapper.Map<RoleDTO>(_roleRepository.GetRole(id));
        }

        public RoleDTO getRoleByName(string name)
        {
            return _mapper.Map<RoleDTO>(_roleRepository.GetRoleByName(name));
        }

        public List<RoleDTO> GetRoles(List<int> ids)
        {
            return _roleRepository.GetRoles(ids).Select(role => _mapper.Map<RoleDTO>(role)).ToList();
        }
    }
}
