<%@ Page Title="Takeout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="shipment.aspx.cs" Inherits="Sterilization.shipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <script type="text/javascript">
        //function stopRKey(evt) {            
        //    var evt = (evt) ? evt : ((event) ? event : null);
        //    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
        //    if ((evt.keyCode == 13) && (node.type == "text")) {
        //        mergeValidate();
        //    }
        //}
        //document.onkeypress = stopRKey;
        $(document).ready(function () {
           // document.getElementById("MainContentHolder_txtReadLabel").focus();
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

        function MergeSuccessMessage(msg) {
            if (document.getElementById("merge_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("merge_msg").innerHTML = "";
                document.getElementById("merge_msg").innerHTML = divHTML;

            }
        }
        function MergeErrorMessage(msg) {
            if (document.getElementById("merge_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' >&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("merge_msg").innerHTML = "";
                document.getElementById("merge_msg").innerHTML = divHTML;
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
        function merge() {
           // document.getElementById("MainContentHolder_btnMergeLabels").disabled = false;
                getAllBatchids();
                $('#myMergeModel').modal('show');
                return false;
           
        }
        function getAllBatchids() {
           
            var controlid = document.getElementById("MainContentHolder_hdnControlId").value;
            //var batchid = document.getElementById("MainContentHolder_hdnCheckedBatchID").value;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetAllBatchesByControlID',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + controlid + '"}',
                success: function (response) {                   
                    var d = JSON.parse(response.d);
                    var fromhtml = "";
                    var tohtml = "";
                    for (i = 0; i < d.length; i++) {
                        //if (d[i].BATCHID != batchid)
                        fromhtml += "<input type='radio'  name='frombatch' value='" + d[i].BATCHID + "'>BatchID " + d[i].BATCHID + "<br/>";
                    }
                    for (i = 0; i < d.length; i++) {
                        //if (d[i].BATCHID != batchid)
                        tohtml += "<input type='radio'  name='tobatch' value='" + d[i].BATCHID + "'>BatchID " + d[i].BATCHID + "<br/>";
                    }
                    //from batchid
                    document.getElementById("divFromBatches").innerHTML = "";
                    document.getElementById("divFromBatches").innerHTML = fromhtml;

                    //to batchid
                    document.getElementById("divToBatches").innerHTML = "";
                    document.getElementById("divToBatches").innerHTML = tohtml;

                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
        function mergeValidate() {
           
            
            var fromgrid = document.getElementById("divFromBatches");
            var frominputs = fromgrid.querySelectorAll("input[type=radio]:checked");

            var togrid = document.getElementById("divToBatches");
            var toinputs = togrid.querySelectorAll("input[type=radio]:checked");

            //if (frominputs[0].value != toinputs[0].value) {
            //    document.getElementById("MainContentHolder_hdnfrombatchid").value = frominputs[0].value;
            //    document.getElementById("MainContentHolder_hdntobatchid").value = toinputs[0].value;
               
            //}
            //else {
            //    alert("You cannot select same batchid's! ");
            //    return false;
            //}

            return mergBatches(frominputs[0].value, toinputs[0].value);
            //var t = toinputs[i].value
            //var f = frominputs[i].value

            //if (inputs.length > 0) {
            //    var val = "";
            //    for (i = 0; i < inputs.length; i++) {
            //        val += inputs[i].value + ',';
            //    }
            //    val = val.substr(0, val.length - 1);
            //    document.getElementById("MainContentHolder_hdnmergeData").value = val;
            //    return true;
            //} else {
            //    ErrorMessage("Please select the batch to merge.");
            //    return false;
            //}
           // return false;
        }
        function mergBatches(from, to) {
           
            var controlid = document.getElementById("MainContentHolder_hdnControlId").value;
            var status = false;
            var data = controlid + "-" + from + "-" + to;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/MergeBatches',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                   
                    status = true;
                    MergeSuccessMessage("Labels are merged successfull.");
                },
                error: function (error) {
                   
                    status = false;
                    MergeErrorMessage("Failed to merge.");
                    //console.log(error);
                }
            });
            return status;
        }
        $(function () {
            //$('#MainContentHolder_btnMerge').bind('keydown', function (e) {
            //    //on keydown for all textboxes
            //    // if (e.target.className != "searchtextbox") {
            //    console.log("outside");
            //    if (e.keyCode == 13) { //if this is enter key
            //        console.log("inside");
            //            e.preventDefault();
            //            return false;
                   
            //    }
            //    //else
            //    //    return true;
            //});


            $("#MainContentHolder_ddlBatches").change(function () {               
                var replacementchecked = document.getElementById("MainContentHolder_chkDamage");
                if (!replacementchecked.checked) {
                    var batchid = this.value;
                    $.ajax({
                        type: 'POST',
                        url: 'Webservices/WebServices.asmx/CheckGeneratedBatch',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: '{data: "' + batchid + '"}',
                        success: function (response) {                          
                            if (response.d == "1") {
                                document.getElementById("MainContentHolder_txtTakeoutLabel").disabled = true;
                                ReadErrorMessage("Labels are generated for this batch.You cannot takeout anymore for this batch.Chose diffrent batchid");
                            } else {
                                document.getElementById("read_msg").innerHTML = "";
                                document.getElementById("MainContentHolder_txtTakeoutLabel").disabled = false;
                                document.getElementById("MainContentHolder_txtTakeoutLabel").focus();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });               
                }
            });
               
        });
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
                <h3>Component/Product Takeout</h3>
            </div>
        </div>
        <br />      
        <div id="read_msg">
        </div>
        <br />
            <%--Merging Labels--%>
        <div class="modal fade" id="myMergeModel" role="dialog">
           
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" onclick="clearmergecreen()">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Merge Label</h3>
                    </div>

                    <div class="modal-body">
                        <div id="merge_msg">

            </div>
                        <div class="row">
                            <div class="col-md-2">
                                <label class="control-label">From:</label>
                            </div>
                            <div class="col-md-4" id="divFromBatches">
                                </div>
                             <div class="col-md-2">
                                <label class="control-label">Merge To:</label>
                            </div>
                            <div class="col-md-3" id="divToBatches">
                            </div>
                        </div>
                       
                        <br />
                    </div>

                    <div class="modal-footer">
                        <%--<asp:Button ID="btnMergeLabels" CssClass="btn btn-primary btn-md" Enabled="false" runat="server" Text="Merge" OnClientClick="return mergeValidate();" OnClick="btnMergeLabels_Click" />--%>
                        
                        <button id="btnMergeLabels" class="btn btn-primary btn-md"  onclick="mergeValidate()">Merge</button>
                    </div>
                </div>
            </div>
        </div>
        
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
        <div class="row">
             <div class="col-md-2 col-sm-6 col-xs-12">
                  <label class="col-md-5 col-sm-6 col-xs-12 control-label">Usage:</label>     
                 </div>
            <div class="col-md-2 col-sm-6 col-xs-12">
                <asp:RadioButton ID="rbtSample" runat="server" GroupName="usage" Text="Samples" AutoPostBack="true"  OnCheckedChanged="rbtSample_CheckedChanged" />
            </div>
            <div class="col-md-2 col-sm-6 col-xs-12">
                <asp:RadioButton ID="rbtDirectShipment" runat="server" GroupName="usage" Text="Direct Shipment" Checked="true"  AutoPostBack="true"  OnCheckedChanged="rbtDirectShipment_CheckedChanged"/>
            </div>
            <div class="col-md-2 col-sm-6 col-xs-12">
                <asp:RadioButton ID="rbtProductKit" runat="server" GroupName="usage"  Text="Product Kit"  AutoPostBack="true"  OnCheckedChanged="rbtProductKit_CheckedChanged"/>
            </div>
        </div>
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12" id="ddlbatchid">
               <label class="col-md-5 col-sm-6 col-xs-12 control-label">Batch ID:</label>   
                <asp:DropDownList ID="ddlBatches" runat="server" CssClass="btn btn-default btn-md"></asp:DropDownList>                
             <%--<asp:Label ID="lblBatchID" runat="server" class="control-label col-md-7 col-sm-6 col-xs-12"></asp:Label>--%>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-12" >
                 <asp:CheckBox ID="chkDamage" runat="server" Text="Replacement for damage?" />
             </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Button ID="btnMerge" runat="server" CssClass="btn btn-primary btn-md" Text="Merge Labels"  OnClientClick="return merge();"  />    
            </div>
        </div>
        <br />
        <div class="row">
            <div class="control-label col-md-4 col-sm-4 col-xs-12">
                <asp:TextBox ID="txtTakeoutLabel" runat="server" placeholder="Label Scan"  CssClass="form-control btn-md" OnTextChanged="txtTakeoutLabel_TextChanged"></asp:TextBox>
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
         <asp:HiddenField ID="hdnControlId" runat="server" />
        <asp:HiddenField ID="hdnfrombatchid" runat="server" />
        <asp:HiddenField ID="hdntobatchid" runat="server" />
        
    </div>
</asp:Content>
