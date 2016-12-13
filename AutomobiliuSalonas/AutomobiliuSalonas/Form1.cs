using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AutomobiliuSalonas
{
    public partial class Form1 : Form
    {
        private AutomobiliuSalonasDataBase dataBase = new AutomobiliuSalonasDataBase();
        private AutomobiliuSalonasDataTables dataTables = new AutomobiliuSalonasDataTables();
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rezult = (string)m_tableSelectComboBox.SelectedItem;
            DataTable table;
            switch (rezult)
            {
                case "Modelis":
                    table = dataTables.ModelisTable;
                    break;
                case "Automobilis":
                    table = dataTables.AutomobilisTable;
                    break;
                case "Pardavejas":
                    table = dataTables.PardavejasTable;
                    break;
                case "Klientas":
                    table = dataTables.KlientasTable;
                    break;
                case "Pardavimas":
                    table = dataTables.PardavimasTable;
                    break;
                default:
                    table = null;
                    break;
            }
            if (table != null)
                m_tableDataGridView.DataSource = table;
        }

        private void m_modelisSaveButton_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from Modelis", System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
            SqlCommand inccmd = new SqlCommand();
            inccmd.CommandText = "insert into modelis(Pavadinimas, Galia, VietuSkaicius, Kuras) values(@Pavadinimas, @Galia, @VietuSkaicius, @Kuras)";
            inccmd.Parameters.Add("@Pavadinimas", SqlDbType.VarChar).SourceColumn = "Pavadinimas";
            inccmd.Parameters.Add("@Galia", SqlDbType.SmallInt).SourceColumn = "Galia";
            inccmd.Parameters.Add("@VietuSkaicius", SqlDbType.SmallInt).SourceColumn = "VietuSkaicius";
            inccmd.Parameters.Add("@Kuras", SqlDbType.VarChar).SourceColumn = "Kuras";
            inccmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
            adapter.InsertCommand = inccmd;
            DataTable table = new DataTable();
            adapter.Fill(table);
            DataRow row = table.NewRow();
            row["Pavadinimas"] = m_modelioNameTextBox.Text;
            row["Galia"] = short.Parse(m_modelioGalisMaskedBox.Text);
            row["VietuSkaicius"] = short.Parse(m_vietosMaskedBox.Text);
            row["Kuras"] = (string)m_kurasComboBox.SelectedItem;
            table.Rows.Add(row);
            adapter.Update(table);
            m_modelioNameTextBox.Clear();
            m_modelioGalisMaskedBox.Clear();
            m_vietosMaskedBox.Clear();
        }

        private void m_autoSaveButton_Click(object sender, EventArgs e)
        {
            Automobilis naujasAutomobilis = new Automobilis();
            naujasAutomobilis.Modelis = (int) m_autoModelisComboBox.SelectedValue;
            naujasAutomobilis.Kaina = int.Parse(m_kainaMaskedBox.Text);
            naujasAutomobilis.Spalva = m_spalvaTextBox.Text;
            dataBase.Automobilis.Add(naujasAutomobilis);
            dataBase.SaveChanges();
            m_kainaMaskedBox.Clear();
            m_spalvaTextBox.Clear();
        }

        private void m_pardavejasSaveButton_Click(object sender, EventArgs e)
        {
            Pardavejas naujasPardavejas = new Pardavejas();
            naujasPardavejas.Vardas = m_pardavejoVardasTextBox.Text;
            naujasPardavejas.Pavarde = m_pasdavejasPavardeTextBox.Text;
            naujasPardavejas.AK = m_AKTextBox.Text;
            dataBase.Pardavejas.Add(naujasPardavejas);
            dataBase.SaveChanges();
            m_pardavejoVardasTextBox.Clear();
            m_pasdavejasPavardeTextBox.Clear();
            m_AKTextBox.Clear();
        }

        private void m_autoModelisComboBox_Click(object sender, EventArgs e)
        {
            m_autoModelisComboBox.DataSource = new string[]{ "Loading..."};
            m_autoModelisComboBox.DataSource = dataBase.Modelis.ToList();
            m_autoModelisComboBox.ValueMember = "Nr";
            m_autoModelisComboBox.DisplayMember = "Pavadinimas";
        }

        private void m_klientoAKOKButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                m_klientoAKOKButton.PerformClick();
            }
        }

        private void m_klientoAKOKButton_Click(object sender, EventArgs e)
        {
            Klientas klientas = dataBase.Klientas.Where(b => b.AK == m_klientoAKTextBox.Text).SingleOrDefault();
            if(klientas != null)
            {
                m_klientoVardasTextBox.Text = klientas.Vardas;
                m_klientoPavardeTextBox.Text = klientas.Pavarde;
                m_klientoTelNrMaskedTextBox.Text = klientas.TelNr;
                m_klientoElPastasTextBox.Text = klientas.Elpastas;

            }
            else
            {
                m_klientoVardasTextBox.Enabled = true;
                m_klientoPavardeTextBox.Enabled = true;
                m_klientoElPastasTextBox.Enabled = true;
                m_klientoTelNrMaskedTextBox.Enabled = true;
            }
        }

        private void m_pardavimasSaveButton_Click(object sender, EventArgs e)
        {
            Klientas klientas = dataBase.Klientas.Where(b => b.AK == m_klientoAKTextBox.Text).SingleOrDefault();
            if(klientas == null)
            {
                klientas = new Klientas();
                klientas.AK = m_klientoAKTextBox.Text;
                klientas.Vardas = m_klientoVardasTextBox.Text;
                klientas.Pavarde = m_klientoPavardeTextBox.Text;
                klientas.TelNr = m_klientoTelNrMaskedTextBox.Text;
                klientas.Elpastas = m_klientoElPastasTextBox.Text;
                dataBase.Klientas.Add(klientas);
                dataBase.SaveChanges();
            }
            Pardavimas naujasPardavimas = new Pardavimas();
            naujasPardavimas.Klientas = klientas.Nr;
            naujasPardavimas.Pardavejas = (int)m_pardavejuComboBox.SelectedValue;
            naujasPardavimas.Automobilis = (int)m_automobiliuComboBox.SelectedValue;
            naujasPardavimas.Data = DateTime.Today;
            dataBase.Pardavimas.Add(naujasPardavimas);
            dataBase.SaveChanges();
        }

        private void m_pardavejuComboBox_Click(object sender, EventArgs e)
        {
            m_pardavejuComboBox.DataSource = new string[] { "Loading..." };
            m_pardavejuComboBox.DataSource = dataTables.executeSelectStatement("SELECT Nr, Vardas + ' ' + Pavarde as PilnasVardas FROM Pardavejas");
            m_pardavejuComboBox.DisplayMember = "PilnasVardas";
            m_pardavejuComboBox.ValueMember = "Nr";
        }

        private void m_pardavimoModeliuComboBox_Click(object sender, EventArgs e)
        {
            m_pardavimoModeliuComboBox.DataSource = new string[] { "Loading..." };
            m_pardavimoModeliuComboBox.DataSource = dataBase.Modelis.ToList();
            m_pardavimoModeliuComboBox.ValueMember = "Nr";
            m_pardavimoModeliuComboBox.DisplayMember = "Pavadinimas";
            m_automobiliuComboBox.Enabled = true;
        }

        private void m_automobiliuComboBox_Click(object sender, EventArgs e)
        {
            m_automobiliuComboBox.DataSource = new string[] { "Loading..." };
            m_automobiliuComboBox.DataSource = (from automobilis in dataBase.Automobilis
                                                where !(from pardavimas in dataBase.Pardavimas select pardavimas.Automobilis).Contains(automobilis.Nr)
                                                select new
                                                {
                                                    Nr = automobilis.Nr,
                                                    InfoText = "" + automobilis.Kaina + "Eur " + automobilis.Spalva
                                                }).ToList();
            m_automobiliuComboBox.DisplayMember = "InfoText";
            m_automobiliuComboBox.ValueMember = "Nr";
        }

        private void m_addDataTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_addDataTab.SelectedTab == statistikosPage)
            {
                var rezult = from pardavimas in dataBase.Pardavimas
                             group pardavimas by pardavimas.Pardavejas into pardavejoPardavimai
                             join pardavejas in dataBase.Pardavejas
                             on pardavejoPardavimai.Key equals pardavejas.Nr
                             select new
                             {
                                 Nr = pardavejas.Nr,
                                 Vardas = pardavejas.Vardas,
                                 Pavarde = pardavejas.Pavarde,
                                 AK = pardavejas.AK,
                                 PardavimuSkaicius = pardavejoPardavimai.Count()
                             };
                m_pardavejuPardavimuDataView.DataSource = rezult.ToList();
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string rezult = (string)m_updateDataComboBox.SelectedItem;
            switch (rezult)
            {
                case "Modelis":
                    m_updateDataListBox.DisplayMember = "InfoText";
                    m_updateDataListBox.ValueMember = "Nr";
                    m_updateDataListBox.DataSource = (from modelis in dataBase.Modelis
                                                     select new
                                                     {
                                                         Nr = modelis.Nr,
                                                         InfoText = modelis.Nr + ". " + modelis.Pavadinimas
                                                     }).ToList();
                    break;
                case "Automobilis":
                    m_updateDataListBox.DisplayMember = "InfoText";
                    m_updateDataListBox.ValueMember = "Nr";
                    m_updateDataListBox.DataSource = (from automobilis in dataBase.Automobilis
                                                        select new
                                                        {
                                                            Nr = automobilis.Nr,
                                                            InfoText = "" + automobilis.Kaina + "Eur " + automobilis.Spalva + " modelio nr. " + automobilis.Modelis
                                                        }).ToList();
                    break;
                case "Pardavejas":
                    m_updateDataListBox.DisplayMember = "PilnasVardas";
                    m_updateDataListBox.ValueMember = "Nr";
                    m_updateDataListBox.DataSource = (from pardavejas in dataBase.Pardavejas
                                                     select new
                                                     {
                                                         Nr = pardavejas.Nr,
                                                         PilnasVardas = pardavejas.Vardas + " " + pardavejas.Pavarde
                                                     }).ToList();
                    break;
                case "Klientas":
                    m_updateDataListBox.DisplayMember = "PilnasVardas";
                    m_updateDataListBox.ValueMember = "Nr";
                    m_updateDataListBox.DataSource = (from klientas in dataBase.Pardavejas
                                                     select new
                                                     {
                                                         Nr = klientas.Nr,
                                                         PilnasVardas = klientas.Vardas + " " + klientas.Pavarde
                                                     }).ToList();
                    break;
                default:
                    m_updateDataListBox.DataSource = null;
                    break;
            }
        }

        private void m_updateDataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control control in m_updateDataPanel.Controls)
            {
                control.Visible = false;
            }
            if (m_updateDataListBox.SelectedIndex >= 0)
            {
                string rezult = (string)m_updateDataComboBox.SelectedItem;
                switch (rezult)
                {
                    case "Modelis":
                        Modelis modelis = dataBase.Modelis.Find(m_updateDataListBox.SelectedValue);
                        m_updateDataLabel1.Text = "Pavadinimas:";
                        m_updateDataLabel1.Visible = true;
                        m_updateDataTextBox1.Text = modelis.Pavadinimas;
                        m_updateDataTextBox1.Visible = true;
                        m_updateDataLabel2.Text = "Galia:";
                        m_updateDataLabel2.Visible = true;
                        m_updateDataMaskedTextBox2.Text = ""+modelis.Galia;
                        m_updateDataMaskedTextBox2.Visible = true;
                        m_updateDataLabel3.Text = "Vietų skaičius:";
                        m_updateDataLabel3.Visible = true;
                        m_updateDataMaskedTextBox3.Text = "" + modelis.VietuSkaicius;
                        m_updateDataMaskedTextBox3.Visible = true;
                        m_updateDataLabel4.Text = "Kuras:";
                        m_updateDataLabel4.Visible = true;
                        m_updateDataComboBox4.Text = modelis.Kuras;
                        m_updateDataComboBox4.Visible = true;
                        break;
                    case "Automobilis":
                        Automobilis automobilis = dataBase.Automobilis.Find(m_updateDataListBox.SelectedValue);
                        m_updateDataLabel1.Text = "Modelio Nr:";
                        m_updateDataLabel1.Visible = true;
                        m_updateDataMaskedTextBox1.Text = "" + automobilis.Modelis;
                        m_updateDataMaskedTextBox1.Visible = true;
                        m_updateDataMaskedTextBox1.Enabled = false;
                        m_updateDataLabel2.Text = "Kaina:";
                        m_updateDataLabel2.Visible = true;
                        m_updateDataMaskedTextBox2.Text = "" + automobilis.Kaina;
                        m_updateDataMaskedTextBox2.Visible = true;
                        m_updateDataLabel3.Text = "Spalva:";
                        m_updateDataLabel3.Visible = true;
                        m_updateDataTextBox3.Text = automobilis.Spalva;
                        m_updateDataTextBox3.Visible = true;
                        break;
                    case "Pardavejas":
                        Pardavejas pardavejas = dataBase.Pardavejas.Find(m_updateDataListBox.SelectedValue);
                        m_updateDataLabel1.Text = "Vardas";
                        m_updateDataLabel1.Visible = true;
                        m_updateDataTextBox1.Text = pardavejas.Vardas;
                        m_updateDataTextBox1.Visible = true;
                        m_updateDataLabel2.Text = "Pavardė";
                        m_updateDataLabel2.Visible = true;
                        m_updateDataTextBox2.Text = "" + pardavejas.Pavarde;
                        m_updateDataTextBox2.Visible = true;
                        m_updateDataLabel3.Text = "Asmens kodas:";
                        m_updateDataLabel3.Visible = true;
                        m_updateDataTextBox3.Text = "" + pardavejas.AK;
                        m_updateDataTextBox3.Visible = true;
                        break;
                    case "Klientas":
                        Klientas klientas = dataBase.Klientas.Find(m_updateDataListBox.SelectedValue);
                        m_updateDataLabel1.Text = "Vardas:";
                        m_updateDataLabel1.Visible = true;
                        m_updateDataTextBox1.Text = klientas.Vardas;
                        m_updateDataTextBox1.Visible = true;
                        m_updateDataLabel2.Text = "Pavardė:";
                        m_updateDataLabel2.Visible = true;
                        m_updateDataTextBox2.Text = "" + klientas.Pavarde;
                        m_updateDataTextBox2.Visible = true;
                        m_updateDataLabel3.Text = "Asmens kodas:";
                        m_updateDataLabel3.Visible = true;
                        m_updateDataTextBox3.Text = "" + klientas.AK;
                        m_updateDataTextBox3.Visible = true;
                        m_updateDataLabel4.Text = "El.Paštas:";
                        m_updateDataLabel4.Visible = true;
                        m_updateDataTextBox4.Text = klientas.Elpastas;
                        m_updateDataTextBox4.Visible = true;
                        m_updateDataLabel5.Text = "Tel. Nr.:";
                        m_updateDataLabel5.Visible = true;
                        m_updateDataMaskedTextBox5.Text = klientas.TelNr;
                        m_updateDataMaskedTextBox5.Visible = true;
                        break;
                }
            }
        }

        private void m_deleteButton_Click(object sender, EventArgs e)
        {
            string rezult = (string)m_updateDataComboBox.SelectedItem;
            switch (rezult)
            {
                case "Modelis":
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from Modelis", System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
                    SqlCommand delcmd = new SqlCommand();
                    delcmd.CommandText = "delete from modelis where Nr = @Nr";
                    delcmd.Parameters.Add("@Nr", SqlDbType.Int).SourceColumn = "Nr";
                    delcmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
                    adapter.DeleteCommand = delcmd;
                    DataTable table = dataTables.ModelisTable;
                    foreach (DataRow row in table.Rows)
                    {
                        if((int)row["Nr"] == (int)m_updateDataListBox.SelectedValue)
                        {
                            row.Delete();
                            adapter.Update(table);
                            break;
                        }
                    }
                    break;
                case "Automobilis":
                    Automobilis automobilisToRemove = dataBase.Automobilis.Find(m_updateDataListBox.SelectedValue);
                    dataBase.Automobilis.Remove(automobilisToRemove);
                    dataBase.SaveChanges();
                    break;
                case "Pardavejas":
                    Pardavejas pardavejasToRemove = dataBase.Pardavejas.Find(m_updateDataListBox.SelectedValue);
                    dataBase.Pardavejas.Remove(pardavejasToRemove);
                    dataBase.SaveChanges();
                    break;
                case "Klientas":
                    Klientas klientasToRemove = dataBase.Klientas.Find(m_updateDataListBox.SelectedValue);
                    dataBase.Klientas.Remove(klientasToRemove);
                    dataBase.SaveChanges();
                    break;
            }
            m_updateDataComboBox.SelectedIndex = -1;
        }

        private void m_updateButton_Click(object sender, EventArgs e)
        {
            string rezult = (string)m_updateDataComboBox.SelectedItem;
            switch (rezult)
            {
                case "Modelis":
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from Modelis", System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
                    SqlCommand updcmd = new SqlCommand();
                    updcmd.CommandText = "update modelis set Pavadinimas = @Pavadinimas, Galia = @Galia, VietuSkaicius = @VietuSkaicius, Kuras = @Kuras where Nr = @Nr";
                    updcmd.Parameters.Add("@Pavadinimas", SqlDbType.VarChar).SourceColumn = "Pavadinimas";
                    updcmd.Parameters.Add("@Galia", SqlDbType.SmallInt).SourceColumn = "Galia";
                    updcmd.Parameters.Add("@VietuSkaicius", SqlDbType.SmallInt).SourceColumn = "VietuSkaicius";
                    updcmd.Parameters.Add("@Kuras", SqlDbType.VarChar).SourceColumn = "Kuras";
                    updcmd.Parameters.Add("@Nr", SqlDbType.Int).SourceColumn = "Nr";
                    updcmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AutomobiliuSalonasDataBase"].ConnectionString);
                    adapter.UpdateCommand = updcmd;
                    DataTable table = dataTables.ModelisTable;
                    foreach (DataRow row in table.Rows)
                    {
                        if ((int)row["Nr"] == (int)m_updateDataListBox.SelectedValue)
                        {
                            row["Pavadinimas"] = m_updateDataTextBox1.Text;
                            row["Galia"] = short.Parse(m_updateDataMaskedTextBox2.Text);
                            row["VietuSkaicius"] = short.Parse(m_updateDataMaskedTextBox3.Text);
                            row["Kuras"] = (string)m_updateDataComboBox4.SelectedItem;
                            adapter.Update(table);
                            break;
                        }
                    }
                    break;
                case "Automobilis":
                    Automobilis automobilisToUpdate = dataBase.Automobilis.Find(m_updateDataListBox.SelectedValue);
                    automobilisToUpdate.Kaina = int.Parse(m_updateDataMaskedTextBox2.Text);
                    automobilisToUpdate.Spalva = m_updateDataTextBox3.Text;
                    dataBase.SaveChanges();
                    break;
                case "Pardavejas":
                    Pardavejas pardavejasToUpdate = dataBase.Pardavejas.Find(m_updateDataListBox.SelectedValue);
                    pardavejasToUpdate.Vardas = m_updateDataTextBox1.Text;
                    pardavejasToUpdate.Pavarde = m_updateDataTextBox2.Text;
                    pardavejasToUpdate.AK = m_updateDataTextBox3.Text;
                    dataBase.SaveChanges();
                    break;
                case "Klientas":
                    Klientas klientasToUpdate = dataBase.Klientas.Find(m_updateDataListBox.SelectedValue);
                    klientasToUpdate.Vardas = m_updateDataTextBox1.Text;
                    klientasToUpdate.Pavarde = m_updateDataTextBox2.Text;
                    klientasToUpdate.AK = m_updateDataTextBox3.Text;
                    klientasToUpdate.Elpastas = m_updateDataTextBox4.Text;
                    klientasToUpdate.TelNr = m_updateDataMaskedTextBox5.Text;
                    dataBase.SaveChanges();
                    break;
            }
            m_updateDataComboBox.SelectedIndex = -1;
        }
    }
}
