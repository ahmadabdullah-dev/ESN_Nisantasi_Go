var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAPI();
builder.Services.AddBusiness(builder.Configuration);
builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts(); 
}

await app.Services.SeedDataAsync();

app.UseForwardedHeaders(); 

app.UseHttpsRedirection();

app.UseCors("AllowWeb"); 

app.UseAuthentication(); 
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();