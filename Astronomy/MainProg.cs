using System;
using System.Collections.Generic;
using SpaceSim;

namespace Astronomy
{
    class MainProg
    {
        static void Main(string[] args)
        {
            List<SpaceObject> solarSystem = XMLReader.ParseXML();
            foreach (SpaceObject obj in solarSystem)
            {
                obj.Draw();
            }
            Console.WriteLine($"The sun is located at: {solarSystem[0].CalculatePosition(25)}");
            Console.ReadLine();
            
        }
    }
}
