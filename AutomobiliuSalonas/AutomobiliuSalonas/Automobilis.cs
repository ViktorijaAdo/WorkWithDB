namespace AutomobiliuSalonas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Automobilis
    {
        [Key]
        public int Nr { get; set; }

        [Required]
        [StringLength(100)]
        public string Spalva { get; set; }

        public int Kaina { get; set; }

        public int Modelis { get; set; }

        public virtual Modelis Modelis1 { get; set; }

        public virtual Pardavimas Pardavimas { get; set; }
    }
}
