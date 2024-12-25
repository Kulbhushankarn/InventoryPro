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
    }
}
