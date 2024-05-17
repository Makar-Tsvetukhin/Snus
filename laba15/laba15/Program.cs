using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ����������� URL Rewriting
var options = new RewriteOptions();

// ���������� ������ �� ����� urlrewrite.xml (������ IIS)   
IHostEnvironment env = app.Services.GetRequiredService<IHostEnvironment>();
options.AddIISUrlRewrite(env.ContentRootFileProvider, "urlrewrite.xml");

// ���������� ������ �� ����� rewrite.txt (������ Apache)
options.AddApacheModRewrite(env.ContentRootFileProvider, "rewrite.txt");

// ���������� ����������������� ������� ��� ��������������� �������� � .php �� .html
options.Add(new RedirectPHPRequests(extension: "html", newPath: "/html"));

app.UseRewriter(options);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();