using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using VSP.Business.Entities;
using ISP.Business.Entities;
using DataIntegrationHub.Business.Entities;

public partial class Bids : Page
{
    public DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper;
    public Search search;
    public Customer Customer;
    public DataIntegrationHub.Business.Entities.Plan DihPlan;
    public VSP.Business.Entities.PlanDetail VspPlan;
    public List<Fund> Funds = new List<Fund>();
    public List<Relational_Funds_Plans> PlanFunds = new List<Relational_Funds_Plans>();
    public List<Service> Services = new List<Service>();
    public List<SearchQuestion> Questions = new List<SearchQuestion>();
    public PlanParticipantsEligible PlanParticipantsEligible = new PlanParticipantsEligible();
    public PlanParticipantsActive PlanParticipantsActive = new PlanParticipantsActive();
    public List<SearchFund> SearchFunds = new List<SearchFund>();

    protected void Page_Load(object sender, EventArgs e)
    {
        string recordKeeperId = Request.QueryString["rk"];
        string searchId = Request.QueryString["s"];
        LoadEntities();
    }

    private void LoadEntities()
    {
        LoadRecordKeeper();
        LoadSearch();

        if (search != null)
        {
            VspPlan = new VSP.Business.Entities.PlanDetail(search.PlanId);
            DihPlan = new DataIntegrationHub.Business.Entities.Plan(search.PlanId);
            Customer = new Customer(DihPlan.CustomerId);

            LoadPlanParticipantsEligible(search.PlanId);
            LoadPlanParticipantsActive(search.PlanId);
            LoadFunds(search.PlanId);
            LoadSearchFunds(search.PlanId);
        }
    }

    private void LoadRecordKeeper()
    {
        string recordKeeperId = Request.QueryString["rk"];

        try
        {
            Guid id = Guid.Parse(recordKeeperId);
            recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(id);
        }
        catch
        {
            recordKeeper = null;
        }
    }

    private void LoadSearch()
    {
        string searchId = Request.QueryString["s"];

        try
        {
            Guid id = Guid.Parse(searchId);
            search = new Search(id);
        }
        catch
        {
            search = null;
        }
    }

    private void LoadPlanParticipantsEligible(Guid planId)
    {
        DataTable dataTable = PlanParticipantsEligible.GetAssociated(planId);
        List<PlanParticipantsEligible> list = new List<PlanParticipantsEligible>();
        foreach (DataRow dr in dataTable.Rows)
        {
            var id = new Guid(dr["PlanParticipantsEligibleId"].ToString());
            var obj = new PlanParticipantsEligible(id);
            list.Add(obj);
        }

        if (list.Count > 0)
        {
            PlanParticipantsEligible = list.OrderByDescending(x => x.AsOfDate).First();
        }
    }

    private void LoadPlanParticipantsActive(Guid planId)
    {
        DataTable dataTable = PlanParticipantsActive.GetAssociated(planId);
        List<PlanParticipantsActive> list = new List<PlanParticipantsActive>();
        foreach (DataRow dr in dataTable.Rows)
        {
            var id = new Guid(dr["PlanParticipantsActiveId"].ToString());
            var obj = new PlanParticipantsActive(id);
            list.Add(obj);
        }

        if (list.Count > 0)
        {
            PlanParticipantsActive = list.OrderByDescending(x => x.AsOfDate).First();
        }
    }

    private void LoadFunds(Guid planId)
    {
        DataTable dataTable = Fund.GetActiveAssociatedFromPlan(search.PlanId);
        foreach (DataRow dr in dataTable.Rows)
        {
            var fundId = new Guid(dr["FundId"].ToString());
            var rfpId = new Guid(dr["Relational_Funds_PlansId"].ToString());
            var fund = new Fund(fundId);
            var rfp = new Relational_Funds_Plans(rfpId);
            Funds.Add(fund);
            PlanFunds.Add(rfp);
        }
    }

    private void LoadSearchFunds(Guid planId)
    {
        DataTable dataTable = SearchFund.GetAssociated(search);
        foreach (DataRow dr in dataTable.Rows)
        {
            var searchFundId = new Guid(dr["SearchFundId"].ToString());
            var searchFund = new SearchFund(searchFundId);
            SearchFunds.Add(searchFund);
        }
    }
}