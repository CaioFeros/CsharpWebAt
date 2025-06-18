using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CsharpWebAt.Data;
using CsharpWebAt.Models;
using Microsoft.AspNetCore.Authorization;
namespace CsharpWebAt.Pages.Clientes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CsharpWebAt.Data.CsharpWebAtContext _context;

        public IndexModel(CsharpWebAt.Data.CsharpWebAtContext context)
        {
            _context = context;
        }

        public IList<Cliente> Cliente { get; set; } = default!;

        // Adiciona um parâmetro para mostrar clientes excluídos
        [FromQuery(Name = "showDeleted")]
        public bool ShowDeleted { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Clientes != null)
            {
                // Filtra clientes que não têm DeletedAt definido (ou seja, não foram logicamente excluídos)
                // Se ShowDeleted for true, mostra todos.
                if (ShowDeleted)
                {
                    Cliente = await _context.Clientes.ToListAsync();
                }
                else
                {
                    Cliente = await _context.Clientes
                        .Where(c => !c.DeletedAt.HasValue) // Filtra apenas clientes não excluídos logicamente
                        .ToListAsync();
                }
            }
        }
    }
}
