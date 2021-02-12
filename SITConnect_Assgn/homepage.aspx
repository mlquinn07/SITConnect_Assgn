<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="SITConnect_Assgn.homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            User Profile:<br />
            <br />
            Email:
            <asp:Label ID="lb_email" runat="server"></asp:Label>
            <br />
            Name:
            <asp:Label ID="lb_name" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Success!"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe"/>
        </div>
    </form>
</body>
</html>
