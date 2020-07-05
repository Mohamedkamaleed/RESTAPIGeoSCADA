using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ODBCConnector;
using RESTAPIConnector;

namespace RESTAPIGeoSCADA
{
    class Program
    {
        static void Main(string[] args)
        {
            ODBC oDBC = new ODBC();
            oDBC.ConnectionString = "DSN=GeoSCADA;Uid=Administrator;Pwd=Administrator@123";
            oDBC.Connect();

            while (oDBC.IsConnected)
            {
                //Extract data from ODBC

                var Query = " Select \"FileId\", FILEOFFSET, \"RecordTime\", SEVERITY, ID, SOURCE, MESSAGE, ALARMSTATEDESC " +
                "From CDBEVENTJOURNAL " +
                "Where " +
                "(\"CDBEVENTJOURNAL\".\"RecordTime\" BETWEEN { OPC 'S - 10S' } AND { OPC 'S' } )";
                Console.WriteLine(Query);
                var data = oDBC.DataReaderMapToList<CDBEVENTJOURNAL>(oDBC.ReturnTable(Query));
                if (data.Count > 0)
                {
                    Console.WriteLine(data.Count + " Alarms are found");
                    //Send Data to REST API server
                    RESTAPI rESTAPI = new RESTAPI();
                    rESTAPI.BaseUrl = "";
                    rESTAPI.UploadUrl = "";
                    rESTAPI.UserName = "";
                    rESTAPI.Password = "";
                    foreach( CDBEVENTJOURNAL item in data)
                    {
                        rESTAPI.UploadData(JsonConvert.SerializeObject(item));

                    }




                }
                else
                {
                    Console.WriteLine("No Alarms to be transmitted");
                }





                Thread.Sleep(10000);
            }
            
        }
    }
}
