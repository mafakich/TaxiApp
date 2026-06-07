namespace TaxiApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } // "В ожидании", "В пути", "Завершён", "Отменён"

        public int? ClientId { get; set; }
        public Client Client { get; set; }

        public int? DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}