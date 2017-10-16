using PensionConsultants.Data.Access;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSP.Business.Components
{
    public class Access
    {
        /// <summary>
        /// Represents a database connection to the production VSP database.
        /// </summary>
        public static DataAccessComponent VspDbAccess = new DataAccessComponent(DataAccessComponent.Connections.PCIDB_VendorServicesProgram, DataAccessComponent.SecurityTypes.Impersonate);

        ///// <summary>
        ///// Represents a database connection to the development VSP database.
        ///// </summary>
        //public static DataAccessComponent IspDbAccess = new DataAccessComponent(DataAccessComponent.Connections.PCIISP_InvestmentDatabase_Test);

        public static bool ConnectionSucceeded()
        {
            return VspDbAccess.ConnectionSucceeded();
        }
    }
}