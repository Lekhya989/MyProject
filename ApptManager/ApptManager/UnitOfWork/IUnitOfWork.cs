using ApptManager.Repo;

namespace ApptManager.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepo Users { get; }
        ITaxProfessionalRepo TaxProfessionals { get; }
        IBookingRepo Bookings { get; }
        ISlotRepo Slots { get; }

        Task<int> CompleteAsync();
    }

}
