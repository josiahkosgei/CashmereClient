
namespace CashmereDeposit.ViewModels.DummyData
{
    public class DummyDataViewModel
    {
        public DummyDataViewModel()
        {
            CurrentTransaction = new DummyAppTransactionDenominations();
        }

        public DummyAppTransactionDenominations CurrentTransaction { get; set; }
    }
}
