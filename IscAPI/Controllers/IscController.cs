//namespace IscAPI.Controllers
//{
//    public class IscController
//    {
//    }
//}

using Microsoft.AspNetCore.Mvc;
using IscAPI.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;


namespace IscAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IscController : ControllerBase
    {
        private readonly string _connectionString;

        public IscController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ingenieria>> GetIngenieros()
        {
            var ingenieros = new List<Ingenieria>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ingenieria", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ingenieros.Add(new Ingenieria
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Caracteristica = reader.IsDBNull(3) ? null : reader.GetString(3),
                        ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
            }
            return Ok(ingenieros);
        }

        [HttpGet("{id}")]
        public ActionResult<Ingenieria> GetIngenieroById(int id)
        {
            Ingenieria? ingeniero = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ingenieria WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ingeniero = new Ingenieria
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Caracteristica = reader.IsDBNull(3) ? null : reader.GetString(3),
                        ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4)
                    };
                }
            }
            return ingeniero != null ? Ok(ingeniero) : NotFound();
        }

        [HttpPost]
        public IActionResult CreateIngeniero([FromBody] Ingenieria ingeniero)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Ingenieria (Nombre, Apellido, Caracteristica, ImagenURL) VALUES (@nombre, @apellido, @caracteristica, @imagenUrl)", conn);
                cmd.Parameters.AddWithValue("@nombre", ingeniero.Nombre);
                cmd.Parameters.AddWithValue("@apellido", ingeniero.Apellido);
                cmd.Parameters.AddWithValue("@caracteristica", (object?)ingeniero.Caracteristica ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imagenUrl", (object?)ingeniero.ImagenURL ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            return CreatedAtAction(nameof(GetIngenieros), new { ingeniero.Nombre }, ingeniero);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateIngeniero(int id, [FromBody] Ingenieria ingeniero)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Ingenieria SET Nombre = @nombre, Apellido = @apellido, Caracteristica = @caracteristica, ImagenURL = @imagenUrl WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", ingeniero.Nombre);
                cmd.Parameters.AddWithValue("@apellido", ingeniero.Apellido);
                cmd.Parameters.AddWithValue("@caracteristica", (object?)ingeniero.Caracteristica ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imagenUrl", (object?)ingeniero.ImagenURL ?? DBNull.Value);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIngeniero(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Ingenieria WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }
    }
}
