using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyHieuThuoc
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set { instance = value; }
        }

        private DataProvider() { }

        private string connectionSTR =
            "Server=QUANG-TRUNG;Database=QuanLyCuaHangThuoc_V2;Integrated Security=True;";

        private static string EnsureSchema(string cmdText)
        {
            if (string.IsNullOrWhiteSpace(cmdText)) return cmdText;
            var t = cmdText.Trim();
            if (t.Contains(".") || t.Contains(" ") || t.Contains("[")) return t;
            return "dbo." + t;
        }
        public DataTable ExecuteQuery(string cmdText, CommandType type, params SqlParameter[] parameters)
        {
            var table = new DataTable();

            try
            {
                using (var connection = new SqlConnection(connectionSTR))
                using (var command = new SqlCommand(type == CommandType.StoredProcedure ? EnsureSchema(cmdText) : cmdText, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = type;
                    if (parameters != null && parameters.Length > 0)
                        command.Parameters.AddRange(parameters);

                    connection.Open();
                    adapter.Fill(table);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return table;
        }


        public int ExecuteNonQuery(string cmdText, CommandType type, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionSTR))
            using (var command = new SqlCommand(type == CommandType.StoredProcedure ? EnsureSchema(cmdText) : cmdText, connection))
            {
                command.CommandType = type;
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string cmdText, CommandType type, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionSTR))
            using (var command = new SqlCommand(type == CommandType.StoredProcedure ? EnsureSchema(cmdText) : cmdText, connection))
            {
                command.CommandType = type;
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteScalar();
            }
        }
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            var paramList = BuildSqlParamsFromQuery(query, parameter);
            return ExecuteQuery(query, CommandType.Text, paramList);
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            var paramList = BuildSqlParamsFromQuery(query, parameter);
            return ExecuteNonQuery(query, CommandType.Text, paramList);
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            var paramList = BuildSqlParamsFromQuery(query, parameter);
            return ExecuteScalar(query, CommandType.Text, paramList);
        }

        private SqlParameter[] BuildSqlParamsFromQuery(string query, object[] values)
        {
            if (values == null || values.Length == 0) return Array.Empty<SqlParameter>();

            var names = Regex.Matches(query, @"@\w+")
                             .Cast<Match>()
                             .Select(m => m.Value)
                             .Distinct()
                             .ToList();

            var list = new List<SqlParameter>();
            for (int i = 0; i < Math.Min(names.Count, values.Length); i++)
                list.Add(new SqlParameter(names[i], values[i] ?? DBNull.Value));

            return list.ToArray();
        }

        internal void ExecuteQuery(object value)
        {
            throw new NotImplementedException();
        }
    }
}
