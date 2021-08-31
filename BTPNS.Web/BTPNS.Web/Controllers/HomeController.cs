using BTPNS.Contracts;
using BTPNS.Core;
using BTPNS.Core.Constans;
using BTPNS.Core.GenericModel;
using BTPNS.Web.Helpers;
using BTPNS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BTPNS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<AppResources> _localizer;
        private readonly IFileManagement _fileManagement;
        private readonly IBLL _bLL;
        public HomeController(IStringLocalizer<AppResources> localizer, IFileManagement fileManagement, IBLL bLL)
        {
            _localizer = localizer;
            _fileManagement = fileManagement;
            _bLL = bLL;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Mode = _localizer["Import"];
            return View("Index", new ImportViewModel());
        }

        [Authorize(PermissionConstants.ViewSuperAdminMode)]
        public IActionResult SaveImport(ImportViewModel model)
        {
            ViewBag.Mode = _localizer["Import"];
            List<string> errors = new List<string>();
            bool systemError = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ExcelFile == null || model.ExcelFile.Length <= 0)
                    {
                        errors.Add("Form file is empty");
                    }

                    if (!Path.GetExtension(model.ExcelFile.FileName).Contains(".xls", StringComparison.OrdinalIgnoreCase) ||
                        !Path.GetExtension(model.ExcelFile.FileName).Contains(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add("Not Support file extension");
                    }

                    if (errors.Any())
                    {
                        throw new CustomException
                        {
                            Errors = errors
                        };
                    }

                    var filePath = string.Empty;
                    using (var stream = new MemoryStream())
                    {
                        model.ExcelFile.CopyTo(stream);
                        filePath = _fileManagement.Save(stream, Path.GetExtension(model.ExcelFile.FileName));
                    }
                    if (string.IsNullOrEmpty(filePath))
                        throw new CustomException { Errors = new List<string> { "File not found." } };


                    using (var package = new ExcelPackage(_fileManagement.GetStream(filePath)))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        //var cells = worksheet.Cells;
                        //var dictionary = cells
                        //    .GroupBy(c => new { c.Start.Row, c.Start.Column })
                        //    .ToDictionary(
                        //        rcg => new KeyValuePair<int, int>(rcg.Key.Row, rcg.Key.Column),
                        //        rcg => cells[rcg.Key.Row, rcg.Key.Column].Value);

                        //_bLL.Save(dictionary, model.ExcelFile.FileName);


                        var rowCount = ExcelHelper.GetTotalRowCountByAnyNonNullData(worksheet);
                        var columnCount = ExcelHelper.GetTotalColumn(worksheet);

                        if (errors.Any())
                        {
                            throw new CustomException
                            {
                                Errors = errors
                            };
                        }

                        var headers = new List<string>();
                        var datas = new List<List<string>>();

                        //Get Headers
                        for (int i = 1; i <= columnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value != null)
                            {
                                headers.Add(worksheet.Cells[1, i].Value?.ToString());
                            }
                        }

                        //Get Datas
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var dataByRow = new List<string>();

                            for (int i = 1; i <= columnCount; i++)
                            {
                                dataByRow.Add(worksheet.Cells[row, i].Value?.ToString());
                            }
                            datas.Add(dataByRow);
                        }

                        model.Datas = _bLL.Save(headers, datas, Path.GetFileNameWithoutExtension(model.ExcelFile.FileName));
                        model.FileName = Path.GetFileName(filePath);
                    }
                }
                catch (CustomException ex)
                {
                    errors.AddRange(ex.Errors);
                }
                catch (Exception ex)
                {
                    systemError = true;
                    errors.Add(ex.Message);
                }
                if (systemError)
                {
                    throw new CustomException(errors);
                }
                else if (errors.Count > 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View("Index", model);
                }
                return View("Index", model);
            }
            else
                return View("Index", model);
        }

        [HttpGet]
        public IActionResult DownloadExceelFile(string fileName)
        {
            return File(_fileManagement.GetByte(fileName), "application/force-download", fileName);
        }
    }
}
