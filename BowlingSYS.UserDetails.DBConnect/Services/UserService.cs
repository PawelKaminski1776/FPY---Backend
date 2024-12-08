using BowlingSys.Contracts.UserDtos;
using BowlingSys.DBConnect;
using BowlingSys.Entities.UserDBEntities;
using Npgsql;
using System.Data;
using System.Text.Json;
namespace BowlingSys.DBConnect
{
    public class UserService
    {
        private readonly DBConnect _dbConnect;
        private readonly StoredProcedureExecutor _executor;

        public UserService(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
            _executor = new StoredProcedureExecutor();
        }

        public async Task<GetUserIDResult> CallCheckUserLogin_SP(string usernameOrEmail, string password)
        {
            var parameters = new[]
            {
            new NpgsqlParameter("p_username_or_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = usernameOrEmail },
            new NpgsqlParameter("p_password", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = password },
            new NpgsqlParameter("user_id_result", NpgsqlTypes.NpgsqlDbType.Uuid) { Direction = ParameterDirection.Output }
        };

            // Strategy Pattern Usage
            _executor.SetStrategy(new CheckUserLoginStrategy(_dbConnect));
            var result = await _executor.ExecuteAsync(parameters);

            Guid userId = result != DBNull.Value ? new Guid(result.ToString()) : Guid.Empty;
            return new GetUserIDResult { User_Id = userId };
        }

        public async Task<GetLoginResult> CallCheckUserExists_SP(string usernameOrEmail)
        {
            var parameters = new[]
            {
            new NpgsqlParameter("p_username_or_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = usernameOrEmail },
            new NpgsqlParameter("result", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output }
        };

            // Strategy Pattern Usage
            _executor.SetStrategy(new CheckUserExistsStrategy(_dbConnect));
            var result = await _executor.ExecuteAsync(parameters);

            bool active = result != DBNull.Value && Convert.ToBoolean(result);
            return new GetLoginResult { Active = active };
        }

        public async Task<ErrorMessage> CallAddNewUser_SP(UserCreationDto message)
        {
            var parameters = new[]
            {
            new NpgsqlParameter("p_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Email },
            new NpgsqlParameter("p_username", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Username },
            new NpgsqlParameter("p_password", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Password },
            new NpgsqlParameter("p_forename", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Forename },
            new NpgsqlParameter("p_surname", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Surname },
        };

            // Strategy Pattern Usage
            _executor.SetStrategy(new AddNewUserStrategy(_dbConnect));
            var result = await _executor.ExecuteAsync(parameters);

            return new ErrorMessage { message = JsonSerializer.Serialize(result) };
        }
    }
}