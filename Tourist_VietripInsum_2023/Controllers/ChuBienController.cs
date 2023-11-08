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