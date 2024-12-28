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
    public class PurchaseOrderController : Controller
    {
        private readonly IclsPurchaseOrderBM _purchaseOrderBM;
        private readonly string _connectionString;


        // Parameterless constructor with manual instantiation
        public PurchaseOrderController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString;

            // Manually instantiate the dependencies
            IclsPurchaseOrderDL purchaseOrderDL = new clsPurchaseOrderDL();
            _purchaseOrderBM = new clsPurchaseOrderBM(purchaseOrderDL);
        }

        public ActionResult Index()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var purchaseOrders = _purchaseOrderBM.GetAllPurchaseOrders(connection);
                    return View(purchaseOrders);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching purchase orders: " + ex.Message;
                return View(new List<clsPurchaseOrder>());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var order = _purchaseOrderBM.GetPurchaseOrderById(connection, id);
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching purchase order details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(clsPurchaseOrder order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    _purchaseOrderBM.CreatePurchaseOrder(connection, order);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error creating purchase order: " + ex.Message;
                return View(order);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var order = _purchaseOrderBM.GetPurchaseOrderById(connection, id);
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching purchase order for edit: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(clsPurchaseOrder order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    _purchaseOrderBM.UpdatePurchaseOrder(connection, order);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error updating purchase order: " + ex.Message;
                return View(order);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var order = _purchaseOrderBM.GetPurchaseOrderById(connection, id);
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching purchase order for deletion: " + ex.Message;
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
                    _purchaseOrderBM.DeletePurchaseOrder(connection, id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error deleting purchase order: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult OrdersAboveThreshold(decimal threshold)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var orders = _purchaseOrderBM.GetOrdersAboveThreshold(connection, threshold);
                    return View(orders);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching orders above threshold: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
