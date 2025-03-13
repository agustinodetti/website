using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SitioWeb.Clases;

namespace MiSitioWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        //comment para primer commit develop
        //protected async void btnObtenerDatos_Click(object sender, EventArgs e)
        //{
        //    string apiUrl = "https://localhost:44346/api/usuario/buscar/2"; // Reemplaza con la URL de tu API

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Realiza la solicitud GET a la API
        //            HttpResponseMessage response = await client.GetAsync(apiUrl);
        //            response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado no es 200-299

        //            // Lee la respuesta como una cadena
        //            string responseData = await response.Content.ReadAsStringAsync();

        //            // Muestra la respuesta en la etiqueta
        //            lblResultado.Text = responseData;
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            // Maneja errores de la solicitud
        //            lblResultado.Text = $"Error al consumir la API: {ex.Message}";
        //        }
        //        catch (Exception ex)
        //        {
        //            // Maneja otros errores
        //            lblResultado.Text = $"Error inesperado: {ex.Message}";
        //        }
        //    }
        //}

        protected async void CargarUsuarios()
        {
            string apiUrl = "https://localhost:44346/api/usuario/listar";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(responseData);

                    gvUsuarios.DataSource = usuarios;
                    gvUsuarios.DataBind();
                }
                catch (Exception ex)
                {
                    lblResultado.Text = $"Error al cargar usuarios: {ex.Message}";
                }
            }
        }

        protected async Task<List<Usuario>> CargarUsuarios2()
        {
            string apiUrl = "https://localhost:44346/api/usuario/listar";

            using (HttpClient client = new HttpClient())
            {
                
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(responseData);

                return usuarios;
                //gvUsuarios.DataSource = usuarios;
                //gvUsuarios.DataBind();
                
            }
        }
        protected async Task CargarUsuariosFiltrados(string filtro)
        {

            List<Usuario> usuarios = await CargarUsuarios2();
            var usuarios2 = usuarios;

            if (usuarios != null)
            {
                var usrFiltrados = usuarios.Where(u => u.Username.Contains(filtro));
                //|| u.Nombres.Contains(filtro) || u.Apellidos.Contains(filtro) || u.Correo.Contains(filtro)


                usuarios2 = usrFiltrados.ToList();
            }
            lblResultado.Text = $"{usuarios2.Count} usuarios encontrados.";

            gvUsuarios.DataSource = usuarios2;
            gvUsuarios.DataBind();

        }

        protected void btnListar_Click(object sender, EventArgs e)
        {
            pnlRegistro.Visible = false;
            pnlModificar.Visible = false;
            gvUsuarios.Visible = true;
            CargarUsuarios();
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            pnlRegistro.Visible = true;

            pnlModificar.Visible = false;
            gvUsuarios.Visible = false;
            lblResultado.Text = "";

        }


        protected async void btnFiltro_Click(object sender, EventArgs e)
        {
            pnlRegistro.Visible = false;

            pnlModificar.Visible = false;

            await CargarUsuariosFiltrados(txtFiltro.Text);

            gvUsuarios.Visible = true;
            //UpdatePanel1.Update();
            //lblResultado.Text = "";
        }


        //protected void btnModificar_Click(object sender, EventArgs e)
        //{
        //    pnlRegistro.Visible = false;
        //    gvUsuarios.Visible = true;
        //    // Implementar lógica para modificar
        //    lblResultado.Text = "Funcionalidad de modificación no implementada aún.";
        //}

        //protected void btnEliminar_Click(object sender, EventArgs e)
        //{
        //    pnlRegistro.Visible = false;
        //    gvUsuarios.Visible = true;
        //    // Implementar lógica para eliminar
        //    lblResultado.Text = "Funcionalidad de eliminación no implementada aún.";
        //}

        protected async void btnGuardarRegistro_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:44346/api/usuario/guardar";

            var nuevoUsuario = new Usuario
            {
                Nombres = txtNombres.Text,
                Apellidos = txtApellidos.Text,
                Correo = txtCorreo.Text,
                Username = txtUsername.Text
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(nuevoUsuario), System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    lblResultado.Text = "Usuario registrado exitosamente.";

                    // Limpiar los campos del formulario
                    txtNombres.Text = "";
                    txtApellidos.Text = "";
                    txtCorreo.Text = "";
                    txtUsername.Text = "";

                    // Ocultar el panel de registro y mostrar la lista actualizada
                    pnlRegistro.Visible = false;
                    pnlModificar.Visible = false;
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    lblResultado.Text = $"Error al registrar usuario: {ex.Message}";
                }
            }

            // Después de guardar exitosamente
            pnlRegistro.Visible = false;
            gvUsuarios.Visible = true;
            CargarUsuarios();
        }
        protected async void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Modificar")
            {
                await CargarUsuarioParaModificar(userId);
                UpdatePanel1.Update();
            }
            else if (e.CommandName == "Eliminar")
            {
                await EliminarUsuario(userId);
                CargarUsuarios();
                UpdatePanel1.Update();
            }
        }

        protected async Task CargarUsuarioParaModificar(int userId)
        {
            string apiUrl = $"https://localhost:44346/api/usuario/buscar/{userId}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(responseData);

                    hdnUsuarioId.Value = usuario.Id.ToString();
                    txtModNombres.Text = usuario.Nombres;
                    txtModApellidos.Text = usuario.Apellidos;
                    txtModCorreo.Text = usuario.Correo;
                    txtModUsername.Text = usuario.Username;

                    pnlModificar.Visible = true;
                }
                catch (Exception ex)
                {
                    lblResultado.Text = $"Error al cargar usuario: {ex.Message}";
                }
            }
            //UpdatePanel1.Update(); // Forzar actualización del UpdatePanel


        }

        protected async void btnGuardarModificacion_Click(object sender, EventArgs e)
        {
            

            var usuarioModificado = new Usuario
            {
                Id = Convert.ToInt32(hdnUsuarioId.Value),
                Nombres = txtModNombres.Text,
                Apellidos = txtModApellidos.Text,
                Correo = txtModCorreo.Text,
                Username = txtModUsername.Text
            };

            string apiUrl = $"https://localhost:44346/api/usuario/actualizar/{usuarioModificado.Id}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(usuarioModificado), System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    lblResultado.Text = "Usuario modificado exitosamente.";
                    pnlModificar.Visible = false;
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    lblResultado.Text = $"Error al modificar usuario: {ex.Message}";
                }
            }
        }

        protected async Task EliminarUsuario(int userId)
        {
            string apiUrl = $"https://localhost:44346/api/usuario/eliminar/{userId}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    lblResultado.Text = "Usuario eliminado exitosamente.";
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    lblResultado.Text = $"Error al eliminar usuario: {ex.Message}";
                }
            }
        }

    }

    //public class Usuario
    //{
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }
    //    public string Email { get; set; }
    //    // Agrega más propiedades según la estructura de tus datos
    //}
}