using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace PetriNetCsharp
{
    public static class Helpers
    {
        public static void FillWholeDgvWithZeros(DataGridView target)
        {
            try
            {
                for (int i = 0; i < target.RowCount; i++)
                {
                    for (int j = 0; j < target.ColumnCount; j++)
                    {
                        target.Rows[i].Cells[j].Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Report(ex);
            }

        }

        public static void FillNullsInDgvWithZeros(DataGridView target)
        {

            for (int i = 0; i < target.RowCount; i++)
            {
                for (int j = 0; j < target.ColumnCount; j++)
                {
                    if (target.Rows[i].Cells[j].Value == null)
                    {
                        target.Rows[i].Cells[j].Value = 0;
                    }
                }
            }
        }


        public static List<int> AddMatrixElementWise(List<int> a, List<int> b)
        {
            try
            {
                List<int> result = new List<int>(a.Count);
                for (int i = 0; i < a.Count; i++)
                {
                    result[i] = a[i] + b[i];
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public static void ImportMatrix2DToDataGridView(List<List<int>> source, DataGridView target)
        {
            try
            {
                for (int i = 0; i < source.Count; i++)
                {
                    for (int j = 0; j < source[0].Count; j++)
                    {
                        target.Rows[i].Cells[j].Value = source[i][j].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                Report(ex);
            }
        }

        public static void ImportVectorToDataGridView(List<int> source, DataGridView target)
        {
            try
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (target.RowCount == 1) //dgv jeden wiersz
                    {
                        target.Rows[0].Cells[i].Value = source[i].ToString();
                    }
                    if (target.ColumnCount == 1) //dgv jedna kolumna
                    {
                        target.Rows[i].Cells[0].Value = source[i].ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                Report(ex);
            }
        }
        public static void ImportVectorToDataGridView(List<bool> source, DataGridView target)
        {
            try
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (target.RowCount == 1) //dgv jeden wiersz
                    {
                        target.Rows[0].Cells[i].Value = source[i].ToString();
                    }
                    if (target.ColumnCount == 1) //dgv jeden wiersz
                    {
                        target.Rows[i].Cells[0].Value = source[i].ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                Report(ex);
            }
        }

        public static void FillListWithValue(List<int> target, int val)
        {
            for (int i = 0; i < target.Capacity; i++)
            {
                target.Add(val);
            }
        }
        public static void FillListWithValue(List<List<int>> target, List<int> val)
        {
            for (int i = 0; i < target.Capacity; i++)
            {
                target.Add(val);
            }
        }
        public static List<List<int>> Matrix2DMultiply(List<List<int>> A, List<List<int>> B)
        {
            try
            {
                int rowsA = A.Count;
                int colsA = A[0].Count;
                int rowsB = B.Count;
                int colsB = B[0].Count;
                if (colsA != rowsB)
                {
                    throw new System.InvalidOperationException("Liczba kolumna Macierzy A\n rozna od liczby wierszy macierzy B");
                }
                //List<int> tmp1D = new List<int>(colsB);
                //FillListWithValue(tmp1D, 0);
                //List<List<int>> result = new List<List<int>>(rowsA);
                //FillListWithValue(result, tmp1D);

                List<List<int>> result = new List<List<int>>(rowsA);
                for (int i = 0; i < rowsA; i++)
                {
                    result.Add(new List<int>(colsB));
                    for (int j = 0; j < colsB; j++)
                    {
                        result[i].Add(0);
                    }
                }

                for (int row = 0; row < rowsA; row++)
                {
                    for (int col = 0; col < colsB; col++)
                    {
                        // Multiply the row of A by the column of B to get the row, column of product.  
                        for (int inner = 0; inner < rowsB; inner++)
                        {
                            result[row][col] += A[row][inner] * B[inner][col];
                        }
                    }
                }
                return result;
            }
            catch (InvalidOperationException ex)
            {
                Report(ex);
                return null;
            }
            catch (Exception ex)
            {
                Report(ex);
                throw;
            }

        }

        public static List<int> AddElementWise(List<int> a, List<int> b)
        {
            try
            {
                List<int> result = new List<int>(a.Count);
                for (int i = 0; i < a.Count; i++)
                {
                    result.Add(a[i] + b[i]);
                }
                return result;
            }
            catch (Exception ex)
            {
                Report(ex); return null;
            }

        }
        public static List<List<int>> AddElementWise(List<List<int>> a, List<List<int>> b)
        {
            try
            {
                int numRows = a.Count;
                int numCols = a[0].Count;


                List<List<int>> result = new List<List<int>>(numRows);
                List<int> resultRow = new List<int>(numCols);
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        resultRow.Add(a[i].ElementAt(j) + b[i].ElementAt(j));
                    }
                    result.Add(resultRow);
                    resultRow = new List<int>(numCols);
                }
                return result;
            }
            catch (Exception ex)
            {
                Report(ex); return null;
            }

        }

        public static List<int> SubtractElementWise(List<int> a, List<int> b)
        {
            try
            {
                List<int> result = new List<int>(a.Count);
                for (int i = 0; i < a.Count; i++)
                {
                    result.Add(a[i] - b[i]);
                }
                return result;
            }
            catch (Exception ex)
            {
                Report(ex); return null;
            }

        }
        public static List<List<int>> SubtractElementWise(List<List<int>> a, List<List<int>> b)
        {
            try
            {
                int numRows = a.Count;
                int numCols = a[0].Count;


                List<List<int>> result = new List<List<int>>(numRows);
                List<int> resultRow = new List<int>(numCols);
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
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
                Report(ex); return null;
            }

        }

       public static List<List<int>> ImportDataGridToMatrix2D(DataGridView source)
        {
            try
            {
                List<List<int>> result = new List<List<int>>(source.RowCount);
                List<int> resultRow = new List<int>(source.ColumnCount);
                for (int i = 0; i < source.RowCount; i++)
                {
                    for (int j = 0; j < source.ColumnCount; j++)
                    {
                        resultRow.Add(int.Parse(source.Rows[i].Cells[j].Value.ToString()));
                    }
                    result.Add(resultRow);
                    resultRow = new List<int>(source.ColumnCount);
                }
                return result;
            }
            catch (Exception ex)
            {

                Report(ex);
            }
            return null;
        }


        public static void Report(Exception ex)
        {
            MessageBox.Show(
                "\r\n\r\nMessage: " + ex.Message +
                "\r\n\r\nType: " + ex.GetType() +
                 "\r\n\r\nMethod: " + ex.TargetSite+
                 "\r\n\r\nStackTrace:\r\n" + ex.StackTrace,
                ex.Source,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
            //MessageBox.Show(ex.Message + "\n\n" + ex.GetType()+"\n\n"+ ex.StackTrace);
        }

        public static void Report(Exception ex,Form sender)
        {
            MessageBox.Show(
                "Message: " + ex.Message +
                "\r\n\r\nType: " + ex.GetType() +
                "\r\n\r\nSource: " + ex.Source+
                 "\r\n\r\nMethod: " + ex.TargetSite +
                 "\r\n\r\nStackTrace:\r\n" + ex.StackTrace,
                sender.Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
               
            //MessageBox.Show(ex.Message + "\n\n" + ex.GetType() + "\n\n" + ex.StackTrace);
        }


        public static bool IntToBool(int a)
        {
            //return (a == 0) ? false : true;
            return a != 0;  //aaale koooosmos   
        }
    }
}
