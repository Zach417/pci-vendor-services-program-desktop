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
        public string GeneralInformation;
        public string RetirementBusiness;
        public string Security;
        public string ValueBalance;

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
            base.AddColumn("GeneralInformation", this.GeneralInformation);
            base.AddColumn("RetirementBusiness", this.RetirementBusiness);
            base.AddColumn("Security", this.Security);
            base.AddColumn("ValueBalance", this.ValueBalance);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.GeneralInformation = (string)base.GetColumn("GeneralInformation");
            this.RetirementBusiness = (string)base.GetColumn("RetirementBusiness");
            this.Security = (string)base.GetColumn("Security");
            this.ValueBalance = (string)base.GetColumn("ValueBalance");
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
