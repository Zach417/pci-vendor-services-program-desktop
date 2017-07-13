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
    public class SearchFund : DatabaseEntity
    {
        public Guid SearchId;
        public string Ticker;
        public string FundName;

        private static string _tableName = "SearchFund";

        public SearchFund()
            : base(_tableName)
        {

        }

        public SearchFund(Guid primaryKey)
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
            base.AddColumn("Ticker", this.Ticker);
            base.AddColumn("FundName", this.FundName);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.SearchId = (Guid)base.GetColumn("SearchId");
            this.Ticker = (string)base.GetColumn("Ticker");
            this.FundName = (string)base.GetColumn("FundName");
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
