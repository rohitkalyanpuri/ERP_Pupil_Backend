using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pupil.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/exportexcel")]
    [ApiController]
    public class ExportExcelController : ControllerBase
    {
        private readonly IParentService _parentService;
        private readonly IGradeService _gradeService;
        private readonly IDivisionService _divisionService;
        private readonly IAcademicYearService _academicYearService;

        public ExportExcelController(IParentService parentService,IGradeService gradeService, IDivisionService divisionService, IAcademicYearService academicYearService)
        {
            _parentService = parentService;
            _gradeService = gradeService;
            _divisionService = divisionService;
            _academicYearService = academicYearService;
        }

        [HttpGet,Route("student")]
        public async Task<IActionResult> WriteDataToExcel()
        {
            DataTable dt = GetStudentColumnHeader();
            //Name of File  
            string fileName = "StudentImportFilePupil.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                var worksheet= wb.Worksheets.Add(dt);

                var Gradeoptions = await _gradeService.GetGradesForExcel();
                var Parentoptions = await _parentService.GetParentsForExcel();
                var Divisionoptions = await _divisionService.GetDivisionsForExcel();
                var Academicoptions = await _academicYearService.GetAcademicYearForExcel();
                var validGradeOptions = $"\"{String.Join(",", Gradeoptions)}\"";
                var validParentOptions = $"\"{String.Join(",", Parentoptions)}\"";
                var validDivisionOptions = $"\"{String.Join(",", Divisionoptions)}\"";
                var validAcademicOptions = $"\"{String.Join(",", Academicoptions)}\"";

                //Academics
                worksheet.Range("E2:E500").SetDataValidation().IgnoreBlanks = true;
                worksheet.Range("E2:E500").SetDataValidation().InCellDropdown = true;
                worksheet.Range("E2:E500").Value = "---";
                worksheet.Range("E2:E500").SetDataValidation().List(validAcademicOptions, true);


                //Grades
                worksheet.Range("F2:F500").SetDataValidation().IgnoreBlanks = true;
                worksheet.Range("F2:F500").SetDataValidation().InCellDropdown = true;
                worksheet.Range("F2:F500").Value = "---";
                worksheet.Range("F2:F500").SetDataValidation().List(validGradeOptions, true);


                //Division
                worksheet.Range("G2:G500").SetDataValidation().IgnoreBlanks = true;
                worksheet.Range("G2:G500").SetDataValidation().InCellDropdown = true;
                worksheet.Range("G2:G500").Value = "---";
                worksheet.Range("G2:G500").SetDataValidation().List(validDivisionOptions, true);


                //Parent
                worksheet.Range("H2:H500").SetDataValidation().IgnoreBlanks = true;
                worksheet.Range("H2:H500").SetDataValidation().InCellDropdown = true;
                worksheet.Range("H2:H500").Value = "---";
                worksheet.Range("H2:H500").SetDataValidation().List(validParentOptions, true);

               


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            
        }

        private DataTable GetStudentColumnHeader()
        {
            //Creating DataTable  
            DataTable dt = new DataTable();
            //Setiing Table Name  
            dt.TableName = "StudentData";
            //Add Columns  
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Phone Number", typeof(string));
            dt.Columns.Add("Enrollment ID", typeof(string));
            dt.Columns.Add("Enrollment Date", typeof(string));
            dt.Columns.Add("AcademicYear-AcademicID", typeof(string));
            dt.Columns.Add("Grade-GradeID", typeof(string));
            dt.Columns.Add("Division-DivisionID", typeof(string));
            dt.Columns.Add("Parent-ParentID", typeof(string));
            dt.Columns.Add("Parent Parent Number", typeof(string));
            
            //Add Rows in DataTable  
            //dt.Rows.Add(1, "Anoop Kumar Sharma", "Delhi");
            //dt.Rows.Add(2, "Andrew", "U.P.");
            dt.AcceptChanges();
            return dt;
        }
    }
}
