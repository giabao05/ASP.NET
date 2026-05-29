using CMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Dịch vụ Database (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Khai báo cấu hình dịch vụ xác thực bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";              // Chuyển hướng nếu chưa đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Chuyển hướng nếu vào trang không đủ quyền
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Tự động hết hạn phiên làm việc sau 60 phút
        options.SlidingExpiration = true;                   // Tự động gia hạn thời gian nếu user còn hoạt động
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. CẦN THIẾT: Phải gọi Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();