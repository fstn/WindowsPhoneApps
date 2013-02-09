using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models.Generated;

namespace MvcApplication2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private Database1Entities context = new Database1Entities();

        //
        // GET: /Authors/

        public ViewResult Index()
        {
            return View(context.Author.Include(author => author.Articles).ToList());
        }

        //
        // GET: /Authors/Details/5

        public ViewResult Details(int id)
        {
            Author author = context.Author.Single(x => x.Id == id);
            return View(author);
        }

        //
        // GET: /Authors/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Authors/Create

        [HttpPost]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                context.Author.Add(author);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(author);
        }
        
        //
        // GET: /Authors/Edit/5
 
        public ActionResult Edit(int id)
        {
            Author author = context.Author.Single(x => x.Id == id);
            return View(author);
        }

        //
        // POST: /Authors/Edit/5

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                context.Entry(author).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        //
        // GET: /Authors/Delete/5
 
        public ActionResult Delete(int id)
        {
            Author author = context.Author.Single(x => x.Id == id);
            return View(author);
        }

        //
        // POST: /Authors/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = context.Author.Single(x => x.Id == id);
            context.Author.Remove(author);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}