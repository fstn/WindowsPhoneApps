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
    [Authorize(Roles="Admin")]
    public class ArticlesController : Controller
    {
        private Database1Entities context = new Database1Entities();

        //
        // GET: /Articles/

        public ViewResult Index()
        {
            return View(context.Article.ToList());
        }

        //
        // GET: /Articles/Details/5

        public ViewResult Details(int? id)
        {
            if (id != null)
            {

                try
                {
                    Article article = context.Article.Single(x => x.Id == id);
                    return View("Details", article);
                }
                catch (InvalidOperationException)
                {
                    return View("Details");
                }
            }
            else
            {
                return View("Error");
            }
        }

        //
        // GET: /Articles/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Articles/Create

        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                context.Article.Add(article);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        //
        // GET: /Articles/Edit/5

        public ActionResult Edit(int id)
        {
            try
            {
                Article article = context.Article.Single(x => x.Id == id);
                return View(article);
            }
            catch (InvalidOperationException)
            {

                return View();
            }
        }

        //
        // POST: /Articles/Edit/5

        [HttpPost]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                context.Entry(article).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        //
        // GET: /Articles/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                Article article = context.Article.Single(x => x.Id == id);
                return View(article);
            }
            catch (InvalidOperationException)
            {

                return View();
            }
        }

        //
        // POST: /Articles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Article article = context.Article.Single(x => x.Id == id);
                context.Article.Remove(article);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet, ActionName("Error")]
        public ActionResult Error()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}