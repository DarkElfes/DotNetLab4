using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Hubs;
using Server.Models;
using Server.Models.Chats;
using Server.Models.Messages;
using Server.Repositories;
using Server.Repositories.IRepositories;
using Server.Services;
using Server.Services.IServices;
using System.Text;
using Newtonsoft.Json;
using Shared.DTOs.Response.ChatsDTO;
using Shared.DTOs.Request.Chat;
using Server.Helpers;
using Server.Hubs.ChatHubs.PersonalChatHub;
using Server.Hubs.ChatHubs.GroupChatHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


JwtSettings jwtSettings = new();
builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);

builder.Services.AddSingleton(Options.Create(jwtSettings));
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new()
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret)),
      };

      options.Events = new JwtBearerEvents
      {
          OnMessageReceived = context =>
          {
              var accessToken = context.Request.Query["access_token"];

              // If the request is for our hub...
              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) &&
                  (path.StartsWithSegments("/chatHub")))
              {
                  // Read the token out of the query string
                  context.Token = accessToken;
              }
              return Task.CompletedTask;
          }
      };
  });


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
});


builder.Services.AddDbContext<ApplicationDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Not set default connection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRepository<PersonalChat>, ChatRepository<PersonalChat>>();
builder.Services.AddScoped<IRepository<GroupChat>, ChatRepository<GroupChat>>();
builder.Services.AddScoped<IRepository<ChatMessage>, MessageRepository>();

builder.Services.AddScoped<IChatService<PersonalChatRequest>, PersonalChatService>();
builder.Services.AddScoped<IGroupChatService, GroupChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<UserHelper>();


builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol(options =>
    {
        options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.All;
        options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GroupChatHub>("/chatHub/group");
app.MapHub<PersonalChatHub>("/chatHub/personal");
app.MapControllers();

app.Run();
