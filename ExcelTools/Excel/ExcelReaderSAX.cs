﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTools.Excel
{
	/// <summary>
	/// 通过SAX方法读取excel文件
	/// </summary>
	public class ExcelReaderSAX : ExcelReader
	{
		public override Task<List<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, Dictionary<string, string> excelHeaders)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var excelReader = OpenXmlReader.Create(worksheetPart);
			T entityData;
			Row row;
			var result = new List<T>();
			return Task.Run(()=>
			{
				while (excelReader.Read())
				{
					entityData = new T();
					row = excelReader.LoadCurrentElement() as Row;
					if (row == null || row.RowIndex == 1)
					{
						continue;
					}
					result.Add(FillEntityData<T>(row, excelHeaders));
				}
				return result;
			});
		}
	}
}
