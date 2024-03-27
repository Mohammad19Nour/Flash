namespace ProjectP.Dtos.OfferDtos;

public class OfferForHotelDto
{
    public int Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public double Discount { get; set; }
}