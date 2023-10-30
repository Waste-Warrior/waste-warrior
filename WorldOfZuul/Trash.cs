using System.Runtime.Serialization;
namespace WorldOfZuul;

public class Trash
{
    public enum TrashType 
    {
        [EnumMember(Value = "Metal")]
        Metal = 0,
        [EnumMember(Value = "Glas")]
        Glas = 1,
        [EnumMember(Value = "Papir & pap")]
        PapirOgPap = 2,
        [EnumMember(Value = "Madaffald")]
        Madaffald = 3,
        [EnumMember(Value = "Plast")]
        Plast = 4,
        [EnumMember(Value = "Plast + MAD-& Drikkekartoner")]
        DrikkeKartoner = 5,
        [EnumMember(Value = "Restaffald")]
        Restaffald = 6,
        [EnumMember(Value = "Batterier")]
        Batterier = 7,
        [EnumMember(Value = "Miljøkasse")]
        Miljøkasse = 8
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