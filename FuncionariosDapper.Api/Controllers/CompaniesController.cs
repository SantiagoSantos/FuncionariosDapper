using FuncionariosDapper.Application.Dtos;
using FuncionariosDapper.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FuncionariosDapper.Api.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository repository)
        {
            _companyRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                return Ok(await _companyRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            try
            {
                var company = await _companyRepository.GetById(id);
                return company == null ? NotFound() : Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyByEmployeeId(int id)
        {
            try
            {
                var company = await _companyRepository.GetCompanyByEmployeeId(id);
                return company == null ? NotFound() : Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/CompanyWithEmployees")]
        public async Task<IActionResult> GetCompanyWithEmployees(int id)
        {
            try
            {
                var company = await _companyRepository.GetCompanyEmployee(id);
                return company == null ? NotFound() : Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var createdCompany = await _companyRepository.CreateCompany(company);
                return CreatedAtAction("GetCompany", new { id = createdCompany.Id }, createdCompany);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await _companyRepository.GetById(id);

                if (dbCompany == null) return NotFound();

                await _companyRepository.UpdateCompany(id, company);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                var dbCompany = await _companyRepository.GetById(id);

                if (dbCompany == null) return NotFound();

                await _companyRepository.DeleteCompany(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
