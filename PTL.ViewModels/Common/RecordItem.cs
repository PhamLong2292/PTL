using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.ViewModels
{
    public class Record<T>
    {
        public long total_count;
        public bool incomplete_results;
        public RecordItemCls[] items;
    }
    public class RecordItemCls
    {
        public Guid id;
        public string text;

        public string Code;
        public string Name;
    }
}
