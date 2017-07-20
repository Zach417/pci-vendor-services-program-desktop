<%@ Page Title="Bids" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Bids.aspx.cs" Inherits="Bids" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%if (recordKeeper != null && search != null) { %>
    <div class="container-fluid" style="padding:15px 0px;background-color:#ffffff;background-image:url(https://pension-consultants.com/wp-content/themes/_pension/img/eye.png);background-repeat:no-repeat;background-position:-300px -180px;border-bottom:3px solid #da383c;">
        <div class="container">
            <div class="row">
                <h2 style="margin-top: 5px;">Welcome, <%=recordKeeper.Name%></h2>
                <div style="font-size: 0.9em;">
                    <p>Use this area to provide additional information.</p>
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
