using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CsharpWebAt.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        // Chave estrangeira para Cliente
        [Required(ErrorMessage = "O ID do cliente é obrigatório para a reserva.")]
        public int ClienteId { get; set; }

        // Propriedade de navegação para o Cliente
        // Permite carregar o objeto Cliente relacionado
        public Cliente Cliente { get; set; }

        // Chave estrangeira para PacoteTuristico
        [Required(ErrorMessage = "O ID do pacote turístico é obrigatório para a reserva.")]
        public int PacoteTuristicoId { get; set; }

        // Propriedade de navegação para o PacoteTuristico
        // Permite carregar o objeto PacoteTuristico relacionado
        public PacoteTuristico PacoteTuristico { get; set; }

        [Required(ErrorMessage = "A data da reserva é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataReserva { get; set; }

    }
}
