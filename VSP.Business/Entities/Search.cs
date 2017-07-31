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
    public class Search : DatabaseEntity
    {
        public Guid PlanId;
        public string Name;
        public string CurrentRkNotes;
        public DateTime? AsOfDate;

        private static string _tableName = "Search";

        public Search()
            : base(_tableName)
        {

        }

        public Search(Guid primaryKey)
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
            base.AddColumn("Name", this.Name);
            base.AddColumn("CurrentRkNotes", this.CurrentRkNotes);
            base.AddColumn("AsOfDate", this.AsOfDate);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.PlanId = (Guid)base.GetColumn("PlanId");
            this.Name = (string)base.GetColumn("Name");
            this.CurrentRkNotes = (string)base.GetColumn("CurrentRkNotes");
            this.AsOfDate = (DateTime?)base.GetColumn("AsOfDate");
        }

        /// <summary>
        /// Inserts matching record keeper results in SearchRecordKeeper table
        /// </summary>
        public void ExecuteSearch(Guid userId)
        {
            Hashtable hashTable = new Hashtable();
            hashTable.Add("@SearchId", this.Id);
            hashTable.Add("@UserId", userId);
            Access.VspDbAccess.ExecuteStoredProcedureNonQuery("usp_ExecuteSearch", hashTable);
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
