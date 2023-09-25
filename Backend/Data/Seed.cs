using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Data
{
    public class Seed
    {
        public static async Task SeedUsers(AppDbContext context, ILogger<Seed> logger)
        {
            if (await context.Users.AnyAsync()) return;

            try
            {
                var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

                if (users == null || !users.Any())
                {
                    logger.LogInformation("No users data found in JSON file.");
                }

                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("teszter"));
                    user.PasswordSalt = hmac.Key;
                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error while deserializing JSON data");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Error while saving changes to the database");

            }
        }

    }
}
