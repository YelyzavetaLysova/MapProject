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

            Manager manager = new Manager(mp, provider);


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

                    var map = manager.PrarseMapFromImage(input);

                    manager.SaveMap(map);

                    //var map = manager.GetMap(input);

                    //int i = 0;

                }




            }
            while (!close);


        }
    }
}
