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
        public bool IsUser()
        {
            foreach (SecurityRole securityRole in SecurityRoles)
            {
                if (securityRole.Id == new Guid("A36467D9-02A3-421C-AC89-6732B274CE43"))
                    return true;
            }

            return false;
        }
    }
}