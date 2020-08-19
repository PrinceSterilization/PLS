<%@ Page Title="Label Voiding" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="voiding.aspx.cs" Inherits="Sterilization.voiding" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
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
        function alertMessage(e) {
            if (confirm("Are you sure you want to void the labels?")) {
                __doPostBack('btnvoid', 'void');
                return true;
            }
            else {
                return false;
            }
        }
        $(document).ready(function () {           
            var gvDrv = document.getElementById("MainContentHolder_grvVoidLabels");

            if (gvDrv.rows.length > 1) {
                document.getElementById("btnVoid").disabled = false;
            }
        });
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
                <h3>Voiding Lables</h3>
            </div>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>     
         <br />
        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <asp:TextBox ID="txtLabel" AutoPostBack="false" runat="server" placeholder="Label Scan" OnTextChanged="txtLabel_TextChanged" ></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="row">
             <div class="col-md-12 text-right">
                 <%--<asp:Button ID="btnVoid" runat="server"  CssClass="btn btn-primary btn-md"  Text="Void" OnClientClick="return alertMessage(this);" OnClick="btnVoid_Click" />--%>
                <%-- <asp:Button ID="btnVoid" runat="server" Enabled="false" CssClass="btn btn-primary btn-md" Text="Button" OnClientClick="return alertMessage(this);" OnClick="btnVoid_Click" />--%>
                 <input type="button" id="btnVoid" class="btn btn-primary btn-md" disabled="disabled" value="Void" onclick="return alertMessage(this);"/>
             </div>
         </div>
        <br />
        <div class="row">
            <asp:GridView ID="grvVoidLabels" runat="server"
            AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
            CssClass="table table-striped table-bordered table-condensed"
            CellPadding="5" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
            BorderWidth="1px" Width="900px" OnRowDataBound="grvVoidLabels_RowDataBound" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="CLABELNO" HeaderText="Label Number" SortExpression="CLABELNO" HeaderStyle-CssClass="header-center">
                    <HeaderStyle CssClass="header-center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="PRODUCTDESC"  HeaderText="Product Description" SortExpression="PRODUCTDESC" HeaderStyle-CssClass="header-center">
                    <HeaderStyle CssClass="header-center"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Void Reason" HeaderStyle-CssClass="header-center">
                    <ItemTemplate>       
                         <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Visible="false" />                             
                        <asp:DropDownList AutoPostBack="true" ID="ddlStatus" CssClass="selectpicker" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" >                          
                            <asp:ListItem Value="1">Printing Error</asp:ListItem>
                            <asp:ListItem Value="2">Wrong Label Requested/Printed</asp:ListItem>
                            <asp:ListItem Value="3">Damaged/Broken Product</asp:ListItem>
                            <asp:ListItem Value="4">Expired Product</asp:ListItem>
                            <asp:ListItem Value="5">Loss of Vacuum</asp:ListItem>   
                            <asp:ListItem Value="6">Damaged Label</asp:ListItem>                                      
                        </asp:DropDownList>
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
        <%--<br />
         <div class="row">
             <div class="col-md-12 text-right">
                 <asp:Button ID="btnVoid" OnClientClick="return alertMessage(this);" runat="server" Enabled="false" CssClass="btn btn-primary btn-md"  Text="Void" OnClick="btnVoid_Click" />
             </div>
         </div>--%>
    </div>
</asp:Content>
