using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;

namespace PetriNetXam
{
    public class Helpers
    {
        public static List<T> CreateList<T>(params T[] values)
        {
            return new List<T>(values);
        }

        public void showToastMessage(String text, int duration)
        {

        }



        public static List<List<int>> SubtractElementWise(List<List<int>> a, List<List<int>> b)
        {
            try
            {
                var numRows = a.Count;
                var numCols = a[0].Count;


                var result = new List<List<int>>(numRows);
                var resultRow = new List<int>(numCols);
                for (var i = 0; i < numRows; i++)
                {
                    for (var j = 0; j < numCols; j++)
                    {
                        resultRow.Add(a[i].ElementAt(j) - b[i].ElementAt(j));
                    }
                    result.Add(resultRow);
                    resultRow = new List<int>(numCols);
                }
                return result;
            }
            catch (Exception ex)
            {
                //Report(ex); 
                return null;
            }
        }
    }
}