using ApptManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApptManager.Repo
{
    public interface ITaxProfessionalRepo
    {
        Task<string> Create(TaxProfessional taxPro);
        Task<List<TaxProfessional>> GetAll();
        Task<TaxProfessional?> GetById(int id);
        Task<string> Update(TaxProfessional taxPro, int id);
        Task<string> Remove(int id);
    }
}