using MapProject;
using MapProject.Model;
using MapProject.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            Project prj = new Project();






            IMapParser mp = new MapParser();
            ISaveProvider provider = new FileSystemSaveProvider();

            Manager manager = new Manager(mp, provider);


            manager.

            string input;

            bool close = false;

            do
            {
                input = System.Console.ReadLine();

                if (input == "close")
                {
                    return;
                }
                else
                {
                    if (String.IsNullOrEmpty(input))
                    {
                        System.Console.WriteLine("File path cannot be empty. Please try again");
                        continue;
                    }

                    if (!File.Exists(input))
                    {
                        System.Console.WriteLine("File does not exist");
                        continue;
                    }

                    manager.ProcessMapFromImage(input);

                    //}
                    //catch(Exception e)
                    //{
                    //    System.Console.WriteLine("Exception happened while processing file");
                    //    System.Console.WriteLine(e.Message);
                    //    continue;
                    //}

                }




            }
            while (!close);


        }
    }
}
