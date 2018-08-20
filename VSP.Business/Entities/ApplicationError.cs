using VSP.Business.Components;

using System;
using System.Data;

namespace VSP.Business.Entities
{
    class ApplicationError : DatabaseEntity
    {
        public DateTime TimeStamp;
        public string Message;
        public string Data;
        public string StackTrace;
        public string Source;

        private static string _tableName = "ApplicationError";

        public ApplicationError()
            : base(_tableName)
        {

        }

        public ApplicationError(Guid primaryKey)
            : base(_tableName, primaryKey)
        {
            RefreshMembers();
        }

        protected override void RegisterMembers()
        {
            base.AddColumn("TimeStamp", this.TimeStamp);
            base.AddColumn("Message", this.Message);
            base.AddColumn("Data", this.Data);
            base.AddColumn("StackTrace", this.StackTrace);
            base.AddColumn("Source", this.Source);
        }

        protected override void SetRegisteredMembers()
        {
            this.TimeStamp = (DateTime)base.GetColumn("TimeStamp");
            this.Message = (string)base.GetColumn("Message");
            this.Data = (string)base.GetColumn("Data");
            this.StackTrace = (string)base.GetColumn("StackTrace");
            this.Source = (string)base.GetColumn("Source");
        }

        public static DataTable GetAll()
        {
            string sql = @"SELECT * FROM " + _tableName;
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetActive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE  WHERE StateCode = 0";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }

        public static DataTable GetInactive()
        {
            string sql = @"SELECT * FROM " + _tableName + " WHERE  WHERE StateCode = 1";
            return Access.VspDbAccess.ExecuteSqlQuery(sql);
        }
    }
}
