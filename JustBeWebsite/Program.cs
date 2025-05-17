using JustBeSports.Core.Context;
using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Distributed in-memory cache for session
builder.Services.AddDistributedMemoryCache();

// Secure session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // GDPR compliance
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Enforce HTTPS
});

// Add DbContext with connection string
builder.Services.AddDbContext<JustBeSportsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Features
builder.Services.AddScoped<CategoryFeatures>();
builder.Services.AddScoped<ProductFeatures>();
builder.Services.AddScoped<ProductImageFeatures>();
builder.Services.AddScoped<ProductVariantFeatures>();
builder.Services.AddScoped<CartItemFeatures>();
builder.Services.AddScoped<OrderFeatures>();

// Register Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; " +
        "script-src 'self' https://cdn.jsdelivr.net https://code.jquery.com https://cdn.datatables.net; " +
        "style-src 'self' 'unsafe-inline' https://cdn.datatables.net https://cdn.jsdelivr.net; " +
        "font-src 'self' https://fonts.gstatic.com; " +
        "img-src 'self' data: https://cdn.datatables.net; " +
        "connect-src 'self'; " +
        "object-src 'none'; " +
        "frame-ancestors 'none';");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); 
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
