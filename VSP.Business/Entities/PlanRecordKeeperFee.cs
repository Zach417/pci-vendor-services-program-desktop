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
    public class PlanRecordKeeperFee : DatabaseEntity
    {
        public Guid RecordKeeperId;
        public Guid PlanId;
        public decimal? Fee;
        public decimal? Benchmark25Fee;
        public decimal? Benchmark50Fee;
        public decimal? Benchmark75Fee;
        public bool RevenueSharingPaid;
        public bool ForfeituresPaid;
        public bool ParticipantsPaid;
        public bool PlanSponsorPaid;
        public DateTime? AsOfDate;
        public string Notes;

        private static string _tableName = "PlanRecordKeeperFee";

        public PlanRecordKeeperFee()
            : base(_tableName)
        {

        }

        public PlanRecordKeeperFee(Guid primaryKey)
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
            base.AddColumn("PlanId", this.PlanId);
            base.AddColumn("Fee", this.Fee);
            base.AddColumn("Benchmark25Fee", this.Benchmark25Fee);
            base.AddColumn("Benchmark50Fee", this.Benchmark50Fee);
            base.AddColumn("Benchmark75Fee", this.Benchmark75Fee);
            base.AddColumn("RevenueSharingPaid", this.RevenueSharingPaid);
            base.AddColumn("ForfeituresPaid", this.ForfeituresPaid);
            base.AddColumn("ParticipantsPaid", this.ParticipantsPaid);
            base.AddColumn("PlanSponsorPaid", this.PlanSponsorPaid);
            base.AddColumn("AsOfDate", this.AsOfDate);
            base.AddColumn("Notes", this.Notes);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.RecordKeeperId = (Guid)base.GetColumn("RecordKeeperId");
            this.PlanId = (Guid)base.GetColumn("PlanId");
            this.Fee = (decimal?)base.GetColumn("Fee");
            this.Benchmark25Fee = (decimal?)base.GetColumn("Benchmark25Fee");
            this.Benchmark50Fee = (decimal?)base.GetColumn("Benchmark50Fee");
            this.Benchmark75Fee = (decimal?)base.GetColumn("Benchmark75Fee");
            this.RevenueSharingPaid = (System.Data.SqlTypes.SqlBoolean)base.GetColumn("RevenueSharingPaid") ? true : false;
            this.ForfeituresPaid = (System.Data.SqlTypes.SqlBoolean)base.GetColumn("ForfeituresPaid") ? true : false;
            this.ParticipantsPaid = (System.Data.SqlTypes.SqlBoolean)base.GetColumn("ParticipantsPaid") ? true : false;
            this.PlanSponsorPaid = (System.Data.SqlTypes.SqlBoolean)base.GetColumn("PlanSponsorPaid") ? true : false;
            this.AsOfDate = (DateTime?)base.GetColumn("AsOfDate");
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

        public static DataTable GetAssociatedActive(PlanRecordKeeper planRK)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 0 AND PlanId = \'" + planRK.PlanId.ToString() + "\' AND RecordKeeperId = \'" + planRK.RecordKeeperId + "\'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetAssociatedInactive(PlanRecordKeeper planRK)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE StateCode = 1 AND PlanId = \'" + planRK.PlanId.ToString() + "\' AND RecordKeeperId = \'" + planRK.RecordKeeperId + "\'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
