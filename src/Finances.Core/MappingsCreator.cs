using System.Collections.Generic;
using Finances.Core.Interfaces;

namespace Finances.Core
{
    public class MappingsCreator
    {
        public MappingsCreator(IEnumerable<IMappingCreator> mappers)
        {
            foreach (var mapper in mappers)
            {
                mapper.CreateMappings();
            }
        }
    }
}
