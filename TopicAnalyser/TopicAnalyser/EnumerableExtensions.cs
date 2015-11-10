using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicAnalyser
{
    public static class EnumerableExtensions
    {
        public static decimal Median(this IEnumerable<int> p_List)
        {
            // Implementation goes here.
            // Create a copy of the input, and sort the copy
            int[] temp = p_List.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            { 
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                int a = temp[count / 2 - 1];
                int b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }
    }
}
