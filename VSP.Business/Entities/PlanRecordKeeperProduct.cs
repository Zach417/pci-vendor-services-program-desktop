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
    public class PlanRecordKeeperProduct : DatabaseEntity
    {
        public Guid RecordKeeperProductId;
        public Guid PlanId;
        public Guid ProductId;
        public DateTime? DateAdded;
        public DateTime? DateRemoved;

        private static string _tableName = "PlanRecordKeeperProduct";

        public PlanRecordKeeperProduct()
            : base(_tableName)
        {

        }

        public PlanRecordKeeperProduct(Guid primaryKey)
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
            base.AddColumn("PlanRecordKeeperProductId", this.RecordKeeperProductId);
            base.AddColumn("PlanId", this.PlanId);
            base.AddColumn("ProductId", this.ProductId);
            base.AddColumn("DateAdded", this.DateAdded);
            base.AddColumn("DateRemoved", this.DateRemoved);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.RecordKeeperProductId = (Guid)base.GetColumn("PlanRecordKeeperProductId");
            this.PlanId = (Guid)base.GetColumn("PlanId");
            this.ProductId = (Guid)base.GetColumn("ProductId");
            this.DateAdded = (DateTime?)base.GetColumn("DateAdded");
            this.DateRemoved = (DateTime?)base.GetColumn("DateRemoved");
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

        public static DataTable GetAssociated(Guid planId)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE PlanId = \'" + planId.ToString() + "\'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
