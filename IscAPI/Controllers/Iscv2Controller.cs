using IscAPI.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace IscAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/")]
    public class Iscv2Controller : ControllerBase
    {
        
        private readonly string _connectionString;

        public Iscv2Controller(IConfiguration configuration)
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
                ingenieros.ForEach(i =>
                {
                    i.Nombre = i.Nombre?.ToUpper();
                    i.Apellido = i.Apellido?.ToUpper();
                    i.Caracteristica = i.Caracteristica?.ToUpper();
                    i.ImagenURL = i.ImagenURL?.ToUpper();
                });

            }
            return Ok(ingenieros);
        }

    }
}
