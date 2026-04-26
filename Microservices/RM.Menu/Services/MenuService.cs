using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Menu.Dtos;
using RM.Menu.UnitOfWorks;
using System.Text.Json;
using static RM.Menu.Dtos.OperationOutput;

namespace RM.Menu.Services
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MenuService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
            //_logger.LogError("Menu Service Constractor RequestOwner is : {RequestOwner}", RequestOwner);
            string result = SetRequestOwner();
            //_logger.LogError("Menu Service Constractor Authorization header is : {Authorization}", _httpContextAccessor.HttpContext.Request.Headers["Authorization"]!);
            //_logger.LogError("Menu Service Constractor SetRequestOwner is : {result}", result);

        }

        public async Task<OperationOutput> GetMenu(MenuDto RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            int MenuId = 0;

            if (!String.IsNullOrEmpty(RequestedData.Code))
            {
                if (_RedisConfiguration.UseRedis)
                {
                    DateTime LastUpdate;
                    DateTime.TryParse(await RedisCache.GetStringAsync("Menu_LastUpdate"), out LastUpdate);

                    var CacheDate = await RedisCache.GetStringAsync("Menu_CacheDate_" + RequestedData.Code);
                    var Menu = await RedisCache.GetStringAsync("Menu_" + RequestedData.Code);
                    if (Menu != null && CacheDate != null && DateTime.Parse(CacheDate) > LastUpdate)
                        return JsonSerializer.Deserialize<OperationOutput>(Menu, jsonOptions);
                }

                MenuId = _unitOfWork.Menu.GetAll()
                     .Where(x => x.IsHidden == false && x.IsDeleted == false)
                     .Where(x => x.ReferenceId == RequestedData.ReferenceId)
                     .Where(x => RequestedData.EntityId.HasValue ? x.EntityId == RequestedData.EntityId : true)
                     .Where(x => x.Code == RequestedData.Code)
                     .Select(x => x.Id).FirstOrDefault();
            }

            if (MenuId == 0)
            {
                try
                {
                    if (!RequestedData.Id.HasValue)
                        RequestedData.ID = RequestedData.Code;

                    if (_RedisConfiguration.UseRedis)
                    {
                        DateTime LastUpdate;
                        DateTime.TryParse(await RedisCache.GetStringAsync("Menu_LastUpdate"), out LastUpdate);

                        var CacheDate = await RedisCache.GetStringAsync("Menu_CacheDate_" + RequestedData.ID);
                        var Menu = await RedisCache.GetStringAsync("Menu_" + RequestedData.ID);
                        if (Menu != null && CacheDate != null && DateTime.Parse(CacheDate) > LastUpdate)
                            return JsonSerializer.Deserialize<OperationOutput>(Menu, jsonOptions);
                    }

                    MenuId = _unitOfWork.Menu.GetAll()
                    .Where(x => x.IsHidden == false && x.IsDeleted == false)
                    .Where(x => x.ReferenceId == RequestedData.ReferenceId)
                    .Where(x => RequestedData.EntityId.HasValue ? x.EntityId == RequestedData.EntityId : true)
                    .Where(x => x.Id == RequestedData.Id).Select(x => x.Id).FirstOrDefault();
                }
                catch { }
            }

            if (_RedisConfiguration.UseRedis)
            {
                Result = await GetMenuById(MenuId, RequestedData.IsCms, RequestedData.ReferenceId);
                if (Result.Header.Success)
                {
                    await RedisCache.SetStringAsync("Menu_" + (RequestedData.Id.HasValue ? RequestedData.ID : RequestedData.Code), JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                    await RedisCache.SetStringAsync("Menu_CacheDate_" + (RequestedData.Id.HasValue ? RequestedData.ID : RequestedData.Code), TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                }
                return Result;
            }
            else
            {
                return await GetMenuById(MenuId, RequestedData.IsCms, RequestedData.ReferenceId);
            }

        }

        public async Task<OperationOutput> GetMenusList(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var menu = await _unitOfWork.Menu.GetAll()
                .Include(x => x.Article)
                .Include(c => c.Entity)
                .Include(c => c.Reference)
            .Where(x => x.IsHidden == false && x.IsDeleted == false)//&& x.IsFirstRoot == true
            .Where(x => x.ReferenceId == RequestedData.ReferenceId)
            .Where(x => RequestedData.EntityId.HasValue ? x.EntityId == RequestedData.EntityId : true)
            .Where(x => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.Code) ? x.Code == RequestedData.Code : true).ToListAsync();

            var menuTree = FillRecursiveMenu(RequestedData, menu, null);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.MenusEntity, menuTree));
        }

        public async Task<OperationOutput> GetMenuTree(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var tree = new List<MenuTree>();

            if (string.IsNullOrEmpty(RequestedData.Code))
            {
                var menus = await _unitOfWork.Menu.GetAll(x => x.IsHidden == false && x.IsDeleted == false
                && x.ReferenceId == RequestedData.ReferenceId)
                .Include(x => x.Article)
                .Include(c => c.Entity)
                .Include(c => c.Reference).ToListAsync();

                tree = FillRecursive(menus, RequestedData.RootParentId, RequestedData.ParentId, null);
            }
            else GetMenuTreeByCode(RequestedData, tree);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.MenusEntity, tree));
        }

        #region HELPER METHODS >> GetMenuTree 
        private void GetMenuTreeByCode(MenuDto RequestedData, List<MenuTree> tree)
        {
            var menu = _unitOfWork.Menu.GetAll()
           .Include(x => x.Article)
           .Include(x => x.Reference)
           .Include(c => c.Entity)
           .Where(x => x.IsHidden == false && x.IsDeleted == false
                  && x.Code == RequestedData.Code && x.ReferenceId == RequestedData.ReferenceId).FirstOrDefault();
            if (menu != null)
            {
                menu = GetMenuChildren(menu);
                var mt = new MenuTree();
                tree.Add(MapMenuToMenuTree(menu, mt));
            }
        }

        private static List<MenuTree> FillRecursive(List<Models.Menu> flatObjects, int? parentRootExtended, int? parentIdExtended, int? parentId = null)
        {

            return flatObjects.Where(x => x.ParentId.Equals(parentId) && x.IsDeleted == false).Select(x => new MenuTree
            {
                _key = x.Id,
                Label = x.ParentId != null ? x.NameAr : x.DescriptionAr,
                LabelEn = x.ParentId != null ? x.NameEn : x.DescriptionEn,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Id = x.Id,
                _parentId = x.ParentId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ReferenceId = x.ReferenceId,
                ReferenceMajorId = x.ReferenceMajorId,
                Code = x.Code,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted,
                OpenBlankPage = x.OpenBlankPage ?? false,
                EntityId = x.EntityId.HasValue ? x.EntityId : null,
                ArticleId = x.ArticleId,
                ArticleNameAr = x.Article != null ? x.Article.NameAr : null,
                EntityNameAr = x.Entity != null ? x.Entity.NameAr : null,
                Url = GenerateMenuUrl(x),
                MenuOrder = x.MenuOrder,
                FontIcon = x.FontIcon,
                ImageIcon = x.ImageIcon,
                SvgIcon = x.SvgIcon,
                BriefeContentAr = x.BriefeContentAr,
                BriefeContentEn = x.BriefeContentEn,
                expanded = x.Id == parentRootExtended ? true : x.Id == parentIdExtended ? true : false,
                Children = FillRecursive(flatObjects, parentRootExtended, parentIdExtended, x.Id)
            }).OrderBy(m => m.MenuOrder).ToList();

        }

        private static List<MenuDto> FillRecursiveMenu(MenuDto RequestedData, List<Models.Menu> flatObjects, int? parentId = null)
        {

            return flatObjects.Where(x => x.ParentId.Equals(parentId) && x.IsDeleted == false).Select(x => new MenuDto
            {
                Id = x.Id,

                NameAr = x.NameAr,
                NameEn = x.NameEn,

                ParentId = x.ParentId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ReferenceId = x.ReferenceId,
                ReferenceMajorId = x.ReferenceMajorId,
                Code = x.Code,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted,
                OpenBlankPage = x.OpenBlankPage ?? false,
                ArticleId = x.ArticleId,
                ArticleNameAr = x.Article != null ? x.Article.NameAr : null,
                Url = GenerateMenuUrl(x),
                MenuOrder = x.MenuOrder,
                FontIcon = x.FontIcon,
                ImageIcon = x.ImageIcon,
                SvgIcon = x.SvgIcon,
                BriefeContentAr = x.BriefeContentAr,
                BriefeContentEn = x.BriefeContentEn,
                TypeId = x.TypeId,
                IsFirstRoot = x.IsFirstRoot,
                EntityId = !x.ArticleId.HasValue ? x.EntityId : null,
                EntityNameAr = !x.ArticleId.HasValue && x.EntityId.HasValue ? x.Entity.NameAr : null,
                IsCms = RequestedData != null ? RequestedData.IsCms : false,
                SubMenus = FillRecursiveMenu(RequestedData, flatObjects, x.Id).OrderBy(s => s.MenuOrder).ToList()
            }).ToList();

        }


        private int GetRootParent(int parentId, List<Models.Menu> menus, int menuId)
        {

            var menu = menus.Find(x => x.Id == parentId && x.ParentId != null);
            if (menu != null)
            {
                menuId = menu.Id;
                GetRootParent(menu.ParentId.Value, menus, menuId);
            }

            return menuId;
        }

        private MenuTree MapMenuToMenuTree(Models.Menu m, MenuTree mt)
        {
            var parent = _unitOfWork.Menu.Find(c => c.Id == m.ParentId);

            mt._key = m.Id;
            mt.Label = m.ParentId != null ? m.NameAr : m.DescriptionAr;
            mt.LabelEn = m.ParentId != null ? m.NameEn : m.DescriptionEn;
            mt.NameAr = m.NameAr;
            mt.NameEn = m.NameEn;
            mt.Id = m.Id;
            mt._parentId = m.ParentId;
            mt.ParentNameAr = parent != null ? parent.NameAr : null;
            mt.ParentNameEn = parent != null ? parent.NameEn : null;
            mt.DescriptionAr = m.DescriptionAr;
            mt.DescriptionEn = m.DescriptionEn;
            mt.ReferenceId = m.ReferenceId;
            mt.ReferenceMajorId = m.ReferenceMajorId;
            mt.Code = m.Code;
            mt.IsHidden = m.IsHidden;
            mt.IsDeleted = m.IsDeleted;
            mt.OpenBlankPage = m.OpenBlankPage ?? false;
            mt.EntityId = m.EntityId.HasValue ? m.EntityId : null;

            mt.EntityNameAr = m.Entity != null ? m.Entity.NameAr : null;

            mt.Url = GenerateMenuUrl(m);
            mt.MenuOrder = m.MenuOrder;
            mt.FontIcon = m.FontIcon;
            mt.ImageIcon = m.ImageIcon;
            mt.SvgIcon = m.SvgIcon;
            mt.BriefeContentAr = m.BriefeContentAr;
            mt.BriefeContentEn = m.BriefeContentEn;
            mt.expanded = false;

            foreach (var item in m.InverseParent)
            {
                mt.Children.Add(MapMenuToMenuTree(item, new MenuTree()));

            }


            return mt;
        }

        private static string GenerateMenuUrl(Models.Menu menu)
        {
            string referenceRouting = string.Empty;

            if (!string.IsNullOrEmpty(menu.Url) && menu.EntityId == null && menu.ArticleId == null)
                return menu.Url;
            else
            {
                if (menu.ReferenceId.HasValue)
                {
                    if (menu.Reference != null)
                    {
                        var urlSegments = menu.Reference.Url.Split('/');
                        referenceRouting = !string.IsNullOrEmpty(urlSegments[urlSegments.Length - 1]) ? "/" + urlSegments[urlSegments.Length - 1] : string.Empty;

                    }
                }
                if (!string.IsNullOrEmpty(referenceRouting) && !string.IsNullOrEmpty(menu.Url) && !menu.Url.Contains(referenceRouting))
                    return $"{referenceRouting}{menu.Url}";


                if (menu.ArticleId is not null)
                    return referenceRouting + "/content/" + Accessor.Get<int?>(menu.ArticleId);

                if (menu.EntityId is not null)
                    return referenceRouting + "/" + menu.Entity.FrontIdentity;

                else return menu.Url;
            }


        }

        #endregion

        public async Task<OperationOutput> GetAdminMenu(UserDto RequestedData)
        {
            //  _logger.LogError("Menu Service GetAdminMenu RequestOwner is : {RequestOwner}", RequestOwner);

            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<AdminMenu> adminMenusItems = null;

            var dynamicForms = _unitOfWork.FormsEntity.GetAll().ToList();
            var user = await _unitOfWork.User.GetAll(x => x.Id == RequestedData.Id)
                .Include(r => r.Reference)
                .Include(x => x.UsersEntities)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    ReferenceID = x.ReferenceId,
                    ReferenceMajorId = x.Reference.ReferencesMajorId,
                    UserEntities = x.UsersEntities.Select(v => new UserDto.EntitiesPermission() { Id = v.EntityId, }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();


            if (!user.Id.HasValue)

                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            var adminMenu = await GetAdminMenu(RequestedData, user);

            if (adminMenu.Any())
            {
                adminMenusItems = adminMenu.FindAll(x => x.ParentId == null).OrderBy(x => x.MenuOrder)
               .Select(x => new AdminMenu()
               {
                   Id = x.Id,
                   EntityId = x.Entity != null ? x.Entity.Id : null,
                   ReferenceId = x.ReferenceId,
                   ReferencesMajorId = x.ReferencesMajorId,
                   NameAr = x.NameAr,
                   NameEn = x.NameEn,
                   Url = x.Entity != null ? x.Entity.BackendIdentity : null,
                   CmsIdentity = x.Entity != null ? x.Entity.CmsIdentity : null,

                   FormId = x.Entity != null &&
                   dynamicForms.Select(c => c.EntityId).Contains(x.EntityId.Value) ?
                   dynamicForms.FirstOrDefault(c => c.EntityId == x.EntityId.Value).FormId : null,

                   DynamicFormUrl = x.Entity != null &&
                   dynamicForms.Select(c => c.EntityId).Contains(x.EntityId.Value) ?
                   dynamicForms.FirstOrDefault(c => c.EntityId == x.EntityId.Value).Url : null,

                   SubMenu = adminMenu.FindAll(m => m.ParentId == x.Id).OrderBy(x => x.MenuOrder)
                   .Select(v => new AdminMenu()
                   {
                       Id = v.Id,
                       EntityId = v.Entity != null ? v.EntityId : null,
                       CmsIdentity = v.Entity != null ? v.Entity.CmsIdentity : null,
                       ReferenceId = v.ReferenceId,
                       ReferencesMajorId = v.ReferencesMajorId,
                       NameAr = v.NameAr,
                       NameEn = v.NameEn,
                       Url = v.Entity != null ? v.Entity.BackendIdentity : null,
                       FormId = v.Entity != null &&
                       dynamicForms.Select(c => c.EntityId).Contains(v.EntityId.Value) ?
                       dynamicForms.FirstOrDefault(c => c.EntityId == v.EntityId.Value).FormId : null,

                       DynamicFormUrl = v.Entity != null &&
                      dynamicForms.Select(c => c.EntityId).Contains(v.EntityId.Value) ?
                      dynamicForms.FirstOrDefault(c => c.EntityId == v.EntityId.Value).Url : null,
                   }).ToList()
               }).ToList();


            }

            #region Filter UnUsed Level1 Menu Item

            adminMenusItems = adminMenusItems.Where(x => string.IsNullOrEmpty(x.Url) ? x.SubMenu.Count > 0 : true).ToList();

            #endregion

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.MenusEntity, adminMenusItems));


        }

        #region HELPER METHODS >> GetAdminMenu
        private async Task<List<Models.AdminMenu>> GetAdminMenu(UserDto RequestedData, UserDto user)
        {
            if (RequestOwner.RoleId.HasValue && RequestOwner.RoleId.Value == (int)Enums.UsersRoles.Administrator)

                return await _unitOfWork.AdminMenu.GetAll(x => x.ReferenceId == null && x.IsSuperAdmin == true)
                                   .Include(x => x.Entity).AsNoTracking().ToListAsync();
            else
            {
                if (user.ReferenceID != RequestedData.ReferenceID)
                    user.UserEntities = await _unitOfWork.UsersEntityReference.GetAll(x => x.UserId == user.Id && x.ReferenceId == RequestedData.ReferenceID)
                                        .AsNoTracking().Select(v => new UserDto.EntitiesPermission()
                                        { Id = v.EntityId, }).ToListAsync();

                return await _unitOfWork.AdminMenu.GetAll(x => x.ReferenceId == RequestedData._rootReferenceId)
                             .Include(x => x.Entity)
                             .Where(x => x.EntityId != null ? user.UserEntities.Select(v => v.Id).Contains(x.EntityId) : true)
                             .AsNoTracking().ToListAsync();
            }

        }

        #endregion

        public async Task<OperationOutput> GetMenuById(int Id, bool isCms, int? referencId)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            string referenceRouting = string.Empty;

            if (referencId.HasValue)
                referenceRouting = await SetReferenceRouting(referencId);

            var menu = await _unitOfWork.Menu.GetAll(x => x.Id == Id)
                .Include(x => x.Article)
                .Include(c => c.Entity)
                .Include(c => c.Reference)
            .Select(x => new MenuDto()
            {
                Id = x.Id,
                NameAr = x.NameAr,
                ReferenceId = x.ReferenceId,
                ReferenceMajorId = x.ReferenceMajorId,
                TypeId = x.TypeId,
                Code = x.Code,
                NameEn = x.NameEn,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted,
                IsFirstRoot = x.IsFirstRoot,
                OpenBlankPage = x.OpenBlankPage ?? false,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                SubMenus = _unitOfWork.Menu.GetAll().Where(xL1 => xL1.ParentId == x.Id && xL1.IsHidden == false && xL1.IsDeleted == false)

            .Select(xL1 => new MenuDto()
            {
                Id = xL1.Id,
                EntityId = xL1.EntityId,
                ArticleId = xL1.ArticleId,
                NameAr = xL1.NameAr,
                NameEn = xL1.NameEn,
                Code = xL1.Code,
                IsCms = isCms,
                Url = isCms == true ? xL1.Url : xL1.Entity != null && xL1.Entity.FrontIdentity.Trim() != "" ? referenceRouting + '/' + xL1.Entity.FrontIdentity + (xL1.Article.FrontIdentity != null ? "/" + xL1.Article.FrontIdentity : "") + (xL1.ArticleId.HasValue ? "/" + Accessor.Get<int?>(xL1.ArticleId) : xL1.Url.Trim() != "" ? "/" + xL1.Url : "") : xL1.Url,
                IsHidden = xL1.IsHidden,
                IsDeleted = xL1.IsDeleted,
                OpenBlankPage = xL1.OpenBlankPage ?? false,
                MenuOrder = xL1.MenuOrder,
                FontIcon = xL1.FontIcon,
                ImageIcon = xL1.ImageIcon,
                SvgIcon = xL1.SvgIcon,
                BriefeContentAr = xL1.BriefeContentAr,
                BriefeContentEn = xL1.BriefeContentEn,
                SubMenus = _unitOfWork.Menu.GetAll().Where(xL2 => xL2.ParentId == xL1.Id && xL2.IsHidden == false && xL2.IsDeleted == false)
                .Select(xL2 => new MenuDto()
                {
                    Id = xL2.Id,
                    EntityId = xL2.EntityId,
                    ArticleId = xL1.ArticleId,
                    NameAr = xL2.NameAr,
                    NameEn = xL2.NameEn,
                    Code = xL2.Code,
                    IsCms = isCms,
                    Url = isCms == true ? xL2.Url : xL2.Entity != null && xL2.Entity.FrontIdentity.Trim() != "" ? referenceRouting + '/' + xL2.Entity.FrontIdentity + (xL2.Article.FrontIdentity != null ? "/" + xL2.Article.FrontIdentity : "") + (xL2.ArticleId.HasValue ? "/" + Accessor.Get<int?>(xL2.ArticleId) : xL2.Url.Trim() != "" ? "/" + xL2.Url : "") : xL2.Url,
                    IsHidden = xL2.IsHidden,
                    IsDeleted = xL2.IsDeleted,
                    MenuOrder = xL2.MenuOrder,
                    FontIcon = xL2.FontIcon,
                    ImageIcon = xL2.ImageIcon,
                    SvgIcon = xL2.SvgIcon,
                    OpenBlankPage = xL2.OpenBlankPage ?? false,
                    BriefeContentAr = xL2.BriefeContentAr,
                    BriefeContentEn = xL2.BriefeContentEn,

                }).OrderBy(xL2 => xL2.MenuOrder).ToList()

            }).OrderBy(xL1 => xL1.MenuOrder).ToList()
            }).FirstOrDefaultAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.MenusEntity, menu));

        }

        #region HELPER METHOD >> GetMenuById
        private async Task<string> SetReferenceRouting(int? referencId)
        {
            var reference = await _unitOfWork.References.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == referencId.Value);
            if (reference != null)
            {
                var urlSegments = reference.Url.Split('/');
                return !string.IsNullOrEmpty(urlSegments[urlSegments.Length - 1]) ? "/" + urlSegments[urlSegments.Length - 1] : string.Empty;
            }
            return string.Empty;

        }

        #endregion

        public async Task<OperationOutput> SaveMenu(MenuDto RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Models.Menu DbItem = new Models.Menu();
            Models.Menu DbItemSubMenuLevenl1 = new Models.Menu();
            Models.Menu DbItemSubMenuLevenl2 = new Models.Menu();

            bool BufferIsNew = true;
            bool IsSaveSuccess = false;
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                if (RequestOwner is null)
                {
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                    return Result;
                }
                if (!RequestedData.Id.HasValue)
                {
                    DbItem = new Models.Menu();
                    DbItem.CreatedDate = TransactionDate;
                    DbItem.CreatedBy = RequestOwner.Id;
                    DbItem.IsDeleted = false;
                    DbItem.IsHidden = false;
                    DbItem.ReferenceId = RequestedData.ReferenceId;
                    DbItem.ReferenceMajorId = RequestedData.ReferenceMajorId;
                    DbItem.IsFirstRoot = true;
                    DbItem.Code = Strings.RandomDigits(10).ToString();
                }
                else
                {
                    BufferIsNew = false;
                    DbItem = _unitOfWork.Menu.GetAll().Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                    if (DbItem == null)
                    {
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoDataReturned);
                        return Result;
                    }


                }
                DbItem.NameAr = RequestedData.NameAr;
                DbItem.NameEn = RequestedData.NameEn;
                DbItem.DescriptionAr = RequestedData.DescriptionAr;
                DbItem.DescriptionEn = RequestedData.DescriptionEn;

                DbItem.Url = RequestedData.Url;
                DbItem.IsDeleted = RequestedData.IsDeleted;
                DbItem.IsHidden = RequestedData.IsHidden;
                DbItem.TypeId = RequestedData.TypeId;
                if (BufferIsNew) _unitOfWork.Menu.Add(DbItem);
                else _unitOfWork.Menu.Update(DbItem);
                IsSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;
                BufferIsNew = true;
                if (!DbItem.IsDeleted.Value)
                {
                    if (RequestedData.SubMenus != null)
                    {
                        foreach (var RequestedMenuLevel1 in RequestedData.SubMenus)
                        {
                            if (!RequestedMenuLevel1.Id.HasValue)
                            {

                                DbItemSubMenuLevenl1 = new Models.Menu();
                                DbItemSubMenuLevenl1.CreatedDate = TransactionDate;
                                DbItemSubMenuLevenl1.CreatedBy = RequestOwner.Id;
                                DbItemSubMenuLevenl1.IsFirstRoot = false;
                            }
                            else
                            {
                                BufferIsNew = false;
                                DbItemSubMenuLevenl1 = _unitOfWork.Menu.GetAll().Where(x => x.Id == RequestedMenuLevel1.Id.Value).FirstOrDefault();
                            }
                            DbItemSubMenuLevenl1.NameAr = RequestedMenuLevel1.NameAr;
                            DbItemSubMenuLevenl1.NameEn = RequestedMenuLevel1.NameEn;
                            DbItemSubMenuLevenl1.MenuOrder = RequestedMenuLevel1.MenuOrder;
                            DbItemSubMenuLevenl1.Url = RequestedMenuLevel1.Url;
                            DbItemSubMenuLevenl1.IsDeleted = RequestedMenuLevel1.IsDeleted;
                            DbItemSubMenuLevenl1.IsHidden = RequestedMenuLevel1.IsHidden;
                            DbItemSubMenuLevenl1.ArticleId = RequestedMenuLevel1.ArticleId;
                            DbItemSubMenuLevenl1.OpenBlankPage = RequestedMenuLevel1.OpenBlankPage;
                            DbItemSubMenuLevenl1.EntityId = RequestedMenuLevel1.ArticleId.HasValue ? (int)Enums.Entities.Articles : RequestedMenuLevel1.EntityId; ;
                            DbItemSubMenuLevenl1.ParentId = DbItem.Id;
                            DbItemSubMenuLevenl1.FontIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel1.FontIcon) ? RequestedMenuLevel1.FontIcon : null;
                            DbItemSubMenuLevenl1.ImageIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel1.ImageIcon) ? RequestedMenuLevel1.ImageIcon : null;
                            DbItemSubMenuLevenl1.SvgIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel1.SvgIcon) ? RequestedMenuLevel1.SvgIcon : null;
                            DbItemSubMenuLevenl1.BriefeContentAr = RequestedMenuLevel1.BriefeContentAr;
                            DbItemSubMenuLevenl1.BriefeContentEn = RequestedMenuLevel1.BriefeContentEn;


                            if (BufferIsNew) _unitOfWork.Menu.Add(DbItemSubMenuLevenl1);
                            else _unitOfWork.Menu.Update(DbItemSubMenuLevenl1);
                            IsSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;
                            BufferIsNew = true;


                            if (RequestedData.TypeId == (int)Enums.MenuType.SubMenu) continue;
                            if (RequestedMenuLevel1.SubMenus == null) continue;

                            foreach (var RequestedMenuLevel2 in RequestedMenuLevel1.SubMenus)
                            {
                                if (!RequestedMenuLevel2.Id.HasValue)
                                {

                                    DbItemSubMenuLevenl2 = new Models.Menu();
                                    DbItemSubMenuLevenl2.CreatedDate = TransactionDate;
                                    DbItemSubMenuLevenl2.CreatedBy = RequestOwner.Id;
                                    DbItemSubMenuLevenl2.IsFirstRoot = false;

                                }
                                else
                                {
                                    BufferIsNew = false;
                                    DbItemSubMenuLevenl2 = _unitOfWork.Menu.GetAll().Where(x => x.Id == RequestedMenuLevel2.Id.Value).FirstOrDefault();
                                }

                                DbItemSubMenuLevenl2.NameAr = RequestedMenuLevel2.NameAr;
                                DbItemSubMenuLevenl2.NameEn = RequestedMenuLevel2.NameEn;
                                DbItemSubMenuLevenl2.Url = RequestedMenuLevel2.Url;
                                DbItemSubMenuLevenl2.IsDeleted = RequestedMenuLevel2.IsDeleted;
                                DbItemSubMenuLevenl2.OpenBlankPage = RequestedMenuLevel2.OpenBlankPage;
                                DbItemSubMenuLevenl2.IsHidden = RequestedMenuLevel2.IsHidden;
                                DbItemSubMenuLevenl2.ArticleId = RequestedMenuLevel2.ArticleId;
                                DbItemSubMenuLevenl2.MenuOrder = RequestedMenuLevel2.MenuOrder;
                                DbItemSubMenuLevenl2.EntityId = RequestedMenuLevel2.ArticleId.HasValue ? (int)Enums.Entities.Articles : RequestedMenuLevel2.EntityId;
                                DbItemSubMenuLevenl2.ParentId = DbItemSubMenuLevenl1.Id;
                                DbItemSubMenuLevenl2.FontIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel2.FontIcon) ? RequestedMenuLevel2.FontIcon : null;
                                DbItemSubMenuLevenl2.ImageIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel2.ImageIcon) ? RequestedMenuLevel2.ImageIcon : null;
                                DbItemSubMenuLevenl2.SvgIcon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedMenuLevel2.SvgIcon) ? RequestedMenuLevel2.SvgIcon : null;
                                DbItemSubMenuLevenl2.BriefeContentAr = DbItemSubMenuLevenl2.BriefeContentAr;
                                DbItemSubMenuLevenl2.BriefeContentEn = DbItemSubMenuLevenl2.BriefeContentEn;
                                if (BufferIsNew) _unitOfWork.Menu.Add(DbItemSubMenuLevenl2);
                                else _unitOfWork.Menu.Update(DbItemSubMenuLevenl2);
                                IsSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;
                                BufferIsNew = true;

                            }
                        }
                    }
                }
                if (IsSaveSuccess)
                    dbContextTransaction.Commit();
                else dbContextTransaction.Rollback();

                RequestedData.IsHidden = false;
                RequestedData.IsDeleted = false;
                RequestedData.IsFirstRoot = true;
                RequestedData.Code = null;
                RequestedData.IsCms = true;

                if (_RedisConfiguration.UseRedis)
                    await RedisCache.SetStringAsync("Menu_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

                return await GetMenusList(RequestedData);
            }
        }

        public async Task<OperationOutput> SaveMenuTree(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Menu menu = null;

            if (RequestedData.Id.HasValue)
            {
                menu = await _unitOfWork.Menu.GetAll().AsNoTracking().FirstOrDefaultAsync(m => m.Id == RequestedData.Id);
                if (menu == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                _unitOfWork.Menu.Update(RequestedData.Adapt(menu, RequestedData.UpdateConfig(RequestOwner.Id)));
            }
            else
            {
                menu = new Models.Menu();
                _unitOfWork.Menu.Add(RequestedData.Adapt(menu, RequestedData.AddConfig(RequestOwner.Id)));
            }

            await _unitOfWork.CompleteAsync();


            RequestedData.Id = menu.Id;
            RequestedData.CreatedBy = menu.CreatedBy;
            RequestedData.CreatedDate = menu.CreatedDate;
            RequestedData.ParentId = menu.ParentId;

            return await GetMenuTree(RequestedData);

        }

        public async Task<OperationOutput> GetEntities(EntitiesItem RequestedData)
        {
            bool isSuperAdmin = RequestOwner.RoleId.HasValue && RequestOwner.RoleId.Value == (int)Enums.UsersRoles.Administrator;

            var entity = await _unitOfWork.Entity.GetAll(x => x.ParentId == null)
                  .Include(c => c.InverseParent)
                  .Where(x => !isSuperAdmin ? x.ReferencesMajorId == RequestedData.ReferencesMajorId || x.ReferencesMajorId == null : true)
                  .Where(x => !isSuperAdmin ? (RequestedData.ReferenceId.HasValue ? x.ReferenceId == RequestedData.ReferenceId : true) || x.ReferenceId == null : true)
                  .AsNoTracking().OrderBy(c => c.Id).ToListAsync();

            var entityDto = entity.Adapt<List<EntitiesItem>>(EntitiesItem.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.EntitiesItem, entityDto));
        }

        public async Task<OperationOutput> GetMenuType()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var menuTypes = await _unitOfWork.MenuTypes.GetAll().AsNoTracking().ToListAsync();
            var menuTypesDto = menuTypes.Adapt<List<MenuType>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.MenusEntity, menuTypesDto));
        }

        public async Task<OperationOutput> SaveAdminMenu(AdminMenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.AdminMenu MainMenu = null;
            Models.AdminMenu SubMenu = null;
            List<int> MenuRemoved = new List<int>();
            List<int> subMenuRemoved = new List<int>();
            bool BufferIsNew = true;
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {

                try
                {
                    //item to remove
                    MenuRemoved = _unitOfWork.AdminMenu.GetAll().Where(x => x.ReferenceId == RequestedData.ReferenceId
                    && x.ParentId == null && x.IsSuperAdmin != true).Select(x => x.Id)
                    .Where(x => !RequestedData.Menus.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();

                    foreach (var menuId in MenuRemoved)
                    {
                        var itemToDelete = _unitOfWork.AdminMenu.GetAll().Where(x => x.Id == menuId).FirstOrDefault();

                        //subItem to remove
                        subMenuRemoved = _unitOfWork.AdminMenu.GetAll().Where(x => x.ParentId == itemToDelete.Id).Select(x => x.Id).ToList();
                        foreach (var subMenuId in subMenuRemoved)
                        {
                            var subToDelete = _unitOfWork.AdminMenu.GetAll().Where(x => x.Id == subMenuId).FirstOrDefault();
                            _unitOfWork.AdminMenu.Delete(subToDelete);
                        }
                        _unitOfWork.AdminMenu.Delete(itemToDelete);
                    }

                    //add or update mainItem
                    foreach (var menuItem in RequestedData.Menus)
                    {
                        if (!menuItem.Id.HasValue)
                        {
                            MainMenu = new Models.AdminMenu();

                            MainMenu.ReferenceId = menuItem.ReferenceId;
                            MainMenu.ReferencesMajorId = menuItem.ReferencesMajorId;
                        }
                        else
                        {
                            BufferIsNew = false;
                            MainMenu = _unitOfWork.AdminMenu.GetAll().Where(x => x.Id == menuItem.Id).FirstOrDefault();
                            if (MainMenu == null)
                                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                        }
                        MainMenu.NameAr = menuItem.NameAr;
                        MainMenu.NameEn = menuItem.NameEn;
                        MainMenu.EntityId = menuItem.EntityId;
                        MainMenu.MenuOrder = menuItem.MenuOrder;

                        if (BufferIsNew) _unitOfWork.AdminMenu.Add(MainMenu);
                        else _unitOfWork.AdminMenu.Update(MainMenu);
                        await _unitOfWork.CompleteAsync();

                        BufferIsNew = true;

                        if (menuItem.SubMenu != null)
                        {
                            //subItem to remove
                            subMenuRemoved = _unitOfWork.AdminMenu.GetAll().Where(x => x.ParentId == MainMenu.Id).Select(x => x.Id).Where(x => !menuItem.SubMenu.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();
                            foreach (var subMenuId in subMenuRemoved)
                            {
                                var itemToDelete = _unitOfWork.AdminMenu.GetAll().Where(x => x.Id == subMenuId).FirstOrDefault();
                                _unitOfWork.AdminMenu.Delete(itemToDelete);
                            }

                            //add or update subItem
                            foreach (var MenuLevel1 in menuItem.SubMenu)
                            {
                                if (!MenuLevel1.Id.HasValue)
                                {
                                    SubMenu = new Models.AdminMenu();

                                    SubMenu.ReferenceId = MenuLevel1.ReferenceId;
                                    SubMenu.ReferencesMajorId = MenuLevel1.ReferencesMajorId;
                                    SubMenu.ParentId = MainMenu.Id;
                                }
                                else
                                {
                                    BufferIsNew = false;
                                    SubMenu = _unitOfWork.AdminMenu.GetAll().Where(x => x.Id == MenuLevel1.Id.Value).FirstOrDefault();
                                }
                                SubMenu.NameAr = MenuLevel1.NameAr;
                                SubMenu.NameEn = MenuLevel1.NameEn;
                                SubMenu.EntityId = MenuLevel1.EntityId;
                                SubMenu.MenuOrder = MenuLevel1.MenuOrder;
                                SubMenu.Url = MenuLevel1.Url;

                                if (BufferIsNew) _unitOfWork.AdminMenu.Add(SubMenu);
                                else _unitOfWork.AdminMenu.Update(SubMenu);
                                BufferIsNew = true;
                            }

                        }
                    }


                    await _unitOfWork.CompleteAsync();

                    dbContextTransaction.Commit();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                }
            }
        }

        public async Task<OperationOutput> GetAllReferences()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var majorReference = await _unitOfWork.ReferencesMajor.GetAll()
                 .Include(x => x.References)
                 .Where(x => x.IsDeleted != true)
                 .AsNoTracking().ToListAsync();

            var majorReferenceDto = majorReference.Adapt<List<MajorReference>>(MajorReference.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.MajorReference, majorReferenceDto));

        }

        public async Task<OperationOutput> GetAdminMenuByRefernceId(ReferenceDto reference)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<AdminMenu> menusDto = null;
            var menusByReference = await _unitOfWork.AdminMenu.GetAll(c => c.ReferenceId == reference.Id)
                                   .Include(e => e.Entity).AsNoTracking().OrderBy(m => m.MenuOrder).ToListAsync();

            if (menusByReference.Any())
            {
                var menus = menusByReference.FindAll(c => c.ParentId == null).ToList();
                menusDto = menus.Adapt<List<AdminMenu>>(AdminMenu.SelectConfigMenuByRefernceId(menusByReference));

            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.AdminMenusEntity, menusDto));

        }

        public async Task<OperationOutput> GetDefaultMenus()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<AdminMenu> defaultMenusDto = null;


            var adminDefaultMenu = await _unitOfWork.AdminMenu.GetAll(c => c.ReferenceId == null).AsNoTracking()
                                .Include(e => e.Entity).ToListAsync();

            if (adminDefaultMenu.Any())
            {
                var defaultMenus = adminDefaultMenu.FindAll(c => c.ParentId == null).ToList();
                defaultMenusDto = defaultMenus.Adapt<List<AdminMenu>>(AdminMenu.SelectConfigMenuByRefernceId(defaultMenus));

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.MenusEntity, defaultMenusDto));

        }

        public async Task<OperationOutput> DeleteMenu(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var menu = await _unitOfWork.Menu.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

            if (menu is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            menu.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : menu.IsDeleted;

            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
                menu.DeletedDate = TransactionDate;

            _unitOfWork.Menu.Update(menu);
            await _unitOfWork.CompleteAsync();

            RequestedData.ParentId = menu.ParentId;
            RequestedData.Code = null;
            return await GetMenuTree(RequestedData);
        }


        public async Task<OperationOutput> ReOrderMenuTree(List<MenuTree> menus, int? parentId)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<Models.Menu> newMenuList = new List<Models.Menu>();
            MenuDto requestedData = new MenuDto();

            var menusList = await _unitOfWork.Menu.GetAll(x => x.IsHidden == false && x.IsDeleted == false
                            && x.ParentId == parentId.Value).AsNoTracking()
                            .OrderBy(c => c.MenuOrder).ToListAsync();

            if (menusList.Count > 0)
            {
                foreach (var item in menusList)
                {
                    var _menu = GetMenuChildren(item);
                    newMenuList.Add(_menu);
                }
                if (newMenuList.Any())
                {
                    foreach (var item in newMenuList)
                    {
                        var newMenu = menus.Find(c => c.Id == item.Id);
                        if (newMenu != null)
                            OrderRecursive(item, newMenu, false);
                    }
                }
                await _unitOfWork.CompleteAsync();
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        #region HELPER METHODS
        private Models.Menu GetMenuChildren(Models.Menu menu)
        {

            var listChildren = _unitOfWork.Menu.GetAll()
            .Include(c => c.Article)
            .Include(c => c.Reference)
            .Include(c => c.Entity)
            .Where(c => c.ParentId == menu.Id && c.IsDeleted == false).OrderBy(c => c.MenuOrder).ToList();
            if (listChildren.Count > 0)
            {

                menu.InverseParent = listChildren.OrderBy(c => c.MenuOrder).ToList();
            }

            if (menu.InverseParent.Count > 0)
                foreach (var item in menu.InverseParent)
                {
                    GetMenuChildren(item);
                }

            return menu;
        }
        private void OrderRecursive(Models.Menu oldMenu, MenuTree newMenu, bool? isChildren)
        {
            if (newMenu.Id == oldMenu.Id)
            {
                oldMenu.MenuOrder = newMenu.MenuOrder;
                oldMenu.ParentId = newMenu._parentId;

                if (oldMenu.InverseParent.Count() > 0 && newMenu.Children.Count > 0)
                {

                    foreach (var subMenu in oldMenu.InverseParent)
                    {
                        var newSubMenuList = newMenu.Children.Where(c => c.Id == subMenu.Id).ToList();
                        if (newSubMenuList.Count > 0)
                            foreach (var item in newSubMenuList)
                            {
                                OrderRecursive(subMenu, item, true);
                            }
                    }

                }
            }
        }

        #endregion


        #region HELPER METHODS
        private void OrderAnotherParent(List<MenuDto> menus, MenuDto RequestedData)
        {
            RequestedData.ReferenceId = menus.First().ReferenceId;
            RequestedData.ParentId = menus.First().ParentId;
            foreach (var menu in menus)
            {
                var item = _unitOfWork.Menu.Find(m => m.Id == menu.Id);
                if (item != null)
                {
                    item.MenuOrder = menu.MenuOrder;
                    item.ParentId = menu.ParentId;
                    _unitOfWork.Menu.Update(item);
                }
            }
        }

        private void OrderSameParent(List<MenuDto> menus, MenuDto RequestedData)
        {
            RequestedData.ReferenceId = menus.First().ReferenceId;
            RequestedData.ParentId = menus.First().ParentId;
            foreach (var menu in menus)
            {
                var item = _unitOfWork.Menu.Find(m => m.Id == menu.Id);
                if (item != null)
                {
                    item.MenuOrder = menu.MenuOrder;
                    _unitOfWork.Menu.Update(item);
                }

            }
        }

        #endregion



        public OperationOutput GetEntitiesByAdminMenuReference(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var entities = new List<EntitiesItem>();

            var user = _unitOfWork.User.GetById(RequestOwner.Id.Value);
            if (user == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            if (RequestedData.ReferenceId == user.ReferenceId)
                entities = GetFromUserEntities(RequestedData);

            else
                entities = GetFromUserEntitiesReferences(RequestedData);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.EntitiesItem, entities));
        }

        #region HELPER METHODS
        private List<EntitiesItem> GetFromUserEntitiesReferences(MenuDto RequestedData)
        {
            return _unitOfWork.UsersEntityReference.GetAll()
                   .Include(x => x.Entity)
                   .ThenInclude(x => x.InverseParent)
                   .Where(x => x.ReferenceId == RequestedData.ReferenceId && x.EntityId != null && x.UserId == RequestOwner.Id.Value)
            .Select(x => new EntitiesItem()
            {

                Id = x.Entity != null ? x.Entity.Id : null,
                TypeId = x.Entity != null ? x.Entity.TypeId : null,
                NameAr = x.Entity != null ? x.Entity.NameAr : null,
                NameEn = x.Entity != null ? x.Entity.NameEn : null,
                ShowMenuIdentifier = x.Entity != null ? x.Entity.ShowMenuIdentifier : null,
                SubEntities = x.Entity.InverseParent.Select(c => new EntitiesItem
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn
                }).ToList()

            }).OrderBy(x => x.Id).ToList();
        }

        private List<EntitiesItem> GetFromUserEntities(MenuDto RequestedData)
        {
            return _unitOfWork.UsersEntity.GetAll()
                   .Include(x => x.Entity)
                   .ThenInclude(x => x.InverseParent)
                   .Where(x => x.EntityId != null && x.UserId == RequestOwner.Id.Value)
            .Select(x => new EntitiesItem()
            {

                Id = x.Entity != null ? x.Entity.Id : null,
                TypeId = x.Entity != null ? x.Entity.TypeId : null,
                NameAr = x.Entity != null ? x.Entity.NameAr : null,
                NameEn = x.Entity != null ? x.Entity.NameEn : null,
                ShowMenuIdentifier = x.Entity != null ? x.Entity.ShowMenuIdentifier : null,
                SubEntities = x.Entity.InverseParent.Select(c => new EntitiesItem
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn
                }).ToList()

            }).OrderBy(x => x.Id).ToList();
        }

        #endregion

        public async Task<OperationOutput> GetMenuDataById(MenuDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var tree = new List<MenuTree>();

            var menu = await _unitOfWork.Menu.GetAll(x => x.IsHidden == false && x.IsDeleted == false
                             && x.Id == RequestedData.Id)
                            .Include(x => x.Article)
                            .Include(c => c.Entity)
                            .AsNoTracking().FirstOrDefaultAsync();

            if (menu != null)
            {
                menu = GetMenuChildren(menu);
                var mt = new MenuTree();
                tree.Add(MapMenuToMenuTree(menu, mt));
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.MenusEntity, tree));

        }


    }
}
