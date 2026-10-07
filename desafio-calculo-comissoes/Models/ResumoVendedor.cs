namespace DesafioComissoes.Models;

// Resultado de um vendedor que será ser exibido
public record ResumoVendedor(
    string Vendedor,
    int QuantidadeVendas,
    decimal TotalVendido,
    decimal TotalComissao);
