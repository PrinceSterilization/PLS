<%@ Page Title="Label Formats" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LabelFormats.aspx.cs" Inherits="Sterilization.LabelFormats" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                  document.getElementById("<%= ddlAddProducts.ClientID %>").value = "0";
                    $("#" + "<%= ddlAddProducts.ClientID %>").trigger('change');
                    document.getElementById("<%= ddlAddCategory.ClientID %>").value = "0";
                    $("#" + "<%= ddlAddCategory.ClientID %>").trigger('change');
                    document.getElementById("<%= ddlAddFormats.ClientID %>").value = "0";
                    $("#" + "<%= ddlAddFormats.ClientID %>").trigger('change');
            } 
      }
        function copyFile(e) {
            document.getElementById("MainContentHolder_hdncopyitem").value = e;
            document.getElementById("<%= ddlCopyProducts.ClientID %>").value = "0";
            $("#" + "<%= ddlCopyProducts.ClientID %>").trigger('change');
            $('#myModalCopy').modal('show');
            return false;
        }
        function updateFile(e) {
            document.getElementById("MainContentHolder_hdnUpdate").value = e;
            document.getElementById("<%= ddlUpProducts.ClientID %>").value = e.split('-')[0];
            $("#" + "<%= ddlUpProducts.ClientID %>").trigger('change');
            document.getElementById("<%= ddlUpProducts.ClientID %>").disabled = true;
            document.getElementById("<%= ddlUpCategory.ClientID %>").value = e.split('-')[1];
            $("#" + "<%= ddlUpCategory.ClientID %>").trigger('change');
            document.getElementById("<%= ddlUpFormats.ClientID %>").value = e.split('-')[2];
            $("#" + "<%= ddlUpFormats.ClientID %>").trigger('change');

            $('#myModalupdate').modal('show');
            return false;
        }



          <%--
        $(function () {
            $('.GridRow').click(function () {

                $(".modal-title").text('');
                $('.modal-title').prepend("Edit User");
                var grvUser = document.getElementById('<%=grvUser.ClientID%>');
                $('.GridRow').removeClass('GridSelectedRow');
                $(this).addClass('GridSelectedRow');
                var rowIndexs = this.rowIndex;
                var ri = parseInt(rowIndexs);
                document.getElementById('<%=hdnRowIndex.ClientID%>').value = rowIndexs;
                document.getElementById('<%=hdnuserid.ClientID%>').value = grvUser.rows[ri].cells[0].innerHTML;

                $("#" + "<%= ddlUsers.ClientID %>").val(grvUser.rows[ri].cells[0].innerHTML);
                $("#" + "<%= ddlUsers.ClientID %>").trigger('change');

                $("#" + "<%= ddlUserGroup.ClientID %>").val(grvUser.rows[ri].cells[6].innerHTML);
                $("#" + "<%= ddlUserGroup.ClientID %>").trigger('change');

                $("#" + "<%= ddlLevelCode.ClientID %>").val(grvUser.rows[ri].cells[7].innerHTML);
                $("#" + "<%= ddlLevelCode.ClientID %>").trigger('change');

                $("#" + "<%= ddlUserType.ClientID %>").val(grvUser.rows[ri].cells[5].innerHTML);
                $("#" + "<%= ddlUserType.ClientID %>").trigger('change');

                document.getElementById("<%= ddlUsers.ClientID %>").disabled = true;
                document.getElementById("<%= btnCancel.ClientID %>").style.display = "block";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "block";
                document.getElementById("<%= btnSave.ClientID %>").disabled = true;

                $('#myModal').modal('show');
            });
        });--%>
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
                <h3>Label Formats</h3>
            </div>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <br />
        <%--Copy Format Model--%>
        <div class="modal fade" id="myModalCopy" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Copy To</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Product:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlCopyProducts" runat="server" CssClass="selectpicker" Style="width: 100%;"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlCopyProducts" runat="server" ForeColor="Red" ControlToValidate="ddlCopyProducts" ValidationGroup="validaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%-- <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Category:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="selectpicker" Style="width: 100%;">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="4">Component</asp:ListItem>
                                    <asp:ListItem Value="1">Product</asp:ListItem>
                                    <asp:ListItem Value="2">Insert</asp:ListItem>
                                    <asp:ListItem Value="3">Case</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlCategory" runat="server" ForeColor="Red" ControlToValidate="ddlCategory" ValidationGroup="validaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
                        <br />
                        <br />

                    </div>
                    <br />
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-2">
                                <asp:Button ID="btnCopy_Save" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="validaiton" OnClick="btnCopy_Save_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Update Format--%>
        <div class="modal fade" id="myModalupdate" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Update Label Format</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Product:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlUpProducts" runat="server" CssClass="selectpicker" Style="width: 100%;"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlUpProducts" runat="server" ForeColor="Red" ControlToValidate="ddlUpProducts" ValidationGroup="upvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Category:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlUpCategory" runat="server" CssClass="selectpicker" Style="width: 100%;">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Component</asp:ListItem>
                                    <asp:ListItem Value="2">Product</asp:ListItem>
                                    <asp:ListItem Value="3">Insert</asp:ListItem>
                                    <asp:ListItem Value="4">Case</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlUpCategory" runat="server" ForeColor="Red" ControlToValidate="ddlUpCategory" ValidationGroup="upvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Format:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlUpFormats" runat="server" CssClass="selectpicker" Style="width: 100%;">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlUpFormats" runat="server" ForeColor="Red" ControlToValidate="ddlUpFormats" ValidationGroup="upvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                    </div>
                    <br />
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                           
                            <div class="col-md-3" style="margin-left: 10%;">
                                <asp:Button ID="btnUp_Update" CssClass="btn btn-primary" runat="server" Text="Update"  ValidationGroup="upvalidaiton" OnClick="btnUp_Update_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Adding new Formst--%>

        <div class="modal fade" id="myModalAdd" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">New Label Format</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Product:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlAddProducts" runat="server" CssClass="selectpicker" Style="width: 100%;"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlAddProducts" runat="server" ForeColor="Red" ControlToValidate="ddlAddProducts" ValidationGroup="addvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Category:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlAddCategory" runat="server" CssClass="selectpicker" Style="width: 100%;">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Component</asp:ListItem>
                                    <asp:ListItem Value="2">Product</asp:ListItem>
                                    <asp:ListItem Value="3">Insert</asp:ListItem>
                                    <asp:ListItem Value="4">Case</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlAddCategory" runat="server" ForeColor="Red" ControlToValidate="ddlAddCategory" ValidationGroup="addvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Format:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlAddFormats" runat="server" CssClass="selectpicker" Style="width: 100%;">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlAddFormats" runat="server" ForeColor="Red" ControlToValidate="ddlAddFormats" ValidationGroup="addvalidaiton" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        
                    </div>
                   
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-3">
                                <asp:Button ID="btnAddSave" CssClass="btn btn-primary" runat="server" Text="Save" Style=" margin-left: 50%;" ValidationGroup="addvalidaiton"  OnClick="btnAddSave_Click"/>
                            </div>                           
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-3">
                <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" onclick="ClearControls('A');" data-target="#myModalAdd">Add New Label Format</button>
            </div>
            <div class="col-md-1  text-right" style="margin-top: 1%; padding: 0;">
                <label class="control-label">Filter By:</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddFilter" runat="server" CssClass="selectpicker" Style="width: 100%;">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="sku">SKU</asp:ListItem>
                    <asp:ListItem Value="productdesc">Product Description</asp:ListItem>
                    <asp:ListItem Value="formatno">Format No</asp:ListItem>
                    <asp:ListItem Value="category">Category</asp:ListItem>
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
                <div class="grid_scroll">
                    <asp:GridView ID="grvLabelFormats" runat="server"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                        CssClass="table table-striped table-bordered table-condensed"
                        CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" OnRowDataBound="grvLabelFormats_RowDataBound" OnSorting="grvLabelFormats_Sorting" OnRowCommand="grvLabelFormats_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="PRODUCTDESC" HeaderText="Product Description" SortExpression="PRODUCTDESC" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CATEGORYDESC" HeaderText="Category" SortExpression="CATEGORYDESC" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FormatNo" HeaderText="Format No" SortExpression="FormatNo" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductID" HeaderText="productid" SortExpression="ProductID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CategoryCode" HeaderText="categorycode" SortExpression="CategoryCode" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CommandArgument='<%# Eval("LKEY") %>' CommandName="Copy"
                                        class="btn btn-success"
                                        OnClientClick='<%# String.Format("javascript:return copyFile(\"{0}\");", Eval("LKEY").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandArgument='<%# Eval("LKEY") %>' CommandName="Update"
                                        class="btn btn-success"
                                        OnClientClick='<%# String.Format("javascript:return updateFile(\"{0}\");", Eval("LKEY").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
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
        <asp:HiddenField ID="hdnRowIndex" runat="server" />
        <asp:HiddenField ID="hdnuserid" runat="server" />
        <asp:HiddenField ID="hdncopyitem" runat="server" />
        <asp:HiddenField ID="hdnUpdate" runat="server" />
    </div>
</asp:Content>
