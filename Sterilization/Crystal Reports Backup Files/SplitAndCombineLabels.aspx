<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SplitAndCombineLabels.aspx.cs" Inherits="Sterilization.SplitAndCombineLabels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <style>
        .labelformat {
            font-weight: bold;
            margin-right: 10px;
        }

        table {
            border-collapse: collapse;
        }

        td {
            padding-top: .5em;
            padding-bottom: .5em;
        }
    </style>
    <script>
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
        function split() {
            var currentsize = document.getElementById("MainContentHolder_hdnCurrentSize").value;
            var requestedsize = document.getElementById("MainContentHolder_hdnRequestedSize").value;
            if (currentsize && requestedsize) {
                var totalsize = 0;
                var items = [];
                for (var i = 1; i <= requestedsize; i++) {
                    var data = document.getElementById("MainContentHolder_txtLB" + i).value;
                    items.push(data);
                    totalsize += parseInt(data);
                }

                if (parseInt(currentsize) == totalsize) {
                    splitLabels(items);
                }
                else {
                    ErrorMessage("Currentsize is not equal to total label size");
                }
            }
            else {
                ErrorMessage("Problem in spliting the labels");
            }
        }
        function splitLabels(items) {
            var controlid = document.getElementById("MainContentHolder_hdnControlid").value;
            var batchid = document.getElementById("MainContentHolder_hdnBatchid").value;
            var label = document.getElementById("MainContentHolder_txtSplitLabel").value;
            var data = controlid + "-" + batchid + "-" + items + "-" + label.split("-")[2];
            //console.log(data);
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/SplitLabels',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                    if (response.d == "1") {
                        SuccessMessage("Split is successfull!");
                        cleardata();
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
        function cleardata() {
            document.getElementById("MainContentHolder_panelrows").innerHTML = "";
            document.getElementById("MainContentHolder_txtLabels").value = "";
            document.getElementById("MainContentHolder_txtSplitLabel").value = ""
            document.getElementById("MainContentHolder_lblTotalLabelsLeftToSplit").innerHTML = "";

            //combine 
            document.getElementById("MainContentHolder_panelrows").innerHTML = "";
            document.getElementById("MainContentHolder_txtCombine").value = "";

        }
        $(function () {
            $("#MainContentHolder_txtCombine").change(function () {
                getLabelDetails();
            });
        });
        function getLabelDetails() {
            var batchid = document.getElementById("MainContentHolder_hdnBatchid").value;
            var label = document.getElementById("MainContentHolder_txtCombine").value + "-" + batchid;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetCaseSizeForCombine',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + label + '"}',
                success: function (response) {
                    if (response.d != "") {
                        createTable(response.d);
                        cleardata();
                    }
                    else {
                        ErrorMessage("label is voided");
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
        function createTable(r) {
            debugger;
            var label = document.getElementById("MainContentHolder_txtCombine").value;
            var tbody = document.getElementById("tbdata");

            var d = document;
            var row = d.createElement("tr");
            row.id = "row" + label.split("-")[0] + label.split("-")[1] + label.split("-")[2];
            var cell1 = d.createElement("td");
            var cell2 = d.createElement("td");
            var cell3 = d.createElement("td");
            var cell4 = d.createElement("td");

            var text1 = document.createTextNode(label);
            cell1.appendChild(text1);


            var text2 = document.createTextNode(r.split(',')[0]);
            cell2.appendChild(text2);

            var text3 = document.createTextNode(r.split(',')[1]);
            cell3.appendChild(text3);


            var btn = d.createElement("input");
            btn.type = "button";
            btn.value = "Delete"
            btn.className = "btn btn-danger"
            //var t = document.createTextNode("Delete");
            //btn.appendChild(t);
            btn.setAttribute("onclick", "deleterow(" + label.split("-")[0] + label.split("-")[1] + label.split("-")[2] + ")");
            //btn.addEventListener("click", deleterow(label));
            cell4.appendChild(btn);

            row.appendChild(cell1);
            row.appendChild(cell2);
            row.appendChild(cell3);
            row.appendChild(cell4);



            //var tr = " <tr id='row" + label + "'><td>"  + label + 
            //            +"</td><td>" + d + "</td><td>" +
            //                +"<a id='delete' class='btn btn-danger' onclick='deleterow(" + label + ")'>Delete</a>" +
            //            +"</td></tr>"


            tbody.append(row);
            return false;
        }
        function deleterow(r) {
            debugger;
            if (document.getElementById("row" + r))
                document.getElementById("row" + r).innerHTML = "";
        }
        function combineLabels() {
            var data = "";
            $('#tbdata tr').each(function (index, tr) {
                var tds = $(tr).find('td');
                if (tds.length > 1) {
                    data += tds[0].innerHTML + "=";
                }
            });
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/CombinedLabels',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + data + '"}',
                success: function (response) {
                    if (response.d != "") {
                        cleardata();
                        alert("Combined successfull");

                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
            console.log(data);
        }

    </script>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Split and Combine Labels </h3>
            </div>
        </div>
        <br />
        <div id="divmsg"></div>
        <br />
        <div>
            <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#split" aria-controls="split" role="tab" data-toggle="tab">Split Labels</a></li>
                <li><a href="#combine" aria-controls="combine" role="tab" data-toggle="tab">Combine Labels</a></li>
            </ul>
        </div>
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="split">
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <label>Scan the Label which you want to split</label>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSplitLabel" AutoPostBack="true" OnTextChanged="txtSplitLabel_TextChanged"></asp:TextBox>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        <label id="lblTotalLabelsLeftToSplit" runat="server"></label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <label>How many Labels would you like to divide?</label>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtLabels" AutoPostBack="true" OnTextChanged="txtLabels_TextChanged"></asp:TextBox>
                    </div>
                </div>
                <asp:Panel ID="panelrows" runat="server"></asp:Panel>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <%--<asp:Button ID="btnSplit" runat="server" CssClass="btn btn-primary btn-md" UseSubmitBehavior="False"  Text="Split Labels" OnClientClick="return split();"  />--%>
                        <button type="button" class="btn btn-primary btn-md" onclick="split()">Split Labels</button>
                        <button type="button" class="btn btn-primary btn-md" id="btnReset" onclick="cleardata()">Reset</button>
                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane" id="combine">
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <label>Scan the Label which you want to combine</label>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCombine"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row">
                    <table class="table table-bordered table-hover" id="ext_logic">
                        <thead class="table table-bordered table-hover">
                            <tr>
                                <th class="text-center">Label No
                                </th>
                                <th class="text-center">Case Size
                                </th>
                                <th class="text-center">Unit Size
                                </th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody id="tbdata">
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <a id="btnCombine" class="btn btn-primary" style="float: right;" onclick="combineLabels();">Combine</a>
                    <%--<button id="btnCombine" type="reset" onkeyup="avoidEnter(this);" class="btn btn-primary" style="float: right;" onclick="combineLabels();">Combine</button>--%>
                </div>
                <%--  <div class="row">
                    <asp:GridView ID="grvLabels" runat="server"
                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                        CssClass="table table-striped table-bordered table-condensed"
                        CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                        BorderWidth="1px" Width="900px" >
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="LabelNo" HeaderText="Label Number" SortExpression="LabelNo" HeaderStyle-CssClass="header-center">
                                <HeaderStyle CssClass="header-center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Size" HeaderText="Case Size" SortExpression="Size" HeaderStyle-CssClass="header-center">
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
                </div>--%>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCurrentSize" />
    <asp:HiddenField runat="server" ID="hdnRequestedSize" />
    <asp:HiddenField runat="server" ID="hdnControlid" />
    <asp:HiddenField runat="server" ID="hdnBatchid" />
</asp:Content>
