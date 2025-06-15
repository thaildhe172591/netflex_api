using System.Data;
using Netflex.Application.Interfaces.Repositories;

namespace Netflex.Persistence.Repositories;

public class ReadOnlyRepository<T>(IDbConnection connection) : IReadOnlyRepository<T> where T : class
{
    protected readonly IDbConnection _connection = connection;
}