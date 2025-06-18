using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CsharpWebAt.Data;
using CsharpWebAt.Models;

namespace CsharpWebAt.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly CsharpWebAt.Data.CsharpWebAtContext _context;

        public DeleteModel(CsharpWebAt.Data.CsharpWebAtContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);

            if (Cliente == null)
            {
                return NotFound();
            }
            // Não deve permitir exclusão de um cliente já logicamente excluído na interface
            if (Cliente.DeletedAt.HasValue)
            {
                return RedirectToPage("./Index"); // Ou exibir uma mensagem de erro
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cliente = await _context.Clientes.FindAsync(id);

            if (Cliente != null)
            {
                // Implementação da Exclusão Lógica
                Cliente.DeletedAt = DateTime.Now; // Define a data e hora da exclusão lógica
                _context.Clientes.Update(Cliente); // Marca a entidade como modificada
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            }

            return RedirectToPage("./Index");
        }
    }
}
