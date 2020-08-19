<%@ Page Title="Master Labels" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MasterLabels.aspx.cs" Inherits="Sterilization.MasterLabels" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebServices/WebServices.asmx" />

        </Services>
    </asp:ScriptManager>
    <script type="text/javascript">

        $(document).ready(function () {
            //$('#MainContentHolder_txtfilter').keypress(function (e) {
            //    if (e.keyCode == 13)
            //        $('#MainContentHolder_btnFilter').click();
            //});
            $('#manufacturingDatePicker').datepicker({
                format: 'mm/dd/yyyy'
            });
            $('#expirationDatePicker').datepicker({
                format: 'mm/dd/yyyy'
            });
        });
        function log(e) {
            console.log(e);
        }
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
        function ClearControls(e) {
            if (e == "A") {
                document.getElementById("<%= btnCancel.ClientID %>").style.display = "none";
                document.getElementById("<%= btnUpdate.ClientID %>").style.display = "none";
                document.getElementById("<%= btnSave.ClientID %>").disabled = false;
            }
            $(".modal-title").text('');
            $('.modal-title').prepend("New Product label");
            document.getElementById("<%= txtskuno.ClientID %>").value = "";
            document.getElementById("<%= txtManufacturingDate.ClientID %>").value = "";
            document.getElementById("<%= txtExpirationDate.ClientID %>").value = "";
            document.getElementById("<%= txtlotno.ClientID %>").value = "";
            document.getElementById("<%= txtNoLables.ClientID %>").value = "";
            document.getElementById("<%= txtPONo.ClientID %>").value = "";
            document.getElementById("MainContentHolder_hdnPONo").value = "";
            $("#" + "<%= ddproducts.ClientID %>").val('0');
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
        function checkLabelGenerate(controlid) {
            var status = false;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/CheckLabels',
                dataType: 'json',
                async: false,
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
        function getFormattedDate(date) {
            var year = date.getFullYear();
            /// Add 1 because JavaScript months start at 0
            var month = (1 + date.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = date.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            return month + '/' + day + '/' + year;
        }
        function clearhiddendata() {
            document.getElementById("MainContentHolder_hdnCheckedControlid").value = "";
            document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value = "";
            document.getElementById("MainContentHolder_hdnCheckedDateApproved").value = "";
            document.getElementById("MainContentHolder_hdnCheckedLabelCount").value = "";
            document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value = "";
            document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value = "";
            document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value = "";
            document.getElementById("MainContentHolder_hdnCheckedTotalLabelsToAdd").value = "";
            document.getElementById("MainContentHolder_hdnCheckedTotalLabelsTakenOut").value = "";
        }
        function checkCheckboxChecked() {
            var grid = document.getElementById("MainContentHolder_grvProductLabeling");
            var inputs = grid.querySelectorAll("input[type=checkbox]:checked");
            if (inputs.length > 0) {
                clearhiddendata();
                var val = inputs[0].parentElement.nextElementSibling.value;
                document.getElementById("MainContentHolder_hdnCheckedControlid").value = val.split('-')[1]; //Controlid
                document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value = val.split('-')[0]; //Categorycode
                document.getElementById("MainContentHolder_hdnCheckedDateApproved").value = val.split('-')[2];  //QA Esign  DateApproved
                document.getElementById("MainContentHolder_hdnCheckedLabelCount").value = val.split('-')[3]; //Total Label count to generate
                document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value = val.split('-')[4]; //Total Labels generated
                document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value = val.split('-')[5]; //Total Labels to print
                document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value = val.split('-')[6]; //Total Labels to read
                document.getElementById("MainContentHolder_hdnCheckedTotalLabelsToAdd").value = val.split('-')[7]; // Total No. of Labels to Add after Voiding.
                document.getElementById("MainContentHolder_hdnCheckedTotalLabelsTakenOut").value = val.split('-')[8]; // Total No. of Labels takenout.
                return true;
            }
            else {
                clearhiddendata();
                ErrorMessage("Please select the Product.");
                return false;
            }
        }
        function checkPreviewed() {
            var controlid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
            var status = false;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/CheckPreviewed',
                dataType: 'json',
                async: false,
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + controlid + '"}',
                success: function (response) {
                    var d = JSON.parse(response.d)[0];

                    if (d.previewed == 1) {
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
        function qaSign() {

            if (checkCheckboxChecked()) {

                if (checkPreviewed()) {
                    $("MainContentHolder_ddqanames").val("0");
                    document.getElementById("MainContentHolder_txtsignature").value = "";
                    document.getElementById("MainContentHolder_rfvtxtname").style.visibility = 'hidden';
                    document.getElementById("MainContentHolder_rfvtxtsignature").style.visibility = 'hidden';
                    document.getElementById("MainContentHolder_rbtApproval").checked = false;
                    document.getElementById("MainContentHolder_rbtRejected").checked = false;

                    $("#MainContentHolder_ddlRejectedReasons").val() === "0";
                    document.getElementById("divReasons").style.display = "none";
                    $('#myQAModal').modal('show');
                    return false;
                }
                else {

                    ErrorMessage("The label is not previewed.");
                    return false;
                }
            }
            else {
                return false;
            }
        }
        function generateLabels() {

            if (checkCheckboxChecked()) {

                var generatedLabels = document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value;
                if (generatedLabels == "0") {
                    var dateapproved = document.getElementById("MainContentHolder_hdnCheckedDateApproved").value;
                    var labelcount = document.getElementById("MainContentHolder_hdnCheckedLabelCount").value;
                    if (dateapproved != "0") {
                        if (confirm("Are you sure you want to generate " + labelcount + " Labels!")) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        ErrorMessage("Labels are pending for esign.");
                        return false;
                    }

                } else {
                    ErrorMessage("Labels are already generated.");
                    return false;
                }
            }
            else {
                return false;
            }
        }

        function update(e) {
            if (!checkLabelGenerate(e)) {
                $(".modal-title").text('');
                $('.modal-title').prepend("Edit Product label");
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetProductByControlID',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{data: "' + e + '"}',
                    success: function (response) {
                        var d = JSON.parse(response.d)[0];
                        $("#MainContentHolder_ddproducts").val(d.ProductID);
                        $("#MainContentHolder_ddproducts").trigger('change');
                        document.getElementById('MainContentHolder_hdnControlID').value = d.ControlID;
                        document.getElementById("MainContentHolder_txtlotno").value = d.LotNo;

                        document.getElementById("MainContentHolder_txtPONo").value = d.PONo;
                        document.getElementById("MainContentHolder_hdnPONo").value = d.PONo;

                        document.getElementById("MainContentHolder_txtskuno").value = d.SKUNO;

                        document.getElementById("MainContentHolder_txtNoLables").value = d.LabelCount;
                        document.getElementById("MainContentHolder_txtManufacturingDate").value = d.ManufacturingDate;
                        document.getElementById("MainContentHolder_txtExpirationDate").value = d.ExpirationDate;

                        document.getElementById("MainContentHolder_btnCancel").style.display = "block";
                        document.getElementById("MainContentHolder_btnUpdate").style.display = "block";
                        document.getElementById("MainContentHolder_btnSave").disabled = true;

                        $('#myModal').modal('show');


                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
            else {

                alert("Labels have been generated. Edit is not allowed at this time.");
            }
            return false;
        }
        function CheckReason(e) {
            if (e == "A") {
                document.getElementById("divReasons").style.display = "none";

            } else {
                document.getElementById("divReasons").style.display = "block";
            }
        }
        function labelFormats() {
            var controlid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
            var categorycode = document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value;
            var batchid = 0;
            var val = controlid + "-" + categorycode + "-" + 0;
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
            // CHANGES TO SHOW READONLY FORMAT INSTED OF SLECTING THE FORMAT-- SUGGESTED BY JM AND ALEX
            document.getElementById("MainContentHolder_hdnLabelFormatName").value = document.getElementById("ddlLabelFormats").value;
           
             //var text = ddlFormat.options[ddlFormat.selectedIndex].text;

            if (toprint > 0 && document.getElementById("MainContentHolder_hdnLabelFormatName").value != "") {
                return true;
            } else {
                PrintErrorMessage("All fields are mandatory.");
                return false;
            }
        }
        function testprintValidate() {
            debugger;

            document.getElementById("MainContentHolder_hdnLabelFormatName").value = document.getElementById("ddlLabelFormats").value;
            var ddlFormat = document.getElementById("ddlLabelFormats");
            // var text = ddlFormat.options[ddlFormat.selectedIndex].text;

            if (ddlFormat.value != "0") {
                return true;
            } else {
                PrintErrorMessage("Please select the format.");
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
                var labelcount = document.getElementById("MainContentHolder_hdnCheckedLabelCount").value;
                var generatedlabelcount = document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value
                var nooflabelremaningtoprint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value
                var nooflabelstoread = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value;
                if (labelcount != generatedlabelcount) {
                    ErrorMessage("Please generate the labels.");
                    return false;
                }
                else if (nooflabelremaningtoprint != "0") {
                    ErrorMessage("Please print the labels.");
                    return false;
                }
                else if (nooflabelstoread > 0) {
                    ErrorMessage("Please read the labels.");
                    return false;
                }
                else {
                    var nooflabeltoread = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value;
                    if (nooflabeltoread == "0") {
                        return true;
                    }
                    else {
                        ErrorMessage("Please read all the labels before takeout.");
                        return false;
                    }
                }

            }
            else {
                return false;
            }

        }
        function read() {

            if (checkCheckboxChecked()) {
                var toPrint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                if (toPrint > 0) {
                    ErrorMessage("Please print all the labels before reading.")
                    return false;

                }
                else {

                    var nooflabeltoread = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToRead").value;
                    if (nooflabeltoread == "0") {
                        ErrorMessage("Labels are already read or need to print before reading.")
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
                var generatedLabels = document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value;
                if (generatedLabels == "0") {
                    ErrorMessage("Please generate the labels before printing.")
                    return false;
                } else {
                    var nooflabelremaning = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value;
                    if (nooflabelremaning != "0") {
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
                    else {
                        ErrorMessage("No more labels to print.")
                        return false;
                    }
                }
            }
            else {
                return false;
            }

        }
        function returnlabel() {

            if (checkCheckboxChecked()) {
                var takenoutlabels = document.getElementById("MainContentHolder_hdnCheckedTotalLabelsTakenOut").value;
                if (takenoutlabels > 0) {
                    return true;
                }
                else {
                    ErrorMessage("No labels to return.");
                    return false;
                }
            } else {
                return false;
            }
        }
        function voidlabels() {

            if (checkCheckboxChecked()) {

                var labelcount = document.getElementById("MainContentHolder_hdnCheckedLabelCount").value;
                var nooflabelremaningtoprint = document.getElementById("MainContentHolder_hdnCheckedNoOfLabelsToPrint").value
                var generatedlabelcount = document.getElementById("MainContentHolder_hdnCheckedGeneratedLabels").value
                if (labelcount != generatedlabelcount) {
                    ErrorMessage("Please generate the labels.");
                    return false;
                }
                else if (labelcount == nooflabelremaningtoprint) {
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
                var batchid = 0;
                var tc = 0
                var voidedlabels = 0;
                var data = cid + ',' + lblc + ',' + catid + ',' + batchid + ',' + tc + ',' + voidedlabels;
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
        function QAvalidate() {
            var chkApproved = document.getElementById("MainContentHolder_rbtApproval");
            var chkRejected = document.getElementById("MainContentHolder_rbtRejected");
            if ($("#MainContentHolder_ddqanames").val() === "0") {
                ReadQAErrorMessage("Please select name!");
                return false;
            }
            else if (document.getElementById("MainContentHolder_txtsignature").value == "") {
                ReadQAErrorMessage("Please enter the e-signature!");
                return false;
            }
            else if (!(chkApproved.checked || chkRejected.checked)) {
                ReadQAErrorMessage("Please select Approved or Rejected!");
                return false;
            }

            if (chkApproved.checked) {
                return true;

            } else if (chkRejected.checked) {
                document.getElementById("divmsg").innerHTML = "";
                if ($("#MainContentHolder_ddlRejectedReasons").val() === "0") {
                    ReadQAErrorMessage("Please select rejected reason!");
                    return false;
                }
                return true;
            }
        }
        $(function () {
            $('#MainContentHolder_txtlotno').change(function () {
                document.getElementById("MainContentHolder_hdnPONo").value = "";
                var lotno = this.value;
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetPoNumber',
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

            $('#MainContentHolder_ddproducts').change(function () {
                var productid = this.value;
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/GetProductDetails',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{value: "' + productid + '"}',
                    success: function (response) {
                        var d = JSON.parse(response.d)[0];
                        document.getElementById("MainContentHolder_txtskuno").value = d.SKUNo;
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            });
            //$('#MainContentHolder_txtfilter').change(function () {
            //    alert("de");
            //    debugger;
            //    if (window.event.keyCode == 13) {                   
            //        document.getElementById("MainContentHolder_btnFilter").click();
            //    }
            //});
            $("#MainContentHolder_ddqanames").change(function () {
                document.getElementById("MainContentHolder_hdnqanames").value = this.value;
            });
            $("#MainContentHolder_ddlRejectedReasons").change(function () {
                document.getElementById("MainContentHolder_hdnRejectedReasons").value = this.value;
            });
        });
        function addNewProductLabel() {
            ClearControls('A');
            $('#myModal').modal('show');
            return false;
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

        .col-md-2 {
            width: 11.66666667%;
        }

        .col-md-3 {
            width: 25%;
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
                <h3>Master Labels </h3>
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
                        <h3 class="modal-title" style="font-weight: 500">Print Label</h3>
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
        <%--QA E-Sign Model Popup--%>
        <div class="modal fade" id="myQAModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: 500">QA Approval</h4>
                    </div>
                    <div id="QAread_msg">
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-lg-3 control-label">Name</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddqanames" runat="server" AutoPostBack="false" CssClass="btn btn-default btn-md"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvtxtname" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="red" ValidationGroup="qavalidate" ControlToValidate="ddqanames"></asp:RequiredFieldValidator>
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
                                <asp:RequiredFieldValidator ID="rfvtxtsignature" runat="server" ErrorMessage="*" ForeColor="red" ValidationGroup="qavalidate" ControlToValidate="txtsignature"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="form-group text-center">
                            <div class="col-lg-3 ">
                            </div>
                            <div class="col-lg-3 ">
                                <asp:RadioButton ID="rbtApproval" GroupName="reasons" runat="server" Text="Approved" onclick="CheckReason('A')" />
                            </div>
                            <div class="col-lg-3">
                                <asp:RadioButton ID="rbtRejected" GroupName="reasons" runat="server" Text="Rejected" onclick="CheckReason('R')" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group" id="divReasons" style="display: none">
                            <label class="col-lg-3 control-label">Reason</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlRejectedReasons" runat="server" AutoPostBack="false" CssClass="btn btn-default btn-md"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btnclose" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnesign" class="btn btn-primary" runat="server" Text="E-Sign" ValidationGroup="qavalidate" OnClientClick="return QAvalidate();" OnClick="btnesign_Click" />
                    </div>
                </div>

            </div>
        </div>

        <%--Details of Labels --%>
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
                                <asp:TextBox ID="txtPONo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                            <div class="col-md-3">
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

        <div id="divmsg"></div>
        <br />
        <%--Details--%>
        <div class="row" style="margin-left: 3px;">
            <%-- <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" data-target="#myModal" onclick="ClearControls('A');">Add New Product label</button>  --%>
            <asp:Button ID="btnaddNewProduct" CssClass="btn btn-primary btn-md" runat="server" Text="Add New Product label" OnClientClick="return addNewProductLabel();" />
            <asp:Button ID="btnPreview" CssClass="btn btn-primary btn-md" runat="server" Text="Preview" OnClientClick="return checkCheckboxChecked();" OnClick="btnPreview_Click" />
            <asp:Button ID="btnQAEsign" CssClass="btn btn-primary btn-md" runat="server" Text="QA E-Sign" OnClientClick="return qaSign();" />
            <asp:Button ID="btnVoid" CssClass="btn btn-primary btn-md" runat="server" Text="Void" OnClientClick="return voidlabels();" OnClick="btnvoid_click" />
            <asp:Button ID="btnAddNewLable" CssClass="btn btn-primary btn-md" runat="server" Text="Add Labels" OnClientClick="return GetLabel();" />
            <asp:Button ID="btnGenerateLabels" runat="server" CssClass="btn btn-primary btn-md" Text="Generate Labels" OnClientClick="return generateLabels();" OnClick="btnGenerateLabels_Click" />
            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-md" Text="Print" OnClientClick="return print();" />
            <asp:Button ID="btnRead" runat="server" CssClass="btn btn-primary btn-md" Text="Read" OnClientClick="return read();" OnClick="btnRead_Click" />
            <asp:Button ID="btnTakeout" runat="server" CssClass="btn btn-primary btn-md" Text="Takeout" OnClientClick="return takeout();" OnClick="btnTakeout_Click" />
            <asp:Button ID="btnReturn" runat="server" CssClass="btn btn-primary btn-md" Text="Return" OnClick="btnReturn_Click" />           
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
        <%--<div class="container-fluid">
            <fieldset class="col-md-12">
                <legend>Filters</legend>

                <div class="panel panel-default">
                    <div class="panel-body">

                        <br />
                        <div class="row">
                            <div class="col-md-3 col-xs-1">
                                <div class="btn-group" role="group" aria-label="Status">
                                    <asp:Button ID="btnCompleted" CssClass="btn btn-primary btn-md" runat="server" Text="Completed" OnClick="btnCompleted_Click" />
                                    <asp:Button ID="btnPending" CssClass="btn btn-primary btn-md" runat="server" Text="Pending" OnClick="btnPending_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>--%>
        <br />

        <div class="row">
            <div class="col-md-12">
                <div id="GridScroll" class="grid_scroll">
                    <asp:GridView ID="grvProductLabeling" runat="server" CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" CssClass="table table-striped table-bordered table-condensed"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" OnRowDataBound="grvProductLabeling_RowDataBound"
                        OnSorting="grvProductLabeling_Sorting" OnRowUpdating="grvProductLabeling_RowUpdating">
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
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandArgument='<%# Eval("ControlID") %>' CommandName="Update"
                                        class="btn btn-success"
                                        OnClientClick='<%# String.Format("javascript:return update(\"{0}\");", Eval("ControlID").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ProductID" HeaderText="Product ID" SortExpression="ProductID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ControlID" HeaderText="Control ID" SortExpression="ControlID">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PONO" SortExpression="PONO">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Reasonid" SortExpression="Reasonid">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="statuscode" SortExpression="statuscode">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="approvedbyid" SortExpression="approvedbyid">
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
        <asp:HiddenField ID="hdnCheckedDateApproved" runat="server" />
        <asp:HiddenField ID="hdnCheckedLabelCount" runat="server" />
        <asp:HiddenField ID="hdnCheckedGeneratedLabels" runat="server" />
        <asp:HiddenField ID="hdnCheckedNoOfLabelsToPrint" runat="server" />
        <asp:HiddenField ID="hdnLabelFormatName" runat="server" />
        <asp:HiddenField ID="hdnCheckedNoOfLabelsToRead" runat="server" />
        <asp:HiddenField ID="hdnCheckedTotalLabelsToAdd" runat="server" />
        <asp:HiddenField ID="hdntxtlabelsToAdd" runat="server" />
        <asp:HiddenField ID="hdnCheckedTotalLabelsTakenOut" runat="server" />
        <asp:HiddenField ID="hdnCheckedBatchID" runat="server" />
        <asp:HiddenField ID="hdnmergeData" runat="server" />
    </div>
    <script>
        //$('#MainContentHolder_txtfilter').keypress(function (e) {
        //    if (e.keyCode == 13)
        //        $('#MainContentHolder_btnFilter').click();
        //});
        document.querySelector("#MainContentHolder_txtfilter").addEventListener("keyup", event => {
            if (event.key !== "Enter") return;
            document.querySelector("#MainContentHolder_btnFilter").click();
            event.preventDefault();
        });
    </script>
</asp:Content>
