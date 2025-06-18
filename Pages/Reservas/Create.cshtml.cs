using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using CsharpWebAt.Services;
using CsharpWebAt.Data;

namespace CsharpWebAt.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly LogService _logService;

        // Propriedades para entrada do usuário
        [BindProperty]
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome do cliente deve ter pelo menos 3 caracteres.")]
        public string NomeClienteInput { get; set; }

        [BindProperty]
        public string TituloPacoteInput { get; set; }

        [BindProperty]
        public DateTime DataReservaInput { get; set; } = DateTime.Today.AddDays(7); // Data futura padrão

        public string StatusMessage { get; set; }

        public CreateModel(LogService logService)
        {
            _logService = logService;
        }

        public void OnGet()
        {
            StatusMessage = "Preencha os dados para criar uma nova reserva.";
        }

        public IActionResult OnPostCreateReservation()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Por favor, corrija os erros no formulário.";
                string validationErrorMessage = "Falha na validação de entrada: " +
                    string.Join("; ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
                _logService.LogToConsole(validationErrorMessage);
                _logService.LogToFile(validationErrorMessage);
                _logService.LogToMemory(validationErrorMessage);
                return Page();
            }

            string logMessage;
            StatusMessage = ""; // Limpa mensagens anteriores

            if (string.IsNullOrWhiteSpace(TituloPacoteInput))
            {
                StatusMessage = "Por favor, selecione um pacote turístico.";
                logMessage = $"Tentativa de reserva falhou (Pacote não selecionado): Cliente: '{NomeClienteInput}'.";
                _logService.LogToConsole(logMessage);
                _logService.LogToFile(logMessage);
                _logService.LogToMemory(logMessage);
                return Page();
            }

            // 1. Encontrar o pacote simulado usando o DataSource centralizado
            var pacote = DataSource.SimulatedPackages.FirstOrDefault(p => p.Titulo.Equals(TituloPacoteInput, StringComparison.OrdinalIgnoreCase));

            if (pacote == null)
            {
                StatusMessage = $"Pacote '{TituloPacoteInput}' não encontrado.";
                logMessage = $"Tentativa de reserva falhou (Pacote não encontrado): Cliente: '{NomeClienteInput}', Pacote: '{TituloPacoteInput}'.";
                _logService.LogToConsole(logMessage);
                _logService.LogToFile(logMessage);
                _logService.LogToMemory(logMessage);
                return Page();
            }

            // As validações de regras de negócio agora são feitas no DataSource.TryAddReservation
            var novaReserva = new SimulatedReservation
            {
                ClienteId = DataSource.SimulatedReservations.Count + 1, // ID simulado
                ClienteNome = NomeClienteInput,
                PacoteId = pacote.Id,
                PacoteTitulo = pacote.Titulo,
                DataReserva = DataReservaInput
            };

            // Tenta adicionar a reserva e verifica as regras de negócio via DataSource
            if (DataSource.TryAddReservation(novaReserva))
            {
                logMessage = $"Nova reserva criada com sucesso! Cliente: '{NomeClienteInput}', Pacote: '{pacote.Titulo}', Data da Reserva: {DataReservaInput:dd/MM/yyyy}. Vagas restantes: {pacote.CapacidadeMaxima - pacote.ParticipantesAtuais}.";
                StatusMessage = "Reserva criada com sucesso!";
                _logService.LogToConsole(logMessage);
                _logService.LogToFile(logMessage);
                _logService.LogToMemory(logMessage);

                // ALERTA: Dispara a notificação de capacidade atingida/excedida via LogService
                if (pacote.ParticipantesAtuais >= pacote.CapacidadeMaxima)
                {
                    string capacityAlertMessage = $"Pacote '{pacote.Titulo}' ATINGIU OU EXCEDEU A CAPACIDADE MÁXIMA de {pacote.CapacidadeMaxima} participantes. Total atual: {pacote.ParticipantesAtuais}.";
                    _logService.LogToConsole($"[ALERTA - CAPACIDADE ATINGIDA] {capacityAlertMessage}");
                    _logService.LogToFile($"[ALERTA - CAPACIDADE ATINGIDA] {capacityAlertMessage}");
                    _logService.LogToMemory($"[ALERTA - CAPACIDADE ATINGIDA] {capacityAlertMessage}");
                }
                NomeClienteInput = string.Empty; // Limpa o campo após sucesso
            }
            else
            {
                // Se TryAddReservation retornar false, é porque uma regra de negócio foi violada
                if (!pacote.EhDataFutura)
                {
                    StatusMessage = $"Pacote '{pacote.Titulo}' não pode ser reservado. A data de início ({pacote.DataInicio:dd/MM/yyyy}) não é futura.";
                    logMessage = $"Tentativa de reserva falhou (Data passada): Cliente: '{NomeClienteInput}', Pacote: '{TituloPacoteInput}'.";
                }
                else if (!pacote.TemVagasDisponiveis)
                {
                    StatusMessage = $"Pacote '{pacote.Titulo}' não pode ser reservado. Capacidade máxima atingida ({pacote.ParticipantesAtuais}/{pacote.CapacidadeMaxima}).";
                    logMessage = $"Tentativa de reserva falhou (Capacidade máxima atingida): Cliente: '{NomeClienteInput}', Pacote: '{TituloPacoteInput}'.";
                }
                else
                {
                    // Assume que a falha é por duplicação de reserva para a mesma data, se as outras validações já passaram antes de TryAddReservation
                    StatusMessage = $"O cliente '{NomeClienteInput}' já possui uma reserva para o pacote '{pacote.Titulo}' na data {DataReservaInput:dd/MM/yyyy}.";
                    logMessage = $"Tentativa de reserva falhou (Reserva duplicada): Cliente: '{NomeClienteInput}', Pacote: '{TituloPacoteInput}', Data: {DataReservaInput:dd/MM/yyyy}.";
                }
                _logService.LogToConsole(logMessage);
                _logService.LogToFile(logMessage);
                _logService.LogToMemory(logMessage);
            }

            return Page();
        }
    }
}
