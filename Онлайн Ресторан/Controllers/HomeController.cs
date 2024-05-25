using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Онлайн_Ресторан.Models;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using Онлайн_Ресторан.Models;

namespace Онлайн_Ресторан.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyBaseContext db;

        public HomeController(MyBaseContext db)
        {
            this.db = db;
        }
        public ActionResult Index(int? KitchenId, string type, int id)
        {
            //====================================================
            // Список отделов
            //====================================================
            List<Kitchen> depsList = db.Kitchens.ToList();

            // Устанавливаем начальный элемент, который позволит выбрать всех
            depsList.Insert(0, new Kitchen { KitchenName = "Все отделы", KitchenId = 0 });
            SelectList Kitchens = new SelectList(depsList, "KitchenId", "KitchenName", KitchenId);

            ViewBag.Kitchens = Kitchens;

            //====================================================
            // Список должностей
            //====================================================         
            SelectList types = new SelectList(new List<string>()
   {
      "Все",
      "Суп",
      "Салат",
      "Второе блюдо",
      "Напиток",
      "Алкоголь"
   }, type);

            ViewBag.Types = types;

            //====================================================
            // Фильтрация списка Служащих
            //==================================================== 
            IQueryable<Person> persons = db.Persons.Include(p => p.Kitchen);

            // 1. Если выбран конкретный Отдел
            if (KitchenId != null && KitchenId != 0)
            {
                persons = persons.Where(p => p.KitchenId == KitchenId);
            }

            // Если выбрана конкретная Должность
            if (!String.IsNullOrEmpty(type) && !type.Equals("Все"))
            {
                persons = persons.Where(p => p.Type == type);
            }

            ViewBag.Persons = persons;

            return View();
        }

        public ActionResult Korzina(int? id)
        {
            Person menu = db.Persons.FirstOrDefault(p => p.Id == id);

            Korzina korzina = new Korzina()
            {
                KitchenId = menu.KitchenId,
                Type = menu.Type,
                Name = menu.Name,
                Price = menu.Price,
                Koll = 1
            };

            db.Korzinas.Add(korzina);
            var count = menu.Koll - 1;
            menu.Koll = count;
            db.SaveChanges();

            // Переход на главную страницу приложения
            return RedirectToAction("Index");
        }

        public ActionResult ViewKorzina()
        {
            // Получаем из БД все записи таблицы Person
            IEnumerable<Korzina> korzinas = db.Korzinas;

            // Передаем все объекты записи в ViewBag
            ViewBag.Korzinas = korzinas;

            // Возвращаем представление
            return View();
        }

        public ActionResult Cancel(int? id)
        {
            Korzina korzina = db.Korzinas.FirstOrDefault(p => p.Id == id);

            db.Korzinas.Remove(korzina);
            db.SaveChanges();

            // Переход на главную страницу приложения
            return RedirectToAction("ViewKorzina");
        }

        public ActionResult ExecuteOrder()
        {
            // Передача списка клиентов в представление
            IEnumerable<Client> clients = db.Clients;
          
            ViewBag.Customers = clients;

            // Передача списка товаров из Корзины в предствление
            IEnumerable<Korzina> korzina = db.Korzinas;
            ViewBag.Korzinas = korzina;

            // Возвращаем представление
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                await db.SaveChangesAsync();
                return RedirectToAction("ListClients"); 
            }
            return View(client);
        }
        /*public ActionResult Prodano(int id)
        {
            // Retrieve the client from the database using the provided ID
            Client client = db.Clients.FirstOrDefault(p => p.Id == id);

            // Create a new Check object and populate its fields
            Check check = new Check()
            {
                Data = DateTime.Now,
                ClientName = client?.Name ?? "Unknown", // Handle potential null value
                Discount = 0,
                Summa = 0
            };

            // Get the list of Korzinas and calculate the total sum
            List<Korzina> korzinas = db.Korzinas.ToList();
            var sum = korzinas.Sum(p => p.Price);
            var discount = sum / 100;

            // Update the Check object with the calculated values
            check.Summa = sum;
            check.Discount = discount;

            // Save the Check object to the database
            db.Checks.Add(check);

            // Create a new Prodano object and populate its fields with the same values
            Prodano prodano = new Prodano()
            {
                Date = DateTime.Now,
                ClientName = client?.Name ?? "Unknown", // Handle potential null value
                Discount = discount,
                Summa = sum
            };

            // Save the Prodano object to the database
            db.Prodanos.Add(prodano);

            // Save all changes to the database in a single transaction
            db.SaveChanges();

            // Redirect to the OrderComplete action
            return RedirectToAction("OrderComplete");
        }

        public ActionResult OrderComplete()
        {
            // Получаем из БД все записи таблицы Person
            IEnumerable<Prodano> prodano = db.Prodanos;

            // Передаем все объекты записи в ViewBag
            ViewBag.Prodanos = prodano;

            // Возвращаем представление
            return View();
        }*/
        public ActionResult OrderComplete(int id)
        {
            Client client = db.Clients.FirstOrDefault(p => p.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            Check check = new Check()
            {
                Data = DateTime.Now,
                ClientName = client.Name,
                Discount = 0,
                Summa = 0
            };

            List<Korzina> depsList = db.Korzinas.ToList();
            var sum = depsList.Sum(p => p.Price);
            check.Summa = sum;
            var disc = sum / 100;
            check.Discount = disc;

            db.Checks.Add(check);
            db.SaveChanges();

            // Передача списка клиентов в представление
            IEnumerable<Check> checks = db.Checks.ToList();

            // Возвращаем представление и передаем модель
            return View(checks);
        }
        [HttpGet]
        public ActionResult DeleteOrder(int? id)
        {
            Check check = db.Checks.FirstOrDefault(p => p.Id == id);
            db.Checks.Remove(check);
            db.SaveChanges();
            //var check = db.Checks.SingleOrDefault(p => p.Id == id);
            //if (check != null)
            //{
            //    db.Checks.Remove(check);
            //    db.SaveChanges();
            //}
            return RedirectToAction("ExecuteOrder"); // Предположим, что ваша главная страница называется Index
        }
        public ActionResult OrderConfirmed()
        {
            return View();
        }
    }
}
