<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="testprint.aspx.cs" Inherits="Sterilization.testprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebServices/WebServices.asmx" />

        </Services>
    </asp:ScriptManager>
    <script>
        function labelFormats() {
            var controlid = document.getElementById("MainContentHolder_hdnCheckedControlid").value;
            var categorycode = document.getElementById("MainContentHolder_hdnCheckedCategoryCode").value;
            var batchid = 0;
            var val = controlid + "-" + categorycode + "-" + 0;
            $.ajax({
                type: 'POST',
                url: 'Webservices/WebServices.asmx/GetLabelFormatsByProduct',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{data: "' + val + '"}',
                success: function (response) {
                    var d = JSON.parse(response.d);
                    document.getElementById("divformats").innerHTML = "";
                    var html = '<select id="ddlLabelFormats" class="btn btn-default btn-md" style="width:100%"><option value="0">-- Select Label Format--</option>'
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


            document.getElementById("MainContentHolder_hdnLabelFormatName").value = document.getElementById("ddlLabelFormats").value;
            var ddlFormat = document.getElementById("ddlLabelFormats");



        }
    </script>
    <div class="content ">
        <div class="panel panel-primary pageheader">
            <div class="panel-body">
                <h3>Test Print Labels </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Product:</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddproducts" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-3">

                        <label class="control-label">Master LabelFormat:</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlMasterFormats" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_3_SKU">1</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_3_SKU_Paragraph">2</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_3_CAT">3</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_3_CAT_Paragraph">4</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_SKU_Paragraph_GBL">5</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_4_SKU">6</asp:ListItem>
                            <asp:ListItem Value="PSS_Product_2_4_CAT_Paragraph">7</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnPrintMaster" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnPrintMaster_Click" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-3">

                        <label class="control-label">Insert LabelFormat:</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlCaseFormats" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_SKU">1</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_SKU_Paragraph">2</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_CAT">3</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_CAT_Paragraph">4</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_4_SKU_Paragraph_GBL">5</asp:ListItem>
                            <asp:ListItem Value="PSS_InsertBox_3_5_CAT_Paragraph">6</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnPrintInsert" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnPrintInsert_Click" />
                    </div>
                     <div class="col-md-3">
                        <asp:CheckBox id ="chkInsert" runat="server" Text ="Insert Units" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Case LabelFormat:</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlInsertFormats" runat="server" CssClass="btn btn-default btn-md" Style="width: 100%;">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_4_SKU">1</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_4_SKU_Paragraph">2</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_4_CAT">3</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_4_CAT_Paragraph">4</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_4_SKU_Paragraph_GBL">5</asp:ListItem>
                            <asp:ListItem Value="PSS_Case_4_5_CAT_Paragraph">6</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnPrintLabels" CssClass="btn btn-primary btn-md" runat="server" Text="Print" OnClick="btnTestPrintLabels_Click" />
                    </div>
                     <div class="col-md-3">
                        <asp:CheckBox id ="chkCase" runat="server" Text ="Case Units"/>
                    </div>
                </div>



            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnLabelFormatName" runat="server" />
</asp:Content>
