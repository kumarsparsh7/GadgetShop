using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : Controller
{
    public static string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Connection Lifetime=0;";

    [HttpPost]
    public async Task<IActionResult> SaveCustomerData(CustomerRequestDTO requestDto)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using (var command = dataSource.CreateCommand("CALL sp_savecustomerdetails(@CustomerId, @FirstName, @LastName, @Email, @RegistrationDate, @PhoneNo)"))
        {
            command.Parameters.AddWithValue("CustomerId", requestDto.CustomerId);
            command.Parameters.AddWithValue("FirstName", requestDto.FirstName);
            command.Parameters.AddWithValue("LastName", requestDto.LastName);
            command.Parameters.AddWithValue("Email", requestDto.Email);
            command.Parameters.AddWithValue("RegistrationDate", NpgsqlTypes.NpgsqlDbType.Date, DateTime.Parse(requestDto.RegistrationDate));
            command.Parameters.AddWithValue("PhoneNo", requestDto.PhoneNo);
            await command.ExecuteNonQueryAsync();
        }
        // return Ok("Customer details saved successfully");
        return Ok(new { message = "Customer details saved successfully" });
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerData()
    {
        var CustomerDTOs = new List<CustomerRequestDTO>();
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        
        await using var command = dataSource.CreateCommand("SELECT customerid,firstname,lastname,email,phoneno,CAST(registrationdate as varchar(12)) FROM customerDetails");
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dto = new CustomerRequestDTO
            {
                CustomerId = reader.GetInt32(reader.GetOrdinal("customerid")),
                RegistrationDate = reader.GetString(reader.GetOrdinal("registrationdate")),
                FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                LastName = reader.GetString(reader.GetOrdinal("lastname")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                PhoneNo = reader.GetString(reader.GetOrdinal("phoneno")),
            };
            CustomerDTOs.Add(dto);
        }
        return Ok(JsonConvert.SerializeObject(CustomerDTOs));
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCustomerData(int CustomerId)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        
        await using (var command = dataSource.CreateCommand("CALL sp_deletecustomerdetails(@customerid)"))
        {
            command.Parameters.AddWithValue("customerid", CustomerId);
            await command.ExecuteNonQueryAsync();
        }
        return Ok(new { message = $"Deleted Customer data with ID: {CustomerId} successfully"});
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateCustomerData(CustomerRequestDTO requestDto)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using (var command = dataSource.CreateCommand("CALL sp_updatecustomerdetails(@_customerid, @_firstname, @_lastname, @_email, @_registrationdate, @_phoneno)"))
        {
            command.Parameters.AddWithValue("@_customerid", requestDto.CustomerId);
            command.Parameters.AddWithValue("@_firstname", requestDto.FirstName);
            command.Parameters.AddWithValue("@_lastname", requestDto.LastName);
            command.Parameters.AddWithValue("@_email", requestDto.Email);
            command.Parameters.AddWithValue("@_phoneno", requestDto.PhoneNo);
            command.Parameters.AddWithValue("@_registrationdate", NpgsqlTypes.NpgsqlDbType.Date, DateTime.Parse(requestDto.RegistrationDate));
            await command.ExecuteNonQueryAsync();
        }
        return Ok(new { message = $"Customer details with ID: {requestDto.CustomerId} updated successfully"});
    }
}