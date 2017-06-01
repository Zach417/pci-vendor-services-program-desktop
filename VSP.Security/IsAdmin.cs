using VSP.Business.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSP.Security
{
    public partial class SecurityComponent
    {
        public bool IsAdmin()
        {
            foreach (SecurityRole securityRole in SecurityRoles)
            {
                if (securityRole.Id == new Guid("3127AB91-3635-4B22-A1FE-7FF083A05F54"))
                    return true;
            }

            return false;
        }
    }
}