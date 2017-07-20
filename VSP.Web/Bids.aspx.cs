using System;
using System.Collections.Generic;
using System.Linq;
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
    public VSP.Business.Entities.PlanDetail Plan;
    public List<Fund> Funds = new List<Fund>();
    public List<Service> Services = new List<Service>();
    public List<SearchQuestion> Questions = new List<SearchQuestion>();

    protected void Page_Load(object sender, EventArgs e)
    {
        string recordKeeperId = Request.QueryString["rk"];
        string searchId = Request.QueryString["s"];
        LoadEntities();
    }

    private void LoadEntities()
    {
        string recordKeeperId = Request.QueryString["rk"];
        string searchId = Request.QueryString["s"];

        try
        {
            Guid id = Guid.Parse(recordKeeperId);
            recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(id);
        }
        catch
        {
            recordKeeper = null;
        }

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
}