using VSP.Business.Components;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace VSP.Business.Entities
{
    public class UserSecurityRole : DatabaseEntity
    {
        public Guid UserId;
        public Guid SecurityRoleId;

        private static string _tableName = "UserSecurityRole";

        public UserSecurityRole()
            : base(_tableName)
        {

        }

        public UserSecurityRole(Guid primaryKey)
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
            base.AddColumn("UserId", this.UserId);
            base.AddColumn("SecurityRoleId", this.SecurityRoleId);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.UserId = (Guid)base.GetColumn("UserId");
            this.SecurityRoleId = (Guid)base.GetColumn("SecurityRoleId");
        }

        public static List<SecurityRole> AssociatedSecurityRolesFromUser(Guid userId)
        {
            List<SecurityRole> list = new List<SecurityRole>();

            foreach (DataRow dataRow in GetAssociatedUser(userId).Rows)
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

        private static DataTable GetAssociatedUser(Guid userId)
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@UserId", userId);
            string sql = @"SELECT * FROM UserSecurityRole WHERE UserId = @UserId";
            return Access.VspDbAccess.ExecuteSqlQuery(sql, parameterList);
        }
    }
}
