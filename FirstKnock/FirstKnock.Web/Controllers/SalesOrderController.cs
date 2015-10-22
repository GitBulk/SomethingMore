using FirstKnock.DataLayer;
using FirstKnock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstKnock.Web.Controllers
{
    public class SalesOrderController : Controller
    {
        ISalesOrderRepository repo;
        public SalesOrderController()
        {
            this.repo = new SalesOrderRepository();
        }

        //
        // GET: /SalesOrder/
        public ActionResult Index()
        {
            List<SalesOrderViewModel> model = repo.GetAll();
            return View(model);
        }

        //
        // GET: /SalesOrder/Details/5
        public ActionResult Details(int id)
        {
            SalesOrderViewModel model = repo.GetItem(id);
            return View(model);
        }

        //
        // GET: /SalesOrder/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SalesOrder/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SalesOrder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SalesOrder/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SalesOrder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SalesOrder/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
