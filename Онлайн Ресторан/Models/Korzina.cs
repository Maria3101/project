namespace Онлайн_Ресторан.Models
{
    public class Korzina
    {
        public int Id { get; set; }
        public int KitchenId { get; set; }
        public Kitchen Kitchen { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Koll { get; set; }
    }
}
