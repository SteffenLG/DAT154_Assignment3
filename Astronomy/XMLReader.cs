using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using SpaceSim;

namespace Astronomy
{
    class XMLReader
    {

        /*public static List<SpaceObject> ParseFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (line == "")
                    continue;

            }
        }*/

        public static List<SpaceObject> ParseXML()
        {
            Dictionary<string, SpaceObject> spaceObjects = new Dictionary<string, SpaceObject>();
            var doc = XDocument.Load("C:\\code\\csharp\\Assignment3\\Resources");
            foreach (var col in doc.Root.Descendants("record"))
            {
                string name = col.Element("Name").Value;
                string dad = col.Element("Dad").Value;
                double or = Double.Parse(col.Element("OrbitalRadius").Value);
                double op = Double.Parse(col.Element("OrbitalPeriod").Value);

                switch (dad)
                {
                    case "0":
                        spaceObjects.Add(name, new Star(name, or, op, 0, "Yellow"));
                        break;
                    case "Sun":
                        spaceObjects.Add(name, new Planet(name, or, op, 0, spaceObjects[dad], "Blue"));
                        break;
                    default:
                        spaceObjects.Add(name, new Moon(name, or, op, 0, spaceObjects[dad], "Gray"));
                        break;
                }
            }
            return new List<SpaceObject>(spaceObjects.Values);
        }

    }
}
