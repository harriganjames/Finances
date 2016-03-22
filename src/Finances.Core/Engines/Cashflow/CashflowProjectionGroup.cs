using Finances.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.ValueObjects;

namespace Finances.Core.Engines.Cashflow
{
    public class CashflowProjectionGroup
    {
        readonly List<ICashflowProjectionMode> modes = new List<ICashflowProjectionMode>();
        //int activeModeIndex = 0;
        Dictionary<string, int> groupModeIndex = new Dictionary<string, int>();

        public CashflowProjectionGroup(IEnumerable<ICashflowProjectionMode> modes)
        {
            this.Items = new Dictionary<ICashflowProjectionMode, List<CashflowProjectionItem>>();

            this.modes.AddRange(modes.OrderBy(m=>m.Order));
        }

        public IEnumerable<ICashflowProjectionMode> Modes
        {
            get
            {
                return modes;
            }
        }

        private int GetModeIndex(string periodGroup)
        {
            if (!groupModeIndex.ContainsKey(periodGroup))
                groupModeIndex.Add(periodGroup, 0);
            return groupModeIndex[periodGroup];
        }

        public void SwitchMode(string periodGroup)
        {
            var index = GetModeIndex(periodGroup);

            if (++index >= modes.Count)
                index = 0;

            groupModeIndex[periodGroup] = index;
        }


        public void ResetModes()
        {
            groupModeIndex.Clear();
        }


        public Dictionary<ICashflowProjectionMode, List<CashflowProjectionItem>> Items { get; set; }

        public IEnumerable<CashflowProjectionItem> DefaultItems
        {
            get
            {
                return Items[modes[0]];
            }
        }

        public IEnumerable<CashflowProjectionItem> GetActiveItems(string periodGroup)
        {
            return Items[modes[GetModeIndex(periodGroup)]].Where(i => i.PeriodGroup == periodGroup);
        }

    }
}
