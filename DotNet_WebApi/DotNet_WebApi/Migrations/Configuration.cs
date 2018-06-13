namespace DotNet_WebApi.Migrations
{
    using DotNet_WebApi.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DotNet_WebApi.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DotNet_WebApi.Models.ApplicationDbContext context)
        {
            var existCheckAdmin =
                (from p in context.Users
                 where p.UserName == "admin"
                 && !p.LockoutEnabled
                 select p).Any();

            if (!existCheckAdmin)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new ApplicationUser();
                        user.UserName = "admin";
                        user.PasswordHash = new PasswordHasher().HashPassword("admin");
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        user.LockoutEnabled = false;
                        context.Users.Add(user);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
