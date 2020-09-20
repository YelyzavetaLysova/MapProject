using MapProject;
using MapProject.Parsing;
using MapProject.Saving;
using System;
using System.IO;
using System.Linq;

namespace Map.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            IMapParser mp = new MapParser();
            ISaveProvider provider = new JsonFileSystemSaveProvider();

            Manager manager = new Manager(mp, provider);


            string[] input;

            bool close = false;

            do
            {
                input = System.Console.ReadLine().Split(' ');

                string command = input[0];
                string parameter = "";
                if (input.Length > 1) 
                {
                    parameter  = input[1];
                }

                switch (command)
                {
                    case "":

                        System.Console.WriteLine("Command cannot be empty. Please try again");
                        break;

                    case "parse":

                        if (!String.IsNullOrWhiteSpace(parameter))
                        {
                            if (!File.Exists(parameter))
                            {
                                System.Console.WriteLine("Target image does not exist. Please place it to the same folder with .exe file");
                                continue;
                            }

                            manager.SaveMap(manager.PrarseMapFromImage(parameter));
                        }
                        else
                        {
                            System.Console.WriteLine("Parse command parameter cannot be empty. Please specify name of the image you want to parse as second parameter");
                        }
                        break;

                    case "close":
                        
                        close = true;
                        break;

                    default:

                        System.Console.WriteLine("Unknown command");

                        break;
                }
                       




            }
            while (!close);


        }
    }
}
