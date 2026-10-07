namespace DesafioComissoes.Models;

// Representa um registro do JSON: quem vendeu e por quanto.
// Usei decimal em vez de double porque estamos lidando com dinheiro,
// e double pode gerar erros de arredondamento (ex.: 0.1 + 0.2 != 0.3).
public record Venda(string Vendedor, decimal Valor);

// Representa o objeto raiz do arquivo, que tem a lista "vendas".
public record ArquivoVendas(List<Venda> Vendas);
