
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace FuncionariosDapper.Infra.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("FuncionariosDapper") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
