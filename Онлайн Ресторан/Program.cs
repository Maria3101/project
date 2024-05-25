using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
var builder = WebApplication.CreateBuilder(args);

// Строка подключения
string connStr = /*"workstation id=MyBase6.mssql.somee.com;packet size=4096; " + "user id=Masha_SQLLogin_1;pwd=twc9vfdam1;" + "data source=MyBase6.mssql.somee.com;" + "persist security info=False;initial catalog=MyBase6;" + "TrustServerCertificate = true";*/
"Server = (localdb)\\mssqllocaldb;Database = MyBase6; Trusted_Connection = true";

// Добавление сервисов
builder.Services.AddDbContext<MyBaseContext>(
options => options.UseSqlServer(connStr));
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
