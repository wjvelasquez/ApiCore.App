using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ApiCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace ApiCore.Controllers;
[EnableCors("CorsRules")]
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly string _stringSql;

    public ProductController(IConfiguration configuration)
    {
        _stringSql = configuration.GetConnectionString("Local")!;
    }

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        List<Product> products = new List<Product>();
        try
        {
            using (var connection = new SqlConnection(_stringSql))
            {
                connection.Open();
                var cmd = new SqlCommand("sp_getAll_products", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            Id = reader.GetInt32(0),
                            BarCod = reader[1].ToString(),
                            Name = reader[2].ToString(),
                            Brand = reader[3].ToString(),
                            Category = reader[4].ToString(),
                            Price = reader.GetDecimal(5)
                        });
                    }
                }
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = products });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message, response = products });
        }
    }
    [HttpGet]
    [Route("Get/{id:int}")]
    public IActionResult Get(int id)
    {
        var product = new Product();

        try
        {
            using (var connection = new SqlConnection(_stringSql))
            {
                connection.Open();

                var cmd = new SqlCommand("sp_get_product_by_id", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;


                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product.Id = reader.GetInt32(0);
                        product.BarCod = reader[1].ToString();
                        product.Name = reader[2].ToString();
                        product.Brand = reader[3].ToString();
                        product.Category = reader[4].ToString();
                        product.Price = reader.GetDecimal(5);
                    }
                }
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = product });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message, response = product });
        }
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create([FromBody] Product product)
    {
        try
        {
            using (var connection = new SqlConnection(_stringSql))
            {
                using (var cmd = new SqlCommand("sp_create_product", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@barCod", SqlDbType.VarChar,50).Value = product.BarCod;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = product.Name;
                    cmd.Parameters.Add("@brand", SqlDbType.VarChar, 50).Value = product.Brand;
                    cmd.Parameters.Add("@category", SqlDbType.VarChar, 50).Value = product.Category;
                    cmd.Parameters.Add("@price", SqlDbType.VarChar, 50).Value = product.Price;
                    cmd.ExecuteNonQuery();
                }
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "Created" });

        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
        }
    }
    [HttpPut]
    [Route("Edit")]
    public IActionResult Editar([FromBody] Product product)
    {
        try
        {

            using (var connection = new SqlConnection(_stringSql))
            {
                connection.Open();
                using (var cmd = new SqlCommand("sp_edit_product", connection))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = 
                                        product.Id== 0 ? DBNull.Value : product.Id;
                    cmd.Parameters.Add("@barCod", SqlDbType.VarChar, 50).Value =
                                        product.BarCod is null ? DBNull.Value : product.BarCod;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value =
                                        product.Name is null ? DBNull.Value : product.Name;
                    cmd.Parameters.Add("@brand", SqlDbType.VarChar, 50).Value = 
                                        product.Brand is null ? DBNull.Value : product.Brand;
                    cmd.Parameters.Add("@category", SqlDbType.VarChar, 50).Value = 
                                        product.Category is null ? DBNull.Value : product.Category;
                    cmd.Parameters.Add("@price", SqlDbType.VarChar, 50).Value =
                                        product.Price == 0 ? DBNull.Value : product.Price;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "Updated" });
        }
        catch (Exception error)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });

        }
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public IActionResult Eliminar(int id)
    {
        try
        {

            using (var connection = new SqlConnection(_stringSql))
            {
                connection.Open();
                using (var cmd = new SqlCommand("sp_delete_product", connection))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "deleted" });
        }
        catch (Exception error)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });

        }
    }


}
