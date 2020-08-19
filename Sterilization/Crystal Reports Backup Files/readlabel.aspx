<%@ Page Title="Read Label" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="readlabel.aspx.cs" Inherits="Sterilization.readlabel" %>

<asp:Content ID="readlabels" ContentPlaceHolderID="MainContentHolder" runat="server">
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


        function Readlabel(ctrID, lblNo, categorycode,damage) {
            // var data = $("#txtRead").val().split('\n');
            if (ctrID != "") {
                var controlid = ctrID;// document.getElementById("<%= hdnControlid.ClientID %>").value;
                var labelno = lblNo;//data[0].split('-')[1];
                var labeldata = controlid + ',' + labelno + ',' + categorycode+ ',' + damage;
                var status = false;
                if (controlid != "" && labelno != "") {
                    $.ajax({
                        type: 'POST',
                        url: 'Webservices/WebServices.asmx/ReadLabelData',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: '{labeldata: "' + labeldata + '"}',
                        success: function (response) {
                            if (response.d == "0") {

                                ReadErrorMessage("The label has been read or voided t!");
                                //document.getElementById("txtRead").value = "";

                            }
                            else {
                                //ReadSuccessMessage("Label read sucessfully!");
                                //document.getElementById("txtRead").value = "";
                                __doPostBack("btnRead", "ReadLabel");
                                //ReadSuccessMessage("Label read sucessfully!");


                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
                return false;
            }
            else {
                ReadErrorMessage("Please Read the Label !");
                return false;
            }

        }
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

        .lbltotal {
            color: red;
            font-size: large;
            font-weight: 600;
        }
    </style>

    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Label Reading</h3>
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
            <div class="col-md-6 col-sm-6 col-xs-12">

                <label class="col-lg-5 control-label">Product Description:</label>
                <asp:Label ID="lblDescription" runat="server" class="control-label col-md-7 col-sm-3 col-xs-12"></asp:Label>
                <%--<label class="control-label col-md-3 col-sm-3 col-xs-12" id="lblDescription"></label>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">

                <label class="col-lg-5 control-label">Lot No:</label>
                <asp:Label ID="lblLotno" runat="server" class="control-label col-md-7 col-sm-3 col-xs-12"></asp:Label>
                <%--<label class="control-label col-md-3 col-sm-3 col-xs-12" id="lblDescription"></label>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">SKU#:</label>
                <asp:Label ID="lblSku" runat="server" class="control-label col-md-7 col-sm-3 col-xs-12"></asp:Label>
                <%--<label class="control-label col-md-3 col-sm-3 col-xs-12" id="lblSku"></label>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">Manufacturing Date:</label>
                <asp:Label ID="lblManufacturingdate" runat="server" class="control-label col-md-7 col-sm-3 col-xs-12"></asp:Label>
                <%--<label class="control-label col-md-3 col-sm-3 col-xs-12" id="lblManufacturingdate"></label>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <label class="col-md-5 col-sm-6 col-xs-12 control-label">Expiration Date:</label>
                <%--<label class="control-label col-md-3 col-sm-3 col-xs-12" id="lblExpirationdate"></label>--%>
                <asp:Label ID="lblExpirationdate" runat="server" class="control-label col-md-7 col-sm-3 col-xs-12"></asp:Label>
            </div>
             <div class="col-md-4 col-sm-6 col-xs-12">
             <asp:CheckBox ID="chkDamage" runat="server" Text="Replacement for damage?" Visible="false"/>
                  </div>
        </div>
       
        <br />
        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <asp:TextBox ID="txtLabel" AutoPostBack="false" runat="server" placeholder="Label Scan" OnTextChanged="txtLabel_TextChanged"></asp:TextBox>
            </div>
            <div class="col-md-4 col-sm-6 col-xs-12">
                <asp:Label ID="lblTotalScanStatus" runat="server" CssClass="lbltotal"></asp:Label>
            </div>
        </div>
       
        
         
        <br />
        <div id="GridScroll" class="grid_scroll">
            <asp:GridView ID="grvLabels" runat="server"
                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                CssClass="table table-striped table-bordered table-condensed"
                CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                BorderWidth="1px" Width="900px">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="CLABELNO" HeaderText="Label Number" SortExpression="CLABELNO" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Dateapplied" DataFormatString="{0:d}" HeaderText="Date Scanned" SortExpression="Dateapplied" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LABELSTATUS" HeaderText="Scanned" SortExpression="LABELSTATUS" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />

            </asp:GridView>

        </div>
        <asp:Panel ID="Panel1" runat="server" class="btn-group" Style="float: right;"></asp:Panel>
    </div>

    <asp:HiddenField ID="hdncategorycode" runat="server" />
    <asp:HiddenField ID="hdnControlid" runat="server" />
    <asp:HiddenField ID="hdnbatchid" runat="server" />
</asp:Content>
