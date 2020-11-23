using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonScheme.Net.Test
{
    public class LeetCode
    {
        public void Run()
        {
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Console.WriteLine("数组 " + (ValidMountainArray(arr) ? "是" : "不是") + " 山脉数组");
        }
        public bool ValidMountainArray(int[] arr)
        {
            if (arr.Length < 3) return false;
            int numIndex = 0;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                numIndex = i + 1;
                if (arr[i] >= arr[i + 1])
                    break;
            }
            if (numIndex == 1 || numIndex == arr.Length)
                return false;
            for (int i = numIndex; i < arr.Length; i++)
            {
                if (arr[i] >= arr[i - 1])
                    break;
                numIndex = i + 1;
            }
            return numIndex == arr.Length ? true : false;
        }
         
    }
}
