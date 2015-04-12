<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmIntegration.aspx.cs" Inherits="Link.WebUI.ConfirmIntegration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Table runat="server" width ="100%">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:TextBox ID="lblAnuncio" text="Ud se ha registrado y hemos publicado sus articulos en mercadoLibre." runat="server"  Width="100%"> </asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnGetLastPurchase" Text="Obtener Ultima Compra" runat="server" OnClick="btnGetLastPurchase_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</body>
</html>
