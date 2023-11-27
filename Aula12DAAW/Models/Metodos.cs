namespace ProjetoAnderson2.Models
{
    public class Metodos

    {
        private string _filePath;

        //valida imagem
        public bool ValidaImagem(IFormFile anexo)
        {
            switch (anexo.ContentType)
            {
                case "image/jpeg":
                    return true;

                case "image/bmp":
                    return true;

                case "image/gif":
                    return true;

                case "image/png":
                    return true;

                default:
                    return false;
                    break;
            }
        }

        //salva imagem
        public string SalvarArquivo(IFormFile anexo)
        {
            var nome = Guid.NewGuid().ToString() + anexo.FileName;

            var filePath = _filePath + "\\fotos";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using (var stream = System.IO.File.Create(filePath + "\\" + nome))
            {
                anexo.CopyToAsync(stream);
            }

            return nome;
        }
    }
}