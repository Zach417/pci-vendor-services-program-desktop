using VSP.Business.Components;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace VSP.Business.Entities
{
    public class SecurityRole : DatabaseEntity
    {
        public string Name;
        public string Description;

        private static string _tableName = "SecurityRole";

        public SecurityRole()
            : base(_tableName)
        {

        }

        public SecurityRole(Guid primaryKey)
            : base(_tableName, primaryKey)
        {
            RefreshMembers();
        }

        /// <summary>
        /// Registers the instance's members with the abstract class in order to perform database operations. Do not register members
        /// that exist within the abstract class (e.g. CreatedOn).
        /// </summary>
        protected override void RegisterMembers()
        {
            base.AddColumn("Name", this.Name);
            base.AddColumn("Description", this.Description);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.Name = (string)base.GetColumn("Name");
            this.Description = (string)base.GetColumn("Description");
        }

        public static List<SecurityRole> ActiveSecurityRoles()
        {
            List<SecurityRole> list = new List<SecurityRole>();

            foreach (DataRow dataRow in GetActive().Rows)
            {
                Guid securityRoleId = new Guid(dataRow["SecurityRoleId"].ToString());
                SecurityRole securityRole = new SecurityRole(securityRoleId);
                list.Add(securityRole);
            }

            return list;
        }

        public static DataTable GetActive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 0";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetInactive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 1";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
