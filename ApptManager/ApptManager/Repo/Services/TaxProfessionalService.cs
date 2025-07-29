using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.UnitOfWork;
using AutoMapper;

namespace ApptManager.Services
{
    public class TaxProfessionalService : ITaxProfessionalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaxProfessionalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> CreateTaxProfessional(CreateTaxProfessionalDto dto)
        {
            var entity = _mapper.Map<TaxProfessional>(dto);
            return await _unitOfWork.TaxProfessionals.Create(entity);
        }

        public async Task<List<TaxProfessionalDto>> GetAllTaxProfessionals()
        {
            var taxPros = await _unitOfWork.TaxProfessionals.GetAllAsync();
            return _mapper.Map<List<TaxProfessionalDto>>(taxPros);
        }

        public async Task<TaxProfessionalDto?> GetTaxProfessionalById(int id)
        {
            var entity = await _unitOfWork.TaxProfessionals.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TaxProfessionalDto>(entity);
        }

        public async Task<string> UpdateTaxProfessional(int id, CreateTaxProfessionalDto dto)
        {
            var entity = _mapper.Map<TaxProfessional>(dto);
            return await _unitOfWork.TaxProfessionals.Update(entity, id);
        }

        public Task<int> RemoveTaxProfessional(int id)
        {
            return _unitOfWork.TaxProfessionals.DeleteAsync(id);
        }
    }
}
