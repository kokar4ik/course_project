using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace curse_work
{
    public partial class headform : Form
    {
        public headform()
        {
            InitializeComponent();
        }
        Data data = new Data();
        
        public headform(Vakans v)
        {
            InitializeComponent();
        }

        List<Vakans> vvv = new List<Vakans>();
        private void show_rezume_but_Click(object sender, EventArgs e)
        {
            
        }

        public void add_but_Click(object sender, EventArgs e)
        {
            AddVakansForm fr = new AddVakansForm(openFileDialog.FileName, true);
            //fr.Owner = this;
            fr.ShowDialog();
            reebot_table();
        }

        private void edit_but_Click(object sender, EventArgs e)
        {
            int index = getID();
            AddVakansForm fr = new AddVakansForm(openFileDialog.FileName, index - 1, false);
            fr.ShowDialog();
            vakans_table.CurrentRow.SetValues(index, Data.VakansList[index - 1].dolzhnost, Data.VakansList[index - 1].otrasl, Data.VakansList[index - 1].company, Data.VakansList[index - 1].zarplata, Data.VakansList[index - 1].opyt, Data.VakansList[index - 1].zanyatost, Data.VakansList[index - 1].education);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            bin b = new bin();
            int index = getID();
            Data.VakansList.RemoveAt(index-1);
            vakans_table.Rows.RemoveAt(vakans_table.CurrentRow.Index);
            b.rewrite_vakans(Data.VakansList, openFileDialog.FileName);
            reebot_table();   
        }

        public void reebot_table()
        {
            vakans_table.Rows.Clear();
            int i = 1;
            foreach (Vakans v in Data.VakansList)
                vakans_table.Rows.Add(i++, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
        }

        public void rezume_reeboot_table()
        {
            rezume_table.Rows.Clear();
            int i = 1;
            foreach (Rezume v in Data.RezumeList)
                rezume_table.Rows.Add(i++, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
        }

        public int getID ()
        {
            return (int)vakans_table[0, vakans_table.CurrentRow.Index].Value;
        }

        public int rezume_getID()
        {
            return (int)rezume_table[0, rezume_table.CurrentRow.Index].Value;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bin b = new bin();
                FileInfo myfile = new FileInfo(openFileDialog.FileName);
                FileStream ff = myfile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                ff.Close(); ff.Dispose();
                Data.VakansList = b.read_vakans(openFileDialog.FileName);
                reebot_table();
            }
        }

        private void headform_Load(object sender, EventArgs e)
        {
            // Выставляю значение "Неважно" для комбобоксов вкладки "Вакансии"
            otrasl_comboBox.SelectedIndex = 0;
            opyt_comboBox.SelectedIndex = 0;
            zanyatost_comboBox.SelectedIndex = 0;
            education_comboBox.SelectedIndex = 0;
            // Выставляю значение "Неважно" для комбобоксов вкладки "Резюме"
            sfera_comboBox_rezume.SelectedIndex = 0;
            opyt_rezume_comboBox.SelectedIndex = 0;
            education_rezume_comboBox.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            ///
            bin b = new bin();
            FileStream ff = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            ff.Close(); ff.Dispose();
            Data.VakansList = b.read_vakans(openFileDialog.FileName);
            reebot_table();
            //// Резюме
            FileStream fs = new FileStream(rezume_openFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Close();fs.Dispose();
            Data.RezumeList = b.read_rezume(rezume_openFileDialog.FileName);
            rezume_reeboot_table();
        }

        private void vakans_table_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = getID();
            AddVakansForm fr = new AddVakansForm(openFileDialog.FileName, index - 1, false);
            fr.ShowDialog();
            vakans_table.CurrentRow.SetValues(index, Data.VakansList[index - 1].dolzhnost, Data.VakansList[index - 1].otrasl, Data.VakansList[index - 1].company, Data.VakansList[index - 1].zarplata, Data.VakansList[index - 1].opyt, Data.VakansList[index - 1].zanyatost, Data.VakansList[index - 1].education);
        }

        private void search1_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (Vakans v in Data.VakansList)
                    if (v.dolzhnost == search1_textbox.Text || v.opisanie.Contains(search1_textbox.Text))
                    {
                        vakans_table.Rows.Clear();
                        vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                    }
            }
        }

        private void search_but_Click(object sender, EventArgs e)
        {
            vakans_table.Rows.Clear();
            foreach (Vakans v in Data.VakansList)
            {
                int switch_expres = 0;
                if (ot_zp_textbox.Text == "" && do_zp_textbox.Text == "") switch_expres = 4;
                if (ot_zp_textbox.Text != "" && do_zp_textbox.Text != "") switch_expres = 3;
                if (ot_zp_textbox.Text != "" && do_zp_textbox.Text == "") switch_expres = 1;
                if (do_zp_textbox.Text != "" && ot_zp_textbox.Text == "") switch_expres = 2; 
                 
                if (otrasl_comboBox.Text == v.otrasl || otrasl_comboBox.Text == "Неважно")
                    if (opyt_comboBox.Text == v.opyt || opyt_comboBox.Text == "Неважно")
                        if (zanyatost_comboBox.Text == v.zanyatost || zanyatost_comboBox.Text == "Неважно")
                            if (education_comboBox.Text == v.education || education_comboBox.Text == "Неважно")
                                switch (switch_expres)
                            {
                                case 1:
                                        try
                                        {
                                            if (Convert.ToInt32(ot_zp_textbox.Text) <= Convert.ToInt32(v.zarplata))
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                        catch (FormatException)
                                        {
                                            if (checkBox_vakans.Checked)
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                case 2:
                                        try
                                        {
                                            if (Convert.ToInt32(do_zp_textbox.Text) >= Convert.ToInt32(v.zarplata))
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                        catch (FormatException)
                                        {
                                            if (checkBox_vakans.Checked)
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                    case 3:
                                        try
                                        {
                                            if (Convert.ToInt32(ot_zp_textbox.Text) <= Convert.ToInt32(v.zarplata) && Convert.ToInt32(do_zp_textbox.Text) >= Convert.ToInt32(v.zarplata))
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                        catch (FormatException)
                                        {
                                            if (checkBox_vakans.Checked)
                                                vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                            break;
                                        }
                                    case 4:
                                        vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                        break;
                                    default:
                                        if (v.zarplata == "По договорённости")

                                            vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                                    break;
                            }
            }
        }

        private void reset_but_Click(object sender, EventArgs e)
        {
            otrasl_comboBox.SelectedIndex = 0;
            opyt_comboBox.SelectedIndex = 0;
            zanyatost_comboBox.SelectedIndex = 0;
            education_comboBox.SelectedIndex = 0;
            ot_zp_textbox.ResetText();
            do_zp_textbox.ResetText();
            reebot_table();
        }

        /////// Вкладка "Резюме"

        private void add_but_rezume_Click(object sender, EventArgs e)
        {
            Rezume_form fr = new Rezume_form(rezume_openFileDialog.FileName, true);
            //fr.Owner = this;
            fr.ShowDialog();
            rezume_reeboot_table();
        }

        private void edit_but_rezume_Click(object sender, EventArgs e)
        {
            int index = rezume_getID();
            Rezume_form fr = new Rezume_form(rezume_openFileDialog.FileName, index - 1, false);
            fr.ShowDialog();
            rezume_reeboot_table();
        }

        private void delete_but_rezume_Click(object sender, EventArgs e)
        {
            bin b = new bin();
            int index = rezume_getID();
            Data.RezumeList.RemoveAt(index - 1);
            rezume_table.Rows.RemoveAt(rezume_table.CurrentRow.Index);
            b.rewrite_rezume(Data.RezumeList, rezume_openFileDialog.FileName);
            rezume_reeboot_table();
        }

        private void rezume_table_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = rezume_getID();
            Rezume_form fr = new Rezume_form(rezume_openFileDialog.FileName, index - 1, false);
            fr.ShowDialog();
            rezume_reeboot_table();
        }

        private void rezume_search_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (Rezume v in Data.RezumeList)
                    if (v.dolzhnost == search1_textbox.Text || v.opisanie.Contains(search1_textbox.Text))
                    {
                        vakans_table.Rows.Clear();
                        rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                    }
            }
        }

        private void search_but_rezume_Click(object sender, EventArgs e)
        {
            rezume_table.Rows.Clear();
            foreach (Rezume v in Data.RezumeList)
            {
                int switch_expres = 0;
                if (ot_zp_txtbox_rezume.Text == "" && do_zp_txtbox_rezume.Text == "") switch_expres = 4;
                if (ot_zp_txtbox_rezume.Text != "" && do_zp_txtbox_rezume.Text != "") switch_expres = 3;
                if (ot_zp_txtbox_rezume.Text != "" && do_zp_txtbox_rezume.Text == "") switch_expres = 1;
                if (do_zp_txtbox_rezume.Text != "" && ot_zp_txtbox_rezume.Text == "") switch_expres = 2;

                if (sfera_comboBox_rezume.Text == v.sfera || sfera_comboBox_rezume.Text == "Неважно")
                    if (opyt_rezume_comboBox.Text == v.opyt || opyt_rezume_comboBox.Text == "Неважно")
                        if (education_rezume_comboBox.Text == v.education || education_rezume_comboBox.Text == "Неважно")
                            switch (switch_expres)
                            {
                                case 1:
                                    try
                                    {
                                        if (Convert.ToInt32(ot_zp_textbox.Text) <= Convert.ToInt32(v.zarplata))
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                    catch (FormatException)
                                    {
                                        if (checkBox_vakans.Checked)
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                case 2:
                                    try
                                    {
                                        if (Convert.ToInt32(do_zp_textbox.Text) >= Convert.ToInt32(v.zarplata))
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                    catch (FormatException)
                                    {
                                        if (checkBox_vakans.Checked)
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                case 3:
                                    try
                                    {
                                        if (Convert.ToInt32(ot_zp_textbox.Text) <= Convert.ToInt32(v.zarplata) && Convert.ToInt32(do_zp_textbox.Text) >= Convert.ToInt32(v.zarplata))
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                    catch (FormatException)
                                    {
                                        if (checkBox_vakans.Checked)
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                        break;
                                    }
                                case 4:
                                    rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                    break;
                                default:
                                    if (v.zarplata == "По договорённости")
                                       
                                            rezume_table.Rows.Add(v.id, v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera, v.zarplata, v.opyt, v.education);
                                    break;
                            }
            }
        }
            private void sbros_but_rezume_Click(object sender, EventArgs e)
            {
                sfera_comboBox_rezume.SelectedIndex = 0;
                opyt_rezume_comboBox.SelectedIndex = 0;
             education_rezume_comboBox.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

                ot_zp_txtbox_rezume.ResetText();
                do_zp_txtbox_rezume.ResetText();
              rezume_reeboot_table();
            } 
         

        bool regexLable;

        private void ot_zp_txtbox_rezume_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                regexLable = true;
            else regexLable = false;
        }

        private void ot_zp_txtbox_rezume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (regexLable == false)
            {
                string c = e.KeyChar.ToString();
                if (!Regex.Match(c, @"[0-9]").Success)
                {
                    e.Handled = true;
                }
            }
        }

        private void ot_zp_txtbox_rezume_TextChanged(object sender, EventArgs e)
        {

        }

        private void do_zp_txtbox_rezume_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                regexLable = true;
            else regexLable = false;
        }

        private void do_zp_txtbox_rezume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (regexLable == false)
            {
                string c = e.KeyChar.ToString();
                if (!Regex.Match(c, @"[0-9]").Success)
                {
                    e.Handled = true;
                }
            }
        }

        private void picture_search_Click(object sender, EventArgs e) // Вакансии
        {
            vakans_table.Rows.Clear();
            foreach (Vakans v in Data.VakansList)
                if (v.dolzhnost.Contains( search1_textbox.Text)
                    || v.opisanie.Contains(search1_textbox.Text)
                    || v.otrasl.Contains(search1_textbox.Text)
                    || v.phone.Contains(search1_textbox.Text)
                    || v.zanyatost.Contains(search1_textbox.Text)
                    || v.zarplata.Contains(search1_textbox.Text)
                    ||v.opyt.Contains(search1_textbox.Text)
                    || v.company.Contains(search1_textbox.Text)
                    || v.education.Contains(search1_textbox.Text))

                {

                    vakans_table.Rows.Add(v.id, v.dolzhnost, v.otrasl, v.company, v.zarplata, v.opyt, v.zanyatost, v.education);
                }
        }

       private void search_pictureBox_Click(object sender, EventArgs e) // Резюме
        {
            rezume_table.Rows.Clear();
            foreach (Rezume v in Data.RezumeList)
                if (v.dolzhnost.Contains(rezume_search_textBox.Text)
                  || v.opisanie.Contains(rezume_search_textBox.Text)
                  || v.lastname.Contains(rezume_search_textBox.Text)
                  || v.phone.Contains(rezume_search_textBox.Text)
                  || v.name.Contains(rezume_search_textBox.Text)
                  || v.zarplata.Contains(rezume_search_textBox.Text)
                  || v.opyt.Contains(rezume_search_textBox.Text)
                  || v.dadname.Contains(rezume_search_textBox.Text)
                  || v.education.Contains(rezume_search_textBox.Text)
                  || v.sfera.Contains(rezume_search_textBox.Text))

                {

                    rezume_table.Rows.Add(v.id,  v.lastname, v.name, v.dadname, v.dolzhnost, v.sfera , v.zarplata, v.opyt,  v.education) ;
                }
        } 

        private void rezume_search_textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Search1_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label_edu_Click(object sender, EventArgs e)
        {

        }

        private void sfera_comboBox_rezume_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
