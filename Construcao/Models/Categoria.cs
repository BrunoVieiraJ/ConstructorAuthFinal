using System.ComponentModel.DataAnnotations;

namespace Construcao.Models
{
    public class Categoria
    {
        [Key]
        public long? CategoriasId { get; set; }

        [Required]
        public string Cat_Nome { get; set; }

        [Required]
        public string Cat_Desc {  get; set; }

        [Required]
        public string Prioridade { get; set; }

        [Required]
        public string Responsavel { get; set; }


        public IList<Produto>? Produtos { get; set; }
    }
}
