using Npgsql;

namespace BowlingSys.DBConnect
{
    // Concrete Strategy for CheckUserExists Procedure
    public class CheckUserExistsStrategy : IStoredProcedureStrategy
    {
        private readonly DBConnect _dbConnect;

        public CheckUserExistsStrategy(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }

        public async Task<object> ExecuteAsync(NpgsqlParameter[] parameters)
        {
            string storedProcedure = "dbo.checkuserexists";
            await _dbConnect.SelectAndRunStoredProcedure(storedProcedure, parameters);

            return parameters[1].Value;
        }
    }
}
