using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpakPriklad56
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void Vek(string radek,ref string mesic,ref bool prvnivprosinci,ref string jmenoprosinec,ref double vek2)
        {
            string[] mesice = { "leden", "únor", "březen", "duben", "květen", "červen", "červenec", "srpen", "září", "říjen", "listopad", "prosinec" };
           

            string[] line = radek.Split(';');

            string rodnycislo = line[2].Substring(0, 6);
            int cislomesice = (int.Parse(rodnycislo.Substring(2, 2)));

            string skutecnyrok;
            if(cislomesice > 12)
            {
                mesic = mesice[cislomesice - 51];
            }
            else
            {
                mesic = mesice[cislomesice - 1];
            }
            

            if (int.Parse(rodnycislo.Substring(0,2)) <= 20 && int.Parse(rodnycislo.Substring(0, 2)) >= 0 && line[2].Length == 10)
            {
                skutecnyrok = "20" + rodnycislo.Substring(0, 2);
            }
            else
            {
                skutecnyrok = "19" + rodnycislo.Substring(0, 2);
            }
            if(cislomesice == 12 || cislomesice == 62 && prvnivprosinci == false)
            {
                jmenoprosinec = line[0];
                prvnivprosinci = true;
            }


            DateTime datumnarozeni = new DateTime(int.Parse(skutecnyrok),(cislomesice > 12)?(cislomesice-51):(cislomesice), int.Parse(rodnycislo.Substring(4, 2)));
            TimeSpan vek = DateTime.Now - datumnarozeni;
            
            //MessageBox.Show("Měsíc : " + mesic + "   věk :" + (int)vek.Days/365.25);
            label1.Text += "Měsíc : " + mesic + "   věk :" + (int)(vek.Days / 365.25) + "\n";
            vek2 = vek.Days / 365.25;
        }
        void Vek(string radek, ref string mesic, ref double vek2)
        {
            string[] mesice = { "leden", "únor", "březen", "duben", "květen", "červen", "červenec", "srpen", "září", "říjen", "listopad", "prosinec" };


            string[] line = radek.Split(';');

            string rodnycislo = line[2].Substring(0, 6);
            int cislomesice = (int.Parse(rodnycislo.Substring(2, 2)));

            string skutecnyrok;
            if (cislomesice > 12)
            {
                mesic = mesice[cislomesice - 51];
            }
            else
            {
                mesic = mesice[cislomesice - 1];
            }


            if (int.Parse(rodnycislo.Substring(0, 2)) <= 20 && int.Parse(rodnycislo.Substring(0, 2)) >= 0 && line[2].Length == 10)
            {
                skutecnyrok = "20" + rodnycislo.Substring(0, 2);
            }
            else
            {
                skutecnyrok = "19" + rodnycislo.Substring(0, 2);
            }


            DateTime datumnarozeni = new DateTime(int.Parse(skutecnyrok), (cislomesice > 12) ? (cislomesice - 51) : (cislomesice), int.Parse(rodnycislo.Substring(4, 2)));
            TimeSpan vek = DateTime.Now - datumnarozeni;

            vek2 = vek.Days / 365.25;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            try
            {
                double vek2 = 0;
                string jmenoprosinec = "";
                bool prvnivprosinci = false;
                string mesic = "";
                int soucet = 0;
                int pocet = 0;
                StreamReader cti = new StreamReader("rodna_cis.txt");

                while (!cti.EndOfStream)
                {
                    string radek = cti.ReadLine();
                    string[] polozky = radek.Split(';');
                    listBox1.Items.Add(radek);
                    Vek(radek, ref mesic, ref prvnivprosinci, ref jmenoprosinec, ref vek2);

                    soucet += int.Parse(polozky[1]);
                    pocet++;
                }
                cti.BaseStream.Position = 0;
                cti.Close();
                float prumer = (float)soucet / (float)pocet;
                MessageBox.Show("Prumer Znamek : " + prumer);
                MessageBox.Show("Prvni clovek v prosinci narozeny je : " + jmenoprosinec);

                label1.Text = "";

                string[] radky1 = File.ReadAllLines("rodna_cis.txt");

                StreamWriter zapis = new StreamWriter("rodna_cis.txt", false);
                foreach (string radek in radky1)
                {
                    Vek(radek, ref mesic, ref prvnivprosinci, ref jmenoprosinec, ref vek2);
                    string novyRadek = radek + ";" + (int)vek2;
                    zapis.WriteLine(novyRadek);
                    listBox2.Items.Add(novyRadek);
                }
                zapis.WriteLine(prumer);
                listBox2.Items.Add(prumer);
                zapis.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string mesic = "";
                double vek = 0;
                SaveFileDialog uloz = new SaveFileDialog();
                uloz.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                uloz.DefaultExt = ".txt";

                if (uloz.ShowDialog() == DialogResult.OK)
                {
                    List<string> jmena = new List<string>();
                    List<int> stari = new List<int>();
                    List<string> mesice = new List<string>();
                    List<bool> listbool = new List<bool>();


                    string[] radky = listBox1.Items.Cast<string>().ToArray();
                    foreach (string radek in radky)
                    {
                        string[] polozky = radek.Split(';');
                        if (Convert.ToInt32(polozky[1]) < 3)
                        {
                            listbool.Add(true);
                        }
                        else
                        {
                            listbool.Add(false);
                        }
                        jmena.Add(polozky[0]);
                        Vek(radek, ref mesic, ref vek);
                        stari.Add(Convert.ToInt32(vek));
                        mesice.Add(mesic);
                    }


                    StreamWriter zapis = new StreamWriter(uloz.FileName);
                    for (int i = 0; i < jmena.Count; i++)
                    {
                        if (listbool[i] == true)
                        {
                            zapis.Write(jmena[i] + ";" + stari[i] + ";" + mesice[i]);
                            listBox3.Items.Add(jmena[i] + ";" + stari[i] + ";" + mesice[i]);
                        }
                    }
                    zapis.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
