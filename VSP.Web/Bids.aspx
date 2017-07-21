<%@ Page Title="Bids" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Bids.aspx.cs" Inherits="Bids" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%if (recordKeeper != null && search != null) { %>
    <div class="container-fluid" style="padding:15px 0px;background-color:#ffffff;background-image:url(https://pension-consultants.com/wp-content/themes/_pension/img/eye.png);background-repeat:no-repeat;background-position:-300px -180px;border-bottom:3px solid #da383c;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Welcome, <%=recordKeeper.Name%></h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                    <div style="padding:15px;background-color:#f3f3f3;border:1px solid #ccc;">
                        <p>Please enter your name and email address:</p>
                        <label for="user-name" class="input-label">
                            Your Name<br>
                            <span>
                                <input type="text" name="user-name" class="input-text" value="" size="40" id="user-name" aria-required="true" aria-invalid="false">
                            </span>
                        </label>
                        <label for="user-email" class="input-label">
                            Your Email<br>
                            <span>
                                <input type="text" name="user-email" class="input-text" value="" size="40" id="user-email" aria-required="true" aria-invalid="false">
                            </span>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding:15px 0px;background-color:#f1f4f6;;border-bottom:1px solid #ccc;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Plan Demographics</h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                    <table>
                        <thead style="display:none;">
                            <tr>
                                <td>Data point</td>
                                <td>Value</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Location</td>
                                <td><%= Customer.Address.City %>, <%= Customer.Address.State %></td>
                            </tr>
                            <tr>
                                <td>Organization Type</td>
                                <td>-</td>
                            </tr>
                            <tr>
                                <td>Plan Type</td>
                                <td><%= DihPlan.Type %></td>
                            </tr>
                            <tr>
                                <td>Plan Info: Eligible Participants</td>
                                <% if (PlanParticipantsEligible.ParticipantCount > 0) { %>
                                    <td><%= PlanParticipantsEligible.ParticipantCount.ToString("#,##") %></td>
                                <% } else { %>
                                    <td>-</td>
                                <% } %>
                            </tr>
                            <tr>
                                <td>Plan Info: Active Participants with balances</td>
                                <% if (PlanParticipantsActive.ParticipantCount > 0) { %>
                                    <td><%= PlanParticipantsActive.ParticipantCount.ToString("#,##") %></td>
                                <% } else { %>
                                    <td>-</td>
                                <% } %>
                            </tr>
                            <tr>
                                <td>Plan Info: Separated Participants with balances</td>
                                <% if ((PlanParticipantsEligible.ParticipantCount - PlanParticipantsActive.ParticipantCount) > 0) { %>
                                    <td><%= (PlanParticipantsEligible.ParticipantCount - PlanParticipantsActive.ParticipantCount).ToString("#,##") %></td>
                                <% } else { %>
                                    <td>-</td>
                                <% } %>
                            </tr>
                            <tr>
                                <td>Three Years of Contributions</td>
                                <td>Test</td>
                            </tr>
                            <tr>
                                <td>Three Years of Distributions</td>
                                <td>Test</td>
                            </tr>
                            <tr>
                                <td>Number of Outstanding Loans</td>
                                <% if (VspPlan.LoansOutstanding != null)
                                   { %>
                                    <td><%= ((int)VspPlan.LoansOutstanding).ToString("#,##") %></td>
                                <% } else { %>
                                    <td>-</td>
                                <% } %>
                            </tr>
                            <tr>
                                <td>Self-directed brokerage accounts</td>
                                <% if (VspPlan.SelfDirectedBrokerageAccounts != null)
                                   { %>
                                    <td><%= ((int)VspPlan.SelfDirectedBrokerageAccounts).ToString("#,##") %></td>
                                <% } else { %>
                                    <td>-</td>
                                <% } %>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding:15px 0px;background-color:#ffffff;border-bottom:3px solid #da383c;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Investments</h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                    <h3>Current Investments</h3>
                    <table>
                        <thead>
                            <tr>
                                <td><b>Investment ID</b></td>
                                <td><b>Investment</b></td>
                                <td><b>Balance</b></td>
                            </tr>
                        </thead>
                        <tbody>
                            <% decimal total = 0; %>
                            <% foreach (var planFund in PlanFunds.OrderBy(x => x.Ordinal)) { %>
                                <% var fund = Funds.Find(x => x.Id == planFund.FundId); %>
                                <tr>
                                    <%if (fund.Ticker.StartsWith("PCI")) { %>
                                        <td> - </td>
                                    <% } else { %>
                                        <td><%=fund.Ticker %></td>
                                    <% } %>
                                    <td><%=fund.FundName%></td>
                                    <%if (planFund.AssetValue == null) { %>
                                        <td> - </td>
                                    <% } else { %>
                                        <% total += (decimal)planFund.AssetValue; %>
                                        <td>$<%=((decimal)planFund.AssetValue).ToString("#,##.##") %></td>
                                    <% } %>
                                </tr>
                            <% } %>
                            <tr>
                                <td></td>
                                <td></td>
                                <td><b>$<%=total.ToString("#,##.##") %></b></td>
                            </tr>
                        </tbody>
                    </table>
                    <h3>Future Investments</h3>
                    <table>
                        <thead>
                            <tr>
                                <td><b>Investment ID</b></td>
                                <td><b>Investment</b></td>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var searchFund in SearchFunds) { %>
                                <tr>
                                    <%if (searchFund.Ticker.StartsWith("PCI")) { %>
                                        <td> - </td>
                                    <% } else { %>
                                        <td><%=searchFund.Ticker %></td>
                                    <% } %>
                                    <td><%=searchFund.FundName%></td>
                                </tr>
                            <% } %>
                        </tbody>
                    </table>
                    <div style="margin:25px 0px;">
                        <label for="confirm-investments" class="input-label">
                            <span>
                                <input type="checkbox" id="confirm-investments" name="confirm-investments" />
                            </span>
                            Investments listed above can be serviced by <%=recordKeeper.Name %>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding:15px 0px;background-color:#f1f4f6;;border-bottom:1px solid #ccc;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Services</h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                </div>
                <div style="margin:25px 0px;">
                    <label for="confirm-services" class="input-label">
                        <span>
                            <input type="checkbox" id="confirm-services" name="confirm-services" />
                        </span>
                        Services listed above can be provided by <%=recordKeeper.Name %>
                    </label>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding:15px 0px;background-color:#ffffff;border-bottom:3px solid #da383c;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Special Issues</h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding:15px 0px;background-color:#f1f4f6;;border-bottom:1px solid #ccc;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Fees</h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
                </div>
                <label for="required-revenue" class="input-label">
                    What is your required revenue and how is it calculated?<br>
                    <textarea name="required-revenue" class="input-text" id="required-revenue" style="min-width:100%;min-height:100px;"></textarea>
                </label>
                <label for="ancillary-services" class="input-label">
                    List fees for any ancillary services that are not included in your required revenue (loans, distributions, fund changes, etc.)<br>
                    <textarea name="ancillary-services" class="input-text" id="ancillary-services" style="min-width:100%;min-height:100px;"></textarea>
                </label>
                <button style="border: 1px solid #ccc;background-color: #da383c;color: #fff;padding: 5px 10px;margin-top:15px;">Submit</button>
            </div>
        </div>
    </div>
    <% } else { %>
    <div class="container-fluid" style="padding:15px 0px;background-color:#ffffff;background-image:url(https://pension-consultants.com/wp-content/themes/_pension/img/eye.png);background-repeat:no-repeat;background-position:-300px -180px;border-bottom:3px solid #da383c;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Whoops! There was an error loading the page.</h2>
                <div style="font-size: 1.2em;">
                    <p>This could likely be a result of using an invalid URL. Please make sure the URL used and the URL provided match. If the problem persists, please contact us for further assistance.</p>
                </div>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>
