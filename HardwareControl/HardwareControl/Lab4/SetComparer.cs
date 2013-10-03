using System.Collections.Generic;

namespace HardwareControl.Lab4
{
    public class SetComparer : IEqualityComparer<List<bool>>
    {
        public bool Equals(List<bool> list1, List<bool> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(List<bool> list)
        {
            return list.GetHashCode();
        }
    }
}
