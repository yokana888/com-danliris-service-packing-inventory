﻿using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingAreaOutput.Aval
{
    public interface IOutputAvalService
    {
        Task<int> Create(OutputAvalViewModel viewModel);
        Task<OutputAvalViewModel> ReadById(int id);
        ListResult<IndexViewModel> Read(int page, int size, string filter, string order, string keyword);
        ListResult<AvailableAvalIndexViewModel> ReadAvailableAval(DateTimeOffset searchDate,
                                                                  string searchShift,
                                                                  string searchGroup,
                                                                  int page,
                                                                  int size,
                                                                  string filter,
                                                                  string order,
                                                                  string keyword);
        ListResult<AvailableAvalIndexViewModel> ReadAllAvailableAval(
                                                                  int page,
                                                                  int size,
                                                                  string filter,
                                                                  string order,
                                                                  string keyword);
        ListResult<AvailableAvalIndexViewModel> ReadByBonAvailableAval(
                                                                  int bonId,
                                                                  int page,
                                                                  int size,
                                                                  string filter,
                                                                  string order,
                                                                  string keyword);
        ListResult<AvailableAvalIndexViewModel> ReadByTypeAvailableAval(
                                                                  string avalType,
                                                                  int page,
                                                                  int size,
                                                                  string filter,
                                                                  string order,
                                                                  string keyword);
        Task<MemoryStream> GenerateExcel(int id);
        Task<MemoryStream> GenerateExcel(int id, int offSet);
        ListResult<AdjAvalItemViewModel> GetDistinctAllProductionOrder(int page, int size, string filter, string order, string keyword);
        MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);

        Task<int> Update(int id, OutputAvalViewModel viewModel);
        Task<int> Delete(int id);
    }
}
