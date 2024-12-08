using BowlingSys.DBConnect;
using Npgsql;

namespace BowlingSys.DBConnect
{
    public class StoredProcedureExecutor
    {
        private IStoredProcedureStrategy _strategy;

        public void SetStrategy(IStoredProcedureStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<object> ExecuteAsync(NpgsqlParameter[] parameters)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Strategy is not set.");
            }

            return await _strategy.ExecuteAsync(parameters);
        }
    }
}
