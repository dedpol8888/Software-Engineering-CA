using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

using System.Windows.Forms;
using WindowsFormsFileApplication.Utils;

namespace WindowsFormsFileApplication
{
    public partial class Form1 : Form
    {
        static bool sourcefile = false;
        static bool destinationfile = false;
        string rootpath = string.Empty;
        string destPath = string.Empty;
        string calcRootPath = string.Empty;


        FilesUtil fileutils = new FilesUtil();
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            btnSubmit.Visible = false;
            button6.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                rootpath = file.SelectedPath;
                MessageBox.Show(rootpath);
                button2.Enabled = true;
                sourcefile = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                destPath = file.SelectedPath;
                MessageBox.Show(destPath);
                destinationfile = true;
                btnSubmit.Visible = true;

            }
        }



        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!sourcefile)
            {
                MessageBox.Show("Select root directory");
                return;
            }
            if (!destinationfile)
            {
                MessageBox.Show("Select destination directory");
                return;
            }
            fileutils.sortFiles(rootpath, destPath);
            comboBox1.Visible = true;
            button4.Visible = true;
            FillCombobox();


        }
        public void FillCombobox()
        {
            comboBox1.Items.Clear();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedValue = "Id";

            comboBox1.Items.Add(new comboboxItems("---Select your option---", 0));
            comboBox1.Items.Add(new comboboxItems("---List All Directories---", 1));
            comboBox1.Items.Add(new comboboxItems("---List All Files For A Given Directory---", 2));
            comboBox1.Items.Add(new comboboxItems("---List All Sorted Files---", 3));
            comboBox1.SelectedIndex = 0;

        }





        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Visible = true;
                Console.WriteLine("Reading Directoryhistory file");
                string configfilePath = ConfigurationManager.AppSettings["AllDirPath"];


                StreamReader sr = new StreamReader(@configfilePath);
                string line;
                string line1 = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    line1 = line1 + Environment.NewLine + line;
                }


                textBox1.Text = @line1;


            }
            else if (comboBox1.SelectedIndex == 2)
            {
                button3.Visible = true;
                textBox1.Clear();

            }
            else if (comboBox1.SelectedIndex == 3)
            {
                textBox1.Visible = true;


                textBox1.Visible = true;
                Console.WriteLine("Reading AllFileshistory file");
                string configfilePath = ConfigurationManager.AppSettings["AllFilePath"];


                StreamReader sr = new StreamReader(@configfilePath);
                string line;
                string line1 = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    line1 = line1 + Environment.NewLine + line;
                }


                textBox1.Text = @line1;
            }
            else if (comboBox1.SelectedIndex == 4)
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox1.Visible = true;
                string directory = file.SelectedPath;
                string sortedfile = fileutils.getAllDirectories(directory);
                textBox1.Text = @sortedfile;
            }
        }

        private void button4_Clear(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            btnSubmit.Visible = false;

        }


        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                calcRootPath = file.SelectedPath;
                MessageBox.Show(calcRootPath);
                button6.Visible = true;

            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            FileCalc lfilecalc = new FileCalc();
            lfilecalc.calculate(calcRootPath);
            MessageBox.Show("Completed!!");
        }
    }
}
