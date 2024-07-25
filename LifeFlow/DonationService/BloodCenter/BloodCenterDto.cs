using System.ComponentModel.DataAnnotations;

namespace DonationService.BloodCenter;

public class BloodCenterDto
{
    public int Id { get; set; }
    [MaxLength(256)]
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int UnitsCapacity { get; set; }
    public int RBCUnits { get; set; }
    public int PlateletsUnits { get; set; }
    public int PlasmaUnits { get; set; }
    public bool IsCentralReserve { get; set; }
    public int? AddressId { get; set; }
    public TimeSpan OpenByTime { get; set; } = new(9, 0, 0); // 9 Am
    public TimeSpan CloseByTime { get; set; } = new(21, 0, 0); // 9 PM
}