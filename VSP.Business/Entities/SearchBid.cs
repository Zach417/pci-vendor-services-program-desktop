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
    public class SearchBid : DatabaseEntity
    {
        public Guid SearchId;
        public Guid RecordKeeperId;
        public SqlBoolean? IsFinalist;
        public SqlBoolean? IsRecommended;
        public SqlBoolean ConfirmInvestments;
        public SqlBoolean ConfirmServices;
        public decimal RequiredRevenue;
        public string RequiredRevenueExplanation;
        public string AncillaryServices;
        public string FullName;
        public string Email;
        public string Notes;


        private static string _tableName = "SearchBid";

        public SearchBid()
            : base(_tableName)
        {

        }

        public SearchBid(Guid primaryKey)
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
            base.AddColumn("RecordKeeperId", this.RecordKeeperId);
            base.AddColumn("IsFinalist", this.IsFinalist);
            base.AddColumn("IsRecommended", this.IsRecommended);
            base.AddColumn("ConfirmInvestments", this.ConfirmInvestments);
            base.AddColumn("ConfirmServices", this.ConfirmServices);
            base.AddColumn("RequiredRevenue", this.RequiredRevenue);
            base.AddColumn("RequiredRevenueExplanation", this.RequiredRevenueExplanation);
            base.AddColumn("AncillaryServices", this.AncillaryServices);
            base.AddColumn("FullName", this.FullName);
            base.AddColumn("Email", this.Email);
            base.AddColumn("Notes", this.Notes);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.SearchId = (Guid)base.GetColumn("SearchId");
            this.RecordKeeperId = (Guid)base.GetColumn("RecordKeeperId");
            this.IsFinalist = (SqlBoolean?)base.GetColumn("IsFinalist");
            this.IsRecommended = (SqlBoolean?)base.GetColumn("IsRecommended");
            this.ConfirmInvestments = (SqlBoolean)base.GetColumn("ConfirmInvestments");
            this.ConfirmServices = (SqlBoolean)base.GetColumn("ConfirmServices");
            this.RequiredRevenue = (decimal)base.GetColumn("RequiredRevenue");
            this.RequiredRevenueExplanation = (string)base.GetColumn("RequiredRevenueExplanation");
            this.AncillaryServices = (string)base.GetColumn("AncillaryServices");
            this.FullName = (string)base.GetColumn("FullName");
            this.Email = (string)base.GetColumn("Email");
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

        public static DataTable GetAssociated(Search search)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE SearchId = '" + search.Id + "'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
