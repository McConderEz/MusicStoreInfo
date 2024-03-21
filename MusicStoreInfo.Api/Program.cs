using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Controllers;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Services.Services.AlbumService;
using MusicStoreInfo.Services.Services.CityService;
using MusicStoreInfo.Services.Services.CompanySerivce;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GenderService;
using MusicStoreInfo.Services.Services.GenreService;
using MusicStoreInfo.Services.Services.GroupService;
using MusicStoreInfo.Services.Services.ListenerTypeService;
using MusicStoreInfo.Services.Services.MemberService;
using MusicStoreInfo.Services.Services.OwnershipTypeService;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.SongService;
using MusicStoreInfo.Services.Services.SpecializationService;
using MusicStoreInfo.Services.Services.StoreService;

var builder = WebApplication.CreateBuilder(args);



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
#endregion

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
