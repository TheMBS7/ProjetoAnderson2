using System.ComponentModel.DataAnnotations;

namespace ProjetoAnderson2.Models
{
    public class Contato
    {
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, insira um email válido.")]
        public string Email { get; set; } = default!;
        [MinLength(3)]
        public string Nome { get; set; } = default!;
        [MinLength(5)]
        public string Assunto { get; set; } = default!;
        [MinLength(10)]
        public string Mensagem { get; set; } = default!;
    }
}
