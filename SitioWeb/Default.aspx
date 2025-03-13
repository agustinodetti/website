<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MiSitioWeb._Default" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Usuarios</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { 
            padding-top: 60px; 
        }
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 0 20px;
        }
        h1 {
            margin-bottom: 40px;
            text-align: center;
        }
        .grid-container {
            margin-top: 40px;
        }
        .form-group {
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <h1>Gestion de usuarios via API</h1>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
            <a class="navbar-brand" href="#">Gestión de Usuarios</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <asp:LinkButton ID="btnListar" runat="server" CssClass="nav-link" OnClick="btnListar_Click">Listar</asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="btnRegistrar" runat="server" CssClass="nav-link" OnClick="btnRegistrar_Click">Registrar</asp:LinkButton>
                    </li>
                    <%--<li class="nav-item">
                        <asp:LinkButton ID="btnModificar" runat="server" CssClass="nav-link" OnClick="btnModificar_Click">Modificar</asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="nav-link" OnClick="btnEliminar_Click">Eliminar</asp:LinkButton>
                    </li>--%>
                </ul>
            </div>
        </nav>

        <div class="container">
<%--            <asp:Button ID="btnObtenerDatos" runat="server" Text="Obtener Datos de la API" OnClick="btnObtenerDatos_Click" CssClass="btn btn-primary mb-3" />--%>
            <asp:Label ID="lblResultado" runat="server" Text="" CssClass="d-block mb-3"></asp:Label>

            <%--<asp:Label ID="lblFiltro" runat="server" Text="Filtro: " CssClass="d-block mb-3"></asp:Label>
            <asp:TextBox ID="txtFiltro" runat="server" Text="" CssClass="d-block mb-3"></asp:TextBox>
            <asp:Button ID="btnFiltro" runat="server" Text="Buscar" CssClass="d-block mb-3" OnClick="btnFiltro_Click"></asp:Button>
            --%>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblFiltro" runat="server" Text="Filtro: " CssClass="d-block mb-3"></asp:Label>
                    <asp:TextBox ID="txtFiltro" runat="server" Text="" CssClass="d-block mb-3"></asp:TextBox>
                    <asp:Button ID="btnFiltro" runat="server" Text="Buscar" CssClass="d-block mb-3" OnClick="btnFiltro_Click"></asp:Button>
                
                    <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false" OnRowCommand="gvUsuarios_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                            <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:BoundField DataField="FechaCreacion" HeaderText="FechaCreacion" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModificar" runat="server" CommandName="Modificar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-primary">Modificar</asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('¿Está seguro que desea eliminar este usuario?');">Eliminar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                
                
                </ContentTemplate>
            </asp:UpdatePanel>

            

            <asp:Panel ID="pnlModificar" runat="server" Visible="false">
                <h3>Modificar Usuario</h3>
                <asp:HiddenField ID="hdnUsuarioId" runat="server" />
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtModNombres">Nombres:</asp:Label>
                    <asp:TextBox ID="txtModNombres" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtModApellidos">Apellidos:</asp:Label>
                    <asp:TextBox ID="txtModApellidos" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtModCorreo">Correo:</asp:Label>
                    <asp:TextBox ID="txtModCorreo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtModUsername">Username:</asp:Label>
                    <asp:TextBox ID="txtModUsername" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardarModificacion" runat="server" Text="Guardar Cambios" OnClick="btnGuardarModificacion_Click" CssClass="btn btn-primary" />
            </asp:Panel>

            <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                <h3>Registrar Nuevo Usuario</h3>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNombres">Nombres:</asp:Label>
                    <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtApellidos">Apellidos:</asp:Label>
                    <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtCorreo">Correo:</asp:Label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtUsername">Username:</asp:Label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardarRegistro" runat="server" Text="Guardar" OnClick="btnGuardarRegistro_Click" CssClass="btn btn-primary" />
            </asp:Panel>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.3/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html>