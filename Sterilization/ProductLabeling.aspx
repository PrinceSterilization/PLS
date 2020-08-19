<%@ Page Title="Product Labeling"  Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ProductLabeling.aspx.cs" EnableEventValidation="false" Inherits="Sterilization.ProductLabeling" %>

<asp:Content ID="ProductLabelingContent" ContentPlaceHolderID="MainContentHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebServices/WebServices.asmx" />

        </Services>
    </asp:ScriptManager>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#manufacturingDatePicker').datepicker({
                format: 'mm/dd/yyyy'
            });
            $('#expirationDatePicker').datepicker({
                format: 'mm/dd/yyyy'
            });
        });
        function permission(st) {
            if (st == "FA") {
                document.getElementById("btnadd").disabled = false;
            }
            else {
                document.getElementById("btnadd").disabled = true;
                $('#MainContentHolder_grvProductLabeling tbody tr').removeClass("GridRow");
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
        function addYear(theField, fName, daysToAdd) {
            var theForm = theField.form;
            var fDate = new Date(theField.value);
            fDate.setDate(fDate.getDate() + daysToAdd);
            var MM = fDate.getMonth() + 1;
            var DD = fDate.getDate();
            var YY = fDate.getFullYear();
            if (MM < 10) MM = "0" + MM;
            if (DD < 10) DD = "0" + DD;
            document.getElementById("<%= txtExpirationDate.ClientID %>").value = MM + "/" + DD + "/" + YY;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function ClearControls(e) {
            if (e == "A") {
                document.getElementById("<%= btnCancel.ClientID %>").style.display = "none";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "none";
                document.getElementById("<%= btnSave.ClientID %>").disabled = false;
            }
            $(".modal-title").text('');
            $('.modal-title').prepend("New Product label");
            var grvProductLabeling = document.getElementById('<%=grvProductLabeling.ClientID%>');
            if (isNaN(parseInt(document.getElementById('MainContentHolder_hdnRowIndex').value)) != true) {
                var ri = parseInt(document.getElementById('<%=hdnRowIndex.ClientID%>').value);
                var grvProduct = grvProductLabeling.rows[ri].classList;
                if (grvProduct.contains("GridSelectedRow")) {
                    grvProduct.remove("GridSelectedRow");
                }
            }
            document.getElementById("<%= txtskuno.ClientID %>").value = "";
            document.getElementById("<%= txtManufacturingDate.ClientID %>").value = "";
            document.getElementById("<%= txtExpirationDate.ClientID %>").value = "";
            document.getElementById("<%= txtlotno.ClientID %>").value = "";
            document.getElementById("<%= txtNoLables.ClientID %>").value = "";
            document.getElementById("<%= txtPONo.ClientID %>").value = "";
            document.getElementById("MainContentHolder_hdnPONo").value = "";
            
            //document.getElementById("<%= ddproducts.ClientID %>").value="0"
            $("#" + "<%= ddproducts.ClientID %>").val('0');
        }
        function checkLabelGenerate(controlid) {           
            var status = false;
            $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/CheckLabels',
                    dataType: 'json',
                    async:false,
                    contentType: 'application/json; charset=utf-8',
                    data: '{value: "' + controlid + '"}',
                    success: function (response) {                      
                        if (response.d > 0) {
                            status = true;
                        }
                        else {
                            status = false;
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            return status;
        }
        //$("#btnadd").click(function () {           
        //    //Save the vertical position of the button before content is added.
        //    var x1 = $(this).offset().top;
        //    //Do whatever the button is suppose to do, including adding the new content.
        //    doAllTheStuff();
        //    //See how much we moved.
        //    var x2 = $(this).offset().top;
        //    var dx = x2 - x1;
        //    //Scroll the same amount to keep the button from moving on the screen.
        //    $(document).scrollTop($(document).scrollTop() + dx);
        //});

        $(function () {
            $("#" + "<%= ddproducts.ClientID %>").change(function () {
                var productid = this.value;
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetSkuNumber',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{value: "' + productid + '"}',
                    success: function (response) {
                        document.getElementById("<%= txtskuno.ClientID %>").value = response.d;
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            });

            $("#" + "<%= ddFilter.ClientID %>").change(function () {
                if (document.getElementById('<%=hdnRowIndex.ClientID%>').value) {
                    var ri = parseInt(document.getElementById('<%=hdnRowIndex.ClientID%>').value);
                    var grvProductLabeling = document.getElementById('<%=grvProductLabeling.ClientID%>');
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

            $('#MainContentHolder_txtlotno').change(function () {
                document.getElementById("MainContentHolder_hdnPONo").value = "";
                var lotno = this.value;
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetPONumber',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{value: "' + lotno + '"}',
                    success: function (response) {
                        //alert("result "+ response.d);
                        document.getElementById("MainContentHolder_txtPONo").value = response.d;
                        document.getElementById("MainContentHolder_hdnPONo").value = response.d;
                        
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            });

            $('.GridRow').click(function () {                
                var rowIndexs = this.rowIndex;
                var ri = parseInt(rowIndexs);
                var grvProductLabeling = document.getElementById('<%=grvProductLabeling.ClientID%>');
                var controlid = grvProductLabeling.rows[ri].cells[6].innerHTML;

                if (!checkLabelGenerate(controlid)) {
                             $(".modal-title").text('');
                        $('.modal-title').prepend("Edit Product label");
                        $('.GridRow').removeClass('GridSelectedRow');
                        $(this).addClass('GridSelectedRow');
               
                        document.getElementById('<%=hdnRowIndex.ClientID%>').value = rowIndexs;
                        $("#" + "<%= ddproducts.ClientID %>").val(grvProductLabeling.rows[ri].cells[5].innerHTML);
                        $("#" + "<%= ddproducts.ClientID %>").trigger('change');                   
                        document.getElementById('<%=hdnControlID.ClientID%>').value = grvProductLabeling.rows[ri].cells[6].innerHTML;
                        document.getElementById("<%= txtlotno.ClientID %>").value = grvProductLabeling.rows[ri].cells[1].innerHTML;
                        //txtPONo
                    document.getElementById("<%= txtPONo.ClientID %>").value = grvProductLabeling.rows[ri].cells[7].innerHTML;
                    document.getElementById("<%= hdnPONo.ClientID %>").value = grvProductLabeling.rows[ri].cells[7].innerHTML;
                    
                        document.getElementById("<%= txtNoLables.ClientID %>").value = grvProductLabeling.rows[ri].cells[2].innerHTML;
                        document.getElementById("<%= txtManufacturingDate.ClientID %>").value = grvProductLabeling.rows[ri].cells[3].innerHTML;
                        document.getElementById("<%= txtExpirationDate.ClientID %>").value = grvProductLabeling.rows[ri].cells[4].innerHTML;
                        var prodid = grvProductLabeling.rows[ri].cells[5].innerHTML;
                        var ControlId = grvProductLabeling.rows[ri].cells[6].innerHTML;
                        document.getElementById("<%= btnCancel.ClientID %>").style.display = "block";
                        document.getElementById("<%= btnUpdate.ClientID %>").style.display = "block";
                        document.getElementById("<%= btnSave.ClientID %>").disabled = true;
                        $('#myModal').modal('show');
                }
                else {
                    alert("Labels have been generated. Edit is not allowed at this time.");
                }
               
            });
        });

    </script>
    <style>
        table#MainContentHolder_grvProductLabeling td {
            padding: 3px;
        }

        body, .navbar-fixed-top, .navbar-fixed-bottom {
            margin-right: 0 !important;
        }

        #form1 .dateContainer .form-control-feedback {
            top: 0;
            right: -15px;
        }

        .datepicker {
            z-index: 1151 !important;
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
               <h3>Product Master Labels </h3>
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
                                <asp:DropDownList ID="ddproducts" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;"></asp:DropDownList>                                                               
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvddproducts" runat="server" ForeColor="Red" ControlToValidate="ddproducts" ValidationGroup="validate" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>                                                         
                        </div>                        
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">SKU Number:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtskuno" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Manufacturing Date:</label>
                            <div class="col-lg-6 dateContainer">
                                <div class="input-group input-append date" id="manufacturingDatePicker">
                                    <asp:TextBox ID="txtManufacturingDate" runat="server" MaxLength="10" CssClass="form-control" onchange="addYear(this,'730Days',730)" PlaceHolder="MM/DD/YYYY"></asp:TextBox>
                                    <span class="input-group-addon add-on"><span class="glyphicon glyphicon-calendar"></span></span>                                    
                                </div>                                
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvManufacturingDate" runat="server" ForeColor="Red" ControlToValidate="txtManufacturingDate" ValidationGroup="validate" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            
                        </div>                       
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Expiration Date:</label>
                            <div class="col-lg-6 dateContainer">
                                <div class="input-group input-append date" id="expirationDatePicker">
                                    <asp:TextBox ID="txtExpirationDate" runat="server" MaxLength="10" CssClass="form-control" PlaceHolder="MM/DD/YYYY"></asp:TextBox>
                                    <span class="input-group-addon add-on"><span class="glyphicon glyphicon-calendar"></span></span>

                                </div>
                            </div>
                             <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" ControlToValidate="txtExpirationDate" ValidationGroup="validate" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>                       
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Lot Number:</label>                          
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtlotno" runat="server" CssClass="form-control" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvtxtlotno" runat="server" ControlToValidate="txtlotno" ForeColor="Red" ErrorMessage="*" ValidationGroup="validate"></asp:RequiredFieldValidator>
                            </div>
                        </div>                        
                        <br />
                        <div class="form-group">
                            <label class="col-lg-4 control-label">PO Number:</label>                       
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtPONo" runat="server" CssClass="form-control"  ReadOnly="true"></asp:TextBox>
                            </div> 
                            <div>
                                <asp:RequiredFieldValidator ID="rfvtxtPONo" runat="server" ControlToValidate="txtPONo" ForeColor="Red" ErrorMessage="Please Enter LotNo." ValidationGroup="validate"></asp:RequiredFieldValidator>
                            </div>                          
                        </div>                        
                       
                        <div class="form-group">
                            <label class="col-lg-4 control-label">Number Of Labels:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtNoLables" runat="server" CssClass="form-control " onkeypress="return isNumberKey(event);" MaxLength="4"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvtxtNoLables" runat="server" ControlToValidate="txtNoLables" ForeColor="Red" ErrorMessage="*" ValidationGroup="validate"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-2">
                                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="validate" OnClick="btnSave_Click" />
                            </div>

                            <div class="col-md-3">
                                <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" Style="display: none; margin-left: 50%;" OnClick="btnCancel_Click" />
                            </div>
                            <div class="col-md-3" style="margin-left: 10%;">
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" Style="display: none;" OnClick="btnUpdate_Click" />
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
            <div class="col-md-2 col-xs-2">
                <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" data-target="#myModal" onclick="ClearControls('A');">Add New Product label</button>
            </div>
            <div class="col-md-1 col-xs-1 text-right" style="margin-top: 1%; padding: 0; margin-left: 3%;">
                <label class="control-label">Filter By:</label>
            </div>
            <div class="col-md-3 col-xs-3">
                <asp:DropDownList ID="ddFilter" runat="server" CssClass="selectpicker" Style="width: 100%;">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="LotNo">Lot Number</asp:ListItem>
                    <asp:ListItem Value="LabelCount">Label Count</asp:ListItem>
                    <asp:ListItem Value="ProductDesc">Product Description</asp:ListItem>
                    <asp:ListItem Value="ManufacturingDate">Manufacturing Date</asp:ListItem>
                    <asp:ListItem Value="ExpirationDate">Expiration Date</asp:ListItem>
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
                <div id="GridScroll" class="grid_scroll">
                    <asp:GridView ID="grvProductLabeling" runat="server" CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" CssClass="table table-striped table-bordered table-condensed"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" OnRowDataBound="grvProductLabeling_RowDataBound"
                        OnSorting="grvProductLabeling_Sorting">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ProductDesc" HeaderText="Product Description" SortExpression="ProductDesc" HeaderStyle-CssClass="header-center">

                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="LotNo" HeaderText="Lot Number" SortExpression="LotNo" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                                 <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LabelCount" HeaderText="Label Count" SortExpression="LabelCount" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ManufacturingDate" DataFormatString="{0:d}" HeaderText="Manufacturing Date" SortExpression="ManufacturingDate" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExpirationDate" DataFormatString="{0:d}" HeaderText="Expiration Date" SortExpression="ExpirationDate" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductID" HeaderText="Product ID" SortExpression="ProductID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ControlID" HeaderText="Control ID" SortExpression="ControlID">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                              <asp:BoundField DataField="PONO"  SortExpression="PONO">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>

                            <%-- <asp:TemplateField HeaderText="Product Description" SortExpression="ProductDesc">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ProductDesc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
        <br />
        <asp:HiddenField ID="hdnRowIndex" runat="server" />
        <asp:HiddenField ID="hdnControlID" runat="server" />
          <asp:HiddenField ID="hdnPONo" runat="server" />
    </div>

</asp:Content>
