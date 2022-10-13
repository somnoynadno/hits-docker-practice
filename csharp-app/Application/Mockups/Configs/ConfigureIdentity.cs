using Mockups.Storage;
using Microsoft.AspNetCore.Identity;

namespace Mockups.Configs
{
    public static class ConfigureIdentity
    {
        public static async Task ConfigureIdentityAsync(this WebApplication app) // ключевое слово this в аргументе метода обозначает, что это будет метод расширения
        {
            using var serviceScope = app.Services.CreateScope();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>(); // регистрация пользователей проходит при помощи UserManager
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>(); // создание ролей происходит через RoleManager
            var config = app.Configuration.GetSection("AdminCreds");
            var adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator); // Пытаемся найти роль админа
            if (adminRole == null) // Если ее еще нет в БД, создаем
            {
                var roleResult = await roleManager.CreateAsync(new Role
                {
                    Name = ApplicationRoleNames.Administrator,
                    Type = RoleType.Administrator
                });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Administrator} role.");
                }
                adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator); // После создания получаем роль еще раз
            }
            var adminUser = await userManager.FindByNameAsync(config["Email"]); // Достаем из БД пользователя админа
            if (adminUser == null) // Если нет, создаем
            {
                var userResult = await userManager.CreateAsync(new User
                {
                    UserName = config["Email"],
                    Email = config["Email"],
                    Name = "Cist Yana",
                    BirthDate = new DateTime(1990, 1, 1),
                    Phone = "88005553535"
                }, config["Password"]); // Стандарный метод регистрации пользователя через создание сущности пользователя и его пароля
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {config["Email"]} user");
                }
                adminUser = await userManager.FindByNameAsync(config["Email"]);
            }
            if (!await userManager.IsInRoleAsync(adminUser, adminRole.Name)) // Если пользователь админ еще не был назначен на роль админа, назначаем
            {
                await userManager.AddToRoleAsync(adminUser, adminRole.Name); // назначение на роль происходит через сущность пользователя и название роли (строка). Если при обычной процедуре регистрации нужно назначить роль обычного пользователя к новому зарегистрированному, необходимо будет после регистрации использовать данный метод для назначения пользователя на роль.
            }
            var userRole = await roleManager.FindByNameAsync(ApplicationRoleNames.User);
            if (userRole == null) // создание роли обычного пользователя
            {
                var roleResult = await roleManager.CreateAsync(new Role
                {
                    Name = ApplicationRoleNames.User,
                    Type = RoleType.User
                });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.User} role.");
                }
            }
        }

    }
}
