using Microsoft.AspNetCore.Identity;
using Project.BLL.ServiceInjection;
using Project.DAL.ContextClasses;
using Project.ENTITIES.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient(); // API consume edilecekse(kullanýlýcaksa) HTTP protokolünde client tarafýnda oldugumuzu Middleware'e belirtiyoruz.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromMinutes(5);
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
});



builder.Services.AddDbContextService();
builder.Services.AddIdentityServices();

// Identity Cookie oluþturuyor biz ise burada nasýl olmasýný yani ayarlamasýný yapýyoruz burada
builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    x.Cookie.Name = "MyCookie";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    // Ayný siteden giriþ varmý        o sitenin cookie'si sadece o site için geçerlidir(Strict)
    x.Cookie.SameSite = SameSiteMode.Strict;

    // Kiþi bulunamadýðý zaman buraya yönlendir
    x.LoginPath = new PathString("/Home/SignIn");

    // Kiþi bulunuyor fakat yetkisi yetmiyorsa buraya yönlendir
    x.AccessDeniedPath = new PathString("/Home/AccessDenied");
});


builder.Services.AddRepositoryService();
builder.Services.AddManagerServices();




WebApplication app = builder.Build();






// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.UseSession();

app.UseAuthentication();// Giriþ Yapmak
app.UseAuthorization(); // Yetkilendirmedir

app.MapControllerRoute(
    name: "Admin",
    pattern: "{Area}/{controller=Product}/{action=GetProducts}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SignIn}/{id?}");

app.Run();
