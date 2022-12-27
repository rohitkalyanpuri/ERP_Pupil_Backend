using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Interfaces;
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
        private readonly IParentService _parentService;
        private readonly IGradeService _gradeService;
        private readonly IDivisionService _divisionService;
        private readonly IAcademicYearService _academicYearService;
        private readonly ITenantService _tenantService;
        private IWebHostEnvironment _WebHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExportExcelController(IWebHostEnvironment webHostEnvironment,IParentService parentService, IGradeService gradeService,
            IDivisionService divisionService, IAcademicYearService academicYearService, ITenantService tenantService, IHttpContextAccessor httpContextAccessor)
        {
            _parentService = parentService;
            _gradeService = gradeService;
            _divisionService = divisionService;
            _academicYearService = academicYearService;
            _tenantService = tenantService;
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

                    var rootFolder = Path.Combine(_WebHostEnvironment.WebRootPath, "Files/SpreadSheets"); ;
                    string AutoKey = Guid.NewGuid().ToString("N").Substring(0, 8);
                    string fileName = "StudentImportFilePupil_" + _tenantService.GetTenant()?.TID + ".xlsx";
                    var filePath = Path.Combine(rootFolder, fileName);
                    var fileLocation = new FileInfo(filePath);
                    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                    var aLink = String.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme,
                            _httpContextAccessor.HttpContext.Request.Host, "/Files/SpreadSheets/" + fileName);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(filePath);
                        response.Message = aLink;
                        response.Status = Pupil.Core.Enums.StatusCode.Ok;
                        //Return xlsx Excel File  
                        //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        //return Ok(filePath);
                    }
                }
            }
            catch(Exception ex)
            {
                response.Message = "";
                response.Status = Pupil.Core.Enums.StatusCode.SystemException;
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
