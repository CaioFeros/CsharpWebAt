using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsharpWebAt.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email do cliente é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string Email { get; set; }

        // Propriedade de navegação para as reservas associadas a este cliente
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();

        //Campo para exclusão lógica
        [Display(Name = "Data de Exclusão Lógica")]
        public DateTime? DeletedAt { get; set; } // Nullable DateTime
    }
}
