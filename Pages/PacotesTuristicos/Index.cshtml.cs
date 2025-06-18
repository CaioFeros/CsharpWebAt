using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CsharpWebAt.Data; // Para acessar os dados simulados

namespace CsharpWebAt.Pages.PacotesTuristicos
{
    public class IndexModel : PageModel
    {
        public List<SimulatedPackage> PacotesTuristicos { get; set; }

        public void OnGet()
        {
            PacotesTuristicos = DataSource.SimulatedPackages;
        }
    }
}