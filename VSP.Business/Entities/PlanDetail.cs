using VSP.Business.Components;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace VSP.Business.Entities
{
    public class PlanDetail : DatabaseEntity
    {
        public Guid PlanId;

        private static string _tableName = "PlanDetail";

        public PlanDetail()
            : base(_tableName)
        {

        }

        public PlanDetail(Guid primaryKey)
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
            base.AddColumn("PlanId", this.PlanId);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.PlanId = (Guid)base.GetColumn("PlanId");
        }

        public static DataTable GetActive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 0 ORDER BY [Type], [Category], [Name]";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetInactive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 1 ORDER BY [Type], [Category], [Name]";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
