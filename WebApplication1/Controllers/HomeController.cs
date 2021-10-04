using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Tours()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();
            

            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Tour> tours = context.Tours.ToList();
                var newList = tours.OrderBy(x => x.DateOfBegin).ToList();
                return View(newList);
            }

        }

        [HttpGet]
        public ActionResult AddTour()
        {
            var userid = User.Identity.GetUserId();
            
            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
               return View();
            }

        }
        [HttpPost]
        public ActionResult AddTour([Bind(Include = "UserId, TourName, Country, Hotel, HotelStar, MealType," +
            "TransportType, Duration, Price, DateOfBegin, DateOfFinish, TOperator, Link")]Tour tour)
        {

            var userid = User.Identity.GetUserId();
            
            if (tour.DateOfBegin <= DateTime.Now)
            {
                ModelState.AddModelError("DateOfBegin", "Некорректна дата");
            }
            if (tour.DateOfFinish <= DateTime.Now && tour.DateOfFinish <= tour.DateOfBegin)
            {
                ModelState.AddModelError("DateOfFinish", "Некорректна дата");
            }
            if (tour.Price <= 0 )
            {
                ModelState.AddModelError("Price", "Некорректні дані");
            }

            var dur = tour.DateOfFinish.DayOfYear - tour.DateOfBegin.DayOfYear;
           
            if (ModelState.IsValid)
            {
                tour.UserId = userid;
                tour.Duration = dur;
                db.Entry(tour).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Tours");
            }

            ViewBag.Message = "Запит не пройшов валідацію";
            return View(tour);
        }

        //Edit
        [HttpGet]
        public ActionResult EditTour(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Tour tours = db.Tours.Find(id);
            if (tours != null)
            {
                return View(tours);
            }
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult EditTour(Tour tour)
        {
            var userid = User.Identity.GetUserId();

            if (tour.DateOfBegin <= DateTime.Now)
            {
                ModelState.AddModelError("DateOfBegin", "Некорректна дата");
            }
            if (tour.DateOfFinish <= DateTime.Now && tour.DateOfFinish <= tour.DateOfBegin)
            {
                ModelState.AddModelError("DateOfFinish", "Некорректна дата");
            }
            if (tour.Price <= 0)
            {
                ModelState.AddModelError("Price", "Некорректні дані");
            }

            var dur = tour.DateOfFinish.DayOfYear - tour.DateOfBegin.DayOfYear;
      
            if (ModelState.IsValid)
            {
                tour.UserId = userid;
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tours");
            }
            return View(tour);
        }

        // Delete
        public async Task<ActionResult> DeleteTour(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = await db.Tours.FindAsync(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        [HttpPost, ActionName("DeleteTour")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedTour(int id)
        {
            Tour tours = await db.Tours.FindAsync(id);
            db.Tours.Remove(tours);
            await db.SaveChangesAsync();
            return RedirectToAction("Tours");
        }



        public ViewResult SearchTour(string sortOrder, string Country, string HotelType, string MealType, 
            string TransportType, string Duration)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            var tours = from s in db.Tours
                        select s;
            
            if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country );
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType);
            }
            else if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.MealType.ToString() == MealType );
            }
            else if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.TransportType.ToString() == TransportType);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.MealType.ToString() == MealType && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s =>  s.TransportType.ToString() == TransportType && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.MealType.ToString() == MealType );
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.TransportType.ToString() == TransportType);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType && s.MealType.ToString() == MealType );
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType && s.TransportType.ToString() == TransportType);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType );
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.TransportType.ToString() == TransportType);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.MealType.ToString() == MealType 
                && s.TransportType.ToString() == TransportType);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country 
                && s.MealType.ToString() == MealType  && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.TransportType.ToString() == TransportType
                && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.TransportType.ToString() == TransportType);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType && s.TransportType.ToString() == TransportType
                && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.MealType.ToString() == MealType && s.TransportType.ToString() == TransportType
                && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.TransportType.ToString() == TransportType);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.TransportType.ToString() == TransportType && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country  && s.MealType.ToString() == MealType 
                && s.TransportType.ToString() == TransportType   && s.Duration.ToString() == Duration);
            }
            else if (String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.TransportType.ToString() == TransportType
                && s.Duration.ToString() == Duration);
            }
            else if (!String.IsNullOrEmpty(Country) && !String.IsNullOrEmpty(HotelType) && !String.IsNullOrEmpty(MealType)
                && !String.IsNullOrEmpty(TransportType) && !String.IsNullOrEmpty(Duration))
            {
                tours = tours.Where(s => s.Country.ToString() == Country && s.HotelStar.ToString() == HotelType
                && s.MealType.ToString() == MealType && s.TransportType.ToString() == TransportType
                && s.Duration.ToString() == Duration);
            }
            else { }

            switch (sortOrder)
            {
                case "name_desc":
                    tours = tours.OrderByDescending(s => s.TourName);
                    break;
                case "Date":
                    tours = tours.OrderBy(s => s.DateOfBegin);
                    break;
                case "date_desc":
                    tours = tours.OrderByDescending(s => s.DateOfBegin);
                    break;
                case "Price":
                    tours = tours.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    tours = tours.OrderByDescending(s => s.Price);
                    break;
                default:
                    tours = tours.OrderBy(s => s.TourName);
                    break;
            }
            return View(tours.ToList());
        }






        public ActionResult Clients()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();


            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Client> clients = context.Clients.ToList();
                var newList = clients.OrderBy(x => x.SecondName).ToList();
                return View(newList);
            }

        }

        [HttpGet]
        public ActionResult AddClient()
        {
            var userid = User.Identity.GetUserId();
            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult AddClient([Bind(Include = "UserId, FirstName, SecondName" +
            "TelNumber, Town, DateOfBirth")]Client client)
        {

            var userid = User.Identity.GetUserId();

            
            if (ModelState.IsValid)
            {
                client.UserId = userid;
                db.Entry(client).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Clients");
            }

            ViewBag.Message = "Запит не пройшов валідацію";
            return View(client);
        }

        //Edit
        [HttpGet]
        public ActionResult EditClient(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Client client = db.Clients.Find(id);
            if (client != null)
            {
                return View(client);
            }
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult EditClient(Client client)
        {
            var userid = User.Identity.GetUserId();

           if (ModelState.IsValid)
            {
                client.UserId = userid;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Clients");
            }
            return View(client);
        }

        // Delete
        public async Task<ActionResult> DeleteClient(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("DeleteClient")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return RedirectToAction("Clients");
        }


        

        public ActionResult Orders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();
            
           
            
            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        [HttpGet]
        public ActionResult AddOrder(int? id)
        {
            var userid = User.Identity.GetUserId();

            if (id == null)
            {
                return HttpNotFound();
            }
            Tour tour = db.Tours.Find(id);
            if (tour != null)
            {
                var neworder = new Order ();
                neworder.TourId = tour.Id;
                                
                return View(neworder);
            }
            return HttpNotFound();
            
        }

        [ChildActionOnly]
        public ActionResult TourPrise(int? id)
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();


            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Tour> tours = context.Tours.Where(t => t.Id == id).ToList();
                var newList = tours.OrderBy(x => x.DateOfBegin).ToList();
                return PartialView(newList);
            }
            
        }

        [HttpPost]
        public ActionResult AddOrder([Bind(Include = "UserId, TourId, ClientFName, ClientSName," +
            "Stage, PaymentStage, DocumentStage, HowManyPeople, Price" +
            "DateOfBegin ")]Order neworder, string ClientFName, string ClientSName, int HowManyPeople)
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();
            var client = context.Clients.Where(t => t.FirstName == ClientFName && t.SecondName == ClientSName).ToList();
            if (client.Any() == false) { ModelState.AddModelError("ClientFName", "Такий клієнт нe зареєстрований"); }
            else { neworder.ClientId = client.First().Id; }
            if (HowManyPeople <=0 && HowManyPeople > 50) { ModelState.AddModelError("HowManyPeople", "Кількість повинна бути > 0 та < 50"); }
            else { neworder.HowManyPeople = HowManyPeople; }
            if (neworder.Stage.ToString() == "Початок_подорожі" || neworder.Stage.ToString() == "Завершене")
            {
                neworder.DocumentStage = StageOfDocuments.Видача;
                neworder.PaymentStage = StageOfPayment.Оплачено;
            }
            
            var tour = context.Tours.Where(t => t.Id == neworder.TourId);
            neworder.Price = HowManyPeople * tour.First().Price;
            if (tour.First().DateOfBegin <= DateTime.Now.Date )
            {
                ModelState.AddModelError("DateOfBegin", "Ви не можете оформити цей тур");
            }
            
            if (ModelState.IsValid)
            {
                neworder.UserId = userid;
                neworder.DateOfBegin = DateTime.Now.Date;
                neworder.DateOfFinish = tour.First().DateOfFinish;
                db.Entry(neworder).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Orders");
            }

            ViewBag.Message = "Запит не пройшов валідацію";
            return View(neworder);
        }

        //Edit
        [HttpGet]
        public ActionResult EditOrder(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                return View(order);
            }
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult EditOrder(Order order)
        {
            var userid = User.Identity.GetUserId();
            var context = new ApplicationDbContext();
            
            var client = context.Clients.Where(t => t.FirstName == order.Client.FirstName &&
            t.SecondName == order.Client.SecondName).ToList();
            if (client.Any() == false)
            { ModelState.AddModelError("ClientFName", "Такий клієнт нe зареєстрований"); }
            else { order.ClientId = client.First().Id; }
            if (order.HowManyPeople <= 0 && order.HowManyPeople > 50)
            { ModelState.AddModelError("HowManyPeople", "Кількість повинна бути > 0 та < 50"); }
            if (order.Stage.ToString() == "Початок_подорожі" || order.Stage.ToString() == "Завершене")
            {
                order.DocumentStage = StageOfDocuments.Видача;
                order.PaymentStage = StageOfPayment.Оплачено;
            }

            var tour = context.Tours.Where(t => t.Id == order.TourId);
            order.Price = order.HowManyPeople * tour.First().Price;
            if (tour.First().DateOfBegin <= DateTime.Now.Date)
            {
                ModelState.AddModelError("DateOfBegin", "Ви не можете оформити цей тур");
            }
            if (tour.First().DateOfBegin == DateTime.Now.Date
                && order.PaymentStage!= StageOfPayment.Оплачено && order.DocumentStage != StageOfDocuments.Видача)
            {
                order.Stage = StageOfOrder.Відхилене;
            }
            else if (tour.First().DateOfBegin <= DateTime.Now.Date && tour.First().DateOfFinish >= DateTime.Now.Date
                && order.PaymentStage == StageOfPayment.Оплачено && order.DocumentStage == StageOfDocuments.Видача)
            {
                order.Stage = StageOfOrder.Початок_подорожі;
            }
            else if (tour.First().DateOfFinish <= DateTime.Now.Date && order.Stage == StageOfOrder.Початок_подорожі)
            {
                order.Stage = StageOfOrder.Завершене;
            }

            if (ModelState.IsValid)
            {
                order.UserId = userid;
                order.DateOfBegin = DateTime.Now.Date;
                order.DateOfFinish = tour.First().DateOfFinish;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Orders");
            }
            return View(order);
        }

        // Delete
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Orders");
        }


        public ActionResult DontPayedOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.PaymentStage.ToString() == "Не_оплачено").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult HalfPayedOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.PaymentStage.ToString() == "Передоплата").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult FullPayedOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.PaymentStage.ToString() == "Оплачено").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult GetDocOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.DocumentStage.ToString() == "Збір").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        public ActionResult SentDocOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.DocumentStage.ToString() == "Відправка").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        public ActionResult GetFromTODocOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.DocumentStage.ToString() == "Отримання_від_ТО").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        public ActionResult GiveToTouristDocOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.DocumentStage.ToString() == "Видача").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult AcceptedOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.Stage.ToString() == "Прийняте").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult WorkWithDocOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.Stage.ToString() == "Опрацювання_документів").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        public ActionResult TripOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.Stage.ToString() == "Початок_подорожі").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }


        public ActionResult FinishedOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.Stage.ToString() == "Завершене").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }

        public ActionResult RejectOrders()
        {
            var context = new ApplicationDbContext();
            var userid = User.Identity.GetUserId();



            if (userid == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                List<Order> orders = context.Orders.Where(x => x.Stage.ToString() == "Відхилене").ToList();
                var newList = orders.OrderBy(x => x.Id).ToList();
                return View(newList);
            }

        }
    }
}