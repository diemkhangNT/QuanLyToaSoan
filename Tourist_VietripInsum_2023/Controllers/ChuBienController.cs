using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Tourist_VietripInsum_2023.Models;
using Tourist_VietripInsum_2023.App_Start;
using System.Data.Entity;

namespace Tourist_VietripInsum_2023.Controllers
{
    [AdminAuthorize(maCV = "CB")]
    public class ChuBienController : Controller
    {
        QuanLyToaSoanEntities database = new QuanLyToaSoanEntities();
        // GET: ChuBien
        public ActionResult Index()
        {
            var dsnhabao = database.NhanViens.Where(s=> s.MaCV == "NB").ToList().Count();
            Session["SoNhaBao"] = dsnhabao;
            var dsdoitac = database.NhanViens.Where(s => s.MaCV == "DT").ToList().Count();
            Session["SoDoiTac"] = dsdoitac;
            var dsbaibao = database.BaiBaos.ToList().Count();
            Session["SoBaiBao"] = dsbaibao;
            var dstheloai = database.TheLoais.ToList().Count();
            Session["SoTheLoai"] = dstheloai;
            return View();
        }

        public ActionResult Delete(string id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sp = database.BaiBaos.Where(s => s.MaBaiBao == id).FirstOrDefault();
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BaiBao bao = database.BaiBaos.Where(s => s.MaBaiBao == id).FirstOrDefault();
            database.BaiBaos.Where(s => s.MaBaiBao == id).FirstOrDefault(); database.BaiBaos.Remove(bao);
            database.SaveChanges();
            TempData["AlertMessage"] = "deleted";
            return Redirect("/ChuBien/IndexQLBaiBao");
        }

        // GET: BaiBaos
        public ActionResult IndexQLBaiBao()
        {
            var baiBaos = database.BaiBaos.ToList();
            return View(baiBaos.ToList());
        }

        // GET: BaiBaos/Details/5
        public ActionResult DetailsBaiBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = database.BaiBaos.Find(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            return View(baiBao);
        }

        // POST: BaiBaos/DuyetBai/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DuyetBai(string idBaiBao, bool flag)
        {
            if (!flag) 
                TempData["AlertMessage"] = "false";
            else
                TempData["AlertMessage"] = "true";
            BaiBao baiBao = database.BaiBaos.Find(idBaiBao);
            baiBao.TrangThaiDuyet = flag;
            if (ModelState.IsValid)
            {
                database.Entry(baiBao).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("IndexQLBaiBao");
            }
            return View(baiBao);
        }
    }
}