using System;

namespace CsharpWebAt.Models
{
    /// <summary>
    /// Delegate para calcular o valor de um item com base em um preço e número de dias.
    /// </summary>
    /// <param name="precoDiaria">O preço original da diária.</param>
    /// <param name="numeroDiasHospedagem">O número de dias de hospedagem.</param>
    /// <returns>O preço com o desconto aplicado, se aplicável.</returns>
    public delegate decimal CalculateDelegate(decimal precoDiaria, int numeroDiasHospedagem);
}