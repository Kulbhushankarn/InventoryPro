using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryPro.BM;
using InventoryPro.BM.ITF;
using InventoryPro.DL;
using InventoryPro.DL.ITF;
using InventoryPro.VO;
using Microsoft.Office.Interop.Excel;


namespace InventoryPro.UIW.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IclsInventoryBM _inventoryBM;
        private readonly string _connectionString;

        // Parameterless constructor with manual instantiation
        public InventoryController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString;

            // Manually instantiate the dependencies
            IclsInventoryDL inventoryDL = new clsInventoryDL();
            _inventoryBM = new clsInventoryBM(inventoryDL);
        }

        public ActionResult Index()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var inventoryItems = _inventoryBM.GetAllInventoryItems(connection);
                    return View(inventoryItems);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching inventory items: " + ex.Message;
                return View(new List<clsInventoryItem>());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var item = _inventoryBM.GetInventoryItemById(connection, id);
                    return View(item);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching inventory item details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(clsInventoryItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    _inventoryBM.AddInventoryItem(connection, item);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error adding inventory item: " + ex.Message;
                return View(item);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var item = _inventoryBM.GetInventoryItemById(connection, id);
                    return View(item);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching inventory item for edit: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(clsInventoryItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    _inventoryBM.UpdateInventoryItem(connection, item);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error updating inventory item: " + ex.Message;
                return View(item);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var item = _inventoryBM.GetInventoryItemById(connection, id);
                    return View(item);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching inventory item for deletion: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    _inventoryBM.DeleteInventoryItem(connection, id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error deleting inventory item: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult ItemsBelowReorderLevel()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var items = _inventoryBM.GetItemsBelowReorderLevel(connection);
                    return View(items);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching items below reorder level: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// This is the Export to Excel Function
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var inventoryItems = _inventoryBM.GetAllInventoryItems(connection);

                    // Create Excel Application
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Add();
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Inventory Items";

                    // Add Headers
                    var headers = new[] { "Item ID", "Name", "Category", "Quantity", "Unit", "Price", "Reorder Level" };
                    for (int col = 0; col < headers.Length; col++)
                    {
                        worksheet.Cells[1, col + 1] = headers[col];

                        var headerCell = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, col + 1];
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SkyBlue);
                        headerCell.Font.Bold = true;
                        headerCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    // Add Data
                    int row = 2;
                    foreach (var item in inventoryItems)
                    {
                        worksheet.Cells[row, 1] = item.ItemId;
                        worksheet.Cells[row, 2] = item.Name;
                        worksheet.Cells[row, 3] = item.Category;
                        worksheet.Cells[row, 4] = item.Quantity;
                        worksheet.Cells[row, 5] = item.Unit;
                        worksheet.Cells[row, 6] = item.Price;
                        worksheet.Cells[row, 7] = item.ReorderLevel;
                        row++;
                    }

                    // Set Auto-Fit
                    worksheet.Columns.AutoFit();

                    // Save the Excel file to a temporary location
                    string tempPath = System.IO.Path.GetTempFileName().Replace(".tmp", ".xlsx");
                    workbook.SaveAs(tempPath);

                    // Clean up Excel Interop objects
                    workbook.Close(false);
                    excelApp.Quit();

                    ReleaseComObject(worksheet);
                    ReleaseComObject(workbook);
                    ReleaseComObject(excelApp);

                    // Read the file and send it to the user as a download
                    byte[] fileBytes = System.IO.File.ReadAllBytes(tempPath);
                    string fileName = "InventoryItems.xlsx";

                    // Delete the temporary file
                    System.IO.File.Delete(tempPath);

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error exporting inventory items: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Helper method to release COM objects
        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch
            {
                // Ignore errors during release
            }
            finally
            {
                GC.Collect();
            }
        }


    }
}
