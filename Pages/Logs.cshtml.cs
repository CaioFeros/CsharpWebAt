using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using CsharpWebAt.Services;           

namespace CsharpWebAt.Pages
{
    public class LogsModel : PageModel
    {
        private readonly LogService _logService;

        public List<string> CurrentMemoryLogs { get; set; } // Para exibir os logs em memória na view

        public LogsModel(LogService logService)
        {
            _logService = logService;
            // Removemos a declaração do delegate LogOperation e CapacityReached
            // e suas subscrições daqui, pois a lógica de log é agora direta via _logService.
        }

        public void OnGet()
        {
            // Apenas carrega os logs existentes em memória para exibição
            CurrentMemoryLogs = _logService.MemoryLogs;
        }
    }
}
