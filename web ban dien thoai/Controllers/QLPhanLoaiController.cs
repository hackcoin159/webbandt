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
    public class QLPhanLoaiController : Controller
    {
        // GET: QLPhanLoai
        testEntities database = new testEntities();
        // GET: QLSanPham

        public ActionResult Index()
        {
            var qlPhanLoai = database.PhanLoais.ToList();
            return View(qlPhanLoai);
        }

     

        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPL,TenPhanLoai")] PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                database.PhanLoais.Add(phanLoai);
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phanLoai);
        }
        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai cHUDE = database.PhanLoais.Find(id);
            if (cHUDE == null)
            {
                return HttpNotFound();
            }
            return View(cHUDE);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPL,TenPhanLoai")] PhanLoai cHUDE)
        {
            if (ModelState.IsValid)
            {
                database.Entry(cHUDE).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cHUDE);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai cHUDE = database.PhanLoais.Find(id);
            if (cHUDE == null)
            {
                return HttpNotFound();
            }
            return View(cHUDE);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhanLoai cHUDE = database.PhanLoais.Find(id);
            database.PhanLoais.Remove(cHUDE);
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult QLPLPartial()
        {
            return PartialView();
        }

    }
}