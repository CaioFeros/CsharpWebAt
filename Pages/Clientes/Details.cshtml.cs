using CsharpWebAt.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    
namespace CsharpWebAt.Pages.Clientes;
public class DetailsModel : PageModel
{
    private readonly CsharpWebAt.Data.CsharpWebAtContext _context;

    public DetailsModel(CsharpWebAt.Data.CsharpWebAtContext context)
    {
        _context = context;
    }

    public Cliente Cliente { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Carrega o Cliente E suas Reservas associadas, E o PacoteTuristico de cada reserva
        Cliente = await _context.Clientes
            .Include(c => c.Reservas) // Inclui a coleção de Reservas do Cliente
                .ThenInclude(r => r.PacoteTuristico) // Inclui o PacoteTuristico para cada Reserva
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Cliente == null)
        {
            return NotFound();
        }
        return Page();
    }
}