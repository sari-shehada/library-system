using MySql.Data.MySqlClient;

namespace DataAccessLayer.Config
{
    public class ConnectionSetting
    {
        public string ConnectionString { get; set; } = string.Empty;

        public async Task ExecuteWithConnection(Func<MySqlConnection, Task> action)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    await action.Invoke(connection);
                }
                finally
                {

                    connection.Close();
                }
            }
        }
    }
}