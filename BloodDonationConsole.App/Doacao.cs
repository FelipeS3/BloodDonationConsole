namespace BancoDoacaoDeSangue.App;

public class Doacao
{
    public int Id { get; set; }
    public int IdDoador { get; set; }
    public DateTime DataDoacao { get; set; } 
    public int QuantidadeMl { get; set; }
}