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

namespace WindowsFormsFileApplication
{
    public partial class Form1 : Form
    {
        static bool sourcefile = false;
        static bool destinationfile = false;
        string rootpath = string.Empty;
        string destPath = string.Empty;
        Dictionary<string, string> dictionaryfile = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            btnSubmit.Visible = false;

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
            sortFiles( rootpath, destPath);
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
            comboBox1.Items.Add(new comboboxItems("---List All Directories---",1));
            comboBox1.Items.Add(new comboboxItems("---List All Files For A Given Directory---", 2));
            comboBox1.Items.Add(new comboboxItems("---List All Sorted Files---", 3));
            comboBox1.SelectedIndex = 0;

        }
        public void sortFiles(string rootpath,string destPath)
        {

            //Read all text files from a given directory '
            string[] files = Directory.GetFiles(rootpath, "*.txt");

            for (int i = 0; i < files.Length; i++)
            {

                //Get the file name from the path
                string orgFileName = Path.GetFileName(files[i]);
                Console.WriteLine("Original text file name is::" + orgFileName);

                //create new file 
                string newfile="sorted" + orgFileName;
                string sortedFileName = destPath + "\\"+newfile;
                Console.WriteLine("New sorted file name is::" + sortedFileName);

                // To Read the original file 
                StreamReader sr = new StreamReader(files[i], Encoding.UTF8);


                //Continue to read until you reach end of file
                string[] wordsSplit = sr.ReadToEnd().Split(' ');

                //Remove Special charters from a string
                List<string> list = new List<string>();

                Console.WriteLine("Checking for special characters..");
                string[] dotwords = null;
                foreach (var singleString in wordsSplit)
                {
                    Regex reg = new Regex("[*'\",_&#^@$()€°?!%]");
                    string singleword = reg.Replace(singleString, string.Empty);

                    if (singleword.Any(c => char.IsDigit(c)))
                    {
                        Regex reg1 = new Regex("[*a-zA-Z]");
                        singleword = reg1.Replace(singleword, string.Empty);

                    }
                    else if (singleword.Contains(".") || singleword.Contains(","))
                    {
                        dotwords = singleword.Split('.');
                        Console.WriteLine("removing full stop" + singleword);

                    }

                    Console.WriteLine("new words:" + singleword);
                    list.Add(singleword);

                }
                string[] listwords = list.ToArray();

                var words = listwords.OrderBy(s => s, new MyComparer());       
                
                //count the number of occurences 
                var dirWordCount = new Dictionary<string, int>();

                foreach (var word in words)
                {
                    if (dirWordCount.ContainsKey(word))
                    {
                        dirWordCount[word] = dirWordCount[word] + 1;
                    }
                    else
                    {
                        dirWordCount.Add(word, 1);
                    }
                }


                //now write the contents into sorted file
                for (int index = 0; index < dirWordCount.Count; index++)
                {
                    var item = dirWordCount.ElementAt(index);
                    string itemKey = item.Key;
                    int itemValue = item.Value;
                    Console.WriteLine("Sorted values: " + itemKey + "," + itemValue);

                    //create a sorted file & then write to it
                    using (StreamWriter sw = File.CreateText(sortedFileName))
                    {
                        foreach (var entry in dirWordCount)
                        {
                            if ((entry.Key) != null)
                            {
                                if (entry.Value > 1)
                                {
                                    sw.WriteLine("{0}{1}{2}", entry.Key, ",", entry.Value);
                                }
                                else
                                {
                                    sw.WriteLine("{0}", entry.Key);
                                }
                            }
                        }
                        sw.Close();

                    }


                }


                //storing all files with respect to directories
                //Check for already existing files
                dictionaryfile.Add(newfile, destPath);
                   


                //writing all sorted files 
                writeToHistoryFile(newfile,"ListAllFiles");

            }
            MessageBox.Show("Sorting of files completed!!");
            //wrtiting  all Directories
            writeToHistoryFile(destPath,"ListDirectory");
        }

        private void writeToAllHistoryFile()
        {
            throw new NotImplementedException();
        }

        private void writeToHistoryFile(String files, string flag)
        {
            StreamWriter writer = null;
            Console.WriteLine("writing files to history file");


            if (flag.Equals("ListDirectory"))
            {
                try
                {
                    string configfilePath = ConfigurationManager.AppSettings["AllDirPath"];
                    using (writer = File.AppendText(@configfilePath))
                    {
                        writer.WriteLine(files);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    writer.Close();
                }
            }
            else if (flag.Equals("ListAllFiles")) {

                try
                {
                    string configfilePath = ConfigurationManager.AppSettings["AllFilePath"];
                    using (writer = File.AppendText(@configfilePath))
                    {
                        writer.WriteLine(files);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    writer.Close();
                }
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Visible = true;
                Console.WriteLine("Reading Directoryhistory file");
                string  configfilePath = ConfigurationManager.AppSettings["AllDirPath"];
                

                StreamReader sr = new StreamReader(@configfilePath);
                string line;
                string line1 = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    line1 =line1+ Environment.NewLine + line;
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
            else if(comboBox1.SelectedIndex == 4)
            {

            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox1.Visible = true;
                string  directory = file.SelectedPath;
                string sortedfile = string.Empty;
                Console.WriteLine("Retreiving all sorted file for given directory");
                foreach (KeyValuePair<string, string> keyValue in dictionaryfile)
                {
                    Console.WriteLine("{0} -> {1}", keyValue.Key, keyValue.Value);
                    if (keyValue.Value.Equals(directory)) {
                        sortedfile = sortedfile+Environment.NewLine + keyValue.Key;
                    }
                }
                textBox1.Text = @sortedfile;
            }
        }

        private void button4_Clear(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            btnSubmit.Visible=false;

        }
    }
}
