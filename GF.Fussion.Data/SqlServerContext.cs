using Microsoft.Data.SqlClient;

namespace GF.Fussion.Data;

public sealed class SqlServerContext (SqlConnection connection) : ISqlServerContext
{
    private readonly SqlConnection _connection = connection;

    public async ValueTask DisposeAsync ()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
    public void Dispose ()
    {
        if (_connection != null)
        {
            _connection.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}

public interface ISqlServerContext : IDisposable, IAsyncDisposable
{

}