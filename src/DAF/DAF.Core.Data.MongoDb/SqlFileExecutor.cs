using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DAF.Core.Data;

namespace DAF.Core.Data.MongoDb
{
    public class SqlFileExecutor : ISqlFileExecutor
    {
        private ProcessStartInfo startInfo;
        private string command;

        public void Initialize(string providerName, string connString)
        {
            startInfo = new ProcessStartInfo(providerName);
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            command = string.Format("mongo {0} ", connString);
        }

        public bool ExecuteSqlFile(string spliter, string sqlFile, bool continueOnError, out string messages)
        {
            messages = string.Empty;
            Process process = null;
            try
            {
                command += sqlFile;
                startInfo.Arguments = command;
                process = Process.Start(startInfo);
                process.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                messages = ex.Message;
                return false;
            }
            finally
            {
                process.Close();
            }
        }
    }
}
