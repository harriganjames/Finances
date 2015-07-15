﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Interfaces
{
    public interface ICashflowEngineFactoryB
    {
        ICashflowEngineB CreateDetail();
        ICashflowEngineB CreateMonthlySummary();
    }
}