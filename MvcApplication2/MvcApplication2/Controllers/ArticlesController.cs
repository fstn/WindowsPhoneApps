using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MvcApplication2.Models.Generated;

namespace MvcApplication2.Controllers
{
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
        // GET: /Articles/JSON

        public void JSON()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var articles = context.Article.Select(article => new { article.Id, article.IdAuthor, article.Title, article.Text }); ;
            string json = serializer.Serialize(articles);
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
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
            if (ModelState.IsValid && article !=null)
            {
                try
                {
                    context.Article.Add(article);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                          validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
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

        [HttpGet, ActionName("Error")]
        public ActionResult Error()
        {
            return View();
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