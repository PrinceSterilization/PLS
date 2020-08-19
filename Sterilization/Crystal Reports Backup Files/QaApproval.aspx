<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="QaApproval.aspx.cs" Inherits="Sterilization.QaApproval" %>

<asp:Content ID="QaApprovalContent" ContentPlaceHolderID="MainContentHolder" runat="server">

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
        function qaSign(e) {
            $("<%= ddqanames.ClientID %>").val("0");
            document.getElementById("<%= txtsignature.ClientID %>").value = "";
            document.getElementById("<%= rfvtxtname.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= rfvtxtsignature.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= hdnControlid.ClientID %>").value = e;
            return false;
        }

        function ReadInsertErrorMessage(msg) {
            if (document.getElementById("insertread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("insertread_msg").innerHTML = divHTML;
            }
        }
        function ReadInsertSuccessMessage(msg) {
            if (document.getElementById("insertread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("insertread_msg").innerHTML = divHTML;
            }
        }

        function ReadCaseErrorMessage(msg) {
            if (document.getElementById("caseread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("caseread_msg").innerHTML = divHTML;
            }
        }
        function ReadCaseSuccessMessage(msg) {
            if (document.getElementById("caseread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("caseread_msg").innerHTML = divHTML;
            }
        }
        function HeaderTextChange(msg) {
            $("#headersubtext").text(msg);

        }
        function printBatch(val) {

            document.getElementById("<%= hdnControlid.ClientID %>").value = val.split(",")[0];
            document.getElementById("<%= hdnlabelcount.ClientID %>").value = val.split(",")[1];

            var status = true;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/CheckPermission',
                dataType: 'json',
                async: false,
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + val.split(",")[0] + '"}',
                success: function (response) {
                    if (response.d == "1") {
                        status = true;
                        return true;
                    }
                    else {
                        status = false;
                        alert("Labels are not ready to E-Sign.");

                    }
                },
                error: function (error) {
                    return false;
                }
            });
            if (status == false) {
                return false;
            }
            else {
                return true;
            }
        }
        function insertBatch(val) {
            GetNumberOfProductsRemainingtoPrint(val.split(",")[0], 1);
            document.getElementById("<%= hdnControlid.ClientID %>").value = val.split(",")[0];
            document.getElementById("<%= hdnlabelcount.ClientID %>").value = val.split(",")[1];
            var status = true;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/ChecklabelPrinted',
                dataType: 'json',
                async: false,
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + val.split(",")[0] + '"}',
                success: function (response) {
                    if (response.d == "1") {
                        status = true;
                       
                    }
                    else {
                        status = false;
                        alert("Please Print or Read or Takeout the Previous Labels");
                    }
                },
                error: function (error) {
                    return false;
                }
            });
            if (status == false) {
                return false;
            }
            else {                
                $('#myInsertModal').modal('show');
                return false;
            }

        }
        function GetNumberOfProductsRemainingtoPrint(controlid, categorycode) {
            var data = controlid + ',' + categorycode;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetNumberOfProductsRemainingtoPrint',
                dataType: 'json',
               
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                   
                    if (response.d == "0") {
                        if(categorycode == 1){
                            document.getElementById("MainContentHolder_txtInsertProduct").value = 0;
                            document.getElementById("MainContentHolder_txtInsertProduct").diabled = true;
                        }
                        else{
                            document.getElementById("MainContentHolder_txtCaseproducts").value = 0;
                            document.getElementById("MainContentHolder_txtCaseproducts").diabled = true;
                        }                        
                    }
                    else {
                        if (categorycode == 1) {
                            document.getElementById("<%= hdntxtinsertproducts.ClientID %>").value = response.d;
                            document.getElementById("MainContentHolder_txtInsertProduct").value = response.d;
                            document.getElementById("MainContentHolder_txtInsertProduct").diabled = false;
                        }
                        else {
                            document.getElementById("<%= hdntxtCaseproducts.ClientID %>").value = response.d;
                            document.getElementById("MainContentHolder_txtCaseproducts").value = response.d;
                            document.getElementById("MainContentHolder_txtCaseproducts").diabled = false;
                        }
                    }
                },
                error: function (error) {
                    return false;
                }
            });

        }
        function checkInsertAvailability() {           
            var toPrint = document.getElementById("<%= hdntxtinsertproducts.ClientID %>").value;
            var entertoPrint = document.getElementById("<%= txtInsertProduct.ClientID %>").value;

            if ((parseInt(entertoPrint) <= parseInt(toPrint)) && parseInt(entertoPrint) > 0) {
                document.getElementById("insertread_msg").innerHTML = "";
                 document.getElementById("<%= btnInsertGenerate.ClientID %>").disabled = false;
                return true;
            }
            else {
                ReadInsertErrorMessage("You can only generate " + toPrint + " labels.");
                 document.getElementById("<%= btnInsertGenerate.ClientID %>").disabled = true;
                return false;
            }
            
        }
        function checkCaseAvailability() {           
            var toPrint = document.getElementById("<%= hdntxtCaseproducts.ClientID %>").value;
            var entertoPrint = document.getElementById("<%= txtCaseproducts.ClientID %>").value;

            if ((parseInt(entertoPrint) <= parseInt(toPrint)) && parseInt(entertoPrint) > 0) {
                document.getElementById("caseread_msg").innerHTML = "";
                 document.getElementById("<%= btnCaseGenerate.ClientID %>").disabled = false;
                return true;
            }
            else {
                ReadCaseErrorMessage("You can only generate " + toPrint + " labels.");
                 document.getElementById("<%= btnCaseGenerate.ClientID %>").disabled = true;
                return false;
            }
            
        }
        function CaseLabel(val) {
            document.getElementById("<%= hdnControlid.ClientID %>").value = val.split(",")[0];
            document.getElementById("<%= hdnlabelcount.ClientID %>").value = val.split(",")[1];
        }
        function OpenCaseModal(controlid, lblcount, categorycode) {

            var controlid = controlid;
            var lblcount = lblcount;
            var categorycode = categorycode;
            var data = controlid + "," + categorycode;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetCaseLabelData',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                    if (response.d == "0") {
                        //Dont Show the popup   
                        OpenWindow(controlid, lblcount, 3);
                        return true;
                    }
                    else {
                        //Show the PopUp
                        $('#myCaseModal').modal('show');
                        return false;
                    }
                },
                error: function (error) {
                    return false;
                }
            });
        }
        function AddCaselabel() {
           <%-- if (document.getElementById("<%= txtcase.ClientID %>").value != "") {
                return true;
            }
            else {
                return false;
            }--%>
        }
        function OpenWindow(cid, lbc, cat) {
            window.open("Printing.aspx?cid=" + cid + "&lbc=" + lbc + "&cat=" + cat);
        }
        function checkLabelCount() {
            var lblcount = document.getElementById("<%= lblleft.ClientID %>").innerText;
            var lbltoprint = document.getElementById("<%= txtPrintNumber.ClientID %>").value;
            if (parseInt(lbltoprint) > parseInt(lblcount)) {
                document.getElementById("<%= divPrintError.ClientID %>").style.display = "block";
                document.getElementById("<%= lblcount.ClientID %>").innerText = "";
                document.getElementById("<%= lblcount.ClientID %>").innerText = lblcount;
                return false;
            }
            return true;
        }

        function QaPreview(e) {

            document.getElementById("<%= hdnControlid.ClientID %>").value = e;
            // return false;
        }
        function reconcile() {
            document.getElementById("<%= btnRead.ClientID %>").style.display = "block";
        }
        $(document).ready(function () {
            $(document).ready(function () {
                $(".selectpicker").selectpicker();
            });
        });

        $(function () {
            $('#aclose').click(function () {
                document.getElementById("<%= divPrintError.ClientID %>").style.display = "none";
            });

            $(".modal").on('shown.bs.modal', function () {
                $("[data-modalfocus]", this).focus();
            });

           <%-- $("#" + "<%= txtcase.ClientID %>").keyup(function () {
                var val = this.value;
                if (val == 6 || val == 12) {
                    document.getElementById("<%= btnCase.ClientID %>").disabled = false;
                    document.getElementById("<%= btnCase.ClientID %>").className = "btn btn-primary";
                }
                else {
                    document.getElementById("<%= btnCase.ClientID %>").disabled = true;
                    document.getElementById("<%= btnCase.ClientID %>").className = "btn btn-primary disabled";
                }
            });--%>
        });

    </script>
    <style>
        table#MainContentHolder_grvQaApproval td {
            padding: 5px;
        }

        .modal-open {
            padding-right: 0 !important;
        }

        html {
            overflow-y: scroll !important;
        }

        .thumbnail {
            border: 1px solid #8a8181 !important;
        }

        .label1 {
            color: #d9534f;
            font-weight: bold;
        }

        .label2 {
            color: #337ab7;
            font-weight: bold;
        }
         .label3 {
            color: #c91414;
            font-weight: bold;
        }
        
    </style>
    <div class="content ">
        <%--QA E-Sign  Popup--%>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">QA Approval</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-3 control-label">Name</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddqanames" runat="server" AutoPostBack="false" CssClass="selectpicker"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvtxtname" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="red" ValidationGroup="validate" ControlToValidate="ddqanames"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-3 control-label">E-Signature</label>
                            <div class="col-lg-5">
                                <asp:TextBox ID="txtsignature" runat="server" name="txtsignature" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:RequiredFieldValidator ID="rfvtxtsignature" runat="server" ErrorMessage="*" ForeColor="red" ValidationGroup="validate" ControlToValidate="txtsignature"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                    <br />
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btnclose" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnesign" class="btn btn-primary" runat="server" Text="E-Sign" ValidationGroup="validate" OnClick="btnesign_Click" />
                    </div>
                </div>

            </div>
        </div>
        <%--Print PopUP--%>
        <div class="modal fade" id="myPrintModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Print Batches</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-3 control-label" style="margin-top: 1%;">No. Of Labels</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtPrintNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPrintNumber" runat="server" ControlToValidate="txtPrintNumber"
                                    ForeColor="Red" ErrorMessage="*" ValidationGroup="validatePrint"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group text-center">
                            <label class="control-label">Your are left with: </label>
                            <asp:Label ID="lblleft" runat="server" ForeColor="red" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="alert alert-danger" id="divPrintError" runat="server" style="display: none">
                            <a href="#" class="close" id="aclose" aria-label="close">&times;</a>
                            <label class="control-label">You can only Print: </label>
                            <asp:Label ID="lblcount" runat="server" ForeColor="red" Font-Bold="true"></asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btnclose2" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnPrintBatch" class="btn btn-primary" runat="server" Text="Print" OnClientClick="return checkLabelCount();" ValidationGroup="validatePrint" OnClick="btnPrintBatch_Click" />
                    </div>
                </div>
            </div>
        </div>

        <%--Reading the Labels--%>

        <div class="modal fade" id="myReadModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="">&times;</button>
                        <h4 class="modal-title">Reading Label</h4>
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtRead" runat="server" TextMode="MultiLine" Height="210px" Width="347px" data-modalfocus></asp:TextBox>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvtxtRead" runat="server" ControlToValidate="txtRead" ForeColor="Red" ErrorMessage="Need to Read the Label!" ValidationGroup="validateread"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-4" style="margin-left: 10%;">
                                <div class="thumbnail">
                                    <div class="caption">
                                        <h4 class="text-center">Total Labels</h4>
                                        <h1 id="h1total" style="text-align: center; color: red; font-size: 6.2em; margin-bottom: 38px;">9</h1>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btnreadclose" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnRead" class="btn btn-primary" runat="server" Text="Reconciled" ValidationGroup="validateread" OnClick="btnRead_Click" />
                    </div>
                </div>
            </div>
        </div>

        <%--Case Model For how many cases it should divide--%>

        <div class="modal fade" id="myCaseModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Case To Print</h4>
                    </div>
                     <div id="caseread_msg">
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-3 control-label">No of Products:</label>
                            <div class="col-lg-5">
                                <asp:TextBox ID="txtCaseproducts" runat="server" name="txtcase" CssClass="form-control" onblur="return checkCaseAvailability();"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:RequiredFieldValidator ID="rfv" runat="server" ErrorMessage="*" ForeColor="red" ValidationGroup="casevalidate" ControlToValidate="txtCaseproducts"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group">
                            <label class="col-lg-3 control-label">PO No:</label>
                            <div class="col-lg-5">
                                <asp:TextBox ID="txtPONo" runat="server" name="txtcase" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:RequiredFieldValidator ID="rfvtxtPONo" runat="server" ErrorMessage="*" ForeColor="red" ValidationGroup="casevalidate" ControlToValidate="txtPONo"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btncaseclose" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnCaseGenerate" class="btn btn-primary disabled" runat="server" Text="Generate Case" ValidationGroup="casevalidate"
                            OnClientClick="return AddCaselabel();"  />
                    </div>
                </div>
            </div>
        </div>


         <%--INSERT how may to print--%>

        <div class="modal fade" id="myInsertModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Insert To Print</h4>
                    </div>
                      <div id="insertread_msg">
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-3 control-label">No of Products:</label>
                            <div class="col-lg-5">
                                <asp:TextBox ID="txtInsertProduct" runat="server" name="txtcase" CssClass="form-control" onblur="return checkInsertAvailability();"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:RequiredFieldValidator ID="rfvtxtInsertProduct" runat="server" ErrorMessage="*" ForeColor="red" ValidationGroup="casevalidate" ControlToValidate="txtInsertProduct"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />                        
                    </div>
                    <br />
                    <div class="modal-footer">
                       
                        <asp:Button ID="btnInsertGenerate" class="btn btn-primary" runat="server" Text="Generate Insert" ValidationGroup="casevalidate"
                           OnClick="btnInsertGenerate_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3 id="headersubtext"></h3>
            </div>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <br />
        <div class="row">
            <div class="col-md-1 col-xs-1 text-right " style="margin-top: 1%; padding: 0;">
                <label class="control-label">Filter By:</label>
            </div>
            <div class="col-md-3 col-xs-3">
                <asp:DropDownList ID="ddFilter" runat="server" CssClass="selectpicker" Style="width: 100%;">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="ProductDesc">Product Description</asp:ListItem>
                    <asp:ListItem Value="LotNo">Lot Number</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3 col-xs-3">
                <asp:TextBox ID="txtfilter" CssClass="form-control btn-md" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1 col-xs-1">
                <asp:Button ID="btnFilter" CssClass="btn btn-primary btn-md" runat="server" Text="Filter" OnClick="btnFilter_Click" />
            </div>
            <div class="col-md-2 col-xs-1">
                <asp:Button ID="btnRefresh" CssClass="btn btn-primary btn-md" runat="server" Text="Refresh" OnClick="btnRefresh_Click" OnClientClick="javascript:document.location.reload(true);" />
            </div>
        </div>

        <br />
        <div id="QAGridScroll" class="grid_scroll">
            <asp:GridView ID="grvQaApproval" runat="server" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                CssClass="table table-striped table-bordered table-condensed"
                OnSorting="grvQaApproval_Sorting"
                CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                BorderWidth="1px" Width="900px" OnRowCommand="grvQaApproval_RowCommand" OnRowDataBound="grvQaApproval_RowDataBound" OnRowCreated="grvQaApproval_RowCreated">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ControlID" HeaderText="ControlID" SortExpression="ControlID" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center "></HeaderStyle>
                    </asp:BoundField>

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


                    <%--PRODUCTS--%>
                    <asp:BoundField DataField="PRODUCT_PRINTED" HeaderText="PRODUCT_PRINTED" SortExpression="PRODUCT_PRINTED" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PRODUCT_TO_PRINT" HeaderText="PRODUCT_TO_PRINT" SortExpression="PRODUCT_TO_PRINT" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PRODUCT_TOTAL" HeaderText="PRODUCT_TOTAL" SortExpression="PRODUCT_TOTAL" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <%--COMPONENETS--%>
                    <asp:BoundField DataField="COMPONET_PRINTED" HeaderText="COMPONET_PRINTED" SortExpression="COMPONET_PRINTED" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="COMPONET_TO_PRINT" HeaderText="COMPONET_TO_PRINT" SortExpression="COMPONET_TO_PRINT" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="COMPONET_TOTAL" HeaderText="COMPONET_TOTAL" SortExpression="COMPONET_TOTAL" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <%--INSERTBOX--%>
                    <asp:BoundField DataField="INSERT_PRINTED" HeaderText="INSERT_PRINTED" SortExpression="INSERT_PRINTED" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="INSERT_TO_PRINT" HeaderText="INSERT_TO_PRINT" SortExpression="INSERT_TO_PRINT" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="INSERT_TOTAL" HeaderText="INSERT_TOTAL" SortExpression="INSERT_TOTAL" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <%--CASEBOX--%>
                    <asp:BoundField DataField="CASE_PRINTED" HeaderText="CASE_PRINTED" SortExpression="CASE_PRINTED" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CASE_TO_PRINT" HeaderText="CASE_TO_PRINT" SortExpression="CASE_TO_PRINT" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CASE_TOTAL" HeaderText="CASE_TOTAL" SortExpression="CASE_TOTAL" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="product_type" HeaderText="product_type" SortExpression="product_type" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CASE_TOTAL_CAL" HeaderText="product_type" SortExpression="CASE_TOTAL_CAL" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UNITSIZE" HeaderText="product_type" SortExpression="UNITSIZE" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Components" HeaderStyle-CssClass="header-center">
                        <ItemTemplate>
                            <asp:Button ID="btnComponents" runat="server" Text="Components" CommandArgument='<%# Eval("ControlID") %>' CommandName="Component"
                                class="btn btn-success"
                                OnClientClick='<%# String.Format("javascript:return printBatch(\"{0}\");", Eval("ControlID").ToString()+","+  Eval("LabelCount").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Products" HeaderStyle-CssClass="header-center">
                        <ItemTemplate>
                            <asp:Button ID="btnGenerate" runat="server" Text="Products" CommandArgument='<%# Eval("ControlID") %>' CommandName="Print"
                                class="btn btn-success"
                                OnClientClick='<%# String.Format("javascript:return printBatch(\"{0}\");", Eval("ControlID").ToString()+","+  Eval("LabelCount").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Insert Box" HeaderStyle-CssClass="header-center">
                        <ItemTemplate>
                            <asp:Button ID="btnInserBox" runat="server" Text="Insert" CommandArgument='<%# Eval("ControlID") %>' CommandName="Insert"
                                class="btn btn-success"
                                OnClientClick='<%# String.Format("javascript:return insertBatch(\"{0}\");", Eval("ControlID").ToString()+","+  Eval("LabelCount").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Case" HeaderStyle-CssClass="header-center">
                        <ItemTemplate>
                            <asp:Button ID="btnCaseBox" runat="server" Text="Case" CommandArgument='<%# Eval("ControlID") %>' CommandName="Case"
                                class="btn btn-success"
                                OnClientClick='<%# String.Format("javascript:return caseBatch(\"{0}\");", Eval("ControlID").ToString()+","+  Eval("LabelCount").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
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
        <br />
        <asp:HiddenField ID="hdnControlid" runat="server" />
        <asp:HiddenField ID="hdnlabelcount" runat="server" />
        <asp:HiddenField ID="hdntxtCaseproducts" runat="server" />
        <asp:HiddenField ID="hdntxtinsertproducts" runat="server" />


    </div>
</asp:Content>
