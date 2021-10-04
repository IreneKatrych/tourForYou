using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Client
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string TelNumber { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public virtual ApplicationUser User { get; set; }
     }


    public class Tour
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        public Countries Country { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public Stars HotelStar { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public Meals MealType { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public TransportTypes TransportType { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string TourName { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string Hotel { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public Double Price { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string TOperator { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        [DataType(DataType.Date)]
        public DateTime DateOfBegin { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        [DataType(DataType.Date)]
        public DateTime DateOfFinish { get; set; }

        public virtual ApplicationUser User { get; set; }
 
    }


    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int TourId { get; set; }
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        public StageOfOrder Stage { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public StageOfPayment PaymentStage { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public StageOfDocuments DocumentStage { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public int HowManyPeople { get; set; }
        [Required(ErrorMessage = "Дані повинні бути введені")]
        public Double Price { get; set; }

        [Required(ErrorMessage = "Дані повинні бути введені")]
        [DataType(DataType.Date)]
        public DateTime DateOfBegin { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfFinish { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Tour Tour { get; set; }
        public virtual Client Client { get; set; }
    }



    public enum StageOfOrder
    {
        Прийняте,
        Опрацювання_документів,
        Початок_подорожі,
        Завершене,
        Відхилене
    }

    public enum StageOfDocuments
    {
       Збір,
       Відправка,
       Отримання_від_то,
       Видача
    }


    public enum StageOfPayment
    {
        Не_оплачено,
        Передоплата,
        Оплачено
    }



    public enum Countries
    {
        Австрія,
        Болгарія,
        Велика_Британія,
        Греція,
        Грузія,
        Єгипет,
        Ізраїль,
        Ірландія,
        Іспанія,
        Італія,
        Кіпр,
        Маврикій,
        Мальдіви,
        Мальта,
        Німеччина,
        Норвегія,
        ОАЕ,
        Польща,
        Португалія,
        Румунія,
        Словаччина,
        Туреччина,
        Угорщина,
        Україна,
        Франція,
        Хорватія,
        Чехія,
        Чорногорія,
        Японія
     
    }

    

   

    public enum TransportTypes
    {
        літак,
        поїзд,
        автобус,
        круїзний_лайнер,
        комбінований

    }

   public enum Meals
    {
        RO, BB, AB, HB, FB, AI, UAI
    }

    public enum Stars
    {
        three_stars,
        four_stars, 
        five_stars,
        Camp,
        FamilyHotel,
        Motel
    }

}