using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OpakPriklad56
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<double> delka = new List<double>();

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                StreamReader precti = new StreamReader(openFileDialog1.FileName);
                while (!precti.EndOfStream)
                {
                    double nejvetsi = double.MinValue;
                    
                    string radek = precti.ReadLine();
                    listBox1.Items.Add(radek);
                    string[] slova = radek.Split(';');
                    foreach(string slovo in slova) { 
                        if(slovo.Length > nejvetsi)
                        {
                            nejvetsi = slovo.Length;
                        }
                    }

                    delka.Add(nejvetsi / 10);
                }
                precti.Close();
            }
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            FileStream filestream1 = new FileStream("cisla.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter zapis = new BinaryWriter(filestream1);
            zapis.BaseStream.Position = 0;
            for (int i = 0; i < delka.Count; i++)
            {
                zapis.Write(delka[i]);
            }
            BinaryReader binaryprecti = new BinaryReader(filestream1);
            binaryprecti.BaseStream.Position = 0;
            for (int i = 0; i < delka.Count; i++)
            {
                double cislo = binaryprecti.ReadDouble();
                listBox2.Items.Add(cislo);
            }

            filestream1.Close();
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            FileStream filestream1 = new FileStream("cisla.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            BinaryReader binaryprecti = new BinaryReader(filestream1);
            BinaryWriter zapis = new BinaryWriter(filestream1);

            binaryprecti.BaseStream.Position = 0;
            for (int i = 0; i < delka.Count; i++)
            {
                double cislo = binaryprecti.ReadDouble();
                if(cislo < 0.999)
                {
                    zapis.BaseStream.Position -= sizeof(double);
                    zapis.Write(cislo*10);
                    cislo *= 10;
                }
                listBox3.Items.Add(cislo);
            }

            filestream1.Close();
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            double soucet = 0;
            int pocet = 0;
            FileStream filestream1 = new FileStream("cisla.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            BinaryReader binaryprecti = new BinaryReader(filestream1);
            BinaryWriter zapis = new BinaryWriter(filestream1);

            binaryprecti.BaseStream.Position = 0;
            for (int i = 0; i < delka.Count; i++)
            {
                double cislo = binaryprecti.ReadDouble();
                if (cislo > 2)
                {
                    soucet += cislo;
                    pocet++;
                }
                listBox4.Items.Add(cislo);
            }
            if(pocet != 0)
            {
                double aritemtickysoucet = soucet / pocet;
                zapis.Write(aritemtickysoucet);
                listBox4.Items.Add(aritemtickysoucet);
            }
            else
            {
                listBox4.Items.Add("nejsou zde cisla vetsi jak 2");
            }
            filestream1.Close();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
