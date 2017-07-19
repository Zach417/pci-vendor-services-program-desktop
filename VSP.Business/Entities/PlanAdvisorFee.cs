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
    public class PlanAdvisorFee : DatabaseEntity
    {
        public Guid AdvisorId;
        public Guid PlanId;
        public decimal? Fee;
        public decimal? BenchmarkFee;
        public DateTime? AsOfDate;

        private static string _tableName = "PlanAdvisorFee";

        public PlanAdvisorFee()
            : base(_tableName)
        {

        }

        public PlanAdvisorFee(Guid primaryKey)
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
            base.AddColumn("AdvisorId", this.AdvisorId);
            base.AddColumn("PlanId", this.PlanId);
            base.AddColumn("Fee", this.Fee);
            base.AddColumn("BenchmarkFee", this.BenchmarkFee);
            base.AddColumn("AsOfDate", this.AsOfDate);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.AdvisorId = (Guid)base.GetColumn("AdvisorId");
            this.PlanId = (Guid)base.GetColumn("PlanId");
            this.Fee = (decimal?)base.GetColumn("Fee");
            this.BenchmarkFee = (decimal?)base.GetColumn("BenchmarkFee");
            this.AsOfDate = (DateTime?)base.GetColumn("AsOfDate");
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

        public static DataTable GetAssociatedActive(PlanAdvisor planAdvisor)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 0 AND PlanId = \'" + planAdvisor.PlanId.ToString() + "\' AND AdvisorId = \'" + planAdvisor.AdvisorId + "\'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetAssociatedInactive(PlanAdvisor planAdvisor)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 1 AND PlanId = \'" + planAdvisor.PlanId.ToString() + "\' AND AdvisorId = \'" + planAdvisor.AdvisorId + "\'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
