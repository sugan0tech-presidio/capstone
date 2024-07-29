using DonationService.Commons.Enums;
using DonationService.Features.DonationSlot;

namespace DonationService.Features.BloodCenter;

public interface IBloodCenterService
{
    public Task<List<BloodCenterFetchDto>> GetNearByCenters(double latitude, double longitude);
    public Task<BloodCenterDto> GetCenterByName(string name);
    public Task<DonationSlotDto> BookAppointment(string centerName, int donorId);
    public Task<List<DonationSlotDto>> GetPendingSlots(string centerName);
    public Task<List<DonationSlotDto>> GetCompletedSlots(string centerName);
    public Task ProcessSlot(int slotId, SlotStatus status);
}