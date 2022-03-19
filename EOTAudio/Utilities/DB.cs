using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ACEAppAPI.Utilities
{
    public class DB
    {
        private readonly string ConnectionString;

        public DB(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public async Task<DataTable> ExecuteQueryForResult(string query)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(query, Connection);
                SqlDataReader reader = await Command.ExecuteReaderAsync();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                await reader.DisposeAsync();
                await Connection.CloseAsync();
                return dataTable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> ExecuteQuery(string query)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(query, Connection);
                int AffectedRows = await Command.ExecuteNonQueryAsync();
                await Connection.CloseAsync();
                return AffectedRows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<DataTable> ExecuteSafeQueryForResult(string query, List<Dictionary<string, dynamic>> parameters)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(null, Connection);
                Command.CommandText = query;
                for (int i = 0; i < parameters.Count; i++)
                {
                    string ParamName = parameters[i]["paramName"];
                    SqlDbType ParamType = parameters[i]["paramType"];
                    dynamic ParamValue = parameters[i]["paramValue"];
                    Command.Parameters.Add(ParamName, ParamType).Value = ParamValue;
                }
                SqlDataReader reader = await Command.ExecuteReaderAsync();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                await reader.DisposeAsync();
                await Connection.CloseAsync();
                return dataTable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> ExecuteSafeQuery(string query, List<Dictionary<string, dynamic>> parameters)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(null, Connection);
                Command.CommandText = query;
                for (int i = 0; i < parameters.Count; i++)
                {
                    string ParamName = parameters[i]["paramName"];
                    SqlDbType ParamType = parameters[i]["paramType"];
                    dynamic ParamValue = parameters[i]["paramValue"];
                    Command.Parameters.Add(ParamName, ParamType).Value = ParamValue;
                }
                int AffectedRows = await Command.ExecuteNonQueryAsync();
                await Connection.CloseAsync();
                return AffectedRows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> ExecuteStoredProcedure(string StoredProcedureName, List<Dictionary<string, dynamic>> parameters)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(StoredProcedureName, Connection);
                Command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parameters.Count; i++)
                {
                    string ParamName = parameters[i]["paramName"];
                    SqlDbType ParamType = parameters[i]["paramType"];
                    dynamic ParamValue = parameters[i]["paramValue"];
                    Command.Parameters.Add(ParamName, ParamType).Value = ParamValue;
                }
                int AffectedRows = await Command.ExecuteNonQueryAsync();
                await Connection.CloseAsync();
                return AffectedRows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<DataTable> ExecuteStoredProcedureForResults(string StoredProcedureName, List<Dictionary<string, dynamic>> parameters)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    await Connection.OpenAsync();
                }
                SqlCommand Command = new SqlCommand(StoredProcedureName, Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if(parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        string ParamName = parameters[i]["paramName"];
                        SqlDbType ParamType = parameters[i]["paramType"];
                        dynamic ParamValue = parameters[i]["paramValue"];
                        Command.Parameters.Add(ParamName, ParamType).Value = ParamValue;
                    }
                }
                SqlDataReader reader = await Command.ExecuteReaderAsync();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                await reader.DisposeAsync();
                await Connection.CloseAsync();
                return dataTable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
