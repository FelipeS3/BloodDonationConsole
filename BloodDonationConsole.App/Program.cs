using System.Text.Json;

namespace BancoDoacaoDeSangue.App
{

    class Program
    {
        static void Main(string[] args)
        {
            GerenciadorDados.CarregarDados(); 
            while (true)
            {
                Console.WriteLine("Bem vindo ao programa.");
                Console.WriteLine("Escolha uma opção");
                Console.WriteLine("1 - Cadastrar Doadores");
                Console.WriteLine("2 - Registrar Doação");
                Console.WriteLine("3 - Consultar Doador");
                Console.WriteLine("4 - Consultar Estoque");
                Console.WriteLine("5 - Gerar Relatorios");
                Console.WriteLine("6 - Listar Doadores");
                Console.WriteLine("0 - Sair");

                if (!int.TryParse(Console.ReadLine(), out int opcao))
                {
                    Console.WriteLine("Opção inválida.");
                    continue;
                }

                switch (opcao) 
                {
                    case 1: GerenciadorDados.CadastrarDoador(); break;
                    case 2: GerenciadorDados.RegistrarDoacao(); break;
                    case 3: GerenciadorDados.ConsultarDoador(); break;
                    case 4: GerenciadorDados.ConsultarEstoque(); break;
                    case 5: GerenciadorDados.GerarRelatorios(); break;
                    case 6: GerenciadorDados.ListarDoadores(); break;
                    case 0: GerenciadorDados.SalvarDados(); Environment.Exit(0); break;

                }
            }
        }
    }
}