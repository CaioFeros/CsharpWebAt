using CsharpWebAt.Services;
using CsharpWebAt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Adicionado para autenticação de cookies

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Registra o LogService como um Singleton
builder.Services.AddSingleton<LogService>();

// Configura o DbContext para usar SQLite
builder.Services.AddDbContext<CsharpWebAtContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração dos serviços de autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Define o caminho para a página de login
        options.LogoutPath = "/Account/Logout"; // Define o caminho para a página de logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Opcional: página para acesso negado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tempo de expiração do cookie de autenticação
        options.SlidingExpiration = true; // Reinicia o tempo de expiração a cada requisição
    });

// Adiciona serviços de autorização
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adiciona os middlewares de autenticação e autorização
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapRazorPages();

app.Run();
