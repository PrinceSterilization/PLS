<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Sterilization.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GPLS Login</title>
</head>
<body>
    <form id="form1" runat="server">
     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.3/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.0.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tether/1.2.0/js/tether.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.3/js/bootstrap.min.js"></script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("header").style.display = "none";
            //CheckingSeassion();
           

        });

        //Checkup if CAPS LOCK is ON when User enters Password
        function capLock(e) {
            kc = e.keyCode ? e.keyCode : e.which;
            sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
            else
                document.getElementById('divMayus').style.visibility = 'hidden';
        }
        //function CheckingSeassion() {
        //  $.ajax({
        //            type: "POST",
        //            url: 'WebServices/WebServices.asmx/LogoutCheck',
        //            data: "{}",
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (response) {
        //                if (response.d == 0) {
        //                    window.location = "Login.aspx";
        //            }
        //        },
        //        failure: function (msg) {
        //            alert(msg);
        //        }
        //    });
        //}
        function UserLog(id) {                        
            if (confirm("You have Loged in to " + cn + ".Are you sure you want to logout from this computer?")) {
                // Delete the user log from UserLog Table.
                deleteUserLog(id);
                document.getElementById("MainContentHolder_hdnUserlog").value = "0";
            }
            else {
                document.getElementById("MainContentHolder_hdnUserlog").value = "1";
            }
        }
        function deleteUserLog(id) {
            $.ajax({
                type: "POST",
                url: 'WebServices/WebServices.asmx/DeleteUserLog',
                data: '{data: "' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == 0) {
                        document.getElementById("MainContentHolder_hdnUserlog").value = "0";
                    }
                    else {
                        document.getElementById("MainContentHolder_hdnUserlog").value = "1";
                    }
                },
                failure: function (msg) {
                    alert(msg);
                }
            });
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

    </script>
    <style>
        body {
            background-color: #F6F6F6;
        }

        .centered {
            left: 50%;
            transform: translate(-50%);
        }

        .set_color {
            background-color: #747679;
            color: #fff;
        }

        .content1 {
            max-width: 964px;
            background-color: white;
            margin: 0 auto;
            /* padding-top: 5%; */
            /* padding-left: 2%; */
            /* padding-right: 2%; */
            padding-bottom: 5%;
            border: 1px solid #496077;
        }
        .bg-title {
            color: #fff!important;
            background-color: #0094d9!important;
        }
    </style>
    <div class="content1 ">
        <div class="bg-title" style="padding: 5px;">
            <h2>Prince Product Label System</h2>
        </div>
        <br />
        <div class="alert alert-success" id="divalertbox" style="display: none;margin: 10px;">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <h5 id="divmsg"></h5>
        </div>
        <br />
        <asp:Image ID="Image1" src="images/Logo.png" runat="server" Style="height: 100px; width: 300px; margin-left: 35%;" />
        <br />
        <br />
        <div class="card text-xs-center centered" style="width: 25rem;">
           
            <div class="card-block">
                <%-- <h4 class="card-title">Special title treatment</h4>
                <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
                <a href="#" class="btn btn-lg btn-primary btn-block">Go somewhere</a>--%>
                <div>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Placeholder="UserName"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Placeholder="Password" TextMode="Password" onkeypress="capLock(event)"></asp:TextBox>
                     <div id="divMayus" style="visibility:hidden;color:red">Caps Lock is on.</div>
                </div>
                
                <div>
                    <asp:Button ID="btnLogin" CssClass="btn btn-lg btn-info btn-block" runat="server" Text="Log In" ValidationGroup="validaiton" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnUserlog" runat="server" />
    </form>
</body>
</html>
