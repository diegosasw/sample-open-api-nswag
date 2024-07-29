var builder = WebApplication.CreateBuilder(args);

// Register NSwag
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Use NSwag
app.UseOpenApi();
app.UseSwaggerUi();

app.MapGet("/", () => "Hello World!");

app.Run();