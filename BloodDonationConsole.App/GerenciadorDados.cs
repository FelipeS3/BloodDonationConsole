using System.Text.Json;

namespace BancoDoacaoDeSangue.App;

public class GerenciadorDados
{
    static List<Doador> doadores = new List<Doador>();
    static List<Doacao> doacoes = new List<Doacao>();
    static List<EstoqueDeSangue> estoquesDeSangue = new List<EstoqueDeSangue>();

    private static int DoadorId = 1;
    private static int DoacoesId = 1;
    private static int EstoqueId = 1;


    public static void CadastrarDoador()
    {
        Console.Clear();
        Console.WriteLine("Por favor informe seu nome: ");
        string nome = Console.ReadLine();

        Console.WriteLine("Insira seu Email: ");
        string email = Console.ReadLine();
        if (doadores.Any(d => d.Email == email))
        {
            Console.WriteLine("Já existe um doador com esse email");
            return;
        }


        Console.WriteLine("Data de Nascimento: (dd/mm/aaaa)");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataNascimento))
        {
            Console.WriteLine("Data em formato inválido.");
            return;
        }

        Console.WriteLine("Informe seu gênero: (M/F)");
        string genero = Console.ReadLine();

        Console.WriteLine("Informe seu peso: ");
        if (!double.TryParse(Console.ReadLine(), out double peso) || peso < 50)
        {
            Console.WriteLine("Peso inválido, mínimo de 50kg.");
            return;
        }

        Console.WriteLine("Informe seu tipo sanguineo");
        string tipoSanguineo = Console.ReadLine();

        Console.WriteLine("Fator RH: ");
        string fatorRH = Console.ReadLine();

        var doador = new Doador
        {
            Id = DoadorId++,
            Nome = nome,
            Email = email,
            DataNascimento = dataNascimento,
            Genero = genero,
            Peso = peso,
            TipoSanguineo = tipoSanguineo,
            FatorRh = fatorRH,
            Doacoes = new List<Doacao>()
        };

        doadores.Add(doador);
        Console.WriteLine("Doador cadastrado com sucesso!");
    }
    public static void RegistrarDoacao()
    {
        Console.Clear();
        Console.WriteLine("Insira seu email: ");
        string email = Console.ReadLine();
        var doador = doadores.FirstOrDefault(x => x.Email == email);

        if (doador is null)
        {
            Console.WriteLine("Doador não encontrado");
            return;
        }

        Console.WriteLine("Quantidade de ML doada: ");
        if (!int.TryParse(Console.ReadLine(), out int quantidadeML))
        {
            Console.WriteLine("Erro: ao adicionar Ml");
            return;
        }

        if (quantidadeML < 420 || quantidadeML > 470)
        {
            Console.WriteLine("Quantidade deve estar entre 420 e 470.");
            return;
        }

        var ultimaDoacao = doador.Doacoes.OrderByDescending(x => x.DataDoacao).FirstOrDefault();

        if (ultimaDoacao != null)
        {
            TimeSpan data = DateTime.Now - ultimaDoacao.DataDoacao;
            if (doador.Genero == "F" && data.TotalDays < 90)
            {
                Console.WriteLine("Doadoras só poderão doar com um intervalo de 90 dias");
                return;
            }
            else if (doador.Genero == "M" && data.TotalDays < 60)
            {
                Console.WriteLine("Doadores só poderão doar com um intervalo de 60 dias");
                return;
            }
        }

        var doacao = new Doacao
        {
            Id = DoacoesId++,
            IdDoador = doador.Id,
            DataDoacao = DateTime.Now,
            QuantidadeMl = quantidadeML
        };

        doador.Doacoes.Add(doacao);

        Console.WriteLine("Doação cadastrada com sucesso.");

        var estoque = estoquesDeSangue.FirstOrDefault(x =>
            x.TipoSanguineo == doador.TipoSanguineo && x.FatorRh == doador.FatorRh);

        if (estoque == null)
        {
            var novoEstoque = new EstoqueDeSangue
            {
                Id = EstoqueId++,
                TipoSanguineo = doador.TipoSanguineo,
                FatorRh = doador.FatorRh,
                QuantidadeMl = doacao.QuantidadeMl
            };

            estoquesDeSangue.Add(novoEstoque);
        }
        else
        {
            estoque.QuantidadeMl += quantidadeML;
        }
    }
    public static void ConsultarDoador()
    {
        Console.Clear();
        Console.WriteLine("Digite o email do doador: ");
        string email = Console.ReadLine();

        var doador = doadores.FirstOrDefault(x => x.Email == email);

        if (doador is null)
        {
            Console.WriteLine("Doador não encontrado.");
            return;
        }

        Console.WriteLine(
            $"Nome: {doador.Nome} \n Tipo sanguineo: {doador.TipoSanguineo} \n Fator RH: {doador.FatorRh}");

        if (doador.Doacoes.Any())
        {
            foreach (var doacao in doador.Doacoes)
            {
                Console.WriteLine($"Data Doação: {doacao.DataDoacao}\n Quantidade: {doacao.QuantidadeMl}ml");
            }
        }
        else
        {
            Console.WriteLine("Nenhuma doação registrada.");
        }
    }
    public static void ConsultarEstoque()
    {
        Console.Clear();
        if (!estoquesDeSangue.Any())
        {
            Console.WriteLine("Nenhum estoque disponível");
            return;
        }

        foreach (var estoque in estoquesDeSangue)
        {
            Console.WriteLine($"Tipos: {estoque.TipoSanguineo}{estoque.FatorRh}, {estoque.QuantidadeMl}ml");
        }

    }
    public static void GerarRelatorios()
    {
        Console.Clear();
        double totalDoacoes = doadores.Sum(d => d.Doacoes.Count);
        double quantidadeML = doadores.Sum(d => d.Doacoes.Sum(doacao => doacao.QuantidadeMl));

        if (totalDoacoes == 0)
        {
            Console.WriteLine("Nenhuma doação registrada.");
            return;
        }


        double mediaML = quantidadeML / totalDoacoes;

        Console.WriteLine($"Total de doadores cadastrados: {doadores.Count} \n Total de doações realizadas: {totalDoacoes} \n Média de sangue doado por doação: {mediaML}ml");
    }
    public static void ListarDoadores()
    {
        Console.Clear();
        if (doadores.Count == 0)
        {
            Console.WriteLine("Nenhum doador cadastrado.");
            return;
        }

        Console.WriteLine($"Lista de Doadores:");

        foreach (var doador in doadores)
        {
            Console.WriteLine($"{doador.Nome} | {doador.Email}");
        }
    }
    public static void SalvarDados()
    {
        File.WriteAllText("doadores.json", JsonSerializer.Serialize(doadores));
        File.WriteAllText("estoque.json", JsonSerializer.Serialize(estoquesDeSangue));
    }
    public static void CarregarDados()
    {
        if (File.Exists("doadores.json"))
        {
            doadores = JsonSerializer.Deserialize<List<Doador>>(File.ReadAllText("doadores.json"));
        }
        if (File.Exists("estoque.json"))
        {
            estoquesDeSangue = JsonSerializer.Deserialize<List<EstoqueDeSangue>>(File.ReadAllText("estoque.json"));
        }
    }
}