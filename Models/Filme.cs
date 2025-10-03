using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArthurTavares.Models
{
    public class Filme
    {
        [Key]
        public int Id_filme { get; set; }
        public string Titulo_filme { get; set; }
        public string Descricao_filme { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly Lancamento_filme { get; set; }


        public int Id_usuario { get; set; }
        [ForeignKey("Id_usuario")]
        public Usuario Usuario { get; set; }
    }
}
