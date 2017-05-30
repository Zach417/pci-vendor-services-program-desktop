using DihEntity = DataIntegrationHub.Business.Entities;
using VspEntity = VSP.Business.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSP.Security
{
    public partial class SecurityComponent
    {
        public DihEntity.User User { get; private set; }
        public List<VspEntity.SecurityRole> SecurityRoles { get; private set; }

        public SecurityComponent(DihEntity.User user)
        {
            User = user;
            SecurityRoles = VspEntity.UserSecurityRole.AssociatedSecurityRolesFromUser(this.User.UserId);
        }
    }
}
