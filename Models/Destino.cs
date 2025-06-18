using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsharpWebAt.Models
{
    public class Destino
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do destino é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O país do destino é obrigatório.")]
        [StringLength(100, ErrorMessage = "O país não pode exceder 100 caracteres.")]
        public string Pais { get; set; }
    }
}
