using Microsoft.EntityFrameworkCore;
using News.Admin.Data;
using News.Admin.IService;
using News.Admin.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("NewsConn");
    options.UseSqlServer(connString);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();
