namespace Онлайн_Ресторан.Models
{
    public class Check
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string ClientName { get; set; }
        public int Discount { get; set; }
        public int Summa { get; set; }
    }
}
