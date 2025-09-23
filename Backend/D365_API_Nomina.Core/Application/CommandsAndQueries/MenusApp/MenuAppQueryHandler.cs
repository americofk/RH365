using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.MenusApp
{
    public interface IMenuAppQueryHandler : IQueryAllWithoutSearchHandler<MenuApp>
    {
        public Task<Response<IEnumerable<MenuApp>>> GetRoles();
    };

    public class MenuAppQueryHandler : IMenuAppQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserInformation _currentUserInformation;

        public MenuAppQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserInformation currentUserInformation)
        {
            _dbContext = applicationDbContext;
            _currentUserInformation = currentUserInformation;
        }

        public async Task<PagedResponse<IEnumerable<MenuApp>>> GetAll(PaginationFilter filter, object queryFilter = null)
        {
            var adminType = (AdminType)Enum.Parse(typeof(AdminType), _currentUserInformation.ElevationType);

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<MenuApp> response;

            if (adminType == AdminType.LocalAdmin)
            {
                response = await _dbContext.MenusApp
                    .Where(x => x.IsViewMenu == true)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            }
            else
            {
                response = await _dbContext.MenuAssignedToUsers.Where(x => x.Alias == _currentUserInformation.Alias)
                    .Join(_dbContext.MenusApp,
                            assigned => assigned.MenuId,
                            menu => menu.MenuId,
                            (assigned, menu) => new { Assigned = assigned, Menu = menu })
                    .Where(x => x.Menu.IsViewMenu == true)
                    .Select(x => new MenuApp
                    {
                        MenuId = x.Menu.MenuId,
                        MenuFather = x.Menu.MenuFather,
                        Description = x.Menu.Description,
                        Action = x.Menu.Action,
                        Icon = x.Menu.Icon,
                        MenuName = x.Menu.MenuName
                    })
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                var parentsId = response.Where(x => !string.IsNullOrEmpty(x.MenuFather))
                    .GroupBy(x => x.MenuFather)
                    .Select(x => new
                    {
                        id = x.Key
                    });

                foreach (var item in parentsId.ToList())
                {
                    var a = await _dbContext.MenusApp.Where(x => x.MenuId == item.id).FirstOrDefaultAsync();

                    response.Add(a);
                }
            }

            return new PagedResponse<IEnumerable<MenuApp>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<IEnumerable<MenuApp>>> GetRoles()
        {
            var adminType = (AdminType)Enum.Parse(typeof(AdminType), _currentUserInformation.ElevationType);

            List<MenuApp> response;

            if (adminType == AdminType.LocalAdmin)
            {
                response = await _dbContext.MenusApp.Where(x => !string.IsNullOrEmpty(x.MenuFather))
                    .ToListAsync();
            }
            else
            {
                response = new List<MenuApp>();
            }

            return new Response<IEnumerable<MenuApp>>(response);
        }
    }
}
