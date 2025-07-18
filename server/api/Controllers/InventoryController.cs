using System.Data;
using System.Text.Json.Serialization;
using Npgsql;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InventoryController : Controller
{
    public static string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Connection Lifetime=0;";

    [HttpPost]
    public async Task<IActionResult> SaveInventoryData(InventoryRequestDTO requestDto)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using (var command = dataSource.CreateCommand("CALL sp_saveinventorydata(@ProductId, @ProductName, @AvailableQty, @ReorderPoint)"))
        {
            command.Parameters.AddWithValue("ProductId", requestDto.ProductId);
            command.Parameters.AddWithValue("ProductName", requestDto.ProductName);
            command.Parameters.AddWithValue("AvailableQty", requestDto.AvailableQty);
            command.Parameters.AddWithValue("ReorderPoint", requestDto.ReorderPoint);
            await command.ExecuteNonQueryAsync();
        }
        return Ok("Inventory details saved successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetInventoryData()
    {
        var inventoryDTOs = new List<InventoryRequestDTO>();
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        
        // await using var command = dataSource.CreateCommand("CALL sp_getinventorydata()");
        await using var command = dataSource.CreateCommand("SELECT * FROM inventory");
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dto = new InventoryRequestDTO
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                AvailableQty = reader.GetInt32(reader.GetOrdinal("AvailableQty")),
                ReorderPoint = reader.GetInt32(reader.GetOrdinal("ReorderPoint"))
            };
            inventoryDTOs.Add(dto);
        }
        return Ok(JsonConvert.SerializeObject(inventoryDTOs));
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteInventoryData(int ProductId)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        
        await using (var command = dataSource.CreateCommand("CALL sp_deleteinventorydata(@pid)"))
        {
            command.Parameters.AddWithValue("pid", ProductId);
            await command.ExecuteNonQueryAsync();
        }
        return Ok($"Deleted inventory data with ID: {ProductId} successfully");
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateInventoryData(InventoryRequestDTO requestDto)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using (var command = dataSource.CreateCommand("CALL sp_updateinventorydata(@ProductId, @ProductName, @AvailableQty, @ReorderPoint)"))
        {
            command.Parameters.AddWithValue("ProductId", requestDto.ProductId);
            command.Parameters.AddWithValue("ProductName", requestDto.ProductName);
            command.Parameters.AddWithValue("AvailableQty", requestDto.AvailableQty);
            command.Parameters.AddWithValue("ReorderPoint", requestDto.ReorderPoint);
            await command.ExecuteNonQueryAsync();
        }
        return Ok("Inventory details updated successfully");
    }
}