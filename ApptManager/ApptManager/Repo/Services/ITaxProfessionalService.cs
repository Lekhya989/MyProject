using ApptManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApptManager.Services
{
    public interface ITaxProfessionalService
    {
        Task<string> CreateTaxProfessional(TaxProfessional taxPro);
        Task<List<TaxProfessional>> GetAllTaxProfessionals();
        Task<TaxProfessional?> GetTaxProfessionalById(int id);
        Task<string> UpdateTaxProfessional(int id, TaxProfessional taxPro);
        Task<string> RemoveTaxProfessional(int id);
    }
}