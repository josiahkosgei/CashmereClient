namespace Cashmere.Finacle.Integration.CQRS.Interfaces
{
    public interface IResult
    {
    }

    public interface IListResult : ICollection<IResult>
    {
    }
}
