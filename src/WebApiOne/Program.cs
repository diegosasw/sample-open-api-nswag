var builder = WebApplication.CreateBuilder(args);

// Register NSwag
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();

// The goal is to skip certain things when running NSwag
// View the nswag.json to see how NSwag is set as environment
var isNSwagExecution = builder.Environment.IsEnvironment("NSwag");
if (!isNSwagExecution)
{
    throw new Exception("Sample exception to demonstrate that NSwag should skip this");
}

var app = builder.Build();

// Use NSwag
app.UseOpenApi();
app.UseSwaggerUi();

app.MapGet("/", () => "Hello World!");

app.Run();