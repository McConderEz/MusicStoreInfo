using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Controllers;
using MusicStoreInfo.Api.Extensions;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Infrastructure;
using MusicStoreInfo.Services;
using MusicStoreInfo.Services.Services;
using MusicStoreInfo.Services.Services.AlbumService;
using MusicStoreInfo.Services.Services.CityService;
using MusicStoreInfo.Services.Services.CompanySerivce;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GenderService;
using MusicStoreInfo.Services.Services.GenreService;
using MusicStoreInfo.Services.Services.GroupService;
using MusicStoreInfo.Services.Services.ImageService;
using MusicStoreInfo.Services.Services.ListenerTypeService;
using MusicStoreInfo.Services.Services.MemberService;
using MusicStoreInfo.Services.Services.OwnershipTypeService;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.ShoppingCartService;
using MusicStoreInfo.Services.Services.SongService;
using MusicStoreInfo.Services.Services.SpecializationService;
using MusicStoreInfo.Services.Services.StoreService;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuthentication(configuration);

builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", config =>
    {
        config.LoginPath = "/Account/Login";
        config.ReturnUrlParameter = "ReturnUrl";
        config.AccessDeniedPath = "/Home/AccessDenied";
    });
//builder.Services.AddAuthorization();


string[] roleNames = { "Admin", "Manager", "Client" };

builder.Services.EnsureRolesExist(configuration.GetConnectionString("DefaultConnection"), roleNames);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MusicStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Внедрение зависимостей
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IDistrictService, DistrictService>();

builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<IGenderService, GenderService>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IListenerTypeRepository, ListenerTypeRepository>();
builder.Services.AddScoped<IListenerTypeService, ListenerTypeService>();

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddScoped<IOwnershipTypeRepository, OwnershipTypeRepository>();
builder.Services.AddScoped<IOwnershipTypeService, OwnershipTypeService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ISongService, SongService>();

builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();

builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStoreService, StoreService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
#endregion

var app = builder.Build();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
