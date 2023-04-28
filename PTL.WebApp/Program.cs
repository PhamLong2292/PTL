using Microsoft.AspNetCore.Authentication.Cookies;
using PTL.ApiIClient;
using PTL.WebApp.LocalizationResources;
using System.Globalization;
using FluentValidation.AspNetCore;
using PTL.ViewModels;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var cultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("vi"),
};
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                {
                    ops.UseAllCultureProviders = false;
                    ops.ResourcesPath = "LocalizationResources";
                    ops.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = cultures;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("vi");
                    };
                });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
 .AddCookie(options =>
 {
     options.LoginPath = "/Account/Login";
     options.AccessDeniedPath = "/User/Forbidden/";
 });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseRequestLocalization();

app.MapControllerRoute(
    name: "Product Category En",
    pattern: "{culture}/categories/{id}", new
    {
        controller = "Product",
        action = "Category"
    });

app.MapControllerRoute(
  name: "Product Category Vn",
  pattern: "{culture}/danh-muc/{id}", new
  {
      controller = "Product",
      action = "Category"
  });

app.MapControllerRoute(
    name: "Product Detail En",
    pattern: "{culture}/products/{id}", new
    {
        controller = "Product",
        action = "Detail"
    });

app.MapControllerRoute(
      name: "Product Detail Vn",
      pattern: "{culture}/san-pham/{id}", new
      {
          controller = "Product",
          action = "Detail"
      });

app.MapControllerRoute(
         name: "default",
         pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");

app.Run();