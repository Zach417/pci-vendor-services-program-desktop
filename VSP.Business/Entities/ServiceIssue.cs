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
    public class ServiceIssue : DatabaseEntity
    {
        public Guid? RecordKeeperId;
        public Guid? AuditorId;
        public Guid? PlanId;
        public string SubjectValue;
        public string DescriptionValue;
        public DateTime AsOfDate;

        private static string _tableName = "ServiceIssue";

        public ServiceIssue()
            : base(_tableName)
        {

        }

        public ServiceIssue(Guid primaryKey)
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
            base.AddColumn("RecordKeeperId", this.RecordKeeperId);
            base.AddColumn("AuditorId", this.AuditorId);
            base.AddColumn("PlanId", this.PlanId);
            base.AddColumn("SubjectValue", this.SubjectValue);
            base.AddColumn("DescriptionValue", this.DescriptionValue);
            base.AddColumn("AsOfDate", this.AsOfDate);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.RecordKeeperId = (Guid?)base.GetColumn("RecordKeeperId");
            this.AuditorId = (Guid?)base.GetColumn("AuditorId");
            this.PlanId = (Guid?)base.GetColumn("PlanId");
            this.SubjectValue = (string)base.GetColumn("SubjectValue");
            this.DescriptionValue = (string)base.GetColumn("DescriptionValue");
            this.AsOfDate = (DateTime)base.GetColumn("AsOfDate");
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
