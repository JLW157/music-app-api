using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Core.interfaces.Utils;

namespace MusicAppApi.Core.Utils
{
    public class PagingUtils : IPagingUtils
    {
        public int GetPagerTotalPages(int pageSize, long totalItems)
        {
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }
    }
}
