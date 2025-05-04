var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(Constants.ProductApiClientName, client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration.GetValue<string>("Url:ProductServiceApi")!}/api/product/");
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Search}/{id?}");

app.Run();
