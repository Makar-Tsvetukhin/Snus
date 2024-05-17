using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Подключение URL Rewriting
var options = new RewriteOptions();

// Добавление правил из файла urlrewrite.xml (формат IIS)   
IHostEnvironment env = app.Services.GetRequiredService<IHostEnvironment>();
options.AddIISUrlRewrite(env.ContentRootFileProvider, "urlrewrite.xml");

// Добавление правил из файла rewrite.txt (формат Apache)
options.AddApacheModRewrite(env.ContentRootFileProvider, "rewrite.txt");

// Добавление пользовательского правила для перенаправления запросов с .php на .html
options.Add(new RedirectPHPRequests(extension: "html", newPath: "/html"));

app.UseRewriter(options);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();