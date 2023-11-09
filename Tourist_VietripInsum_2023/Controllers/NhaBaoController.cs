using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tourist_VietripInsum_2023.App_Start;
using Tourist_VietripInsum_2023.Models;

namespace Tourist_VietripInsum_2023.Controllers
{
    [AdminAuthorize(maCV = "NB")]
    public class NhaBaoController : Controller
    {
        QuanLyToaSoanEntities db = new QuanLyToaSoanEntities();
        Random rd = new Random();
        public string GenerateId(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            var id = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                id.Append(chars[random.Next(chars.Length)]);
            }

            return id.ToString();
        }
        public ActionResult TrangChuNhaBao()
        {
            return View();
        }

        public ActionResult QuanLyBaiBao()
        {
            var list = db.BaiBaos.ToList();
            return View(list);
        }

        // GET: BaiBaos/Details/5
        public ActionResult DetailsBaiBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = db.BaiBaos.Find(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            return View(baiBao);
        }
        public ActionResult CreateBaiBao()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBaiBao(BaiBao baiBao)
        {
            var id = "B" + GenerateId(5);
            if (ModelState.IsValid)
            {
                baiBao.MaBaiBao="B"+GenerateId(5);
                baiBao.TrangThaiBaiBao = false;
                baiBao.TrangThaiDuyet = false;
                db.BaiBaos.Add(baiBao);
                db.SaveChanges();
            }
            else
            {
                return View();

            }
            return RedirectToAction("ImportImage/"+id);
        }
        public void LuuAnh(string id, HttpPostedFileBase hinh)
        {
            HinhAnh img = new HinhAnh();
            var urlTuongdoi = "/images/";
            string ten = Path.GetFileNameWithoutExtension(hinh.FileName);
            string duoi = Path.GetExtension(hinh.FileName);
            string tenFile = ten + duoi;
            var urlTuyetDoi = Server.MapPath(urlTuongdoi);
            string fullDuongDan = Path.Combine(urlTuyetDoi, tenFile);

            int i = 1;
            while (System.IO.File.Exists(fullDuongDan))
            {
                tenFile = $"{ten}-{i}{duoi}";
                fullDuongDan = Path.Combine(urlTuyetDoi, tenFile);
                i++;
            }

            hinh.SaveAs(fullDuongDan);
            img.MaHinhAnh = GenerateId(10);
            img.MaBaiBao = id;
            img.TenHinhAnh = urlTuongdoi + tenFile;

            db.HinhAnhs.Add(img);
            db.SaveChanges();


        }
        public ActionResult ImportImage(string id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult ImportImage(List<HttpPostedFileBase> listimg,string id)
        {

            if (ModelState.IsValid)
            {
                foreach(var img in listimg)
                {
                    LuuAnh(id, img);
                }
                TempData["thongbao"] = "taothanhcong";
                return RedirectToAction("QuanLyBaiBao");
            }
            else
            {
                return View();

            }
           
        }
        public ActionResult EditBaiBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baibao = db.BaiBaos.Find(id);
            if (baibao == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTL = new SelectList(db.TheLoais, "MaTL", "TenTL", baibao.MaTL);
            return View(baibao);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBaiBao([Bind(Include = "MaBaiBao,TieuDe,NoiDung,NgayDangBai,MaHinhAnh,TrangThaiDuyet,TrangThaiBaiBao,MaTL,MaUser")] BaiBao baiBao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baiBao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuanLyBaiBao");
            }
            ViewBag.MaTL = new SelectList(db.TheLoais, "MaTL", "TenTL", baiBao.MaTL);

            return View(baiBao);
        }

    }
}