using BowlingSys.DBConnect;
using Npgsql;

namespace BowlingSys.DBConnect
{
    // Concrete Strategy for CheckUserLogin Procedure
    public class CheckUserLoginStrategy : IStoredProcedureStrategy
    {
        private readonly DBConnect _dbConnect;

        public CheckUserLoginStrategy(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }

        public async Task<object> ExecuteAsync(NpgsqlParameter[] parameters)
        {
            string storedProcedure = "dbo.checklogindetails";
            await _dbConnect.SelectAndRunStoredProcedure(storedProcedure, parameters);

            return parameters[2].Value; 
        }
    }
}
