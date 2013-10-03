using System;
using System.Collections.Generic;

namespace HardwareControl.Lab4
{
    public class LFSRInfo
    {
        private List<List<bool>> _sets;
        private String _info;

        public List<List<bool>> Sets
        {
            get
            {
                return _sets;
            }
        }
        public String Info
        {
            get
            {
                return _info;
            }
        }

        public LFSRInfo(List<List<bool>> sets, String info)
        {
            _sets = sets;
            _info = info;
        }
    }
}
