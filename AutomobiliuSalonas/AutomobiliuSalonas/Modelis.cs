namespace AutomobiliuSalonas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Modelis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Modelis()
        {
            Automobilis = new HashSet<Automobilis>();
        }

        [Key]
        public int Nr { get; set; }

        [Required]
        [StringLength(254)]
        public string Pavadinimas { get; set; }

        public short Galia { get; set; }

        public short VietuSkaicius { get; set; }

        [Required]
        [StringLength(50)]
        public string Kuras { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Automobilis> Automobilis { get; set; }
    }
}
