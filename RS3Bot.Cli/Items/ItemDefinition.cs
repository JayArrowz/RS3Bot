using RS3Bot.Abstractions.Extensions;
using RS3Bot.Cli.Commands;
using System.IO;

namespace RS3Bot.Cli
{
    public class ItemDefinition
    {
        static readonly ItemDefinition[] _definitions = new ItemDefinition[80000];

        public string Name { get; set; }

        public static string GetItemName(int id)
        {
            return _definitions[id]?.Name;
        }

        static ItemDefinition()
        {
            Start();
        }

        private static void Start()
        {
            using (var itemList = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Data.ItemList.txt"))
            using (var streamReader = new StreamReader(itemList))
            {
                string line = null;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var data = line.Split(" - ");
                    int id = int.Parse(data[0]);
                    var name = data[1];
                    _definitions[id] = new ItemDefinition { Name = name };
                }
            }
        }
    }
}
