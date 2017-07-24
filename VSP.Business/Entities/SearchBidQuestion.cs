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
    public class SearchBidQuestion : DatabaseEntity
    {
        public Guid SearchBidId;
        public Guid SearchQuestionId;
        public SqlBoolean AnswerValue;


        private static string _tableName = "SearchBidQuestion";

        public SearchBidQuestion()
            : base(_tableName)
        {

        }

        public SearchBidQuestion(Guid primaryKey)
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
            base.AddColumn("SearchBidId", this.SearchBidId);
            base.AddColumn("SearchQuestionId", this.SearchQuestionId);
            base.AddColumn("AnswerValue", this.AnswerValue);
        }

        /// <summary>
        /// Resets the values of all public members to their values in the database.
        /// </summary>
        protected override void SetRegisteredMembers()
        {
            this.SearchBidId = (Guid)base.GetColumn("SearchBidId");
            this.SearchQuestionId = (Guid)base.GetColumn("SearchQuestionId");
            this.AnswerValue = (SqlBoolean)base.GetColumn("AnswerValue");
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

        public static DataTable GetAssociated(SearchBid searchBid)
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE SearchBidId = '" + searchBid.Id + "'";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
