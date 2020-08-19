<%@ Page Title="Case TakeOut" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CaseTakeout.aspx.cs" Inherits="Sterilization.CaseTakeout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("MainContentHolder_txtReadLabel").focus();
        });
      
        function ErrorMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = "";
                document.getElementById("read_msg").innerHTML = divHTML;
            }
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
        function SuccessMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = "";
                document.getElementById("read_msg").innerHTML = divHTML;
                
            }
        }
        
     
        function ErrorStatus() {

            document.getElementById("h2Status").innerHTML = "FAILED";
            //document.getElementById("divStatus").className = "failure";
            $("#divStatus").addClass("animated flash failure");
        }
        function ErrorStatusExpired() {

            document.getElementById("h2Status").innerHTML = "EXPIRED";
            //document.getElementById("divStatus").className = "failure";
            $("#divStatus").addClass("animated flash failure");
        }
        function SuccessStatus() {
           
            document.getElementById("h2Status").innerHTML = "SUCCESS";
            $("#divStatus").addClass("animated flash success");
            //document.getElementById("divStatus").className = "success";
        }
    </script>
    <style>
        .success {
            color: #fff; text-align: center; border-radius: 5px;
            background-color:green;
        }
        .failure {
            color: #fff; text-align: center; border-radius: 5px;
            background-color:red;
        }
        table#MainContentHolder_grvProducts td {
            padding: 3px;
        }

        body, .navbar-fixed-top, .navbar-fixed-bottom {
            margin-right: 0 !important;
        }

        .GridSelectedRow {
            background-color: #507cd1 !important;
            color: #fff;
        }

        .modal-open {
            padding-right: 0 !important;
        }

        html {
            overflow-y: scroll !important;
        }
         .lbltotal {
            color:red;
            font-size: large;
    font-weight: 600;
        }
    </style>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Case Takeout</h3>
            </div>
        </div>
        <br />      
        <div id="read_msg">
        </div>
        <br />
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblRemaningLabel" runat="server" CssClass="lbltotal"></asp:Label>
            </div>
        </div>
        <br />
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label"">Product Description:</label>
                <asp:Label ID="lblDescription" runat="server" class="control-label col-md-7 col-sm-6 col-xs-12"></asp:Label>               
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">SKU#:</label>
                <asp:Label ID="lblSku" runat="server" class="control-label col-md-7 col-sm-6 col-xs-12"></asp:Label>              
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">Manufacturing Date:</label>
                <asp:Label ID="lblManufacturingdate" runat="server" class="control-label col-md-7 col-sm-6 col-xs-12"></asp:Label>
               
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">Expiration Date:</label>              
                <asp:Label ID="lblExpirationdate" runat="server" class="control-label col-md-7 col-sm-6 col-xs-12"></asp:Label>
            </div>
        </div>
       
        <br />
        <div class="row">
            <div class="control-label col-md-4 col-sm-4 col-xs-12">
                <asp:TextBox ID="txtTakeoutLabel" runat="server" placeholder="Label Scan" CssClass="form-control btn-md" OnTextChanged="txtTakeoutLabel_TextChanged"></asp:TextBox>
            </div>
            <div class="col-md-4 col-sm-6 col-xs-12">
                 <asp:Label ID="lblTotalScanStatus" runat="server" CssClass="lbltotal" ></asp:Label>
                 </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div id="divStatus">
                    <h2 style="padding: 5px; font-weight: 700;" id="h2Status"></h2>
                </div>
            </div>
        </div>
        <br />       
        <asp:HiddenField runat="server" ID="hdnUsage" />
        <asp:HiddenField runat="server" ID="hdnbatchid" />
    </div>
</asp:Content>
