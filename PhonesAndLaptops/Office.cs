namespace PhonesAndLaptops;

public class Office
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Price { get; set; }
    public ICollection<Asset> Assets { get; set; }
}