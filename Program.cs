using CsharpWebAt.Services;
using CsharpWebAt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Adicionado para autentica��o de cookies

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Registra o LogService como um Singleton
builder.Services.AddSingleton<LogService>();

// Configura o DbContext para usar SQLite
builder.Services.AddDbContext<CsharpWebAtContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o dos servi�os de autentica��o
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Define o caminho para a p�gina de login
        options.LogoutPath = "/Account/Logout"; // Define o caminho para a p�gina de logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Opcional: p�gina para acesso negado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tempo de expira��o do cookie de autentica��o
        options.SlidingExpiration = true; // Reinicia o tempo de expira��o a cada requisi��o
    });

// Adiciona servi�os de autoriza��o
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

// Adiciona os middlewares de autentica��o e autoriza��o
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapRazorPages();

app.Run();
