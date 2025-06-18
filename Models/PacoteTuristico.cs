using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsharpWebAt.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título do pacote turístico é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A data de início do pacote é obrigatória.")]
        [DataType(DataType.Date)] // Indica que é apenas data, sem hora
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A capacidade máxima do pacote é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima deve ser no mínimo 1.")]
        public int CapacidadeMaxima { get; set; }

        [Required(ErrorMessage = "O preço do pacote é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        [DataType(DataType.Currency)] // Indica que é um valor monetário
        public decimal Preco { get; set; }

        public List<Destino> Destinos { get; set; } = new List<Destino>();

        // Propriedade de navegação para as reservas feitas para este pacote
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
