using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTPNS.Web.Helpers
{
    public class ExcelHelper
    {

        /// <summary>
        /// Get total row count by checking all rows that have non-null (empty string is allowd)
        /// value in the first cell. 
        /// </summary>
        /// <param name="sheet">Worksheet data</param>
        /// <returns>Total row count</returns>
        public static int GetTotalRowCountByFirstCell(ExcelWorksheet sheet)
        {
            int counter = 1;
            while (sheet.Cells[counter, 1].Value != null)
            {
                counter++;
            }
            return --counter;
        }

        /// <summary>
        /// Get total row count by checking all rows and all cells that have at least 
        /// one non-null cell value.
        /// </summary>
        /// <param name="sheet">Worksheet data</param>
        /// <returns>Total row count</returns>
        public static int GetTotalRowCountByAnyNonNullData(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        /// <summary>
        /// Get Total Columns
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static int GetTotalColumn(ExcelWorksheet sheet)
        {
            var allowAfterTwoColumnEmpty = 2;
            var column = 1;
            while (column >= 1)
            {
                var range = sheet.Cells[1, column, 1, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    column++;
                }
                else
                {
                    if (allowAfterTwoColumnEmpty > 0)
                        allowAfterTwoColumnEmpty--;
                    else
                        break;
                }
            }
            return column - 1;
        }

    }
}
