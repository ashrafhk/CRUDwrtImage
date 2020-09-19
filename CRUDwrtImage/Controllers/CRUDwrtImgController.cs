using CRUDwrtImage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CRUDwrtImage.Controllers
{
    public class CRUDwrtImgController : Controller
    {
        employeeEntities db = new employeeEntities();
        // GET: CRUDwrtImg
        public ActionResult Index()
        {
            return View(db.tbl_img.ToList());
        }
        //by default Get method
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, tbl_img emp)
        {
            //store filename
            string filename = Path.GetFileName(file.FileName);
            //store filename with date
            string _filename = DateTime.Now.ToString("ddmmyyyy") + filename;
            //store file extension
            string extension = Path.GetExtension(file.FileName);
            //store file path
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            //call db table
            emp.img = "~/images/" + _filename;
            //extension types
            if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if(file.ContentLength <= 1000000)
                {
                    //save image in db table
                    db.tbl_img.Add(emp);
                    //after chkng omg properties now chk the size of image
                    if(db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.msg = "Image added successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.msg = "Size limit is 1MB";
                }
            }
            return View();
        }
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find data behalf of id
            var tbl_img = db.tbl_img.Find(id);
            Session["imgPath"] = tbl_img.img;
            if(tbl_img == null)
            {
                HttpNotFound();
            }
            return View(tbl_img);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, tbl_img emp)
        {
            if (ModelState.IsValid)
            {
                if(file != null)
                {
            //store filename
            string filename = Path.GetFileName(file.FileName);
            //store filename with date
            string _filename = DateTime.Now.ToString("ddmmyyyy") + filename;
            //store file extension
            string extension = Path.GetExtension(file.FileName);
            //store file path
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            //call db table
            emp.img = "~/images/" + _filename;
            //extension types
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if (file.ContentLength <= 1000000)
                {
                    //update image in db table
                    db.Entry(emp).State = EntityState.Modified;
                    string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                    //after chkng omg properties now chk the size of image
                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                        TempData["msg"] = "Record Updated";
                    }
                }
                else
                {
                    emp.img = Session["imgPath"].ToString();
                    db.Entry(emp).State = EntityState.Modified;
                    if(db.SaveChanges() > 0)
                    {
                        TempData["msg"] = "Record Updated";
                        return RedirectToAction("Index");
                    }
                }
            }
                }
            }
            else
            {

            }
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find data behalf of id
            var tbl_img = db.tbl_img.Find(id);
            //Session["imgPath"] = tbl_img.img;
            if (tbl_img == null)
            {
                HttpNotFound();
            }
            return View(tbl_img);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find data behalf of id
            var tbl_img = db.tbl_img.Find(id);
            //Session["imgPath"] = tbl_img.img;
            if (tbl_img == null)
            {
                HttpNotFound();
            }
            string currentimg = Request.MapPath(tbl_img.img);
            db.Entry(tbl_img).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(currentimg))
                {
                    System.IO.File.Delete(currentimg);
                }
                TempData["msg"] = "Record Deleted";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}