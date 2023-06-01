var builder = WebApplication.CreateBuilder(args);
String AllowSpecificOrigins = "AllowSpecificOrigins";
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

var app = builder.Build();

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
