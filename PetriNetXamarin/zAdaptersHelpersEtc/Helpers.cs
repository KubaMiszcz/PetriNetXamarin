using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;

namespace PetriNetXamarin
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

        public static List<int> AddElementWise(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();
            try
            {
                    for (var i = 0; i < a.Count; i++)
                    {
                        result.Add(a.ElementAt(i) + b.ElementAt(i));    //PLUSSS!!!!!1111
                    }
                return result;
            }
            catch (Exception ex)
            {
                //Report(ex); 
                return null;
            }
        }


        public static List<List<int>> ResizeMatrix(List<List<int>> source, int oldRows, int oldCols, int newRows, int newCols)
        {
            List<List<int>> target = new List<List<int>>();
            for (int i = 0; i < newRows; i++) //wiersze
            {
                List<int> tmpRow = new List<int>();
                if (i < oldRows)//przepisujemy stary wiersz
                {
                    for (int j = 0; j < newCols; j++) 
                    {
                        if (j < oldCols)
                        {
                            tmpRow.Add(source[i][j]);
                        }
                        else
                        {
                            tmpRow.Add(0); //jakby co dopelniamy
                        }
                    }
                    target.Add(tmpRow);
                }
                else
                {
                    for (int j = 0; j < newCols; j++)
                    {
                        tmpRow.Add(0);
                    }
                    target.Add(tmpRow);
                }
            }
            return target;
        }

        public static List<List<int>> FillWithZeros(List<List<int>> source)
        {
            List<List<int>> target=new List<List<int>>();
            int rows = source.Count;
            int cols = source[0].Count;
            for (int i = 0; i < rows; i++)
            {
                List<int> tmpRow=new List<int>();
                for (int j = 0; j < cols; j++)
                {
                    tmpRow.Add(0);
                }
                target.Add(tmpRow);
            }
            return target;
        }
    }
}
