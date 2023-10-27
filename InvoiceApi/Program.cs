using AutoMapper;
using Invoice.Api.MapperProfiles;
using Invoice.Service.Interfaces;
using Invoice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new InvoiceProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

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
app.UseCors(options => options
        .WithOrigins("http://localhost:3000") // Adjust this if your client is on a different origin
        .AllowAnyMethod() // Allows all HTTP methods. Be more specific if needed.
        .AllowAnyHeader() // Allows all headers. Be more specific if needed.
        .AllowCredentials() // Allows cookies and credentials.
);

app.MapControllers();
app.Run();
