using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoAnderson2.Models
{
    public class Filme
    {
        public int Id { get; set; }

        [DisplayName("Código de Barras")]
        public string CodigoDeBarras { get; set; } = default!;
        [DisplayName("Título")]
        public string Titulo { get; set; } = default!;

        [DisplayName("Genêro")]
        public int GeneroId { get; set; }
        [DisplayName("Genêro")]
        public Genero Genero { get; set; } = default!;
        [DisplayName("Ano de Lançamento")]
        public int Ano { get; set; }

        [DisplayName("Midia")]
        public string Tipo { get; set; } = default!; // DVD - BLURAY
        
        [DisplayName("Preço")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Preco { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data Adquirida")]
        public DateTime DataAdquirida { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Valor de Custo")]
        public float ValorCusto { get; set; }

        [DisplayName("Situação")]
        public int SituacaoId { get; set; }

        [DisplayName("Situação")]
        public Situacao Situacao { get; set; } = default!;

        [DisplayName("Artista")]
        public int ArtistaId { get; set; }
        public Artista Artista { get; set; } = default!;
        public string Diretor { get; set; } = default!;

        [DisplayName("Capa do Filme")]
        public string Imagem { get; set; } = default!;
    }
}