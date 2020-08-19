<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Sterilization.Inventory" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
      <script type="text/javascript">
        function ErrorMessage(msg) {

            document.getElementById("divalertbox").className = "";
            document.getElementById("divalertbox").className = "alert alert-danger";
            document.getElementById("divalertbox").style.display = "block";
            document.getElementById("divmsg").innerHTML = msg;
        }
        function SuccessMessage(msg) {
            document.getElementById("divalertbox").className = "";
            document.getElementById("divalertbox").className = "alert alert-success";
            document.getElementById("divalertbox").style.display = "block";
            document.getElementById("divmsg").innerHTML = msg;
        }
        function ReadErrorMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = "";
                document.getElementById("read_msg").innerHTML = divHTML;
            }
        }
        function ReadSuccessMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = "";
                document.getElementById("read_msg").innerHTML = divHTML;
            }
        }
        $(function () {
            $('#MainContentHolder_ddlType').change(function () {
                var selectedvalue = this.value;
                if (selectedvalue == 1 || selectedvalue == 2) {
                    $("#MainContentHolder_ddlcomponent").prop("disabled", true);
                }
                else {
                    $("#MainContentHolder_ddlcomponent").prop("disabled", false);
                }
            });
        });
    </script>
    <style>
        .toggle.ios, .toggle-on.ios, .toggle-off.ios {
            border-radius: 20px;
        }

            .toggle.ios .toggle-handle {
                border-radius: 20px;
            }

        .textalign {
            text-align: center;
            width: 150px;
        }
    </style>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Inventory</h3>
            </div>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <div id="read_msg">
        </div>
        <br />
        <div class="row">

            <div class="col-md-2">
                <label class="control-label">Report type:</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlType" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;">
                     <asp:ListItem Value="0">-- Select --</asp:ListItem>
                    <%--<asp:ListItem Value="1">Stock cleanroom</asp:ListItem>
                    <asp:ListItem Value="2">Takeouts from cleanroom</asp:ListItem>   
                    <asp:ListItem Value="3">Stock in warehouse</asp:ListItem>
                    <asp:ListItem Value="4">Takeouts from warehouse</asp:ListItem> --%>
                     <asp:ListItem Value="1">Warehouse Inventory</asp:ListItem>   
                    <asp:ListItem Value="2">Quarantine Inventory</asp:ListItem>   
                </asp:DropDownList>
            </div>
        </div>
         <br />
         <div class="row">

            <div class="col-md-2">
                <label class="control-label">Component:</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlcomponent" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;"></asp:DropDownList>
            </div>
        </div>
         <br />
        <div class="row">
            <div class="col-md-2  text-center">
                <asp:Button ID="btnGenerateReport" runat="server" Text="Print Preview" CssClass="btn btn-primary "  OnClick="btnGenerateReport_Click" />
            </div>
        </div>
        <div class="row">            
            <div class="col-md-12">
               <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
            </div>
        </div>
    </div>
</asp:Content>
