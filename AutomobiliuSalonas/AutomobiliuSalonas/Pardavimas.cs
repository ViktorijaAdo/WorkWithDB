namespace AutomobiliuSalonas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pardavimas
    {
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Automobilis { get; set; }

        public int? Pardavejas { get; set; }

        public int Klientas { get; set; }

        public virtual Automobilis Automobilis1 { get; set; }

        public virtual Klientas Klientas1 { get; set; }

        public virtual Pardavejas Pardavejas1 { get; set; }
    }
}
