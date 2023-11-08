using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Tourist_VietripInsum_2023.Models;

namespace Tourist_VietripInsum_2023.Controllers
{
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
            database.BaiBaos.Where(s => s.MaBaiBao == id).FirstOrDefault();database.BaiBaos.Remove(bao);
            database.SaveChanges();
            return Redirect("/Management/ProductsManagement");
        }
    }
}