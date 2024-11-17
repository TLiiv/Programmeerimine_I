using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {


            if (!context.Users.Any())
            {
                
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "AwesomeUser",
                        FirstName = "Jaan",
                        LastName = "Mustikas",
                        Email = "jaan@mustikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "CoolUser",
                        FirstName = "Jaanus",
                        LastName = "Kaalikas",
                        Email = "jaanus@kaalikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "LameUser",
                        FirstName = "Kristjan",
                        LastName = "Kuul",
                        Email = "kristjan@kuul.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "CheatingUser",
                        FirstName = "Jaana",
                        LastName = "Vaarikas",
                        Email = "jaana@vaarikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    }
                };

                context.AddRange(users);
                context.SaveChanges();
            }
        }
    } 
}
