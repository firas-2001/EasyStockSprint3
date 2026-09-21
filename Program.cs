using EasyStock.Controllers;
using EasyStock.Data;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var emailSettingsSection = builder.Configuration.GetSection("EmailSettings");
if (!emailSettingsSection.Exists())
{
    emailSettingsSection = builder.Configuration.GetSection("Email");
}

builder.Services.Configure<EmailSettings>(emailSettingsSection);

builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<FournisseurService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<EmployeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AffectationService>();
builder.Services.AddScoped<StockMovementService>();
builder.Services.AddScoped<HistoryService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<SessionAuthFilter>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

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
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

