namespace Онлайн_Ресторан.Models
{
    public class Kitchen
    {
        public int KitchenId { get; set; }
        public string KitchenName { get; set; }

        public List<Person> Persons { get; set; }

        public Kitchen()
        {
            Persons = new List<Person>();
        }
    }
}
