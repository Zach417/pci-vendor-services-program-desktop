﻿using VSP.Business.Components;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace VSP.Business.Entities
{
    public class PlanAdvisor : DatabaseEntity
    {
        public Guid AdvisorId;
        public Guid PlanId;
        public DateTime? DateAdded;
        public DateTime? DateRemoved;

        private static string _tableName = "PlanAdvisor";

        public PlanAdvisor()
            : base(_tableName)
        {

        }

        public PlanAdvisor(Guid primaryKey)
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
            base.AddColumn("DateAdded", this.DateAdded);
            base.AddColumn("DateRemoved", this.DateRemoved);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.AdvisorId = (Guid)base.GetColumn("AdvisorId");
            this.PlanId = (Guid)base.GetColumn("PlanId");
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
