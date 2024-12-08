using Npgsql;

namespace BowlingSys.DBConnect
{
    // Concrete Strategy for AddNewUSerStrategy Procedure
    public class AddNewUserStrategy : IStoredProcedureStrategy
    {
        private readonly DBConnect _dbConnect;

        public AddNewUserStrategy(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }

        public async Task<object> ExecuteAsync(NpgsqlParameter[] parameters)
        {
            string storedProcedure = "dbo.insert_user";
            return await _dbConnect.ExecuteInsertStoredProcedure(storedProcedure, parameters);
        }
    }
}