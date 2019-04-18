using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsFileApplication.Utils
{
    class FilesUtil
    {
        Dictionary<string, string> dictionaryfile = new Dictionary<string, string>();
        public void writeToHistoryFile(String files, string flag)
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
            else if (flag.Equals("ListAllFiles"))
            {

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


            //sorting of words
            public void sortFiles(string rootpath, string destPath)
            {

                //Read all text files from a given directory '
                string[] files = Directory.GetFiles(rootpath, "*.txt");

                for (int i = 0; i < files.Length; i++)
                {

                    //Get the file name from the path
                    string orgFileName = Path.GetFileName(files[i]);
                    Console.WriteLine("Original text file name is::" + orgFileName);

                    //create new file 
                    string newfile = "sorted" + orgFileName;
                    string sortedFileName = destPath + "\\" + newfile;
                    Console.WriteLine("New sorted file name is::" + sortedFileName);

                    // To Read the original file 
                    StreamReader sr = new StreamReader(files[i], Encoding.UTF8);


                    //Continue to read until you reach end of file
                    string[] wordsSplit = sr.ReadToEnd().Split(' ');

                    //Remove Special charters from a string
                    List<string> list = new List<string>();


                    Console.WriteLine("Checking for special characters..");

                    foreach (var line in wordsSplit)
                    {

                        var abcd = line;
                        string singleword1 = string.Empty;

                        //Regex to remove special characters
                        Regex reg = new Regex("[*'\",_&#^@$()€°\t|\n|\r?!%'']");
                        singleword1 = reg.Replace(line, string.Empty);


                        //Remove full stop incase of string
                        if (!line.Any(c => char.IsDigit(c)))
                        {
                            if (line.Contains('.'))
                            {
                                singleword1 = line.Replace(".", String.Empty);
                            }
                        }
                        else
                        {
                            Regex reg1 = new Regex("[*a-zA-Z]");
                            singleword1 = reg1.Replace(singleword1, string.Empty);
                        }


                        list.Add(singleword1);
                        Console.WriteLine(singleword1);
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
                    writeToHistoryFile(newfile, "ListAllFiles");

                }
               

                //wrtiting  all Directories
                writeToHistoryFile(destPath, "ListDirectory");
            }

        public string getAllDirectories(string directory) {
            string sortedfile = string.Empty;
            Console.WriteLine("Retreiving all sorted file for given directory");
            foreach (KeyValuePair<string, string> keyValue in dictionaryfile)
            {
                Console.WriteLine("{0} -> {1}", keyValue.Key, keyValue.Value);
                if (keyValue.Value.Equals(directory))
                {
                    sortedfile = sortedfile + Environment.NewLine + keyValue.Key;
                }
            }
            return sortedfile;
        }
    }

    
}
