namespace PhonesAndLaptops;

public class Asset
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public int Price { get; set; }
    public DateTime ProductionDate { get; set; }
    public int OfficeId { get; set; } // Foreign key

    public Office Office { get; set; }
}