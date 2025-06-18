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

        public List<string> CurrentMemoryLogs { get; set; } // Para exibir os logs em mem�ria na view

        public LogsModel(LogService logService)
        {
            _logService = logService;
            // Removemos a declara��o do delegate LogOperation e CapacityReached
            // e suas subscri��es daqui, pois a l�gica de log � agora direta via _logService.
        }

        public void OnGet()
        {
            // Apenas carrega os logs existentes em mem�ria para exibi��o
            CurrentMemoryLogs = _logService.MemoryLogs;
        }
    }
}
