using PTL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.ViewModels
{
    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}