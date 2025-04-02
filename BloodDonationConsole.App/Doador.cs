namespace BancoDoacaoDeSangue.App;

public class Doador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Genero { get; set; }
    public double Peso { get; set; }
    public string TipoSanguineo { get; set; }
    public string FatorRh { get; set; }
    public List<Doacao> Doacoes { get; set; }
}