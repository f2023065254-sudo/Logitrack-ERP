var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// 1. Session Services ko add karein aur Cookie timeout set karein
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session 30 mins baad expiry hogi
    options.Cookie.HttpOnly = true;                // Security: XSS attacks se bachane ke liye
    options.Cookie.IsEssential = true;             // GDPR/Privacy compliant
});

// 2. HTTP Context Accessor ko add karein (Taakay Layouts/Views mein session access ho sake)
builder.Services.AddHttpContextAccessor();

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

// 3. IMPORTANT: Authorization se PEHLE Session Middleware lazmi call karein
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Default route login par set karein

app.Run();