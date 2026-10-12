
// I, Dhruv Patel, student number 000919050, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using LAB1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LAB1.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            var configuration =
                serviceProvider.GetRequiredService<IConfiguration>();

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var supervisorPassword =
                configuration["SeedUsers:SupervisorPassword"];

            var employeePassword =
                configuration["SeedUsers:EmployeePassword"];

            if (string.IsNullOrWhiteSpace(supervisorPassword) ||
                string.IsNullOrWhiteSpace(employeePassword))
            {
                throw new InvalidOperationException(
                    "Seed user passwords must be configured securely.");
            }

            string[] roles = { "Supervisor", "Employee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result =
                        await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Could not create role: {role}");
                    }
                }
            }

            await CreateUserAsync(
                userManager, "supervisor@lab.local", supervisorPassword,
                "John", "Supervisor", "Hamilton", "Supervisor");

            await CreateUserAsync(
                userManager, "employee@lab.local", employeePassword,
                "Jane", "Employee", "Hamilton", "Employee");
        }

        private static async Task CreateUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string firstName,
            string lastName,
            string city,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    City = city
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ", result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Could not create {email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not assign {role} role to {email}");
                }
            }
        }
    }
}
