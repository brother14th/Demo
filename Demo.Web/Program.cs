using Demo.Shared.ApiClient;
using Demo.Shared.Service;
using Demo.Shared.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure IChannelEngineApiClient 
builder.Services.AddHttpClient<IChannelEngineApiClient, ChannelEngineApiClient>(client  =>
{

    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]??"");
    client.DefaultRequestHeaders.Add("X-CE-KEY", builder.Configuration["ApiKey"]);
});

//Configure OrderService
builder.Services.AddScoped<IOrderService, OrderService>();

//Configure TopProductService
builder.Services.AddScoped<TopProductService, TopProductService>();

//Configure ProductService
builder.Services.AddScoped<IProductService, ProductService>();

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
