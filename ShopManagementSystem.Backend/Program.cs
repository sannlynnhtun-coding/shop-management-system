using ShopManagementSystem.Backend.Features;
using ShopManagementSystem.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<LocationRepository>();
builder.Services.AddScoped<ShopRepository>();
builder.Services.AddScoped<StaffRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddLocationEndpoint();
app.AddShopEndpoint();
app.AddStaffEndpoint();

app.Run();
