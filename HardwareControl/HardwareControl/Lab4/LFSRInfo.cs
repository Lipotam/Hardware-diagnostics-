using System;
using System.Collections.Generic;

namespace HardwareControl.Lab4
{
    public class LFSRInfo
    {
        private List<List<bool>> sets;
        private readonly String info;

        public List<List<bool>> Sets
        {
            get
            {
                return this.sets;
            }
        }
        public String Info
        {
            get
            {
                return this.info;
            }
        }

        public LFSRInfo(List<List<bool>> sets, String info)
        {
            this.sets = sets;
            this.info = info;
        }
    }
}
