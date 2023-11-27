using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProjetoAnderson2.Models
{
    public class MovieGenreViewModel
    {
        public List<Filme>? Filmes { get; set; }
        public SelectList? Genres { get; set; }
        public string? MovieGenre { get; set; }
        public string? Title { get; set; }
    }
}