
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using CsharpWebAt.Data; // Para acessar os dados simulados

namespace CsharpWebAt.Pages.PacotesTuristicos
{
    public class DetailsModel : PageModel
    {
        public SimulatedPackage Pacote { get; set; }

        public IActionResult OnGet(int id) // 'id' será preenchido do parâmetro de rota
        {
            // Busca o pacote pelo ID no DataSource simulado
            Pacote = DataSource.SimulatedPackages.FirstOrDefault(p => p.Id == id);

            if (Pacote == null)
            {
                // Se o pacote não for encontrado, redireciona para a página de erro ou lista
                return NotFound(); // Retorna um status 404 Not Found
                // Ou return RedirectToPage("/PacotesTuristicos/Index");
            }

            return Page(); // Exibe a página com os detalhes do pacote
        }
    }
}