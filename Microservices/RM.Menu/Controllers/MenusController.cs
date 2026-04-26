using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Menu.Dtos;
using RM.Menu.Records;
using RM.Menu.Services;

namespace RM.Menu.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
            => _menuService = menuService;


        [HttpPost]

        public async Task<OperationOutput> GetMenus(GetMenusRecord RequestedData)
             => await _menuService.GetMenu(RequestedData.Adapt<MenuDto>());


        [HttpPost]

        public async Task<OperationOutput> GetMenusList(GetMenusListRecord RequestedData)
             => await _menuService.GetMenusList(RequestedData.Adapt<MenuDto>());


        [HttpPost]

        public async Task<OperationOutput> GetMenuType()
            => await _menuService.GetMenuType();

        [HttpPost]

        public async Task<OperationOutput> SaveMenu(SaveMenuRecord RequestedData)
            => await _menuService.SaveMenu(RequestedData.Adapt<MenuDto>());

        [HttpPost]

        public async Task<OperationOutput> GetAdminMenu(GetAdminMenuRecord RequestedData)
        => await _menuService.GetAdminMenu(RequestedData.Adapt<UserDto>());

        [HttpPost]

        public async Task<OperationOutput> GetEntities(GetEntitiesRecord RequestedData)
        => await _menuService.GetEntities(RequestedData.Adapt<EntitiesItem>());


        [HttpPost]

        public async Task<OperationOutput> SaveAdminMenu(AdminMenuDto dto)
            => await _menuService.SaveAdminMenu(dto);

        [HttpPost]

        public async Task<OperationOutput> GetAllReferences()
       => await _menuService.GetAllReferences();


        [HttpPost]

        public async Task<OperationOutput> GetAdminMenuByRefernceId(GetAdminMenuByRefernceIdRecord reference)
       => await _menuService.GetAdminMenuByRefernceId(reference.Adapt<ReferenceDto>());


        [HttpPost]

        public async Task<OperationOutput> GetDefaultMenus()
            => await _menuService.GetDefaultMenus();


        [HttpPost]

        public async Task<OperationOutput> SaveMenuTree(SaveMenuTreeRecord RequestedData)
       => await _menuService.SaveMenuTree(RequestedData.Adapt<MenuDto>());

        [HttpPost]

        public async Task<OperationOutput> GetMenuTree(GetMenuTreeRecord RequestedData)
        => await _menuService.GetMenuTree(RequestedData.Adapt<MenuDto>());


        [HttpPost]

        public async Task<OperationOutput> DeleteMenu(DeleteMenuRecord RequestedData)
       => await _menuService.DeleteMenu(RequestedData.Adapt<MenuDto>());


        [HttpPost]
        public async Task<OperationOutput> ReOrderMenuTree(ReOrderMenuTreeRecord RequestedData)
        {
            var menuOrder = RequestedData.Adapt<MenuOrderEntity>();
            return await _menuService.ReOrderMenuTree(menuOrder.menus, menuOrder._id);
        }

        [HttpPost]

        public async Task<OperationOutput> GetMenuDataById(GetMenuDataByIdRecord RequestedData)
              => await _menuService.GetMenuDataById(RequestedData.Adapt<MenuDto>());

        [HttpPost]

        public OperationOutput GetEntitiesByAdminMenuReference(GetEntitiesByAdminMenuReferenceRecord RequestedData)
        => _menuService.GetEntitiesByAdminMenuReference(RequestedData.Adapt<MenuDto>());

    }
}
