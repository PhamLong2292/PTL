using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.AdminApp.Models
{
    public class Helper
    {
        public enum eEffect
        {
            No,
            Yes
        }
        public static class Effect
        {
            public const string No = "Hết hiệu lực";
            public const string Yes = "Hiệu lực";
        }
    }
}
