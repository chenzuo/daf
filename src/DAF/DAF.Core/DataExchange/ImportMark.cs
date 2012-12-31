using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public class ImportMark
    {
        public string MarkId { get; set; }
        public string PackageId { get; set; }
        public string DataTypeName { get; set; }
        public string ZipFileName { get; set; }
        public string ObjectTypeName { get; set; }
        public string LastMarkId { get; set; }
        public string BeginMark { get; set; }
        public string EndMark { get; set; }
        public int InsertedObjectCount { get; set; }
        public int UpdatedObjectCount { get; set; }
        public int DeletedObjectCount { get; set; }
        public bool Success { get; set; }
        public string Errors { get; set; }
        public string OperatorId { get; set; }
        public DateTime OperationTime { get; set; }
        public string Remark { get; set; }
    }
}
