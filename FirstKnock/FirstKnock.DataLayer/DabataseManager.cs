using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FirstKnock.DataLayer
{
    class DabataseManager
    {
        public string ConnectionString { get; set; }

        public DabataseManager(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        void Excute(string sql, object param)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, param);
            }
        }

    }
}
