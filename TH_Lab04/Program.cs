using TH_Lab04.Data;
using Microsoft.EntityFrameworkCore;
using TH_Lab04.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký SchoolContext và kết nối với SQL Server
builder.Services.AddDbContext<SchoolContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Khởi tạo dữ liệu mẫu khi ứng dụng chạy
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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