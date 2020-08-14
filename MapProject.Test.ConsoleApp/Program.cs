using MapProject;
using MapProject.Model;
using MapProject.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapProject.Saving;
using Microsoft.Extensions.Logging;

namespace Map.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var loggerFactory = LoggerFactory.Create(builder => 
            {
                builder.AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger("MapProject.Console");
       

            IMapParser mp = new MapParser(logger);
            ISaveProvider provider = new JsonFileSystemSaveProvider();

            MapProcessingSettings settings = new MapProcessingSettings();

            Manager manager = new Manager(settings, mp, provider, logger);


            string[] input;

            bool close = false;

            do
            {
                input = System.Console.ReadLine().Split(' ');

                string command = input[0];
                string parameter = String.Empty;
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

                    case "load":


                        if (!String.IsNullOrWhiteSpace(parameter))
                        {
                            if (!File.Exists(parameter))
                            {
                                System.Console.WriteLine("Target map does not exist. Please ensure there is the map with the name specified under the Maps folder");
                                continue;
                            }

                            var map = manager.GetMap(parameter);
                        }
                        else
                        {
                            System.Console.WriteLine("Target map name cannot be empty. Please specify name of the map you want to open as second parameter");
                        }
                        break;

                    case "close":
                        return;

                    default:

                        System.Console.WriteLine("Unknown command");

                        break;
                }
                       




            }
            while (!close);


        }
    }
}
