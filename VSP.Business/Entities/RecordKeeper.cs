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
    public class RecordKeeper : DatabaseEntity
    {
        public string PhilosophyStrategy;
        public string PoliciesProcedures;
        public string SecurityTechnology;
        public string CredentialsExperiences;
        public string ActionsPenalties;
        public string MergersAcquisitions;
        public string FeeArrangementsRelationships;
        public int? PlansServiced;
        public decimal? AssetsServiced;

        private static string _tableName = "RecordKeeper";

        public RecordKeeper()
            : base(_tableName)
        {

        }

        public RecordKeeper(Guid primaryKey)
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
            base.AddColumn("PhilosophyStrategy", this.PhilosophyStrategy);
            base.AddColumn("PoliciesProcedures", this.PoliciesProcedures);
            base.AddColumn("SecurityTechnology", this.SecurityTechnology);
            base.AddColumn("CredentialsExperiences", this.CredentialsExperiences);
            base.AddColumn("ActionsPenalties", this.ActionsPenalties);
            base.AddColumn("MergersAcquisitions", this.MergersAcquisitions);
            base.AddColumn("FeeArrangementsRelationships", this.FeeArrangementsRelationships);
            base.AddColumn("PlansServiced", this.PlansServiced);
            base.AddColumn("AssetsServiced", this.AssetsServiced);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.PhilosophyStrategy = (string)base.GetColumn("PhilosophyStrategy");
            this.PoliciesProcedures = (string)base.GetColumn("PoliciesProcedures");
            this.SecurityTechnology = (string)base.GetColumn("SecurityTechnology");
            this.CredentialsExperiences = (string)base.GetColumn("CredentialsExperiences");
            this.ActionsPenalties = (string)base.GetColumn("ActionsPenalties");
            this.MergersAcquisitions = (string)base.GetColumn("MergersAcquisitions");
            this.FeeArrangementsRelationships = (string)base.GetColumn("FeeArrangementsRelationships");
            this.PlansServiced = (int?)base.GetColumn("PlansServiced");
            this.AssetsServiced = (decimal?)base.GetColumn("AssetsServiced");
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
