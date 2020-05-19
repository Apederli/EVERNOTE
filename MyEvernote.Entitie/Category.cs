using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entitie
{
    [Table("Catagories")]
    public class Category : MyEntitiesBase
    {
        [DisplayName("Kategori İsmi") ,Required(ErrorMessage ="Boş Geçilemez"), StringLength(50)]
        public string Title { get; set; }

        [DisplayName("Açıklama"),StringLength(150)]
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
