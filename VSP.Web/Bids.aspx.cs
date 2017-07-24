using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using VSP.Business.Entities;
using ISP.Business.Entities;
using DataIntegrationHub.Business.Entities;

public partial class Bids : Page
{
    public SearchBid searchBid = new SearchBid();
    public List<SearchBidQuestion> searchBidQuestions = new List<SearchBidQuestion>();
    public DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper;
    public Search search;
    public Customer Customer;
    public DataIntegrationHub.Business.Entities.Plan DihPlan;
    public VSP.Business.Entities.PlanDetail VspPlan;
    public List<Fund> Funds = new List<Fund>();
    public List<Relational_Funds_Plans> PlanFunds = new List<Relational_Funds_Plans>();
    public List<SearchQuestion> SearchQuestions = new List<SearchQuestion>();
    public PlanParticipantsEligible PlanParticipantsEligible = new PlanParticipantsEligible();
    public PlanParticipantsActive PlanParticipantsActive = new PlanParticipantsActive();
    public List<SearchFund> SearchFunds = new List<SearchFund>();
    public List<SearchService> SearchServices = new List<SearchService>();
    public List<PlanDistribution> PlanDistributions = new List<PlanDistribution>();
    public List<PlanContribution> PlanContributions = new List<PlanContribution>();

    protected void Page_Load(object sender, EventArgs e)
    {
        string recordKeeperId = Request.QueryString["rk"];
        string searchId = Request.QueryString["s"];
        LoadEntities();

        if (search != null && recordKeeper != null)
        {
            searchBid.SearchId = search.Id;
            searchBid.RecordKeeperId = recordKeeper.RecordKeeperId;
        }

        if (!this.IsPostBack)
        {
            DataBindRepeaterSearchQuestions();
        }
    }

    protected void ButtonSubmit_OnClick(object sender, EventArgs e)
    {
        searchBid.FullName = fullname.Value;
        searchBid.Email = email.Value;
        searchBid.ConfirmInvestments = SqlBoolean.Parse(confirminvestments.Checked.ToString());
        searchBid.ConfirmServices = SqlBoolean.Parse(confirmservices.Checked.ToString());
        searchBid.RequiredRevenue = decimal.Parse(requirerevenue.Value);
        searchBid.RequiredRevenueExplanation = requirerevenueexplanation.Value;
        searchBid.AncillaryServices = ancillaryservices.Value;
        searchBid.SaveRecordToDatabase(new Guid("17F6FCEB-CF02-E411-9726-D8D385C29900"));

        foreach (RepeaterItem item in RepeaterSearchQuestions.Items)
        {
            HiddenField hiddenField = (HiddenField)item.FindControl("SearchQuestionId");
            SearchQuestion searchQuestion = new SearchQuestion(new Guid(hiddenField.Value.ToString()));
            SearchBidQuestion searchBidQuestion = searchBidQuestions.Find(x => x.SearchQuestionId == searchQuestion.Id);
            CheckBox checkBox = (CheckBox)item.FindControl("SearchQuestionAnswer");
            searchBidQuestion.AnswerValue = SqlBoolean.Parse(checkBox.Checked.ToString());
            searchBidQuestion.SaveRecordToDatabase(new Guid("17F6FCEB-CF02-E411-9726-D8D385C29900"));
        }
    }

    private void DataBindRepeaterSearchQuestions()
    {
        RepeaterSearchQuestions.DataSource = SearchQuestions;
        RepeaterSearchQuestions.DataBind();
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
            LoadSearchFunds(search);
            LoadSearchServices(search);
            LoadPlanContributions(search.PlanId);
            LoadPlanDistributions(search.PlanId);
            LoadSearchQuestions(search);
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

    private void LoadSearchServices(Search search)
    {
        DataTable dataTable = SearchService.GetAssociated(search);
        foreach (DataRow dr in dataTable.Rows)
        {
            var searchServiceId = new Guid(dr["SearchServiceId"].ToString());
            var searchService = new SearchService(searchServiceId);
            if (searchService.ServiceRequired == SqlBoolean.True)
            {
                SearchServices.Add(searchService);
            }
        }
    }

    private void LoadSearchFunds(Search search)
    {
        DataTable dataTable = SearchFund.GetAssociated(search);
        foreach (DataRow dr in dataTable.Rows)
        {
            var searchFundId = new Guid(dr["SearchFundId"].ToString());
            var searchFund = new SearchFund(searchFundId);
            SearchFunds.Add(searchFund);
        }
    }

    public void LoadPlanContributions(Guid planId)
    {
        DataTable dataTable = PlanContribution.GetAssociated(planId);
        foreach (DataRow dr in dataTable.Rows)
        {
            var planContributionId = new Guid(dr["PlanContributionId"].ToString());
            var planContribution = new PlanContribution(planContributionId);
            var dateDiffMonths = ((DateTime.Now.Year - planContribution.AsOfDate.Year) * 12) + DateTime.Now.Month - planContribution.AsOfDate.Month;
            if (dateDiffMonths <= 12 * 3 && dateDiffMonths >= 0)
            {
                PlanContributions.Add(planContribution);
            }
        }
    }

    public void LoadPlanDistributions(Guid planId)
    {
        DataTable dataTable = PlanDistribution.GetAssociated(planId);
        foreach (DataRow dr in dataTable.Rows)
        {
            var planDistributionId = new Guid(dr["PlanDistributionId"].ToString());
            var planDistribution = new PlanDistribution(planDistributionId);
            var dateDiffMonths = ((DateTime.Now.Year - planDistribution.AsOfDate.Year) * 12) + DateTime.Now.Month - planDistribution.AsOfDate.Month;
            if (dateDiffMonths <= 12 * 3 && dateDiffMonths >= 0)
            {
                PlanDistributions.Add(planDistribution);
            }
        }
    }

    public void LoadSearchQuestions(Search search)
    {
        DataTable dataTable = SearchQuestion.GetAssociated(search);
        foreach (DataRow dr in dataTable.Rows)
        {
            var searchQuestionId = new Guid(dr["SearchQuestionId"].ToString());
            var searchQuestion = new SearchQuestion(searchQuestionId);
            SearchQuestions.Add(searchQuestion);

            var searchBidQuestion = new SearchBidQuestion();
            searchBidQuestion.SearchBidId = searchBid.Id;
            searchBidQuestion.SearchQuestionId = searchQuestion.Id;
            searchBidQuestions.Add(searchBidQuestion);
        }
    }
}