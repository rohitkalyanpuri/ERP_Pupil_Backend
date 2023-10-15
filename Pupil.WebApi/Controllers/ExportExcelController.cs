using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Pupil.WebApi.Controllers
{
    [Route("api/exportexcel")]
    [ApiController]
    public class ExportExcelController : ControllerBase
    {
        private readonly ParentService _parentService;
        private readonly GradeService _gradeService;
        private readonly DivisionService _divisionService;
        private readonly AcademicYearService _academicYearService;
        private IWebHostEnvironment _WebHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExportExcelController(IWebHostEnvironment webHostEnvironment,ParentService parentService, GradeService gradeService,
            DivisionService divisionService, AcademicYearService academicYearService,  IHttpContextAccessor httpContextAccessor)
        {
            _parentService = parentService;
            _gradeService = gradeService;
            _divisionService = divisionService;
            _academicYearService = academicYearService;
            _WebHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet,Route("student")]
        public async Task<IActionResult> WriteDataToExcel()
        {
            var response = new Response();
            try
            {
                DataTable dt = GetStudentColumnHeader();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    var worksheet = wb.Worksheets.Add(dt);

                    var Gradeoptions = await _gradeService.GetGradesForExcel();
                    var Parentoptions = await _parentService.GetParentsForExcel();
                    var Divisionoptions = await _divisionService.GetDivisionsForExcel();
                    var Academicoptions = await _academicYearService.GetAcademicYearForExcel();
                    var validGradeOptions = $"\"{String.Join(",", Gradeoptions)}\"";
                    var validParentOptions = $"\"{String.Join(",", Parentoptions)}\"";
                    var validDivisionOptions = $"\"{String.Join(",", Divisionoptions)}\"";
                    var validAcademicOptions = $"\"{String.Join(",", Academicoptions)}\"";

                    //Date Of Birth
                    worksheet.Range("C2:C500").Value = "'mm/dd/yyyy";


                    //Enrollment Date
                    worksheet.Range("F2:F500").Value = "'mm/dd/yyyy";

                    //AcademicYear_AcademicID
                    worksheet.Range("G2:G500").SetDataValidation().IgnoreBlanks = true;
                    worksheet.Range("G2:G500").SetDataValidation().InCellDropdown = true;
                    worksheet.Range("G2:G500").Value = "---";
                    worksheet.Range("G2:G500").SetDataValidation().List(validAcademicOptions, true);


                    //Grade_GradeID
                    worksheet.Range("H2:H500").SetDataValidation().IgnoreBlanks = true;
                    worksheet.Range("H2:H500").SetDataValidation().InCellDropdown = true;
                    worksheet.Range("H2:H500").Value = "---";
                    worksheet.Range("H2:H500").SetDataValidation().List(validGradeOptions, true);


                    //Division
                    worksheet.Range("I2:I500").SetDataValidation().IgnoreBlanks = true;
                    worksheet.Range("I2:I500").SetDataValidation().InCellDropdown = true;
                    worksheet.Range("I2:I500").Value = "---";
                    worksheet.Range("I2:I500").SetDataValidation().List(validDivisionOptions, true);


                    //Parent
                    worksheet.Range("J2:J500").SetDataValidation().IgnoreBlanks = true;
                    worksheet.Range("J2:J500").SetDataValidation().InCellDropdown = true;
                    worksheet.Range("J2:J500").Value = "---";
                    worksheet.Range("J2:J500").SetDataValidation().List(validParentOptions, true);

                    var rootFolder = Path.Combine(_WebHostEnvironment.WebRootPath, "Files/SpreadSheets"); ;
                    string AutoKey = Guid.NewGuid().ToString("N").Substring(0, 8);
                    string fileName = "StudentImportFilePupil.xlsx";
                    var filePath = Path.Combine(rootFolder, fileName);
                    var fileLocation = new FileInfo(filePath);
                    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                    var aLink = String.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme,
                            _httpContextAccessor.HttpContext.Request.Host, "/Files/SpreadSheets/" + fileName);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(filePath);
                        response.Message = aLink;
                        response.Status = Pupil.Model.StatusCode.Ok;
                        //Return xlsx Excel File  
                        //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        //return Ok(filePath);
                    }
                }
            }
            catch(Exception ex)
            {
                response.Message = "";
                response.Status = Pupil.Model.StatusCode.SystemException;
                response.Error = ex.Message.ToString();
            }

            return Ok(response);
        }

        private DataTable GetStudentColumnHeader()
        {
            //Creating DataTable  
            DataTable dt = new DataTable();
            //Setiing Table Name  
            dt.TableName = "StudentData";
            //Add Columns  
            dt.Columns.Add("FirstName", typeof(string));//Column-A
            dt.Columns.Add("LastName", typeof(string));//Column-B
            dt.Columns.Add("DateOfBirth", typeof(string));//Column-C
            dt.Columns.Add("Phone_Number", typeof(string));//Column-D
            dt.Columns.Add("Enrollment_ID", typeof(string));//Column-E
            dt.Columns.Add("Enrollment_Date", typeof(string));//Column-F
            dt.Columns.Add("AcademicYear_AcademicID", typeof(string));//Column-G
            dt.Columns.Add("Grade_GradeID", typeof(string));//Column-H
            dt.Columns.Add("Division_DivisionID", typeof(string));//Column-I
            dt.Columns.Add("Parent_ParentID", typeof(string));//Column-J
            dt.Columns.Add("Parent_Parent_Number", typeof(string));//Column-K

            //Add Rows in DataTable  
            //dt.Rows.Add(1, "Anoop Kumar Sharma", "Delhi");
            //dt.Rows.Add(2, "Andrew", "U.P.");
            dt.AcceptChanges();
            return dt;
        }
    }
}
