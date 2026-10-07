using System.Globalization;
using System.Text.Json;
using DesafioComissoes.Models;
using DesafioComissoes.Services;

// Aqui utiliza o formato brasileiro (R$ 1.234,56) na saída, independente da configuração da máquina
var cultura = new CultureInfo("pt-BR");

// Por padrão lê o vendas.json que fica ao lado do executável
// Também dá para informar outro caminho: dotnet run -- caminho/do/arquivo.json
var caminhoArquivo = args.Length > 0
    ? args[0]
    : Path.Combine(AppContext.BaseDirectory, "vendas.json");

try
{
    // Leitura do arquivo
    var json = File.ReadAllText(caminhoArquivo);

    // O JSON usa nomes em minúsculo ("vendedor", "valor"), enquanto as propriedades
    // em C# começam com maiúscula. Ignorar maiúsculas/minúsculas resolve o mapeamento.
    var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var arquivo = JsonSerializer.Deserialize<ArquivoVendas>(json, opcoes);

    if (arquivo?.Vendas is null || arquivo.Vendas.Count == 0)
    {
        Console.WriteLine("Nenhuma venda foi encontrada no arquivo.");
        return 1;
    }

    // Cálculo das comissões por vendedor
    var resumos = CalculadoraComissao.CalcularPorVendedor(arquivo.Vendas);

    // Aqui a exibição do resultado
    Console.WriteLine("COMISSÃO POR VENDEDOR");
    Console.WriteLine(new string('-', 62));
    Console.WriteLine($"{"Vendedor",-18}{"Vendas",7}{"Total vendido",20}{"Comissão",17}");
    Console.WriteLine(new string('-', 62));

    foreach (var r in resumos)
    {
        Console.WriteLine(string.Format(cultura,
            "{0,-18}{1,7}{2,20:C}{3,17:C}",
            r.Vendedor, r.QuantidadeVendas, r.TotalVendido, r.TotalComissao));
    }

    Console.WriteLine(new string('-', 62));
    Console.WriteLine(string.Format(cultura,
        "{0,-18}{1,7}{2,20:C}{3,17:C}",
        "TOTAL",
        resumos.Sum(r => r.QuantidadeVendas),
        resumos.Sum(r => r.TotalVendido),
        resumos.Sum(r => r.TotalComissao)));

    return 0;
}
catch (FileNotFoundException)
{
    Console.Error.WriteLine($"Arquivo não foi encontrado: {caminhoArquivo}");
    return 1;
}
catch (JsonException ex)
{
    Console.Error.WriteLine($"O arquivo JSON está em um formato inválido: {ex.Message}");
    return 1;
}
