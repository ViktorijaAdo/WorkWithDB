namespace AutomobiliuSalonas
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data;
    using System.Data.SqlClient;

    public class AutomobiliuSalonasDataTables
    {
        DataSet ds = new DataSet();
        public DataTable AutomobilisTable
        {
            get
            {
                DataTable dt = executeSelectStatement("select * from Automobilis");
                dt.TableName = "Automobilis";
                ds.Tables.Add(dt);
                return dt;
            }
        }

        public DataTable ModelisTable
        {
            get
            {
                DataTable dt = executeSelectStatement("select * from Modelis");
                dt.TableName = "Modelis";
                return dt;
            }
        }

        public DataTable KlientasTable
        {
            get
            {
                DataTable dt = executeSelectStatement("select * from Klientas");
                dt.TableName = "Klientas";
                return dt;
            }
        }

        public DataTable PardavejasTable
        {
            get
            {
                DataTable dt = executeSelectStatement("select * from Pardavejas");
                dt.TableName = "Pardavejas";
                return dt;
            }
        }

        public DataTable PardavimasTable
        {
            get
            {
                DataTable dt = executeSelectStatement("select * from Pardavimas");
                dt.TableName = "Pardavimas";
                return dt;
            }
        }
        public DataTable executeSelectStatement(string selectStatement)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(selectStatement, System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }

    public partial class AutomobiliuSalonasDataBase : DbContext
    {
        public AutomobiliuSalonasDataBase()
            : base("name=AutomobiliuSalonasDataBase")
        {
        }

        public virtual DbSet<Automobilis> Automobilis { get; set; }
        public virtual DbSet<Klientas> Klientas { get; set; }
        public virtual DbSet<Modelis> Modelis { get; set; }
        public virtual DbSet<Pardavejas> Pardavejas { get; set; }
        public virtual DbSet<Pardavimas> Pardavimas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Automobilis>()
                .Property(e => e.Spalva)
                .IsUnicode(false);

            modelBuilder.Entity<Automobilis>()
                .HasOptional(e => e.Pardavimas)
                .WithRequired(e => e.Automobilis1)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Klientas>()
                .Property(e => e.AK)
                .IsUnicode(false);

            modelBuilder.Entity<Klientas>()
                .Property(e => e.Vardas)
                .IsUnicode(false);

            modelBuilder.Entity<Klientas>()
                .Property(e => e.Pavarde)
                .IsUnicode(false);

            modelBuilder.Entity<Klientas>()
                .Property(e => e.TelNr)
                .IsUnicode(false);

            modelBuilder.Entity<Klientas>()
                .Property(e => e.Elpastas)
                .IsUnicode(false);

            modelBuilder.Entity<Klientas>()
                .HasMany(e => e.Pardavimas)
                .WithRequired(e => e.Klientas1)
                .HasForeignKey(e => e.Klientas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Modelis>()
                .Property(e => e.Pavadinimas)
                .IsUnicode(false);

            modelBuilder.Entity<Modelis>()
                .Property(e => e.Kuras)
                .IsUnicode(false);

            modelBuilder.Entity<Modelis>()
                .HasMany(e => e.Automobilis)
                .WithRequired(e => e.Modelis1)
                .HasForeignKey(e => e.Modelis)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pardavejas>()
                .Property(e => e.AK)
                .IsUnicode(false);

            modelBuilder.Entity<Pardavejas>()
                .Property(e => e.Vardas)
                .IsUnicode(false);

            modelBuilder.Entity<Pardavejas>()
                .Property(e => e.Pavarde)
                .IsUnicode(false);

            modelBuilder.Entity<Pardavejas>()
                .HasMany(e => e.Pardavimas)
                .WithOptional(e => e.Pardavejas1)
                .HasForeignKey(e => e.Pardavejas);

            modelBuilder.Entity<Pardavimas>()
                .Property(e => e.Data);
        }
    }
}
