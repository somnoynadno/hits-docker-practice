using Mockups.Configs;
using Mockups.Repositories.MenuItems;
using Mockups.Repositories.Addresses;
using Mockups.Repositories.Carts;
using Mockups.Services.Addresses;
using Mockups.Services.Carts;
using Mockups.Services.Users;
using Mockups.Storage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mockups.Services.MenuItems;
using Mockups.Repositories.Orders;
using Mockups.Services.Orders;
using Mockups.Services.CartsCleanerService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

#region Configs
OrderConfig OrderConfig = new OrderConfig();
CartsCleanerConfig CartsCleanerConfig = new CartsCleanerConfig();
builder.Configuration.Bind("OrderTimeParams", OrderConfig);;
builder.Configuration.Bind("CartsCleaner", CartsCleanerConfig);
#endregion

#region Auth

builder.Services.AddIdentity<User, Role>() // ���������� identity � �������
    .AddEntityFrameworkStores<ApplicationDbContext>() // �������� ���������
    .AddSignInManager<SignInManager<User>>() // ����� �������� ����, ��� �������� ����������� ������ �������� � ���������������� ������� ������������
    .AddUserManager<UserManager<User>>() // ���������� ��� ��������� ������
    .AddRoleManager<RoleManager<Role>>(); // ���������� ��� ��������� �����

// ���������� cookie �������������� � ������ (����������� ������ �������������� � MVC �������� � Identity)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

#endregion

#region DependencyInjections
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAddressesService, AddressesService>();
builder.Services.AddScoped<IMenuItemsService, MenuItemsService>();
builder.Services.AddScoped<ICartsService, CartsService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddSingleton<CartsRepository>();

builder.Services.AddSingleton(OrderConfig);
builder.Services.AddSingleton(CartsCleanerConfig);

builder.Services.AddHostedService<CartsCleaner>();

builder.Services.AddHttpContextAccessor();

#endregion

#region DbConnection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

var app = builder.Build();

#region DbScope 

using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

#endregion


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

#region Middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
#endregion

await app.ConfigureIdentityAsync();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");

app.Run();
