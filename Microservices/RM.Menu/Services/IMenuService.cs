
using RM.Menu.Dtos;

namespace RM.Menu.Services
{
    public interface IMenuService
    {
        public Task<OperationOutput> GetMenu(MenuDto RequestedData);
        public Task<OperationOutput> GetMenusList(MenuDto RequestedData);
        public Task<OperationOutput> GetMenuTree(MenuDto RequestedData);
        public Task<OperationOutput> GetAdminMenu(UserDto RequestedData);
        public Task<OperationOutput> GetMenuById(int Id, bool isCms, int? referencId);
        public Task<OperationOutput> SaveMenu(MenuDto RequestedData);
        public Task<OperationOutput> SaveMenuTree(MenuDto RequestedData);
        public Task<OperationOutput> GetEntities(EntitiesItem RequestedData);
        public Task<OperationOutput> GetMenuType();
        public Task<OperationOutput> SaveAdminMenu(AdminMenuDto RequestedData);
        public Task<OperationOutput> GetAllReferences();
        public Task<OperationOutput> GetAdminMenuByRefernceId(ReferenceDto reference);
        public Task<OperationOutput> GetDefaultMenus();
        public Task<OperationOutput> DeleteMenu(MenuDto RequestedData);
        public Task<OperationOutput> ReOrderMenuTree(List<MenuTree> menus, int? parentId);

        public OperationOutput GetEntitiesByAdminMenuReference(MenuDto RequestedData);
        public Task<OperationOutput> GetMenuDataById(MenuDto RequestedData);
    }
}
