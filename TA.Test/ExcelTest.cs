using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TA.PracticeService.ExcelTest;

namespace TA.Test
{
    [TestClass]
    public class ExcelTest
    {
        [TestMethod]
        public void ExcelBasicTest()
        {
            using (var excelPractitioner = ExcelPractice.Instance)
            { 
                var dataTableWithDefaultHeaders = excelPractitioner.ReadExcelFile(@"D:\ExcelTestC#\TestRead.xlsx");
                var dataTableWithNoHeaders = excelPractitioner.ReadExcelFile(@"D:\ExcelTestC#\TestRead.xlsx", false);
            }
        }
    }
}
