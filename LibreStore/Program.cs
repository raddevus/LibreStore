using System.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

String AllowSpecificOrigins = "AllowSpecificOrigins";
// Configuring ForwardHeaders so we can get IP address when runnnig NGINX
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddHostedService<LifetimeEventsHostedService>();
builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                builder =>{
                    builder /*WithOrigins("http://raddev.us",
                                        "http://www.raddev.us",
                                        "https://newlibre.com"
                                        )*/
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowAnyOrigin();             
                                        //.WithMethods("GET");
                });
            });
builder.Services.AddSingleton<AppConfig>();


var app = builder.Build();

String dbPassword = String.Empty;
if (args.Length > 0){
    dbPassword = args[0];
}

new AppConfig (app.Configuration,dbPassword);

// We only need UseForwardHeaders when running on Linux behind NGINX - to get ip addresses
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux)){
    Console.WriteLine("Running on on Linux...using ForwardHeaders");
    app.UseForwardedHeaders();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseCors(AllowSpecificOrigins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
