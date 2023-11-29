using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjetoAnderson2.Models
{
    public class Artista
    {
        public int Id { get; set; }
        [MinLength(3)]
        public string Nome { get; set; } = default!;

        [DataType(DataType.Date)]
        [DisplayName("Data de Nascimento")]
        public DateTime DataDeNascimento { get; set; }

        [DisplayName("País de Nascimento")]
        public string PaisDeNascimento { get; set; } = default!;

        [DisplayName("Foto")]
        public string Imagem { get; set; } = default!;
    }
}