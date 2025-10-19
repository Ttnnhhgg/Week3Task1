var builder = WebApplication.CreateBuilder(args);

// Добавьте сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ЗАКОММЕНТИРУЙТЕ эту строку:
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
