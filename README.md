# Desafio Técnico – Cálculo de Comissões

Programa em C# (.NET 8 ou 10) que vai ler um JSON de vendas e calcular a comissão de cada vendedor de acordo com as regras estabelecidas.

## Regra de comissão

- **Abaixo de R$ 100,00:** Não gera comissão.
- **De R$ 100,00 até R$ 499,99:** Gera 1% de comissão.
- **A partir de R$ 500,00:** Gera 5% de comissão.

## Como executar

Pré-requisito: [.NET SDK 8](https://dotnet.microsoft.com/download) ou superior.

```bash
dotnet run
```

Para usar outro arquivo de vendas:

```bash
dotnet run -- caminho/do/arquivo.json
```

## Estrutura

```
Models/
  Venda.cs                 registro lido do JSON (vendedor e valor)
  ResumoVendedor.cs        resultado por vendedor
Services/
  CalculadoraComissao.cs   regra de negócio das comissões
Program.cs                 leitura do arquivo, chamada do cálculo e exibição
vendas.json                dados de entrada do desafio
```

## Decisões

- **`decimal` em vez de `double`:** é uma boa prática para mexer com dinheiro, evita a perda de precisão nos centavos causada por ponto flutuante.
- **Regra isolada em `CalculadoraComissao`:** para facilitar testes e alterar as faixas sem mexer na leitura/saída.
- **Arredondamento por venda:** a comissão de cada venda é arredondada para centavos antes de somar, como aconteceria na realidade.
- **Faixas:** os limites de R$ 100,00 e R$ 500,00 usam `<`, então R$ 100,00 já gera 1% e R$ 500,00 já gera 5%.

## Resultado esperado com o `vendas.json` do desafio

| Vendedor        | Vendas | Total vendido | Comissão |
|-----------------|-------:|--------------:|---------:|
| Ana Lima        |      9 |     8.763,95  |  404,99  |
| Carlos Oliveira |      8 |     7.928,35  |  379,38  |
| João Silva      |     10 |    10.754,70  |  495,69  |
| Maria Souza     |      9 |     9.874,30  |  465,96  |
| **Total**       | **36** | **37.321,30** | **1.746,02** |
