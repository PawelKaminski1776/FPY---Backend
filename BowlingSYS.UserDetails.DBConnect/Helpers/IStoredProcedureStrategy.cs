using Npgsql;

namespace BowlingSys.DBConnect
{
    public interface IStoredProcedureStrategy
    {
        Task<object> ExecuteAsync(NpgsqlParameter[] parameters);
    }

}
