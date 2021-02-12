<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login2.aspx.cs" Inherits="SITConnect_Assgn.Login2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LdRpkoaAAAAACLeecaVz4L4l4kzekvlWKom5SNE"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:Label ID="lb_email" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lb_pwd" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="LoginMe" />
            <br />
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdRpkoaAAAAACLeecaVz4L4l4kzekvlWKom5SNE', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
