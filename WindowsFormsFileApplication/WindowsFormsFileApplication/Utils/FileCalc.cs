using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsFileApplication.Utils
{
    class FileCalc
    {
        public void calculate(string calcPath) {
           
                string newfile = DateTime.Now.ToString("dddd-dd-MMMM-yyyy");
                Console.WriteLine(newfile.ToString());

                string newFilepath = calcPath + "\\answ" + newfile + ".answ";
                StreamWriter sw = File.CreateText(newFilepath);


                //Read all text files from a given directory '
                string[] files = Directory.GetFiles(calcPath, "*.calc");

                try
                {

                    for (int i = 0; i < files.Length; i++)
                    {
                        string orgFileName = Path.GetFileName(files[i]);
                         Console.WriteLine("Original text file name is::" + orgFileName);

                            //Read files using stream reader
                            StreamReader sr = new StreamReader(files[i], Encoding.UTF8);

                            //Continue to read until you reach end of file
                            string[] lines = File.ReadAllLines(files[i]);
                            var res = (dynamic)null;
                            foreach (var mathString in lines){

                                Console.WriteLine(mathString);

                                if (mathString.Contains('^'))
                                {
                                    string[] arr = mathString.Split('^');
                                    double firstdigit = double.Parse(arr[0]);
                                    double seconddigit = double.Parse(arr[1]);
                                    res = Math.Pow(firstdigit, seconddigit);
                                    Console.WriteLine("Power is " + res);

                                }else
                                {
                                    res = Evaluate(mathString);
                                    Console.WriteLine("Result is:" + res);                                
                                }
                                sw.WriteLine(mathString + "=" + res);
                            
                        }
                        sw.WriteLine("-------------" + orgFileName + " Completed-------------");
                    }
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured: " + ex);
                }
                                             
        }
        public static double Evaluate(string expression){

            return (double)new System.Xml.XPath.XPathDocument
            (new System.IO.StringReader("<r/>")).CreateNavigator().Evaluate
            (string.Format("number({0})", new
            System.Text.RegularExpressions.Regex(@"([\+\-\*])")
            .Replace(expression, " ${1} ")
            .Replace("/", " div ")
            .Replace("%", " mod ")));
        }
    }
}
