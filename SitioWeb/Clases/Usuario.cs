using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SitioWeb.Clases
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Username { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}