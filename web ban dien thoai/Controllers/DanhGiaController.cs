using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;


namespace web_ban_dien_thoai.Controllers
{
    public class DanhGiaController : Controller
    {
        // GET: DanhGia

        testEntities database = new testEntities();

        public ActionResult QLDGPartial()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            // Retrieve feedback entries and order by NgayDang in descending order
            var danhGiaEntries = database.DanhGiaSPs.OrderByDescending(d => d.NgayDang).ToList();
            return View(danhGiaEntries);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGiaSP dg = database.DanhGiaSPs.Find(id);
            if (dg == null)
            {
                return HttpNotFound();
            }
            return View(dg);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhGiaSP dg = database.DanhGiaSPs.Find(id);
            database.DanhGiaSPs.Remove(dg);
            database.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}