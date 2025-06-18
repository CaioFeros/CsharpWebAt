using System;
using System.Collections.Generic;
using System.Linq;

namespace CsharpWebAt.Data
{
    // Classe para simular um Pacote Turístico
    public class SimulatedPackage
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public int CapacidadeMaxima { get; set; }
        public int ParticipantesAtuais { get; set; }
        public decimal Preco { get; set; }
        public List<string> Destinos { get; set; } = new List<string>();

        public bool TemVagasDisponiveis => ParticipantesAtuais < CapacidadeMaxima;
        public bool EhDataFutura => DataInicio > DateTime.Today;
    }

    // Classe para simular uma Reserva
    public class SimulatedReservation
    {
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public int PacoteId { get; set; } 
        public string PacoteTitulo { get; set; }
        public DateTime DataReserva { get; set; }
    }

    // Classe estática para manter os dados simulados
    public static class DataSource
    {
        public static List<SimulatedPackage> SimulatedPackages { get; private set; }
        public static List<SimulatedReservation> SimulatedReservations { get; private set; }

        static DataSource() // Construtor estático, executado uma vez quando a classe é acessada
        {
            SimulatedPackages = new List<SimulatedPackage>
            {
                new SimulatedPackage { Id = 1, Titulo = "Praias do Nordeste", DataInicio = DateTime.Today.AddMonths(2), CapacidadeMaxima = 10, ParticipantesAtuais = 5, Preco = 1500m, Destinos = new List<string> { "Porto Seguro", "Salvador", "Recife" } },
                new SimulatedPackage { Id = 2, Titulo = "Serra Gaúcha Aventura", DataInicio = DateTime.Today.AddDays(30), CapacidadeMaxima = 5, ParticipantesAtuais = 4, Preco = 1200m, Destinos = new List<string> { "Gramado", "Canela", "Bento Gonçalves" } },
                new SimulatedPackage { Id = 3, Titulo = "Viagem no Tempo - Ruínas", DataInicio = DateTime.Today.AddDays(-10), CapacidadeMaxima = 8, ParticipantesAtuais = 0, Preco = 900m, Destinos = new List<string> { "Ouro Preto", "Tiradentes" } }, // Pacote com data passada
                new SimulatedPackage { Id = 4, Titulo = "Pacote Cheio", DataInicio = DateTime.Today.AddMonths(1), CapacidadeMaxima = 2, ParticipantesAtuais = 2, Preco = 2000m, Destinos = new List<string> { "Rio de Janeiro", "Paraty" } } // Pacote cheio
            };

            SimulatedReservations = new List<SimulatedReservation>();
        }

        public static bool TryAddReservation(SimulatedReservation newReservation)
        {
            var pacote = SimulatedPackages.FirstOrDefault(p => p.Id == newReservation.PacoteId);
            if (pacote == null)
            {
                return false; // Pacote não encontrado
            }

            // Regra: cliente não pode reservar o mesmo pacote mais de uma vez para a mesma data
            var reservaExistente = SimulatedReservations.Any(r =>
                r.ClienteNome.Equals(newReservation.ClienteNome, StringComparison.OrdinalIgnoreCase) &&
                r.PacoteId == newReservation.PacoteId &&
                r.DataReserva.Date == newReservation.DataReserva.Date);

            if (reservaExistente)
            {
                return false; // Reserva duplicada
            }

            // Regra: Apenas pacotes com data futura e vagas disponíveis podem ser reservados.
            if (!pacote.EhDataFutura || !pacote.TemVagasDisponiveis)
            {
                return false; // Pacote não disponível (data passada ou sem vagas)
            }

            SimulatedReservations.Add(newReservation);
            pacote.ParticipantesAtuais++; // Incrementa participantes

            return true;
        }
    }
}