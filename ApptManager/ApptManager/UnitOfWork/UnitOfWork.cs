using ApptManager.Models.Data.WebApi.Models.Data;
using ApptManager.Repo;

namespace ApptManager.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperDBContext _context;

        public IUserRepo Users { get; private set; }
        public ITaxProfessionalRepo TaxProfessionals { get; private set; }
        public IBookingRepo Bookings { get; private set; }
        public ISlotRepo Slots { get; private set; }

        public UnitOfWork(DapperDBContext context,
                          IUserRepo users,
                          ITaxProfessionalRepo taxPros,
                          IBookingRepo bookings,
                          ISlotRepo slots)
        {
            _context = context;
            Users = users;
            TaxProfessionals = taxPros;
            Bookings = bookings;
            Slots = slots;
        }

        public Task<int> CompleteAsync()
        {
            // In Dapper, you usually don't track context changes like EF.
            // If using transactions, you can commit them here.
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            
        }
    }

}
