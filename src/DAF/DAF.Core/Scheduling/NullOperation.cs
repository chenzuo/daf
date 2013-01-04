﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Scheduling
{
    public class NullOperation : IOperation
    {
        public void Start(Dictionary<string, string> paras)
        {
        }

        public void Active(Dictionary<string, string> paras)
        {
        }

        public void HandleError(Exception ex)
        {
        }

        public void Stop(Dictionary<string, string> paras)
        {
        }

        public string Name
        {
            get { return "NullOperation"; }
        }

        public string Description
        {
            get { return "NullOperation"; }
        }
    }
}