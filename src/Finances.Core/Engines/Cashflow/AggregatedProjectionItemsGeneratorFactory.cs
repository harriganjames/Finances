using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Engines.Cashflow
{
    //public class AggregatedProjectionItemsGeneratorFactory : IAggregatedProjectionItemsGeneratorFactory
    //{
    //    readonly IEnumerable<IAggregatedProjectionItemsGenerator> aggregatedProjectionItemsGenerators;

    //    Dictionary<string, IAggregatedProjectionItemsGenerator> index;


    //    public AggregatedProjectionItemsGeneratorFactory(
    //            IEnumerable<IAggregatedProjectionItemsGenerator> aggregatedProjectionItemsGenerators            
    //            )
    //    {
    //        this.aggregatedProjectionItemsGenerators = aggregatedProjectionItemsGenerators;

    //        index = this.aggregatedProjectionItemsGenerators.ToDictionary(g => g.ProjectionModeCode);
    //    }


    //    public IAggregatedProjectionItemsGenerator Create(string projectionMode)
    //    {
    //        IAggregatedProjectionItemsGenerator g;
    //        if (!index.TryGetValue(projectionMode, out g))
    //        {
    //            throw new Finances.Core.Exceptions.FinancesCoreException("Generator not found for ProjectionMode={0}", projectionMode);
    //        }
    //        return g;
    //    }
    //}
}
