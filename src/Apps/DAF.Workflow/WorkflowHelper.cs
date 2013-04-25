using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Workflow;
using DAF.Workflow.Models;

namespace DAF.Workflow
{
    public class WorkflowHelper
    {
        public static IStateFlowService StateFlowService
        {
            get { return IocInstance.Container.Resolve<IStateFlowService>(); }
        }
    }
}
