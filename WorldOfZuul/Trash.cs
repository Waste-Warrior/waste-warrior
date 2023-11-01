using System.Runtime.Serialization;
namespace WorldOfZuul;

public class Trash
{
    public enum TrashType 
    {
        Metal,
        Glas,
        PapirOgPap,
        Madaffald,
        Plast,
        Madkartoner,
        Restaffald,
        Batterier,
        Miljøkasse,
    }
    
    public readonly string Name;
    public readonly TrashType Type; //This will later be used to add the trash sorting feature
    public readonly Game.Days Day; // Add a Day property to the Trash class

    public Trash(string name, TrashType type, Game.Days day)
    {
        this.Name = name;
        this.Type = type;
        this.Day = day;
    }
}