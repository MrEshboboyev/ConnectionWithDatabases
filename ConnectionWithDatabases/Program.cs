using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connection using "NPGSQL" with DB
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();

    // see PostgreSQL database version
    using (var command = new NpgsqlCommand("SELECT version()", connection))
    {
        string version = command.ExecuteScalar().ToString();
        Console.WriteLine($"PostgreSQL version : {version}");
    }

    // retrieve all data from "Cities"
    using (var command = new NpgsqlCommand("SELECT city_id, city_name, city_population" +
        " FROM \"Cities\"", connection))
    {
        using  (var reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                Console.WriteLine($"City ID : {reader.GetInt32(0)}, " +
                    $"City Name : {reader.GetString(1)}" +
                    $"City Population : {reader.GetInt32(2)}");
            }
        }
    }

    //// insert data to "Cities"
    //using (var command = new NpgsqlCommand("INSERT INTO \"Cities\" (city_id, city_name)" +
    //    " VALUES (@city_id, @city_name)", connection))
    //{
    //    command.Parameters.AddWithValue("city_id", 10);
    //    command.Parameters.AddWithValue("city_name", "Washington");
    //    int rowsAffected = command.ExecuteNonQuery();
    //    Console.WriteLine($"Rows affected : {rowsAffected}");
    //}

    //// update data to "Cities"
    //using (var command = new NpgsqlCommand("UPDATE \"Cities\" SET city_population = @population" +
    //    " WHERE city_id = @cityId", connection))
    //{
    //    command.Parameters.AddWithValue("population", 20000);
    //    command.Parameters.AddWithValue("cityId", 10);
    //    int rowsAffected = command.ExecuteNonQuery();
    //    Console.WriteLine($"Rows affected : {rowsAffected}");
    //}

    //// delete data to "Cities"
    //using (var command = new NpgsqlCommand("DELETE FROM \"Cities\" " +
    //    " WHERE city_id = @cityId", connection))
    //{
    //    command.Parameters.AddWithValue("cityId", 10);
    //    int rowsAffected = command.ExecuteNonQuery();
    //    Console.WriteLine($"Rows affected : {rowsAffected}");
    //}
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
