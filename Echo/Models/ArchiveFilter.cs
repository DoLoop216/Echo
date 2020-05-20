using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Models
{
    public class ArchiveFilter
    {
        public bool ShowFilter { get; set; }
        public bool OnlySalzburg { get; set; }
        public int FilterGroupID { get; set; }

        public ArchiveFilter(int? ShowFilter, int? OnlySalzburg, int? cid)
        {
            this.ShowFilter = (ShowFilter == null) ? false : Convert.ToBoolean(ShowFilter);
            this.OnlySalzburg = (OnlySalzburg != null && OnlySalzburg == 1) ? true : false;
            this.FilterGroupID = (cid == null) ? -1 : (int)cid;
        }
    }
}
