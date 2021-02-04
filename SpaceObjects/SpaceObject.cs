using System;
using System.Collections.Generic;

namespace SpaceSim
{
    public abstract class SpaceObject
    {
        protected string name;
        public List<SpaceObject> Children { get; set; }

        public string Name { get { return name; } }
        public double OrbitalRadius { get; }
        public double OrbitalPeriod { get; }
        public double RotationalPeriod { get; }
        public SpaceObject DadBod { get; }
        public string Color { get; }

        public SpaceObject(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color)
        {
            this.name = name;
            Children = new List<SpaceObject>();
            OrbitalRadius = orbitalRadius;
            OrbitalPeriod = orbitalPeriod;
            RotationalPeriod = rotationalPeriod;
            if(dadBod != null)
			{
                DadBod = dadBod;                         
                dadBod.Children.Add(this);
            }

            Color = color;
        }

        public virtual void Draw() 
        {
            Console.WriteLine(name);
        }
        public virtual (double x, double y) CalculatePosition(double time)
        {
            double progress = (time % OrbitalPeriod) / OrbitalPeriod;
            double theta = progress * 2 * Math.PI;
            (double dadX, double dadY) = DadBod.CalculatePosition(time);
            return (Math.Cos(theta) * OrbitalRadius + dadX, Math.Sin(theta) * OrbitalRadius + dadY);
        }
    }

    public class Star : SpaceObject
    {
        public Star(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, string color) 
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, null, color) { }
        public override void Draw()
        {
            Console.Write("Star: ");
            base.Draw();
        }
        public override (double x, double y) CalculatePosition(double time)
        {
            return (0, 0);
        }
    }

    public class Planet : SpaceObject
    {
        public Planet(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color) 
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, dadBod, color) { }
        public override void Draw()
        {
            Console.Write("Planet: ");
            base.Draw();
        }
    }

    public class Moon : SpaceObject
    {
        public Moon(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color)
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, dadBod, color) { }
        public override void Draw()
        {
            Console.Write("Moon: ");
            base.Draw();
        }
    }

    public class Asteroid : SpaceObject
    {
        public Asteroid(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color)
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, dadBod, color) { }
        public override void Draw()
        {
            Console.Write("Asteroid: ");
            base.Draw();
        }
    }

    public class Comet : SpaceObject
    {
        public Comet(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color)
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, dadBod, color) { }
        public override void Draw()
        {
            Console.Write("Comet: ");
            base.Draw();
        }
    }

    public class DwarfPlanet : SpaceObject
    {
        public DwarfPlanet(string name, double orbitalRadius, double orbitalPeriod, double rotationalPeriod, SpaceObject dadBod, string color)
            : base(name, orbitalRadius, orbitalPeriod, rotationalPeriod, dadBod, color) { }
        public override void Draw()
        {
            Console.Write("Dwarf Planet: ");
            base.Draw();
        }
    }

    public class AsteroidBelt : SpaceObject
    {
        public List<Asteroid> Asteroids { get; }
        public AsteroidBelt(string name, SpaceObject dadBod, string color) : base(name, 0, 0, 0, dadBod, color) 
        { 
            Asteroids = new List<Asteroid>();
        }
        public AsteroidBelt(string name, List<Asteroid> asteroids, SpaceObject dadBod, string color) : base(name, 0, 0, 0, dadBod, color)
        {
            Asteroids = asteroids;
        }
        public AsteroidBelt(string name, SpaceObject dadBod, string color, params Asteroid[] asteroids) : base(name, 0, 0, 0, dadBod, color)
        {
            Asteroids = new List<Asteroid>(asteroids);
        }
        public override void Draw()
        {
            Console.Write("Asteroid Belt: ");
            base.Draw();
            Asteroids.ForEach(a => a.Draw());
        }
    }
}
