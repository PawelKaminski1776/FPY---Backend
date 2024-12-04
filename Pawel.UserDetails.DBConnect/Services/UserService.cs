using BowlingSys.DBConnect;
using BowlingSys.Entities.UserDBEntities;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using AutoMapper;
using System;
using BowlingSys.Contracts.UserDtos;
using System.Text.Json;

namespace BowlingSys.Services.UserService
{
    public class UserService
    {
        private DBConnect.DBConnect _DBConnect;

        public UserService( DBConnect.DBConnect dBConnect)
        {
            _DBConnect = dBConnect;
        }

        public async Task<GetUserIDResult> CallCheckUserLogin_SP(string usernameOrEmail, string password, bool isEmail = false)
        {
            try
            {
                var parameters = new[]
                {
                new NpgsqlParameter("p_username_or_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = usernameOrEmail },
                new NpgsqlParameter("p_password", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = password },
                new NpgsqlParameter("user_id_result", NpgsqlTypes.NpgsqlDbType.Uuid)
                {
                    Direction = ParameterDirection.Output
                }
            };

                string storedProcedure = "dbo.checklogindetails";

                await _DBConnect.SelectAndRunStoredProcedure(storedProcedure, parameters);

                var result = parameters[2].Value;

                Guid userid = Guid.Empty;

                if (result != DBNull.Value)
                {
                    userid = new Guid(result.ToString());
                }

                return new GetUserIDResult
                {
                    User_Id = userid,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");
                return new GetUserIDResult
                {
                    User_Id = Guid.Empty,
                };
            }
        }


        public async Task<GetLoginResult> CallCheckUserExists_SP(string usernameOrEmail, bool isEmail = false)
        {
            try
            {
                var parameters = new[]
                {
                new NpgsqlParameter("p_username_or_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = usernameOrEmail },
                new NpgsqlParameter("result", NpgsqlTypes.NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
            };

                string storedProcedure = "dbo.checkuserexists";

                await _DBConnect.SelectAndRunStoredProcedure(storedProcedure, parameters);

                var result = parameters[1].Value;

                bool active = false;

                if (result != DBNull.Value)
                {
                    active = Convert.ToBoolean(result);
                }

                return new GetLoginResult
                {
                    Active = active,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");
                return new GetLoginResult
                {
                    Active = false,
                };
            }
        }


        public async Task<ErrorMessage> CallAddNewUser_SP(UserCreationDto message, bool isEmail = false)
        {
            try
            {
                var parameters = new[]
                {
                new NpgsqlParameter("p_email", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Email },
                new NpgsqlParameter("p_username", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Username },
                new NpgsqlParameter("p_password", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Password },
                new NpgsqlParameter("p_forename", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Forename },
                new NpgsqlParameter("p_surname", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.Surname },
            };

                string storedProcedure = "dbo.insert_user";

                var result = await _DBConnect.ExecuteInsertStoredProcedure(storedProcedure, parameters);

                return new ErrorMessage
                {
                    message = JsonSerializer.Serialize(result),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");
                return new ErrorMessage
                {
                    message = "unsuccessful: " + ex.Message,
                };
            }
        }
    }
}
