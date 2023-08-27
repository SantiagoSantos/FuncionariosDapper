
using Dapper;
using FuncionariosDapper.Application.Dtos;
using FuncionariosDapper.Domain.Entities;
using FuncionariosDapper.Domain.Repositories;
using FuncionariosDapper.Infra.Context;
using System.Data;
using System.Text;

namespace FuncionariosDapper.Infra.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DapperContext _dapperContext;
        public CompanyRepository(DapperContext context) => _dapperContext = context;

        public async Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            var query = "INSERT Companies (Name, Address, Country) VALUES (@name, @address, @country); SELECT LAST_INSERT_ID();";
            var parameters = new DynamicParameters();
            using var conn = _dapperContext.CreateConnection();
            parameters.Add("name", company.Name, DbType.String);
            parameters.Add("address", company.Address, DbType.String);
            parameters.Add("country", company.Country, DbType.String);

            var id = await conn.ExecuteScalarAsync<int>(query, parameters);

            var createdCompany = new Company
            {
                Id = id,
                Name = company.Name,
                Address = company.Address,
                Country = company.Country,
            };

            return createdCompany;
        }

        public async Task DeleteCompany(int id)
        {
            var query = "DELETE FROM companies WHERE id = @id";
            using var conn = _dapperContext.CreateConnection();

            await conn.ExecuteAsync(query);
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            var query = "SELECT * FROM companies";

            using var conn = _dapperContext.CreateConnection();
            var companies = await conn.QueryAsync<Company>(query);
            return companies.ToList();

        }

        public async Task<Company> GetById(int id)
        {
            var query = "SELECT * FROM companies WHERE id = @id";

            using var conn = _dapperContext.CreateConnection();
            var company = await conn.QueryFirstOrDefaultAsync<Company>(query, new { id });

            return company;
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            using var conn = _dapperContext.CreateConnection();
            var procedure = "SP_SHOW_COMPANIIES_WITH_EMPLOYEE";
            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            return await conn.QueryFirstOrDefaultAsync<Company>(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Company> GetCompanyEmployee(int id)
        {
            using var conn = _dapperContext.CreateConnection();
            var query = new StringBuilder();
            query.Append("SELECT * FROM companies WHERE id = @Id;");
            query.Append("SELECT * FROM Employees WHERE companyId = @Id");
            using var result = await conn.QueryMultipleAsync(query.ToString(), new { id });

            var company = await result.ReadSingleOrDefaultAsync<Company>();
            if (company != null) company.Employees = (await result.ReadAsync<Employee>()).ToList();

            return company!;

        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var conn = _dapperContext.CreateConnection();
            var query = new StringBuilder();
            query.AppendLine("UPDATE companies SET name = @name, address = @address, country = @country ");
            query.AppendLine("WHERE id = @id");

            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("name", company.Name, DbType.String);
            parameters.Add("address", company.Address, DbType.String);
            parameters.Add("country", company.Country, DbType.String);

            await conn.ExecuteAsync(query.ToString(), parameters);
        }
    }
}
