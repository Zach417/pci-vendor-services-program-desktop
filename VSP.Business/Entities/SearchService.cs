using VSP.Business.Components;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace VSP.Business.Entities
{
    public class SearchService : DatabaseEntity
    {
        public Guid SearchId;
        public Guid ServiceId;
        public SqlBoolean ServiceRequired;
        public SqlBoolean ServicePreferred;
        public SqlBoolean ServiceOptional;

        private static string _tableName = "SearchService";

        public SearchService()
            : base(_tableName)
        {

        }

        public SearchService(Guid primaryKey)
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
            base.AddColumn("SearchId", this.SearchId);
            base.AddColumn("ServiceId", this.ServiceId);
            base.AddColumn("ServiceRequired", this.ServiceRequired);
            base.AddColumn("ServicePreferred", this.ServicePreferred);
            base.AddColumn("ServiceOptional", this.ServiceOptional);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.SearchId = (Guid)base.GetColumn("SearchId");
            this.ServiceId = (Guid)base.GetColumn("ServiceId");
            this.ServiceRequired = (SqlBoolean)base.GetColumn("ServiceRequired");
            this.ServicePreferred = (SqlBoolean)base.GetColumn("ServicePreferred");
            this.ServiceOptional = (SqlBoolean)base.GetColumn("ServiceOptional");
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

        public static DataTable GetAssociated(Search search)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE SearchId = '" + search.Id + "'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
