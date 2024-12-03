﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.LocalSalesDOTS
{
    public class GarmentShippingLocalSalesDOTSPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(GarmentShippingLocalSalesDOTSViewModel viewModel, int timeoffset)
        {
            const int MARGIN = 20;

            Font header_font_bold_big = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            Font header_font_bold = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            Font header_font_bold_underlined = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10, Font.UNDERLINE);
            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font normal_font_underlined = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8, Font.UNDERLINE);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font small_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font small_font_underlined = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, Font.UNDERLINE);
            //Font body_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A5, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();


            #region header
            PdfPTable tableHeader = new PdfPTable(2);
            tableHeader.WidthPercentage = 100;
            tableHeader.SetWidths(new float[] { 2f, 2f });

            PdfPCell cellHeaderContent1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderContent2 = new PdfPCell() { Border = Rectangle.NO_BORDER };


            cellHeaderContent1.AddElement(new Phrase("\n", normal_font));
            cellHeaderContent1.AddElement(new Phrase("PT. DAN LIRIS", header_font_bold_big));
            cellHeaderContent1.AddElement(new Phrase("Head Office : Jl. Merapi No 23", small_font));
            cellHeaderContent1.AddElement(new Phrase("Banaran, Grogol, Sukoharjo, 57552", small_font));
            cellHeaderContent1.AddElement(new Phrase("Central Java, Indonesia", small_font));
            cellHeaderContent1.AddElement(new Phrase("Phone : (+62 271) 740888, 714400", small_font));
            cellHeaderContent1.AddElement(new Phrase("Fax : (+62 271) 740777, 735222", small_font));
            cellHeaderContent1.AddElement(new Phrase("PO BOX 166 Solo, 57100", small_font));
            cellHeaderContent1.AddElement(new Phrase("www.danliris.com", small_font));
            cellHeaderContent1.AddElement(new Phrase("\n", normal_font));
            cellHeaderContent1.AddElement(new Phrase(viewModel.localSalesDONo, bold_font));
            tableHeader.AddCell(cellHeaderContent1);

            cellHeaderContent2.AddElement(new Phrase("Surakarta, " + viewModel.date.ToOffset(new TimeSpan(timeoffset, 0, 0)).ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID")), normal_font));
            cellHeaderContent2.AddElement(new Phrase("\n", normal_font));
            cellHeaderContent2.AddElement(new Phrase("Kepada", normal_font));
            cellHeaderContent2.AddElement(new Phrase("Yth. Sdr. " + viewModel.to, small_font));
            cellHeaderContent2.AddElement(new Phrase("Bag. Gudang " + viewModel.storageDivision, small_font));
            cellHeaderContent2.AddElement(new Phrase("Export/Banaran ", small_font));
            cellHeaderContent2.AddElement(new Phrase("\n", normal_font));
            cellHeaderContent2.AddElement(new Phrase("D.O. Penjualan Lokal", header_font_bold_underlined));
            cellHeaderContent2.AddElement(new Phrase("\n", normal_font));
            cellHeaderContent2.AddElement(new Phrase("Untuk melengkapi nota No. ", small_font));
            cellHeaderContent2.AddElement(new Phrase("Harap dikeluarkan barang-barang tersebut dibawah ini.", small_font));
            tableHeader.AddCell(cellHeaderContent2);

            tableHeader.SpacingAfter = 15;
            document.Add(tableHeader);
            #endregion

            #region tableBody
            PdfPTable tableBody = new PdfPTable(8);
            tableBody.WidthPercentage = 100;
            tableBody.SetWidths(new float[] { 1f, 5f, 3f, 2.5f, 2.5f, 3f, 2.5f, 2.5f });

            PdfPCell cellBodyLeft = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellBodyRight = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellBodyCenter = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };

            cellBodyCenter.Phrase = new Phrase("No", normal_font);
            cellBodyCenter.Rowspan = 3;
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("No Invoice", normal_font);
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Jenis/Kode", normal_font);
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("BANYAKNYA", normal_font);
            cellBodyCenter.Rowspan = 1;
            cellBodyCenter.Colspan = 5;
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Jumlah", normal_font);
            cellBodyCenter.Rowspan = 2;
            cellBodyCenter.Colspan = 1;
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Satuan", normal_font);
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Jumlah\nKemasan", normal_font);
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Weight", normal_font);
            cellBodyCenter.Rowspan = 1;
            cellBodyCenter.Colspan = 2;
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Gross", normal_font);
            cellBodyCenter.Rowspan = 1;
            cellBodyCenter.Colspan = 1;
            tableBody.AddCell(cellBodyCenter);

            cellBodyCenter.Phrase = new Phrase("Nett", normal_font);
            tableBody.AddCell(cellBodyCenter);

            int index = 0;
            foreach (var item in viewModel.items)
            {
                index++;
                cellBodyLeft.Phrase = new Phrase($"{index}", normal_font);
                tableBody.AddCell(cellBodyLeft);

                cellBodyLeft.Phrase = new Phrase(item.invoiceNo, normal_font);
                tableBody.AddCell(cellBodyLeft);

                cellBodyLeft.Phrase = new Phrase(item.description, normal_font);
                tableBody.AddCell(cellBodyLeft);

                cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", item.quantity), normal_font);
                tableBody.AddCell(cellBodyRight);

                cellBodyLeft.Phrase = new Phrase(item.uom.Unit, normal_font);
                tableBody.AddCell(cellBodyLeft);

                cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", item.packQuantity) + " " + item.packUom.Unit, normal_font);
                tableBody.AddCell(cellBodyRight);

                cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", item.grossWeight), normal_font);
                tableBody.AddCell(cellBodyRight);

                cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", item.nettWeight), normal_font);
                tableBody.AddCell(cellBodyRight);
            }

            double totalQty = viewModel.items.Sum(a => a.quantity);
            double totalCtn = viewModel.items.Sum(a => a.packQuantity);
            double totalGW = viewModel.items.Sum(a => a.grossWeight);
            double totalNW = viewModel.items.Sum(a => a.nettWeight);

            cellBodyRight.Phrase = new Phrase("Jumlah", normal_font);
            cellBodyRight.Colspan = 3;
            tableBody.AddCell(cellBodyRight);

            cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", totalQty), normal_font);
            cellBodyRight.Colspan = 1;
            tableBody.AddCell(cellBodyRight);

            cellBodyRight.Phrase = new Phrase("", normal_font);
            tableBody.AddCell(cellBodyRight);

            cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", totalCtn), normal_font);
            tableBody.AddCell(cellBodyRight);

            cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", totalGW), normal_font);
            tableBody.AddCell(cellBodyRight);

            cellBodyRight.Phrase = new Phrase(string.Format("{0:n2}", totalNW), normal_font);
            tableBody.AddCell(cellBodyRight);

            tableBody.SpacingAfter = 15;
            document.Add(tableBody);
            #endregion

            document.Add(new Paragraph("Untuk bagian / dikirim kepada " + viewModel.buyer.Name, normal_font));

            document.Add(new Paragraph("Keterangan : \n" + viewModel.remark, normal_font));
            document.Add(new Paragraph("\n", normal_font));
            #region sign
            PdfPTable tableSign = new PdfPTable(4);
            tableSign.WidthPercentage = 100;
            tableSign.SetWidths(new float[] { 1f, 1f, 1f, 3f });

            PdfPCell cellSignBorder = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellSignNoBorder = new PdfPCell() { Border = Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };

            cellSignBorder.Phrase = new Phrase("Penjualan", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("Bag.Akuntansi", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("Gudang", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignNoBorder.Phrase = new Phrase("Terima Kasih", normal_font);
            tableSign.AddCell(cellSignNoBorder);

            cellSignBorder.Phrase = new Phrase("\n\n\n\n", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("\n\n\n\n", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("\n\n\n\n", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignNoBorder.Phrase = new Phrase("Bagian Shipping\n\n\n", normal_font);
            tableSign.AddCell(cellSignNoBorder);

            cellSignBorder.Phrase = new Phrase("", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignBorder.Phrase = new Phrase("", normal_font);
            tableSign.AddCell(cellSignBorder);

            cellSignNoBorder.Phrase = new Phrase("(___________________________)", normal_font);
            tableSign.AddCell(cellSignNoBorder);

            document.Add(tableSign);

            document.Add(new Phrase("Model DL 3b", normal_font));
            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
