using System;
using System.Collections.Generic;
using System.Globalization;
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
            

            Console.WriteLine("What time is it?");
            double time = Double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.WriteLine("What planet do you wish to investigate, my good sir?");
            string name = Console.ReadLine();
            bool found = false;
            SpaceObject targetObject;
            do
            {
                targetObject = solarSystem.Find(so => so.Name.ToLowerInvariant() == name.ToLowerInvariant());
                if (targetObject != null)
                {
                    found = true;
                }
                else
                {
                    Console.WriteLine(
                        $"That's not a planet I've ever heard of! Do they speak english in {name}?");
                    name = Console.ReadLine();
                }
            }
            while (!found);
            Console.WriteLine($"Name: {targetObject.Name}, Position: {targetObject.CalculatePosition(time)}");
            Console.WriteLine("Thank you for using SkyNet");
            Console.WriteLine();
            Console.WriteLine("(Press enter to exit)");
            Console.ReadLine();

        }
    }
}
