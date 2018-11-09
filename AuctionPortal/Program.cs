using AuctionPortal.DataAccessLayer.EntityFramework;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using System;
using System.Linq;

namespace AuctionPortal
{
    class Program
    {

        static void Main(string[] args)
        {
            using (var db = new AuctionPortalDbContext())
            {
                db.Users.Add(new User() { Username = "Milosko", PasswordSalt = "koniec", PasswordHash = "nepoviem", IsAdmin = true});

                db.SaveChanges();
            }

            using (var db = new AuctionPortalDbContext())
            {
                var users = db.Users.OrderBy(team => team.Username).ToList();

                foreach (var user in users)
                {
                    Console.WriteLine(user.Username + " " + user.PasswordSalt);
                }
            }

            Console.ReadLine();
        }
    }
}
