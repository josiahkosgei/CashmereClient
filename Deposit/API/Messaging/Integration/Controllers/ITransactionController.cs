// ITransactionController


using Cashmere.API.Messaging.Integration.Transactions;

namespace Cashmere.API.Messaging.Integration.Controllers
{
    public interface ITransactionController
    {
        Task<PostTransactionResponse> PostTransactionAsync(PostTransactionRequest request);

        Task<PostCITTransactionResponse> PostCITTransactionAsync(PostCITTransactionRequest request);
    }
}
