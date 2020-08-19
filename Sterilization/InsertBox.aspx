<%@ Page Title="Insert Labels" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InsertBox.aspx.cs" EnableEventValidation="false" Inherits="Sterilization.InsertBox" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebServices/WebServices.asmx" />

        </Services>
    </asp:ScriptManager>
    <script type="text/javascript">


        function permission(st) {
            if (st == "FA") {
                //document.getElementById("btnadd").disabled = false;
            }
            else {
                //document.getElementById("btnadd").disabled = true;
                //$('#MainContentHolder_grvProductLabeling tbody tr').removeClass("GridRow");
            }
        }
        function ReadQAErrorMessage(msg) {
            if (document.getElementById("QAread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("QAread_msg").innerHTML = divHTML;
            }
        }
        function ReadQASuccessMessage(msg) {
            if (document.getElementById("QAread_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("QAread_msg").innerHTML = divHTML;
            }
        }
        function ErrorMessage(msg) {
            clearhiddendata();
            document.getElementById("divmsg").innerHTML = "";
            if (document.getElementById("divmsg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("divmsg").innerHTML = divHTML;
            }

        }

        function SuccessMessage(msg) {
            clearhiddendata();
            document.getElementById("divmsg").innerHTML = "";
            if (document.getElementById("divmsg")) {

                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("divmsg").innerHTML = divHTML;
            }
        }
        function PrintErrorMessage(msg) {
            if (document.getElementById("print_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("print_msg").innerHTML = divHTML;
            }
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
            document.getElementById("MainContentHolder_txtExpirationDate").value = MM + "/" + DD + "/" + YY;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        function CheckOne(obj) {
            var grid = obj.parentNode.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }
        function clearhiddendata() {
            document.getElementById("MainContentHolder_hdnCheckedControlid").value = "";
            document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value = "";
            document.getElementById("MainContentHolder_hdnCheckedBatchID").value = "";
            document.getElementById("MainContentHolder_hdnCheckedLabelCount").value = "";
            document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value = "";
            document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value = "";
            document.getElementById("MainContentHolder_hdnCheckedTotalLabelsToAdd").value = "";
            document.getElementById("MainContentHolder_hdnCheckedTotalLabelsVoided").value = "";
        }
        function checkCheckboxChecked() {
            var grid = document.getElementById("MainContentHolder_grvInsertLabeling");
            var inputs = grid.querySelectorAll("input[type=checkbox]:checked");
            if (inputs.length > 0) {
                clearhiddendata();
                var val = inputs[0].parentElement.nextElementSibling.value;
                document.getElementById("MainContentHolder_hdnCheckedControlid").value = val.split('-')[0]; //Controlid
                document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value = val.split('-')[1]; //Categorycode  
                document.getElementById("MainContentHolder_hdnCheckedBatchID").value = val.split('-')[2]; //BatchID  
                document.getElementById("MainContentHolder_hdnCheckedLabelCount").value = val.split('-')[3]; //Total Label count                                  
                document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value = val.split('-')[4]; //Total Labels to print
                document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value = val.split('-')[5]; //Total Labels to READ
                document.getElementById("MainContentHolder_hdnCheckedTotalLabelsToAdd").value = val.split('-')[6]; // Total No. of Labels to Add after Voiding.
                document.getElementById("MainContentHolder_hdnCheckedTotalLabelsVoided").value = val.split('-')[7]; // Total No. of Labels to Add after Voiding.
                return true;
            }
            else {
                clearhiddendata();
                ErrorMessage("Please select the InsertBox Label.");
                return false;
            }
        }

        //function generateLabels() {
        //    if (checkCheckboxChecked()) {
        //        var generatedLabels = getLabelsForInsertBox();
        //        if (generatedLabels == "0") {                
        //            var labelcount = document.getElementById("MainContentHolder_hdnCheckedLabelCount").value;                   
        //                if (confirm("Are you sure you want to generate insert box labels for " + labelcount + " Products/Components!")) {
        //                    return true;
        //                }
        //                else {
        //                    return false;
        //                }

        //        } else {
        //            ErrorMessage("Labels are already generated for this batch.");
        //            return false;
        //        } 
        //    }
        //    else {
        //        return false;
        //    }
        //}


        //function CheckReason(e) {
        //    if (e == "A") {
        //        document.getElementById("divReasons").style.display = "none";

        //    } else {
        //        document.getElementById("divReasons").style.display = "block";
        //    }
        //}
        function labelFormats() {

            var controlid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
            var categorycode = document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value;
            var batchid = document.getElementById("MainContentHolder_hdnCheckedBatchID").value;
            var val = controlid + "-" + categorycode + "-" + batchid;
            $.ajax({
                type: 'POST',
                //url: 'Webservices/WebServices.asmx/GetLabelFormatsByProduct',
                url: 'Webservices/WebServices.asmx/GetLabelFormat',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + val + '"}',
                success: function (response) {
                    var d = JSON.parse(response.d);
                    document.getElementById("divformats").innerHTML = "";
                    var html = '<select id="ddlLabelFormats" class="btn btn-default btn-md" style="width:100%;" disabled="true">'
                    for (i = 0; i < d.length; i++) {
                        html += "<option value='" + d[i].FileName + "'>" + d[i].FormatNo + "</option>";
                    }
                    html += '</select>';
                    document.getElementById("divformats").innerHTML = html;


                    //$('#myPrintModel').modal('show');


                },
                error: function (error) {
                    console.log(error);
                }
            });

        }
        function printValidate() {

            var toprint = document.getElementById("MainContentHolder_txtTotalLabelsremaningtoprint").value;
            document.getElementById("MainContentHolder_hdnLabelFormatName").value = document.getElementById("ddlLabelFormats").value;
            //var ddlFormat = document.getElementById("ddlLabelFormats");
            //var text = ddlFormat.options[ddlFormat.selectedIndex].text;

            if (toprint > 0 && document.getElementById("MainContentHolder_hdnLabelFormatName").value != "") {
                return true;
            } else {
                PrintErrorMessage("All fields are mandatory.");
                return false;
            }
        }

        function checkAvailability() {
            var toPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
            var entertoPrint = document.getElementById("MainContentHolder_txtTotalLabelsremaningtoprint").value;

            if ((parseInt(entertoPrint) <= parseInt(toPrint)) && parseInt(entertoPrint) > 0) {
                document.getElementById("print_msg").innerHTML = "";
                document.getElementById("MainContentHolder_btnPrintLabels").disabled = false;
                return true;
            }
            else {
                PrintErrorMessage("You can only print " + toPrint + " labels.");
                document.getElementById("MainContentHolder_btnPrintLabels").disabled = true;
                return false;
            }

        }
        function takeout() {
            if (checkCheckboxChecked()) {
                var nooflabeltoread = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value;
                if (nooflabeltoread == "0") {
                    return true;
                }
                else {
                    ErrorMessage("Please read all the labels before takeout.");
                    return false;
                }
            }
            else {
                return false;
            }

        }
        function reject() {
            if (checkCheckboxChecked()) {
                var nooflabelsToPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                if (nooflabelsToPrint == 0) {
                    ErrorMessage("You cannot reject the labels which are already printed.QA can void the Labels.");
                    return false;
                }
                else {
                    $('#myRejectModel').modal('show');
                    return false;
                }

            } else {
                return false;
            }
        }
        function avoidInserts() {
            if (checkCheckboxChecked()) {
                var nooflabelsToPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;

                if (nooflabelsToPrint == 0) {
                    ErrorMessage("Labels are already printed.");
                    return false;
                }
                else {
                    return true;
                }
                //$('#myAvoidInsertModal').modal('show');
                //return false;
            }
            else {
                //$('#myAvoidInsertModal').modal('show');
                return false;
            }
        }
        function read() {
            if (checkCheckboxChecked()) {
                var nooflabelsToPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                if (nooflabelsToPrint > 0) {
                    ErrorMessage("Please print all the labels to read.");
                    return false;
                }
                else {
                    var nooflabeltoread = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value;
                    if (nooflabeltoread == "0") {
                        ErrorMessage("Labels are already read or need to print before reading.");
                        return false;
                    }
                    else {
                        //var controlid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
                        //var categorycode = document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value ;
                        // window.open("ReadLabels.aspx?ctrid" + controlid + "&pwd=" + categorycode, "Reading Labels");
                        return true;
                    }
                }
            }
            else {
                return false;
            }
        }


        function print() {

            if (checkCheckboxChecked()) {
                var noofLabeltoPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                if (noofLabeltoPrint == "0") {
                    ErrorMessage("No more labels to print.")
                    return false;
                } else {
                    labelFormats();
                    var totallabelstoprint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                    document.getElementById("MainContentHolder_txtTotalLabelsremaningtoprint").value = totallabelstoprint;
                    if (totallabelstoprint > 0) {
                        document.getElementById("MainContentHolder_btnPrintLabels").disabled = false;
                    }
                    else {
                        document.getElementById("MainContentHolder_btnPrintLabels").disabled = true;
                    }
                    $('#myPrintModel').modal('show');
                    return false;
                }
            }
            else {
                return false;
            }

        }
        function voidlabels() {
            if (checkCheckboxChecked()) {

                var labelcount = document.getElementById("MainContentHolder_hdnCheckedLabelCount").value;
                var nooflabelremaningtoprint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value

                if (labelcount == nooflabelremaningtoprint) {
                    ErrorMessage("Please print the labels before voiding.");
                    return false;
                }
                else {
                    if (confirm("Do you want void all the labels?")) {
                        $('#myVoidModel').modal('show');
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            else {
                return false;
            }
        }
        function clearprintscreen() {
            document.getElementById("MainContentHolder_txtTotalLabelsremaningtoprint").value = "";
            document.getElementById("print_msg").innerHTML = "";
            document.getElementById("divformats").innerHTML = "";

        }
        function GetLabel() {
            if (checkCheckboxChecked()) {
                document.getElementById("MainContentHolder_txtlabels").value = "";
                var cid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
                var catid = document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value;
                var lblc = document.getElementById("MainContentHolder_hdnCheckedTotalLabelsToAdd").value;
                var batchid = document.getElementById("MainContentHolder_hdnCheckedBatchID").value;
                var voidedlabels = document.getElementById("MainContentHolder_hdnCheckedTotalLabelsVoided").value;
                var data = cid + ',' + lblc + ',' + catid + ',' + batchid + ',' + voidedlabels;
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetLabelCountToAdd',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{data: "' + data + '"}',
                    success: function (response) {
                        document.getElementById("MainContentHolder_txtlabels").value = response.d;
                        document.getElementById("MainContentHolder_hdntxtlabelsToAdd").value = response.d;
                        if (response.d == "0" || response.d == " ") {
                            document.getElementById("MainContentHolder_btnAddLabels").disabled = true;
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
                $('#myAddNewLabelModal').modal('show');
                return false;
            }
            else {
                return false;
            }
        }

        function generateValidate() {
            if (document.getElementById("MainContentHolder_ddlInsertLables").value != "" || document.getElementById("MainContentHolder_ddlInsertLables").value != null) {
                if (confirm("Are you sure you want to generate insert labels?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                alert("Please enter how many products/components you want in InsertBox!");
                return false;
            }

        }
        $(function () {

            $("#MainContentHolder_ddlInsertLables").change(function () {
                document.getElementById("MainContentHolder_hdntxtTotalCount").value = this.value.split('-')[0];
                document.getElementById("MainContentHolder_txtTotalCount").value = this.value.split('-')[0];
                document.getElementById("MainContentHolder_txtTotalCount").disabled = true;
            });

        });
        function generateInsertLabels() {
            var ddlinsertlist = document.getElementById("MainContentHolder_ddlInsertLables");
            var length = ddlinsertlist.options.length;
            if (length > 1) {
                $('#myInserGenerationModel').modal('show');
                return false;
            }
            else {
                ErrorMessage("No more labels to generate.");
                return false;
            }

        }
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
        <%-- <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Insert Labels </h3>
            </div>
        </div>--%>

        <br />

        <div id="divmsg"></div>
        <br />
        <%--Rejecting Reasons--%>
        <div class="modal fade" id="myRejectModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="clearrejectscreen()">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Reject</h3>
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Reject Reason:</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:DropDownList ID="ddlRejectedReasons" CssClass="btn btn-default btn-md" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlRejectedReasons" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="red" ValidationGroup="rejectvalidate" ControlToValidate="ddlRejectedReasons"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnRejected" CssClass="btn btn-primary btn-md" runat="server" Text="Reject" ValidationGroup="rejectvalidate" OnClick="btnRejected_Click" />
                    </div>
                </div>
            </div>
        </div>

        <%--Avoiding Insert Labels--%>
        <%-- <div class="modal fade" id="myAvoidInsertModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: 500">Avoid Insert Labels</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-5 control-label">How many products/components in CaseBox?</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtAvoidSize" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>                       
                    </div>
                    <br />
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-3">
                                <asp:Button ID="btnAvoidInsert" CssClass="btn btn-primary" runat="server" Text="Avoid"   OnClick="btnAvoidInserts_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        <%--Adding Extra Labels--%>
        <div class="modal fade" id="myAddNewLabelModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: 500">Add New Label</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-4 control-label">No of labels:</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtlabels" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <h5 id="addedmsg" style="color: red; text-align: center;"></h5>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-3">
                                <asp:Button ID="btnAddLabels" CssClass="btn btn-primary" runat="server" Text="Add" OnClick="btnAddLabels_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--Voiding Or Cancelling--%>
        <div class="modal fade" id="myVoidModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="clearprintscreen()">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Void Label</h3>
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Void Reason:</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:DropDownList ID="ddlvoidreasons" CssClass="btn btn-default btn-md" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlvoidreasons" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="red" ValidationGroup="voidvalidate" ControlToValidate="ddlvoidreasons"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnVoided" CssClass="btn btn-primary btn-md" runat="server" Text="Void" ValidationGroup="voidvalidate" OnClick="btnVoided_Click" />
                    </div>
                </div>
            </div>
        </div>
        <%--Printing the Label Required--%>
        <div class="modal fade" id="myPrintModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="clearprintscreen()">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Print Label</h3>
                    </div>
                    <div>
                        <h4 id="halertmsg" style="text-align: center; color: red;"></h4>
                    </div>
                    <div id="print_msg">
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">No. of Labels:</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:TextBox ID="txtTotalLabelsremaningtoprint" CssClass="form-control" runat="server" onblur="return checkAvailability();"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Format:</label>
                            </div>
                            <div id="divformats" class="col-lg-6" style="float: none; margin: 0 auto;">
                            </div>
                        </div>
                        <br />
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnPrintLabels" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnPrintLabels_Click" OnClientClick="return printValidate();" />
                        <asp:Button ID="btnTestPrint" CssClass="btn btn-primary btn-md" runat="server" Text="Test Print" OnClick="btnTestPrintLabels_Click" OnClientClick="return printValidate();" />
                        <p style="color: red">(Test print can print only one label !)</p>
                    </div>
                </div>
            </div>
        </div>

        <%--Generating Labels--%>
        <div class="modal fade" id="myInserGenerationModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="clearmergecreen()">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Add Insert Label</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Labels:</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:DropDownList ID="ddlInsertLables" CssClass="btn btn-default btn-md" runat="server" Width="100%"></asp:DropDownList>

                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4">
                                <label class="control-label">Components/Products:</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">

                                <asp:TextBox ID="txtTotalCount" runat="server" CssClass="btn btn-default btn-md"></asp:TextBox>

                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4">
                                <label class="control-label">How many products/components in InsertBox?</label>
                            </div>
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:TextBox ID="txtInsertBoxSize" runat="server" CssClass="btn btn-default btn-md"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGenerate" CssClass="btn btn-primary btn-md" runat="server" Text="Generate Labels" OnClientClick="return generateValidate();" OnClick="btnGenerate_Click" />
                    </div>
                </div>
            </div>
        </div>
        <%--Details--%>
        <div class="row" style="margin-left: 3px;">
            <asp:Button ID="btnPreview" CssClass="btn btn-primary btn-md" runat="server" Text="Preview" OnClientClick="return checkCheckboxChecked();" OnClick="btnPreview_Click" />

            <asp:Button ID="btnAddInsertLabels" CssClass="btn btn-primary btn-md" runat="server" Text="Add Insert Labels" OnClientClick="return generateInsertLabels();" />
            <%-- <asp:Button ID="btnGenerateLabels" runat="server" CssClass="btn btn-primary btn-md" Text="Generate Labels" OnClientClick="return generateLabels();"  />--%>

            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-md" Text="Print" OnClientClick="return print();" />
            <asp:Button ID="btnRead" runat="server" CssClass="btn btn-primary btn-md" Text="Read" OnClientClick="return read();" OnClick="btnRead_Click" />
            <asp:Button ID="btnReject" runat="server" CssClass="btn btn-primary btn-md" Text="Reject" OnClientClick="return reject();" />
            <asp:Button ID="btnAvoidInserts" runat="server" CssClass="btn btn-primary btn-md" Text="Avoid Inserts" OnClientClick="return avoidInserts();" OnClick="btnAvoidInserts_Click" />
            <asp:Button ID="btnVoid" CssClass="btn btn-primary btn-md" runat="server" Text="Void & Add" OnClientClick="return voidlabels();" OnClick="btnvoid_click" />
            <%--<asp:Button ID="btnAddNewLable" CssClass="btn btn-primary btn-md" runat="server" Text="Add Voided Labels" OnClientClick="return GetLabel();" />--%>
        </div>
        <br />
        <%--Filter Bar--%>
        <div class="row" style="margin-left: -5%;">

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
                    <asp:GridView ID="grvInsertLabeling" runat="server" CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" CssClass="table table-striped table-bordered table-condensed"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" OnRowDataBound="grvInsertLabeling_RowDataBound"
                        OnSorting="grvInsertLabeling_Sorting">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelect" runat="server" Style="font-size: 22px; margin: 20px; height: 20px; width: 20px;" onclick="CheckOne(this)" />
                                    <asp:HiddenField runat="server" ID="hdngrdcontrolid" Value='<%# Eval("CATID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ControlID" HeaderText="Control ID" SortExpression="ControlID" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BatchID" HeaderText="BatchID" SortExpression="BatchID" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductDesc" HeaderText="Product Description" SortExpression="ProductDesc" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="LOTNO" HeaderText="Lot Number" SortExpression="LOTNO" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TOTALLABELCOUNT" HeaderText="Actual Label Count" SortExpression="TOTALLABELCOUNT" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VOIDEDLABELCOUNT" HeaderText="Voided Label Count" SortExpression="VOIDEDLABELCOUNT" HeaderStyle-CssClass="header-center">
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
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <asp:Button ID="btnStatus" runat="server" Text="Pending" CommandArgument='<%# Eval("ControlID") %>' CommandName="Pending"
                                        class="btn btn-danger" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProductID" HeaderText="Product ID" SortExpression="ProductID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CATID" HeaderText="" SortExpression="CATID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
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
        <asp:HiddenField ID="hdnPONo" runat="server" />
        <asp:HiddenField ID="hdnControlID" runat="server" />
        <asp:HiddenField ID="hdnCheckedControlid" runat="server" />
        <asp:HiddenField ID="hdnqanames" runat="server" />
        <asp:HiddenField ID="hdnRejectedReasons" runat="server" />
        <asp:HiddenField ID="hdnCheckedCategoryCode" runat="server" />
        <asp:HiddenField ID="hdnCheckedBatchID" runat="server" />
        <asp:HiddenField ID="hdnCheckedDateApproved" runat="server" />
        <asp:HiddenField ID="hdnCheckedLabelCount" runat="server" />
        <asp:HiddenField ID="hdnCheckedGeneratedLabels" runat="server" />
        <asp:HiddenField ID="hdnCheckedNoOfLabelsToPrint" runat="server" />
        <asp:HiddenField ID="hdnLabelFormatName" runat="server" />
        <asp:HiddenField ID="hdnCheckedNoOfLabelsToRead" runat="server" />
        <asp:HiddenField ID="hdnCheckedTotalLabelsToAdd" runat="server" />
        <asp:HiddenField ID="hdnmergeData" runat="server" />
        <asp:HiddenField ID="hdntxtTotalCount" runat="server" />
        <asp:HiddenField ID="hdntxtlabelsToAdd" runat="server" />
        <asp:HiddenField ID="hdnCheckedTotalLabelsVoided" runat="server" />
    </div>
</asp:Content>
