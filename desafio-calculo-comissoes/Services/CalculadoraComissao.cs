using DesafioComissoes.Models;

namespace DesafioComissoes.Services;

// Concentra a regra de negócio das comissões.
// Fica separada do Program.cs para que a regra possa ser testada e alterada
// sem mexer na leitura do arquivo nem na exibição do resultado.
public static class CalculadoraComissao
{
    // Valores da regra do desafio, nomeados para evitar "números mágicos" no código.
    private const decimal LimiteSemComissao = 100m;      // abaixo disso: sem comissão
    private const decimal LimiteComissaoReduzida = 500m; // abaixo disso: 1%; a partir daqui: 5%
    private const decimal TaxaReduzida = 0.01m;
    private const decimal TaxaPadrao = 0.05m;

    // Calcula a comissão de UMA venda, aplicando a regra por faixa de valor.
    public static decimal CalcularComissaoDaVenda(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor da venda não pode ser negativo.");

        // Faixa 1: menor que R$ 100,00 -> não gera comissão.
        if (valor < LimiteSemComissao)
            return 0m;

        // Faixa 2: de R$ 100,00 até R$ 499,99 -> 1%.
        // Faixa 3: a partir de R$ 500,00 -> 5%.
        var taxa = valor < LimiteComissaoReduzida ? TaxaReduzida : TaxaPadrao;

        // Arredonda para centavos venda a venda, como seria pago na prática.
        return Math.Round(valor * taxa, 2, MidpointRounding.AwayFromZero);
    }

    // Agrupa as vendas por vendedor e soma o total vendido e a comissão devida.
    public static IReadOnlyList<ResumoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(v => v.Vendedor)
            .Select(grupo => new ResumoVendedor(
                Vendedor: grupo.Key,
                QuantidadeVendas: grupo.Count(),
                TotalVendido: grupo.Sum(v => v.Valor),
                TotalComissao: grupo.Sum(v => CalcularComissaoDaVenda(v.Valor))))
            .OrderBy(r => r.Vendedor)
            .ToList();
    }
}
