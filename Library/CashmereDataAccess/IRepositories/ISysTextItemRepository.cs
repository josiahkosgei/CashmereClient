using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ISysTextItemRepository : IAsyncRepository<SysTextItem>
    {
        public Task<SysTextItem> GetByTokenId(string tokenID);
    }
}