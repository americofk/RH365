using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries
{
    public class ValidatePrivilege : IValidatePrivilege
    {
        private readonly IApplicationDbContext _dbContext;

        public ValidatePrivilege(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<bool> ValidateAction(string Alias, string MenuId, bool View, bool Delete, bool Edit)
        {
            if (Delete)
            {
                return await _dbContext.MenuAssignedToUsers
                             .AnyAsync(x => x.Alias == Alias &&
                             x.MenuId == MenuId && x.PrivilegeDelete == true);
            }

            if (Edit)
            {
                return await _dbContext.MenuAssignedToUsers
                             .AnyAsync(x => x.Alias == Alias &&
                             x.MenuId == MenuId && ( x.PrivilegeDelete == true || x.PrivilegeEdit == true));
            }

            if(View)
            {
                return await _dbContext.MenuAssignedToUsers
                             .AnyAsync(x => x.Alias == Alias &&
                             x.MenuId == MenuId && (x.PrivilegeDelete == true || x.PrivilegeEdit == true || x.PrivilegeView == true));
            }

            return false;
        }
    }
}
