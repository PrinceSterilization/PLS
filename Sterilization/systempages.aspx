<%@ Page Title="System Pages" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="systempages.aspx.cs" Inherits="Sterilization.systempages" %>

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
         function ClearControls(e) {            
            if (e == "A") {
                document.getElementById("<%= btnCancel.ClientID %>").style.display = "none";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "none";
                document.getElementById("<%= btnSave.ClientID %>").disabled = false;
            }
            $(".modal-title").text('');
            $('.modal-title').prepend("New System page");
            var grvProduct = document.getElementById('<%=grvPages.ClientID%>');
            if (isNaN(parseInt(document.getElementById('MainContentHolder_hdnRowIndex').value)) != true) {
                var ri = parseInt(document.getElementById('<%=hdnRowIndex.ClientID%>').value);
                 var grvProduct = grvProduct.rows[ri].classList;
                 if (grvProduct.contains("GridSelectedRow")) {
                     grvProduct.remove("GridSelectedRow");
                 }
             }
             document.getElementById("<%= txtPageName.ClientID %>").value = "";
            
            document.getElementById("<%= txtPagedesc.ClientID %>").value = "";
        }
       
        $(function () {
            $('.GridRow').click(function () {            
                $(".modal-title").text('');
                $('.modal-title').prepend("Edit System Page");
                var grvPages = document.getElementById('<%=grvPages.ClientID%>');
                $('.GridRow').removeClass('GridSelectedRow');
                $(this).addClass('GridSelectedRow');
                var rowIndexs = this.rowIndex;
                var ri = parseInt(rowIndexs);
                document.getElementById('<%=hdnRowIndex.ClientID%>').value = rowIndexs;
                document.getElementById('<%=hdnpageid.ClientID%>').value = grvPages.rows[ri].cells[0].innerHTML;
                document.getElementById("<%= txtPageName.ClientID %>").value = grvPages.rows[ri].cells[1].innerHTML;               
                document.getElementById("<%= txtPagedesc.ClientID %>").value = grvPages.rows[ri].cells[2].innerHTML;              
                document.getElementById("<%= btnCancel.ClientID %>").style.display = "block";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "block";
                document.getElementById("<%= btnSave.ClientID %>").disabled = true;

                $('#myModal').modal('show');
            });
         });
    </script>
    <style type="text/css">
         table#MainContentHolder_grvUserGroups td {
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
    </style>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>System Pages</h3>
            </div>
        </div>
        <br />
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="ClearControls('c');">&times;</button>
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">                       
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Page Name:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtPageName" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtPageName" ID="rfvtxtPageName" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                      
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Page Description:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtPagedesc" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtPagedesc" ID="rfvtxtPagedesc" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-2">
                                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="validaiton" OnClick="btnSave_Click" />
                            </div>

                            <div class="col-md-3">
                                <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" Style="display: none; margin-left: 50%;" OnClick="btnCancel_Click" />
                            </div>
                            <div class="col-md-3" style="margin-left: 10%;">
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" Style="display: none;" OnClick="btnUpdate_Click" ValidationGroup="validaiton" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
          <br />
         <div class="row">
            <div class="col-md-2">
                <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" onclick="ClearControls('A');" data-target="#myModal">Add System Page</button>
            </div>
            <div class="col-md-1  text-right" style="margin-top: 1%; padding: 0;">
                <label class="control-label">Filter By:</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddFilter" runat="server" CssClass="selectpicker" Style="width: 100%;">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="PageName">Page Name</asp:ListItem>
                    <asp:ListItem Value="PageDesc">Page Description</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtfilter" CssClass="form-control btn-md" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFilter" CssClass="btn btn-primary btn-md" runat="server" Text="Filter" OnClick="btnFilter_Click" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnRefresh" CssClass="btn btn-primary btn-md" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                <div id="SystemPagesGridScroll" class="grid_scroll">
                       <asp:GridView ID="grvPages" runat="server"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                        CssClass="table table-striped table-bordered table-condensed"
                        CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" OnRowDataBound="grvUserGroups_RowDataBound" OnSorting="grvUserGroups_Sorting"  >
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="PageID" HeaderText="PageID" SortExpression="PageID" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PageName" HeaderText="Page Name" SortExpression="PageName" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PageDesc" HeaderText="Page Description" SortExpression="PageDesc" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" DataFormatString="{0:d}" HeaderText="Date Created" SortExpression="DateCreated" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                           
                        </Columns>
                        <RowStyle CssClass="GridRow" />
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
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnRowIndex" runat="server" />
        <asp:HiddenField ID="hdnpageid" runat="server" />
</asp:Content>
