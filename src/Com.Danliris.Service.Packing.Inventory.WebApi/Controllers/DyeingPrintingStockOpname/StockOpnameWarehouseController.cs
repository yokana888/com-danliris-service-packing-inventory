﻿using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingStockOpname.Warehouse;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.Utilities;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Danliris.Service.Packing.Inventory.WebApi.V8.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.WebApi.V8.Controllers.DyeingPrintingStockOpname
{
    [Produces("application/json")]
    [Route("v1/stock-opname-warehouse")]
    [Authorize]
    public class StockOpnameWarehouseController : ControllerBase
    {
        private readonly IStockOpnameWarehouseService _service;
        private readonly IIdentityProvider _identityProvider;
        private readonly IValidateService ValidateService;

        public StockOpnameWarehouseController(IStockOpnameWarehouseService service, IIdentityProvider identityProvider, IValidateService validateService)
        {
            _service = service;
            _identityProvider = identityProvider;
            ValidateService = validateService;
        }

        protected void VerifyUser()
        {
            _identityProvider.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityProvider.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityProvider.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StockOpnameWarehouseViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    var excpetion = new
            //    {
            //        error = ResultFormatter.FormatErrorMessage(ModelState)
            //    };
            //    return new BadRequestObjectResult(excpetion);
            //}
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);
                var result = await _service.Create(viewModel);

                return Created("/", result);
            }
            catch (ServiceValidationException ex)
            {
                var result = new
                {
                    error = ResultFormatter.Fail(ex),
                    apiVersion = "1.0.0",
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Data does not pass validation"
                };

                return new BadRequestObjectResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        //Tanggal 23-03-2023 by Gama
        //[HttpPost("mobile")]
        //public async Task<IActionResult> PostMobile([FromBody] StockOpnameBarcodeFormDto viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var excpetion = new
        //        {
        //            error = ResultFormatter.FormatErrorMessage(ModelState)
        //        };
        //        return new BadRequestObjectResult(excpetion);
        //    }
        //    try
        //    {
        //        VerifyUser();
        //        ValidateService.Validate(viewModel);
        //        var result = await _service.Create(viewModel);

        //        return Created("/", result);
        //    }
        //    catch (ServiceValidationException ex)
        //    {
        //        var Result = new
        //        {
        //            error = ResultFormatter.Fail(ex),
        //            apiVersion = "1.0.0",
        //            statusCode = HttpStatusCode.BadRequest,
        //            message = "Data does not pass validation"
        //        };

        //        return new BadRequestObjectResult(Result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {

                var data = await _service.ReadById(id);
                return Ok(new
                {
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                VerifyUser();
                var data = await _service.Delete(id);
                return Ok(new
                {
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public IActionResult Get([FromQuery] string keyword = null, [FromQuery] int page = 1, [FromQuery] int size = 25, [FromQuery] string order = "{}",
           [FromQuery] string filter = "{}", [FromQuery] bool isStockOpname = false)
        {
            try
            {

                var data = _service.Read(page, size, filter, order, keyword, isStockOpname);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] StockOpnameWarehouseViewModel viewModel)
        {
            VerifyUser();
            //if (!ModelState.IsValid)
            //{
            //    var exception = new
            //    {
            //        error = ResultFormatter.FormatErrorMessage(ModelState)
            //    };
            //    return new BadRequestObjectResult(exception);
            //}

            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);
                await _service.Update(id, viewModel);

                return NoContent();
            }
            catch (ServiceValidationException ex)
            {
                var Result = new
                {
                    error = ResultFormatter.Fail(ex),
                    apiVersion = "1.0.0",
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Data does not pass validation"
                };

                return new BadRequestObjectResult(Result);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    error = ex.Message
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, error);
            }
        }


        [HttpGet("list-bon")]
        public IActionResult GetListBon([FromQuery] string keyword = null)
        {
            try
            {

                var data = _service.Read(keyword);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }

        [HttpGet("xls-document/{id}")]
        public async Task<IActionResult> GetExcel(int id)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int clientTimeZoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var Result = await _service.GenerateExcelDocumentAsync(id, clientTimeZoneOffset);
                string filename = "Bon Keluar Gudang Jadi Dyeing/Printing.xlsx";
                xlsInBytes = Result.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("monitoring")]
        public IActionResult GetScanView([FromQuery] long productionOrderId = 0, [FromQuery] string barcode = null, [FromQuery] string documentNo = null, [FromQuery] string grade = null, [FromQuery] string userFilter = null)
        {
            try
            {
                VerifyUser();
                //int clientTimeZoneOffset = Convert.ToInt32(timezone);
                var data = _service.GetMonitoringScan(productionOrderId, barcode, documentNo, grade, userFilter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("monitoring/download")]
        public IActionResult GetXlsScanView([FromQuery] long productionOrderId = 0, [FromQuery] string barcode = null, [FromQuery] string documentNo = null, [FromQuery] string grade = null, [FromQuery] string userFilter = null)
        {

            try
            {


                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : Convert.ToDateTime(dateFrom);
                //DateTime DateTo = dateTo == null ? DateTime.Now : Convert.ToDateTime(dateTo);

                MemoryStream xls = _service.GenerateExcelMonitoringScan(productionOrderId, barcode, documentNo, grade, userFilter);


                string filename = string.Format("Laporan Stock Gudang All Unit - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));
                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("code")]
        public IActionResult GetBarcode(string itemData, int trackId)
        {

            string accept = Request.Headers["Accept"];
            try
            {

                //var data = _service.getDatabyCode(itemData, trackId);
                var data = _service.getDatabyCodeSummary(itemData, trackId);
                //var model = mapper.Map<List<InventoryViewModel>>(data);

                return Ok(new
                {
                    //apiVersion = ApiVersion,
                    data,
                    info = new { count = data.Count(), total = data.Count() },

                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("report-so")]
        public IActionResult Get([FromQuery] DateTimeOffset dateFrom, [FromQuery] DateTimeOffset dateTo, [FromQuery] int productionOrderId, [FromQuery] string barcode, [FromQuery] int track)
        {
            try
            {
                VerifyUser();
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = _service.GetReportDataSO(dateFrom, dateTo, productionOrderId, barcode, track, offset);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("report-so-xls")]
        public IActionResult GetXls([FromQuery] DateTimeOffset dateFrom, [FromQuery] DateTimeOffset dateTo, [FromQuery] int productionOrderId, [FromQuery] string barcode, [FromQuery] int track)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var Result = _service.GenerateExcel(dateFrom, dateTo, productionOrderId, barcode, track, offset);
                string filename = $"Laporan Stock Opname {dateFrom.ToString("yyyy MM dd")} - {dateTo.ToString("yyyy MM dd")}.xlsx";
                xlsInBytes = Result.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("monitoring-so")]
        public IActionResult GetMonitoring([FromQuery] DateTimeOffset dateFrom, [FromQuery] DateTimeOffset dateTo, [FromQuery] int productionOrderId, [FromQuery] int track)
        {
            try
            {
                VerifyUser();
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = _service.GetMonitoringSO(dateFrom, dateTo, productionOrderId, track, offset);
                return Ok(new
                {
                    //apiVersion = ApiVersion,
                    data,
                    info = new { count = data.Count(), total = data.Count() },

                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("monitoring-so-xls")]
        public IActionResult GetMonitoringXls([FromQuery] DateTimeOffset dateFrom, [FromQuery] DateTimeOffset dateTo, [FromQuery] int productionOrderId, [FromQuery] int track)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var Result = _service.GenerateExcelMonitoring(dateFrom, dateTo, productionOrderId, track, offset);
                string filename = "";

                if (dateFrom == DateTimeOffset.MinValue && dateTo == DateTimeOffset.MinValue)
                {
                    filename = $"Monitoring Stock Opname Masuk .xlsx";
                }
                else
                {
                    filename = $"Monitoring Stock Opname Masuk {dateFrom.ToString("yyyy MM dd")} - {dateTo.ToString("yyyy MM dd")}.xlsx";
                }
                xlsInBytes = Result.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }






    }
}
