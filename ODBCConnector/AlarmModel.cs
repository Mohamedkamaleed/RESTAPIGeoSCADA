using System;
using System.Collections.Generic;
using System.Text;

namespace ODBCConnector
{

    public class CDBEVENTJOURNAL
    {
        public string FileId { get; set; }
        public int FileOffset { get; set; }
        public DateTime RecordTime { get; set; }
        public  string Severity { get; set; }
        public int Id { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string AlarmStateDes { get; set; }
    }



   public class AlarmInformation
    {
       public string SiteId { get; set; }
        public string AlarmId { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public string Message { get; set; }
    }

    public class AlarmPacket
    {
        public uint id { get; set; }
        public string baseID { get; set; }
        public int status = -1;
        public CDBEVENTJOURNAL data = new CDBEVENTJOURNAL();

    }
}
