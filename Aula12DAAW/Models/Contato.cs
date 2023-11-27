namespace ProjetoAnderson2.Models
{
    public class Contato
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Nome { get; set; } = default!;
        public string Assunto { get; set; } = default!;
        public string Mensagem { get; set; } = default!;
    }
}
