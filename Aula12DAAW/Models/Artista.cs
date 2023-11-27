namespace ProjetoAnderson2.Models
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nome { get; set; } = default!;
        public DateTime DataDeNascimento { get; set; }
        public string PaisDeNascimento { get; set; } = default!;
        public string Imagem { get; set; } = default!;
    }
}