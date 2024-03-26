namespace ProjectP.Data.Entities;

public class Offer
{
    public int Id { get; set; }
    public Hotel Hotel { get; set; }
    public int HotelId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public double Discount { get; set; }
}