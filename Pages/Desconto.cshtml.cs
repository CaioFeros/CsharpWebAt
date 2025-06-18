using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsharpWebAt.Models;

namespace CsharpWebAt.Pages
{
    public class DescontoModel : PageModel
    {
        // --- Propriedades para o Delegate de Desconto---
        [BindProperty]
        public decimal PrecoDiaria { get; set; } // Usado para o c�lculo do desconto

        [BindProperty]
        public int NumeroDiasHospedagem { get; set; } // Usado para o c�lculo do desconto

        public decimal PrecoFinalComDesconto { get; set; } // Resultado do c�lculo do desconto

        // Delegate para calcular o desconto
        public CalculateDelegate CalculateDiscount;

        [BindProperty]
        public decimal ValorDiariaReserva { get; set; } // Valor da di�ria para o c�lculo da reserva

        [BindProperty]
        public int NumeroDiariasReserva { get; set; } // N�mero de di�rias para o c�lculo da reserva

        public decimal ValorTotalCalculado { get; set; } // Resultado do c�lculo do valor total da reserva

        // Func para calcular o valor total da reserva
        public Func<decimal, int, decimal> CalcularValorTotalReserva;

        public DescontoModel()
        {
            // Inicializa o delegate de desconto
            CalculateDiscount = (preco, dias) =>
            {
                if (dias >= 3)
                {
                    return preco * 0.90m; // 10% de desconto
                }
                return preco;
            };

            // Inicializa o Func para o c�lculo do valor total da reserva
            CalcularValorTotalReserva = (valorDiaria, numDiarias) => valorDiaria * numDiarias;
        }

        public void OnPostCalcularDesconto()
        {
            // Invoca o delegate para calcular o pre�o final com desconto
            PrecoFinalComDesconto = CalculateDiscount(PrecoDiaria, NumeroDiasHospedagem);
            // Certifica que os outros valores sejam resetados ou mantidos consistentes
            ValorTotalCalculado = 0; // Resetar para n�o confundir o usu�rio
        }

        public void OnPostCalcularReserva()
        {
            // Invoca o Func para calcular o valor total da reserva
            ValorTotalCalculado = CalcularValorTotalReserva(ValorDiariaReserva, NumeroDiariasReserva);
            // Certifica que os outros valores sejam resetados ou mantidos consistentes
            PrecoFinalComDesconto = 0;
        }
    }
}
