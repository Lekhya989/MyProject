using ApptManager.DTOs;
using ApptManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApptManager.Services
{
    public interface ITaxProfessionalService
    {
        Task<string> CreateTaxProfessional(CreateTaxProfessionalDto dto); // Custom logic
        Task<List<TaxProfessionalDto>> GetAllTaxProfessionals();        // From generic GetAllAsync
        Task<TaxProfessionalDto?> GetTaxProfessionalById(int id);       // From generic GetByIdAsync
        Task<string> UpdateTaxProfessional(int id, CreateTaxProfessionalDto dto); // Custom logic
        Task<int> RemoveTaxProfessional(int id);                     // From generic DeleteAsync
    }
}
