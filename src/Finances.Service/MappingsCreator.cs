using System.Collections.Generic;
using Finances.Interface;

namespace Finances.Service
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
