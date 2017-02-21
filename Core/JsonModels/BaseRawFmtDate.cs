using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonModels
{
    public class BaseRawFmtDate
    {
        [Column(TypeName = "INT")]
        public int raw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string fmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string longFmt { get; set; }
    }
}
