using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;
using BowlingSys;
using BowlingSys.Handlers.Handlers;
using BowlingSys.DBConnect;
using BowlingSys.Contracts.UserDtos;
using System.Reflection.PortableExecutable;
using Pawel.UserDetails.Process;
using BowlingSYS.UserDetails.Controllers.DtoFactory;
using NServiceBus;
using Microsoft.ServiceBus.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppConfiguration>(_ => AppConfiguration.Instance);
builder.Services.AddSingleton<IDtoFactory, DtoFactory>();

var appConfig = AppConfiguration.Instance;

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5001);
        options.ListenAnyIP(5002, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5000);
    });
}

builder.Services.AddScoped<DBConnect>(provider =>
{
    var connectionString = appConfig.GetSetting("ConnectionStrings:DefaultConnection");
    return new DBConnect(connectionString);
});

builder.Services.AddScoped<UserService>(provider =>
{
    var dbConnect = provider.GetRequiredService<DBConnect>();
    return new UserService(dbConnect);
});

builder.Services.AddScoped<CheckUserLoginStrategy>();
builder.Services.AddScoped<CheckUserExistsStrategy>();
builder.Services.AddScoped<AddNewUserStrategy>();

builder.Services.AddScoped<StoredProcedureExecutor>();
builder.Services.AddScoped<MyMessageHandler>();
builder.Services.AddScoped<UserExistsHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var endpointConfiguration = new EndpointConfiguration("NServiceBusHandlers");
string instanceId = Environment.MachineName;
endpointConfiguration.MakeInstanceUniquelyAddressable(instanceId);
endpointConfiguration.EnableCallbacks();

var settings = new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto,
    Converters =
    {
        new IsoDateTimeConverter
        {
            DateTimeStyles = DateTimeStyles.RoundtripKind
        }
    }
};
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.Settings(settings);

var transport = endpointConfiguration.UseTransport<LearningTransport>();
transport.StorageDirectory("/app/.learningtransport");
var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

var routing = transport.Routing();
routing.RouteToEndpoint(typeof(LoginDto), "NServiceBusHandlers");
routing.RouteToEndpoint(typeof(UserExistsDto), "NServiceBusHandlers");
routing.RouteToEndpoint(typeof(UserCreationDto), "NServiceBusHandlers");

var scanner = endpointConfiguration.AssemblyScanner().ScanFileSystemAssemblies = true;

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowAll");
app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
