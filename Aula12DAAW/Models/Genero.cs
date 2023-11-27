using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoAnderson2.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; } = default!;

        public override string ToString()
        {
            return Nome;
        }
    }
}