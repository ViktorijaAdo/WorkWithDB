namespace AutomobiliuSalonas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Klientas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Klientas()
        {
            Pardavimas = new HashSet<Pardavimas>();
        }

        [Key]
        public int Nr { get; set; }

        [Required]
        [StringLength(20)]
        public string AK { get; set; }

        [Required]
        [StringLength(50)]
        public string Vardas { get; set; }

        [Required]
        [StringLength(50)]
        public string Pavarde { get; set; }

        [StringLength(10)]
        public string TelNr { get; set; }

        [StringLength(254)]
        public string Elpastas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pardavimas> Pardavimas { get; set; }
    }
}
