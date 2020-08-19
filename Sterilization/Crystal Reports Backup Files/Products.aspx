<%@ Page Title="Products"  Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Products.aspx.cs" EnableEventValidation="false" Inherits="Sterilization.Products" %>

<asp:Content ID="ProductsContent" ContentPlaceHolderID="MainContentHolder" runat="server">
    <script type="text/javascript">
        function permission(st) {
            if (st == "FA") {
                document.getElementById("btnadd").disabled = false;
            }
            else {
                document.getElementById("btnadd").disabled = true;
                $('#MainContentHolder_grvProducts tbody tr').removeClass("GridRow");
            }
        }
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
            $('.modal-title').prepend("New Product");
            var grvProduct = document.getElementById('<%=grvProducts.ClientID%>');
            if (isNaN(parseInt(document.getElementById('MainContentHolder_hdnRowIndex').value)) != true) {
                var ri = parseInt(document.getElementById('<%=hdnRowIndex.ClientID%>').value);
                 var grvProduct = grvProduct.rows[ri].classList;
                 if (grvProduct.contains("GridSelectedRow")) {
                     grvProduct.remove("GridSelectedRow");
                 }
             }
             document.getElementById("<%= txtProductDesc.ClientID %>").value = "";
            document.getElementById("<%= txtSKUNo.ClientID %>").value = "";
            document.getElementById("<%= txtStoragecond.ClientID %>").value = "";
            document.getElementById("<%= rbtUnits.ClientID %>").checked = false;
            document.getElementById("<%= rbtBags.ClientID %>").checked = false;
            document.getElementById("<%= txtUnits.ClientID %>").value = "";
           <%-- document.getElementById("<%= txtCasesize.ClientID %>").value = "";
            document.getElementById("<%= txtUnitsize.ClientID %>").value = "";--%>
            document.getElementById("<%= txtItemDesc.ClientID %>").value = "";
            
            document.getElementById("<%= ddlCategory.ClientID %>").value = "0";           
            $("#" + "<%= ddlCategory.ClientID %>").trigger('change');


              document.getElementById("<%= ddlMasterLabels.ClientID %>").value = "0";           
            $("#" + "<%= ddlMasterLabels.ClientID %>").trigger('change');

              document.getElementById("<%= ddlInsertLabelFormat.ClientID %>").value = "0";           
            $("#" + "<%= ddlInsertLabelFormat.ClientID %>").trigger('change');

              document.getElementById("<%= ddlCaseLabelFormat.ClientID %>").value = "0";           
            $("#" + "<%= ddlCaseLabelFormat.ClientID %>").trigger('change');


        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        $(function () {
            $('.GridRow').click(function () {
               
                $(".modal-title").text('');
                $('.modal-title').prepend("Edit Product");
                var grvProductLabeling = document.getElementById('<%=grvProducts.ClientID%>');
                $('.GridRow').removeClass('GridSelectedRow');
                $(this).addClass('GridSelectedRow');
                var rowIndexs = this.rowIndex;
                var ri = parseInt(rowIndexs);
                document.getElementById('<%=hdnRowIndex.ClientID%>').value = rowIndexs;
                document.getElementById('<%=hdnproductid.ClientID%>').value = grvProductLabeling.rows[ri].cells[0].innerHTML;
                document.getElementById("<%= txtProductDesc.ClientID %>").value = grvProductLabeling.rows[ri].cells[1].innerHTML;
                document.getElementById("<%= txtSKUNo.ClientID %>").value = grvProductLabeling.rows[ri].cells[3].innerHTML;
                document.getElementById("<%= txtStoragecond.ClientID %>").value = grvProductLabeling.rows[ri].cells[4].innerHTML;
                if (grvProductLabeling.rows[ri].cells[2].innerHTML != "&nbsp;") {
                    document.getElementById("<%= txtItemDesc.ClientID %>").value = grvProductLabeling.rows[ri].cells[2].innerHTML;
                }
                if (grvProductLabeling.rows[ri].cells[6].innerHTML != "&nbsp;") {
                    document.getElementById("<%= txtUnits.ClientID %>").value = grvProductLabeling.rows[ri].cells[6].innerHTML;
                }
                

                if (grvProductLabeling.rows[ri].cells[8].innerHTML == 1) {
                    document.getElementById("<%= rbtUnits.ClientID %>").checked = true;
                }
                else if (grvProductLabeling.rows[ri].cells[8].innerHTML == 2) {
                    document.getElementById("<%= rbtBags.ClientID %>").checked = true;
                }
                
              
                //rd[grvProductLabeling.rows[ri].cells[8].innerHTML].checked = true;
               
                 <%--if (grvProductLabeling.rows[ri].cells[7].innerHTML != "&nbsp;") {
                    document.getElementById("<%= txtUnitsize.ClientID %>").value = grvProductLabeling.rows[ri].cells[7].innerHTML;
                } --%>               
                $("#" + "<%= ddlCategory.ClientID %>").val(grvProductLabeling.rows[ri].cells[9].innerHTML);                
                $("#" + "<%= ddlCategory.ClientID %>").trigger('change');


                 $("#" + "<%= ddlMasterLabels.ClientID %>").val(grvProductLabeling.rows[ri].cells[10].innerHTML);                
                $("#" + "<%= ddlMasterLabels.ClientID %>").trigger('change');

                 $("#" + "<%= ddlInsertLabelFormat.ClientID %>").val(grvProductLabeling.rows[ri].cells[11].innerHTML);                
                $("#" + "<%= ddlInsertLabelFormat.ClientID %>").trigger('change');

                 $("#" + "<%= ddlCaseLabelFormat.ClientID %>").val(grvProductLabeling.rows[ri].cells[12].innerHTML);                
                $("#" + "<%= ddlCaseLabelFormat.ClientID %>").trigger('change');

                document.getElementById("<%= btnCancel.ClientID %>").style.display = "block";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "block";
                document.getElementById("<%= btnSave.ClientID %>").disabled = true;

                $('#myModal').modal('show');
            });


            $("#" + "<%= ddFilter.ClientID %>").change(function () {
                if (document.getElementById('<%=hdnRowIndex.ClientID%>').value) {
                    var ri = parseInt(document.getElementById('<%=hdnRowIndex.ClientID%>').value);
                    var grvProductLabeling = document.getElementById('<%=grvProducts.ClientID%>');
                    var grvProduct = grvProductLabeling.rows[ri].classList;
                    if (grvProduct.contains("GridSelectedRow")) {
                        grvProduct.remove("GridSelectedRow");
                    }
                }
                if (document.getElementById("<%= ddFilter.ClientID %>").value != "0") {
                    document.getElementById("<%= txtfilter.ClientID %>").disabled = false;
                }
                else {
                    document.getElementById("<%= txtfilter.ClientID %>").disabled = true;

                }
            });

            <%-- $("#" + "<%= txtfilter.ClientID %>").keyup(function () {
                if ($("#" + "<%= txtfilter.ClientID %>").val().trim().length > 0) {
                     $("#" + "<%= btnFilter.ClientID %>").removeAttr('disabled');
                } else
                    $("#" + "<%= btnFilter.ClientID %>").attr('disabled', 'disabled');
             });--%>

        });
    </script>
    <style>
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
    </style>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Products</h3>
            </div>
        </div>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="ClearControls('c');">&times;</button>
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Product Description:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtProductDesc" runat="server" TextMode="MultiLine" Width="100%" Height="50"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtProductDesc" ID="rfvProductDesc" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                         <div class="form-group">
                            <label class="col-lg-4 control-label">Item Description:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtItemDesc" runat="server" TextMode="MultiLine" Width="100%" Height="50"></asp:TextBox>
                            </div>
                            
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">SKU Number:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtSKUNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtSKUNo" ID="rfvSKUNo" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Storage Condition:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtStoragecond" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtStoragecond" runat="server" ForeColor="Red" ValidationGroup="validaiton"  ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Category:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlCategory" CssClass="btn btn-default btn-md" runat="server" Style="width: 100%;">
                                      <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Component</asp:ListItem>
                                    <asp:ListItem Value="2">Product</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="ddlCategory" ID="rfvCategory" InitialValue="0" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />                      
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Master Label Format:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlMasterLabels" CssClass="btn btn-default btn-md" runat="server" Style="width: 100%;">
                                      <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="ddlMasterLabels" ID="rfvddlMasterLabels" InitialValue="0" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Insert Label Format:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlInsertLabelFormat" CssClass="btn btn-default btn-md" runat="server" Style="width: 100%;">
                                      <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="ddlInsertLabelFormat" ID="rfvddlInsertLabelFormat" InitialValue="0" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Case Label Format:</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlCaseLabelFormat" CssClass="btn btn-default btn-md" runat="server" Style="width: 100%;">
                                      <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="ddlCaseLabelFormat" ID="rfvddlCaseLabelFormat" InitialValue="0" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Units:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtUnits" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtUnits" ID="rfvCasesize" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Unit Type:</label>
                            <div class="col-lg-6">    
                                
                                <asp:RadioButton ID="rbtUnits" GroupName="UnitType" runat="server" Text="Units" />&nbsp;

                                <asp:RadioButton ID="rbtBags" GroupName="UnitType" runat="server" Text="Bags" />&nbsp;                           
                              <%--  <asp:RadioButtonList runat="server" ID="rdUnit" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                    CssClass="labels">
                                    <asp:ListItem Text="Units" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Bags" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>--%>
                            </div>
                            <%--<div>
                                <asp:RequiredFieldValidator ControlToValidate="rdUnit" ID="rfvUnits" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>--%>
                        </div>
                      <%--  <div class="form-group">
                            <label class="col-lg-4 control-label">Case Size:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtCasesize" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtCasesize" ID="rfvCasesize" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Unit Size:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtUnitsize" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                          
                        </div>--%>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-3">
                                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="validaiton" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click" Style="display: none;" />
                            </div>
                            <div class="col-md-3" style="margin-left: 4%;">
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="btnUpdate_Click" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <br />
        <div class="row">
            <asp:Button ID="btnTestPrint" CssClass="btn btn-primary btn-md" runat="server" Text="Test Print" OnClick="btnTestPrint_Click" />
        </div>
        <br />
        <div class="row">
            <div class="col-md-2 col-xs-2">
                <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" onclick="ClearControls('A');" data-target="#myModal">Add New Product</button>
            </div>
            <div class="col-md-1 col-xs-1 text-right" style="margin-top: 1%; padding: 0;">
                <label class="control-label">Filter By:</label>
            </div>
            <div class="col-md-3 col-xs-3">
                <asp:DropDownList ID="ddFilter" runat="server" CssClass="selectpicker" Style="width: 100%;">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="ProductDesc">Product Description</asp:ListItem>
                    <asp:ListItem Value="SKUNo">SKU Number</asp:ListItem>

                </asp:DropDownList>
            </div>
            <div class="col-md-3 col-xs-3">
                <asp:TextBox ID="txtfilter" CssClass="form-control btn-md" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1 col-xs-1">
                <asp:Button ID="btnFilter" CssClass="btn btn-primary btn-md" runat="server" Text="Filter" OnClick="btnFilter_Click" />
            </div>
            <div class="col-md-1 col-xs-1">
                <asp:Button ID="btnRefresh" CssClass="btn btn-primary btn-md" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                 <div id="PrintingGridScroll" class="grid_scroll">
                <asp:GridView ID="grvProducts" runat="server" CellPadding="4"
                    CssClass="table table-striped table-bordered table-condensed"
                    AutoGenerateColumns="False" AllowSorting="True" OnSorting="grvProducts_Sorting" OnRowDataBound="grvProducts_RowDataBound"
                    Width="900px" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" SortExpression="ProductID" HeaderStyle-CssClass="header-center">
                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductDesc" HeaderText="Product Description" SortExpression="ProductDesc" HeaderStyle-CssClass="header-center">
                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                          <asp:BoundField DataField="ItemsDesc" HeaderText="Items Description" SortExpression="ItemsDesc" HeaderStyle-CssClass="header-center">
                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="SKUNo" HeaderText="SKU/CAT Number" SortExpression="SKUNo" HeaderStyle-CssClass="header-center">

                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="StorageCondition" HeaderText="Storage Condition" SortExpression="StorageCondition" HeaderStyle-CssClass="header-center">

                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" HeaderStyle-CssClass="header-center">

                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Units" HeaderText="Units" SortExpression="Units" HeaderStyle-CssClass="header-center">

                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="UnitTypeDesc"  HeaderText="Unit Description" SortExpression="UnitTypeDesc" HeaderStyle-CssClass="header-center">
                            <HeaderStyle CssClass="header-center"></HeaderStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="UnitType" SortExpression="UnitType" HeaderStyle-CssClass="header-center">
                           <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CategoryCode" HeaderText="CategoryCode" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">

                            <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="MasterFormat" SortExpression="MasterFormat" HeaderStyle-CssClass="header-center">
                           <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="InsertFormat" SortExpression="InsertFormat" HeaderStyle-CssClass="header-center">
                           <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="CaseFormat" SortExpression="CaseFormat" HeaderStyle-CssClass="header-center">
                           <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
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
        <asp:HiddenField ID="hdnproductid" runat="server" />
    </div>
</asp:Content>
