using MapProject;
using MapProject.Parsing;
using MapProject.Saving;
using MapProject.Statistic;
using MapProject.Statistic.FileSystem;
using System;
using System.IO;
using System.Linq;

namespace Map.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            IMapParser mapParser = new MapParser();
            IMapProvider mapProvider = new JsonFileSystemMapProvider();
            IStatisticProvider statisticProvider = new JsonFyleSystemStatisticProvider();

            Manager manager = new Manager(mapParser, mapProvider, statisticProvider);


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

                            MapProject.Model.Map map = manager.PrarseMapFromImage(parameter);

                            manager.SaveMap(map);

                            DataSet set = new DataSet("my new set");

                            DataItem mainDataItem = new DataItem(map.Name);

                            Property p1 = new Property("Name", "Map1");
                            Property p2 = new Property("Description", "This property was added for testing purposes only");
                            mainDataItem.Properties.Add(p1);
                            mainDataItem.Properties.Add(p2);


                            Statistic s1 = new Statistic("RegionCount", map.Regions.Count());

                            mainDataItem.Statistics.Add(s1);

                            set.StructureDatas.Add(mainDataItem);

                            manager.SaveDataSet(set, map);
                            


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
