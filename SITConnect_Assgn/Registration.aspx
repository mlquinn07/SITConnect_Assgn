<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect_Assgn.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("lb_pwdchecker").innerHTML = "Password length must have a minimum of 8 characters.";
                document.getElementById("lb_pwdchecker").style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lb_pwdchecker").innerHTML = "Password requires 1 numeral";
                document.getElementById("lb_pwdchecker").style.color = "Grey";
                return ("no_number");
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lb_pwdchecker").innerHTML = "Password requires 1 uppercase";
                document.getElementById("lb_pwdchecker").style.color = "Green";
                return ("no_uppercase");
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lb_pwdchecker").innerHTML = "Password requires 1 lowercase";
                document.getElementById("lb_pwdchecker").style.color = "Orange";
                return ("no_lowercase");
            }

            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lb_pwdchecker").innerHTML = "Password requires 1 special character";
                document.getElementById("lb_pwdchecker").style.color = "Pink";
                return ("no_specialchar");
            }

            document.getElementById("lb_pwdchecker").innerHTML = "Excellent!"
            document.getElementById("lb_pwdchecker").style.color = "Blue";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbRegister" runat="server" Text="Registration Form" Font-Size="20pt"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lb_fname" runat="server" Text="First Name:"></asp:Label>
            <br />
            <asp:TextBox ID="tb_fname" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lb_lname" runat="server" Text="Last Name:"></asp:Label>
            <br />
            <asp:TextBox ID="tb_lname" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lb_CCI" runat="server" Text="Credit Card Info: (to be encrypted)"></asp:Label>
            <br />
            <asp:TextBox ID="tb_CCI" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lb_email" runat="server" Text="Email Address: (must be unique)"></asp:Label>
            <br />
            <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lb_password" runat="server" Text="Password:"></asp:Label>
            <br />
            <asp:TextBox ID="tb_password" runat="server" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
            <asp:Label ID="lb_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
            <br />
            <asp:Label ID="lb_dob" runat="server" Text="Date of Birth:"></asp:Label>
            <br />
            <asp:TextBox ID="tb_dob" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" />
        </div>
    </form>
</body>
</html>
