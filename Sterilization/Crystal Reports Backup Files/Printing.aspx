<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Printing.aspx.cs" Inherits="Sterilization.Printing" %>

<asp:Content ID="PrintingContent" ContentPlaceHolderID="MainContentHolder" runat="server">
    <script type="text/javascript">

        function Disablebutton(e) {
            if (e == 1) {
                document.getElementById("MainContentHolder_btnaddnew").style.display = "block";
            } else {
                document.getElementById("MainContentHolder_btnaddnew").style.display = "none";
            }
        }
        function DisablePreview(e) {
            if (e == 1) {
                document.getElementById("divpreview").style.display = "block";
            } else {
                document.getElementById("divpreview").style.display = "none";
                document.getElementById("divesign").style.display = "none";
                document.getElementById("divadd").style.display = "none";
            }
        }

        function OpenWindow(cid, lbc, cat) {
            window.open("Printing.aspx?cid=" + cid + "&lbc=" + lbc + "&cat=" + cat);
        }
        function changename(c, proDesc) {

            if (c == "1") {
                document.getElementById("titlename").innerHTML = "Product - " + proDesc;
            }
            else if (c == "2") {
                document.getElementById("titlename").innerHTML = "Insert Box - " + proDesc;
            }
            else if (c == "4") {
                document.getElementById("titlename").innerHTML = "Component - " + proDesc;
            }
            else if (c == "3") {
                document.getElementById("titlename").innerHTML = "Case - " + proDesc;
            }
        }
        function qaSign() {
            $("<%= ddqanames.ClientID %>").val("0");
            document.getElementById("<%= txtsignature.ClientID %>").value = "";
            document.getElementById("<%= rfvtxtname.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= rfvtxtsignature.ClientID %>").style.visibility = 'hidden';
            document.getElementById("MainContentHolder_rbtApproval").checked = false;
            document.getElementById("MainContentHolder_rbtRejected").checked = false;

            $("#MainContentHolder_ddlRejectedReasons").val() === "0";
            document.getElementById("divReasons").style.display = "none";
            return false;
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
        function ReadErrorMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = divHTML;
            }
        }
        function ReadSuccessMessage(msg) {
            if (document.getElementById("read_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("read_msg").innerHTML = divHTML;
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
        function PrintErrorMessage(msg) {
            if (document.getElementById("print_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-danger'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("print_msg").innerHTML = divHTML;
            }
        }
        function PrintSuccessMessage(msg) {
            if (document.getElementById("print_msg")) {
                var divHTML = "";
                divHTML = "<div class='alert alert-success'  style='margin: 10px;'>";
                divHTML += "<a href='#' class='close' data-dismiss='alert' aria-label='close' onclick='clearcontrol();'>&times;</a>";
                divHTML += "<h5 >" + msg + "</h5></div>";
                document.getElementById("print_msg").innerHTML = divHTML;
            }
        }
        function readBatch(val) {
            __doPostback("btnRead", "ReadLabel");

            // document.getElementById("txtRead").value = "";
            //// document.getElementById("<= rfvtxtRead.ClientID %>").style.visibility = 'hidden';
            // if (document.getElementById("divread")) { document.getElementById("divread").style.display = "none"; }
            // document.getElementById("txtRead").focus();
            // $("#txtRead").focus();
            // if (!("autofocus" in document.createElement("input"))) {
            //     document.getElementById("txtRead").focus();
            // }
           <%-- document.getElementById("<%= hdnControlid.ClientID %>").value = val.split(",")[0];//need to capture data from scaning
            document.getElementById("<%= hdnlabelno.ClientID %>").value = val.split(",")[1];//need to capture data from scaning
            document.getElementById("h1total").innerText = val.split(",")[2];//need to capture data from scaning--%>

            //return false;
        }

        function Readlabel(e) {
            var data = $("#txtRead").val().split('\n');
            if (data != "") {
                var controlid = document.getElementById("<%= hdnControlid.ClientID %>").value;
                var labelno = data[0].split('-')[1];
                var labeldata = controlid + ',' + labelno;

                if (controlid != "" && labelno != "") {
                    $.ajax({
                        type: 'POST',
                        url: 'Webservices/WebServices.asmx/ReadLabelData',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: '{labeldata: "' + labeldata + '"}',
                        success: function (response) {
                            if (response.d == "0") {
                                ReadErrorMessage("The Label is Alredy Read OR Voided !");
                                document.getElementById("txtRead").value = "";

                            }
                            else {
                                ReadSuccessMessage("Sucess ! Label is Read");
                                document.getElementById("txtRead").value = "";
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
        function GetLabel() {
            document.getElementById("<%= txtlabels.ClientID %>").value = "";
            var cid = document.getElementById("<%= hdnControlid.ClientID %>").value;
            var lblc = document.getElementById("<%= hdnlabelcount.ClientID %>").value;
            var catid = document.getElementById("<%= hdncategorycode.ClientID %>").value;

            var data = cid + ',' + lblc + ',' + catid;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetLabelCountToAdd',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {

                    document.getElementById("<%= txtlabels.ClientID %>").value = response.d;
                    document.getElementById("<%= hdntxtlabels.ClientID %>").value = response.d;
                },
                error: function (error) {
                    console.log(error);
                }
            });
            return false;
        }
        function getLabelsToPrint() {
            document.getElementById("<%= ddlLabelFormats.ClientID %>").value = "0";
            $("#" + "<%= ddlLabelFormats.ClientID %>").trigger('change');
            document.getElementById("<%= txtTotalLabelsremaningtoprint.ClientID %>").value = "";
            document.getElementById("<%= hdntxtlabelstoprint.ClientID %>").value = "";
            document.getElementById("halertmsg").innerHTML = "";
            document.getElementById("print_msg").innerHTML = "";

            var cid = document.getElementById("<%= hdnControlid.ClientID %>").value;
            var catid = document.getElementById("<%= hdncategorycode.ClientID %>").value;

            var data = cid + ',' + catid;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetLabelCountToPrint',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                   
                    document.getElementById("halertmsg").innerHTML = "Your are left with " + response.d + " labels to prints.";
                    document.getElementById("<%= txtTotalLabelsremaningtoprint.ClientID %>").value = response.d;
                     document.getElementById("<%= hdntxtlabelstoprint.ClientID %>").value = response.d;
                     if (response.d == "0") {
                         document.getElementById("<%= btnPrint.ClientID %>").disabled = true;
                         document.getElementById("<%= ddlLabelFormats.ClientID %>").disabled = true;
                     } else {
                         document.getElementById("<%= btnPrint.ClientID %>").disabled = false;
                         document.getElementById("<%= ddlLabelFormats.ClientID %>").disabled = false;
                     }
                     return false;
                 },
                 error: function (error) {
                     console.log(error);
                 }
             });
             return false;
         }
        function checkAvailability() {           
            var toPrint = document.getElementById("<%= hdntxtlabelstoprint.ClientID %>").value;
            var entertoPrint = document.getElementById("<%= txtTotalLabelsremaningtoprint.ClientID %>").value;

            if ((parseInt(entertoPrint) <= parseInt(toPrint)) && parseInt(entertoPrint) > 0) {
                document.getElementById("print_msg").innerHTML = "";
                 document.getElementById("<%= btnPrint.ClientID %>").disabled = false;
                return true;
            }
            else {
                PrintErrorMessage("You can only print " + toPrint + " labels.");
                 document.getElementById("<%= btnPrint.ClientID %>").disabled = true;
                return false;
            }
            
        }
        function printValidate() {
           
            var toprint = document.getElementById("<%= txtTotalLabelsremaningtoprint.ClientID %>").value;
            var ddlFormat = document.getElementById("<%= ddlLabelFormats.ClientID %>").value;

            if (toprint > 0 && ddlFormat != 0) {
                return true;
            } else {
                PrintErrorMessage("All fields are mandatory.");
                return false;
            }
        }
        function Addvoided() {
            var data = document.getElementById("<%= hdnlabelno.ClientID %>").value;
            var obj = document.getElementById("<%= hdnobject.ClientID %>").value;
            var voiddescription = document.getElementById("<%= txtvoiddescription.ClientID %>").value;
            var data = data + ',' + voiddescription;
            if (voiddescription != "") {
                $.ajax({
                    type: 'POST',
                    url: 'Webservices/WebServices.asmx/SaveVoidedData',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: '{data: "' + data + '"}',
                    success: function (response) {
                        __doPostBack('btnRefresh', '');
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
                return true;
            }
            else {
                return false;
            }
        }
        function clearcontrol() {
            document.getElementById("txtRead").value = "";
            document.getElementById("txtRead").focus();
        }
        function Changevoid(val, data) {
            document.getElementById("<%= txtvoiddescription.ClientID %>").value = "";
            $("#myModalVoided").modal("show");
            document.getElementById("<%= hdnlabelno.ClientID %>").value = data;
            document.getElementById("<%= hdnobject.ClientID %>").value = val;
            $(val).bootstrapToggle('off');

        }
        $(function () {
            $(".modal").on('shown.bs.modal', function () {
                $("[data-modalfocus]", this).focus();
            });
            $("#" + "<%= ddqanames.ClientID %>").change(function () {
                document.getElementById("<%= hdnqanames.ClientID %>").value = this.value;
            });
            $("#" + "<%= ddlRejectedReasons.ClientID %>").change(function () {
                document.getElementById("MainContentHolder_hdnRejectedReasons").value = this.value;
            });
        });
        function CheckReason(e) {
            if (e == "A") {
                document.getElementById("divReasons").style.display = "none";

            } else {
                document.getElementById("divReasons").style.display = "block";
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
    </style>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3 id="titlename"></h3>
            </div>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <%--QA E sign--%>
        <div class="modal fade" id="myQAModal" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">

                <!-- Modal content-->
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
                        <asp:Button ID="btnesign" class="btn btn-primary" runat="server" Text="E-Sign" ValidationGroup="validate" OnClientClick="return QAvalidate();" OnClick="btnesign_Click" />
                    </div>
                </div>

            </div>
        </div>
        <%--Adding Extra Labels--%>
        <div class="modal fade" id="myModal" role="dialog">
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
                                <asp:Button ID="btnAdd" CssClass="btn btn-primary" runat="server" Text="Add" OnClick="btnAdd_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Adding Voided Description--%>
        <div class="modal fade" id="myModalVoided" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: 500">Void Description</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <asp:TextBox ID="txtvoiddescription" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Void Description" Height="100px"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ControlToValidate="txtvoiddescription" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validaiton"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="row pull-right">
                            <div class="col-md-3">
                                <asp:Button ID="btnAddVoided" CssClass="btn btn-primary" runat="server" Text="Add" OnClientClick="return Addvoided();" ValidationGroup="validaiton" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Reading the Labels--%>
        <div class="modal fade" id="myReadModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: 500">Reading Label</h4>
                    </div>
                    <div id="read_msg">
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <%--<asp:TextBox ID="txtRead" runat="server" TextMode="MultiLine" Height="210px" Width="570px" data-modalfocus></asp:TextBox>--%>
                                <textarea id="txtRead" rows="6" style="height: 210px; width: 570px" autofocus></textarea>
                                <%--  <div>
                                    <asp:RequiredFieldValidator ID="rfvtxtRead" runat="server" ControlToValidate="txtRead" ForeColor="Red" ErrorMessage="Need to Read the Label!" ValidationGroup="validateread"></asp:RequiredFieldValidator>
                                </div>--%>
                            </div>


                            <%-- <div class="col-lg-4" style="margin-left: 10%;">
                                <div class="thumbnail">
                                    <div class="caption">
                                        <h4 class="text-center">Total Labels to Read</h4>
                                        <h1 id="h1total" style="text-align: center; color: red; font-size: 3.2em; margin-bottom: 85px;"></h1>
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="btnreadclose" data-dismiss="modal">Close</button>
                        <%-- <asp:Button ID="btnRead" class="btn btn-primary" runat="server" Text="Read" ValidationGroup="validateread" OnClientClick="return Readlabel(this);" />--%>
                        <%--<input type="button" class="btn btn-primary"  value="Read" onclick="return Readlabel(this);" />--%>
                    </div>
                </div>
            </div>
        </div>


        <%--Printing the Label Required--%>

        <div class="modal fade" id="myPrintModel" role="dialog">
            <div class="modal-dialog" style="margin-top: 15%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title" style="font-weight: 500">Print Label</h3>
                    </div>
                    <div>
                        <h4 id="halertmsg" style="text-align: center; color: red;"></h4>
                    </div>
                    <div id="print_msg">
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:TextBox ID="txtTotalLabelsremaningtoprint" CssClass="form-control"   runat="server" onblur="return checkAvailability();"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">                           
                            <div class="col-lg-6" style="float: none; margin: 0 auto;">
                                <asp:DropDownList ID="ddlLabelFormats" runat="server" CssClass="selectpicker" Style="width: 100%;"></asp:DropDownList>
                               
                            </div>
                        </div>
                        <br />
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnPrint_Click"  OnClientClick="return printValidate();"/> 
                    </div>
                </div>
            </div>
        </div>



        <br />
        <div class="row">
            <div class="col-md-2 col-sm-6 col-xs-12" style="width: auto;" id="divpreview">
                <asp:Button ID="btnPreview" CssClass="btn btn-primary btn-md" runat="server" Text="Preview"
                    OnClick="btnPreview_Click" />
            </div>
            <div class="col-md-2 col-sm-6 col-xs-12" style="width: auto;" id="divesign">
                <asp:Button ID="btnQAEsign" CssClass="btn btn-primary btn-md" runat="server" Text="QA E-Sign"
                    OnClientClick="return qaSign();" data-toggle="modal" data-target="#myQAModal" />
            </div>

            <div class="col-md-2 col-sm-6 col-xs-12" style="width: auto;" id="divadd">
                <asp:Button ID="btnaddnew" CssClass="btn btn-primary btn-md" runat="server" Text="Add New label"
                    OnClientClick="return GetLabel();" data-toggle="modal" data-target="#myModal" />


                <%-- <button type="button" id="btnadd" class="btn btn-primary btn-md" data-toggle="modal" data-target="#myModal"
                    onclick="return GetLabel();">
                    Add New label</button>--%>
            </div>

            <div class="col-md-2 col-sm-6 col-xs-12" style="width: auto;">
                <%-- <asp:Button ID="btnPrint" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
                <asp:Button ID="btnPrintRequired" runat="server" CssClass="btn btn-primary btn-md" Text="Print Labels"
                    OnClientClick="return getLabelsToPrint();" data-toggle="modal" data-target="#myPrintModel" />
            </div>

            <div class="col-md-1 col-sm-6 col-xs-12" style="width: auto;">
                <asp:Button ID="btnReading" CssClass="btn btn-primary btn-md" runat="server" Text="Read"
                    OnClientClick="readBatch();" OnClick="btnReading_Click" />
            </div>
            <div class="col-md-1 col-sm-6 col-xs-12" style="width: auto;">
                <asp:Button ID="btnCase" CssClass="btn btn-primary btn-md" runat="server" Text="Case Labels" OnClick="btnCase_Click" />
            </div>
            <div class="col-md-2 col-sm-6 col-xs-12" style="width: auto;">
                <asp:Button ID="btnRefresh" CssClass="btn btn-primary btn-md" runat="server" Text="Refresh" />
            </div>

        </div>
        <br />
        <div id="PrintingGridScroll" class="grid_scroll">
            <asp:GridView ID="grvPrinting" runat="server" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                CssClass="table table-striped table-bordered table-condensed"
                CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                BorderWidth="1px" Width="900px" OnRowDataBound="grvPrinting_RowDataBound">

                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="CLABELNO" HeaderText="Label Number" SortExpression="CLABELNO" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LABELSTATUS" HeaderText="Label Status" SortExpression="LABELSTATUS" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StatusReason" HeaderText="Status Reason" SortExpression="StatusReason" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="DateApproved" DataFormatString="{0:d}" HeaderText="Date Approved" SortExpression="DateApproved" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="REQUESTED_USER" HeaderText="Requested By" SortExpression="REQUESTED_USER" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="APPROVE_USER" HeaderText="Approved By" SortExpression="APPROVE_USER" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="header-center"></HeaderStyle>
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Printed" HeaderStyle-CssClass="header-center">
                        <ItemStyle CssClass="textalign"></ItemStyle>
                        <ItemTemplate>
                            <input type="checkbox" runat="server" data-onstyle="success" data-offstyle="danger" data-toggle="toggle" id="chkprinted" data-on="Printed" data-off="Not Printed" data-style="ios" disabled />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Voided" HeaderStyle-CssClass="header-center">
                    <ItemStyle CssClass="textalign"></ItemStyle>
                    <ItemTemplate>
                        <input type="checkbox" runat="server" class="chgvoided" data-onstyle="info" data-offstyle="warning" data-toggle="toggle" id="chkvoided" data-on="Voided" data-off="Not Voided" data-style="ios"
                            onchange='<%# String.Format("javascript:return Changevoid(this,\"{0}\");", Eval("ControiD").ToString()+","+  Eval("LABELNO").ToString()+","+  Eval("CATEGORYCODE").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>

                    <asp:BoundField DataField="ControiD" HeaderText="ControlID" SortExpression="ControiD" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Printed" HeaderText="Printed" SortExpression="Printed" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Voided" HeaderText="Voided" SortExpression="Voided" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LABELNO" SortExpression="LABELNO" HeaderStyle-CssClass="header-center">
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
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

        <asp:HiddenField ID="hdnlabelno" runat="server" />
        <asp:HiddenField ID="hdnobject" runat="server" />
        <asp:HiddenField ID="hdnControlid" runat="server" />
        <asp:HiddenField ID="hdnqanames" runat="server" />
        <asp:HiddenField ID="hdnlabelcount" runat="server" />
        <asp:HiddenField ID="hdncategorycode" runat="server" />
        <asp:HiddenField ID="hdntxtlabels" runat="server" />
        <asp:HiddenField ID="hdntxtlabelstoprint" runat="server" />
        <asp:HiddenField ID="hdntxtCaselabelstoprint" runat="server" />
        <asp:HiddenField ID="hdnRejectedReasons" runat="server" />
    </div>
</asp:Content>
