using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//�������� ������ ����������� �� ����� ������������ appsettings.json
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//��������� �������� MusicStoreDbContext � �������� ������� � ����������
builder.Services.AddDbContext<MusicStoreDbContext>(options => options.UseSqlServer(connection));

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
