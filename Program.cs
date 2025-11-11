using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with the connection string from the appsettings.json file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Authorization service
builder.Services.AddAuthorization();

// Optional: Add authentication if you're using cookies or JWT for authentication
// Example for cookie authentication (optional, if you're not using it, remove this)
builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Enables HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add authentication middleware (if using authentication)
app.UseAuthentication();  // Make sure this is before UseAuthorization

app.UseRouting();

// Add authorization middleware
app.UseAuthorization();

// Set up default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
