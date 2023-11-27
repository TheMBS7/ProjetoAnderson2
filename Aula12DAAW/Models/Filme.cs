namespace ProjetoAnderson2.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string CodigoDeBarras { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public int GeneroId { get; set; }
        public Genero Genero { get; set; } = default!;
        public int Ano { get; set; }
        public string Tipo { get; set; } = default!; // DVD - BLURAY
        public decimal Preco { get; set; }
        public DateTime DataAdquirida { get; set; }
        public float ValorCusto { get; set; }
        public int SituacaoId { get; set; }
        public Situacao Situacao { get; set; } = default!;
        public int ArtistaId { get; set; }
        public Artista Artista { get; set; } = default!;
        public string Diretor { get; set; } = default!;
        public string Imagem { get; set; } = default!;
    }
}