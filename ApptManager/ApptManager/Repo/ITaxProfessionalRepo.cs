using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface ITaxProfessionalRepo : IGenericRepository<TaxProfessional>
    {
        Task<string> Create(TaxProfessional taxPro);       
        Task<string> Update(TaxProfessional taxPro, int id); 
        Task<string> Remove(int id);                        
    }
}
