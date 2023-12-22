using Newtonsoft.Json;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;

namespace ApiTest.Repository
{
    public class BaseRepositository
    {
        public static BaseRepositository Insten()
        {
            return new BaseRepositository();
        }

        private DbConnection ConnectionString()
        {
            return new SqlConnection("Server=KH-NB042;Database=master;User Id=sa;Password=123;");
        }

        public IEnumerable<T2> QuerySql<T2>(string sqlScript, object param = null, int? commandTimeout = null)
        {
            using (DbConnection sqlConnection = ConnectionString())
            {
                return sqlConnection.Query<T2>(sqlScript, param, null, true, commandTimeout);
            }
        }

    }
}
