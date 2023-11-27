using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjetoAnderson2.Models
{
    public class PesquisaArtista
    {
        public List<Artista>? Artistas { get; set; }
        public SelectList? PaisDeNascimento2 { get; set; }
        public string? PaisSelecionado { get; set; }
        public string? SearchString { get; set; }
    }
}