using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.interfaces.Utils
{
    public interface IPagingUtils
    {
        int GetPagerTotalPages(int pageSize, long totalItems);
    }
}
