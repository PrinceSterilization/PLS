<%@ Page Title="Report"  Language="C#" AutoEventWireup="true" CodeBehind="Reportpage.aspx.cs" Inherits="Sterilization.Reportpage" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style>
        .content {
            max-width: 964px;
            background-color: white;
            margin: 0 auto;           
            padding-top: 5%;
            padding-left: 2%;
            padding-right: 2%;
            padding-bottom: 5%;
          
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
        <div class="content">
            <CR:CrystalReportViewer ID="crPreview" runat="server" AutoDataBind="true" />
     </div>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
       
    </form>
</body>
</html>
