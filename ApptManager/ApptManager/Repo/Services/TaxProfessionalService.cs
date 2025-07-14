using ApptManager.Models;

using ApptManager.Repo;

namespace ApptManager.Services
{
    public class TaxProfessionalService : ITaxProfessionalService
    {
        private readonly ITaxProfessionalRepo _repo;
        public TaxProfessionalService(ITaxProfessionalRepo repo)
        {
            _repo = repo;
        }
        public Task<string> CreateTaxProfessional(TaxProfessional taxPro)
        {
            return _repo.Create(taxPro);
        }
        public Task<List<TaxProfessional>> GetAllTaxProfessionals()
        {
            return _repo.GetAll();
        }
        public Task<TaxProfessional?> GetTaxProfessionalById(int id)
        {
            return _repo.GetById(id);
        }
        public Task<string> UpdateTaxProfessional(int id, TaxProfessional taxPro)
        {
            return _repo.Update(taxPro, id);
        }
        public Task<string> RemoveTaxProfessional(int id)
        {
            return _repo.Remove(id);
        }
    }
}

