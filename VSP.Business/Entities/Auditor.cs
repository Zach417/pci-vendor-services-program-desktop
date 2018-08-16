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
    public class Auditor : DatabaseEntity
    {
        public Guid AuditorId;
        public string Notes;

        private static string _tableName = "Auditor";

        public Auditor()
            : base(_tableName)
        {

        }

        public Auditor(Guid primaryKey)
            : base(_tableName, primaryKey)
        {
            RefreshMembers(true);
        }

        /// <summary>
        /// Registers the instance's members with the abstract class in order to perform database operations. Do not register members
        /// that exist within the abstract class (e.g. CreatedOn).
        /// </summary>
        protected override void RegisterMembers()
        {
            base.AddColumn("AuditorId", this.AuditorId);
            base.AddColumn("Notes", this.Notes);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.AuditorId = (Guid)base.GetColumn("AuditorId");
            this.Notes = (string)base.GetColumn("Notes");
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
