var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

IronPdf.License.LicenseKey = "";

IronPdf.Logging.Logger.EnableDebugging = true;
IronPdf.Logging.Logger.LogFilePath = "Default.log";
IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
