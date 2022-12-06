using Strider.Posterr.Common;

namespace Strider.Posterr.Domain.Repositories;

public sealed class QueryResult<T> where T : Entity
{
    public QueryResult(IList<T> results, int totalRecords)
    {
        Results = results;
        TotalRecords = totalRecords;
    } 

    public IList<T> Results { get; }
    public int TotalRecords { get; }
}