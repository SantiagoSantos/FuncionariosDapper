using FuncionariosDapper.Application.Dtos;
using FuncionariosDapper.Domain.Entities;

namespace FuncionariosDapper.Domain.Repositories
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetAll();
        public Task<Company> GetById(int id);
        public Task<Company> CreateCompany(CompanyForCreationDto company);
        public Task UpdateCompany(int id, CompanyForUpdateDto company);
        public Task DeleteCompany(int id);
        public Task<Company> GetCompanyByEmployeeId(int id);
        public Task<Company> GetCompanyEmployee(int id);
    }
}
