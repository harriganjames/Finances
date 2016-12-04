//using Castle.MicroKernel.ModelBuilder.Inspectors;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Castle.MicroKernel.SubSystems.Conversion;
//using Castle.Core;

//namespace Finances.Utility
//{
//    class PropertiesDependenciesModelInspectorExplicit : PropertiesDependenciesModelInspector
//    {
//        public PropertiesDependenciesModelInspectorExplicit(IConversionManager converter) : base(converter)
//        {
//        }

//        protected override void InspectProperties(ComponentModel model)
//        {
//            if(model.ComponentName.Name== "SortedListViewModelBase")
//                base.InspectProperties(model);

//        }
//    }
//}
