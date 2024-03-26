using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;

namespace ProjectP.Interfaces;

public interface IOfferService
{
    public Task<List<OfferDto>> GetAllOffersAsync();
    public Task<(Offer? offer, string message)> GetOfferByIdAsync(int offerId);
    public Task<(Offer? offer, string message)> AddOffer(NewOfferDto offerDto);
    public Task<(Offer? offer, string message)> UpdateOffer(int offerId,UpdateOfferDto offerDto);
    public Task<(bool succeed, string message)> DeleteOffer(int offerId);
}