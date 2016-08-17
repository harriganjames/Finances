using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Windsor;
using System.Diagnostics;

namespace Finances.IntegrationTests.MS
{
    public abstract class IntegrationBase
    {
        public WindsorContainer container;

        public IntegrationBase()
        {
            container = new IntegrationContainer().Container;

            //container.Resolve<Finances.Core.MappingsCreator>();
        }



    }
}
