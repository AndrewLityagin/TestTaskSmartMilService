namespace Sms.Test.Service.DTO;

public class MenuItemDto
{
    public string Id { get; set; }
    public string Article { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool IsWeighted { get; set; }
    public string FullPath { get; set; }
    public string[] Barcodes { get; set; }

    public override string ToString()
    {
        return $"{Name} - {Article} - {Price}";
    }
}