using System;
using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.Modelos.ViewModels
{
    public class UsuarioPerfilVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Correo electrónico")]
        [EmailAddress]
        public string Correo { get; set; }

        [Display(Name = "Nueva Contraseña")]
        [DataType(DataType.Password)]
        public string? HashContrasena { get; set; }


    }
}
