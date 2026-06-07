namespace TaxiApp.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}