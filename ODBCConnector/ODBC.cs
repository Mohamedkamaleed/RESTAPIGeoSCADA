using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ODBCConnector
{
   public class ODBC
    {
        public string ConnectionString { get; set; }
        private OdbcConnection DbConnection { get; set; }
        public bool IsConnected { get; set; }
        public  List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (Enumerable.Range(0, dr.FieldCount).Any(i => string.Equals(dr.GetName(i), prop.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }


                }
                list.Add(obj);
            }
            return list;
        }

        public void Connect()
        {
            try
            {
                 DbConnection = new OdbcConnection(ConnectionString);
                DbConnection.Open();
                IsConnected = true;
                Console.WriteLine("Connected Successfully");

            }
            catch ( Exception e)
            {

                Console.WriteLine("Communication Error");
                Console.WriteLine("Error: " + e.Message);
                Console.ReadLine();
            }


        }
        public  OdbcDataReader ReturnTable(string Query)
        {

           if (DbConnection.State == ConnectionState.Open)
            {
                OdbcCommand DbCommand = DbConnection.CreateCommand();
                DbCommand.CommandText = Query;
                //DbCommand.CommandText = "Select Id,FormattedValue,RecordTime from CDBHistoric where Id=140449";
                OdbcDataReader DbReader = DbCommand.ExecuteReader();

                return DbReader;
            }
            else
            {
                Console.WriteLine("Connection is not openned");

                return null;

            }

        }
  

    }
}
