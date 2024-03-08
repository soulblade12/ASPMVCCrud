using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BLL.DTOs;
using Microsoft.AspNetCore.Components.Web;
using MyWebFormApp.BO;

namespace SampleMVC.Controllers;

public class ArticlesController : Controller
{
    private readonly IArticleBLL _ArticleBLL;
    private readonly ICategoryBLL _categoryBLL;

    public ArticlesController(IArticleBLL ArticleBLL,ICategoryBLL categoryBLL)
    {
        _ArticleBLL = ArticleBLL;
        _categoryBLL = categoryBLL;
    }

    public IActionResult Index(int? CategoryID)
    {
       ViewBag.CategoryID = CategoryID;
    ViewBag.Message = CategoryID != null ? $"You selected {CategoryID}" : "";

    // Get the list of categories
    ViewBag.Categories = _categoryBLL.GetAll();

    // Fetch articles based on the selected CategoryID if provided
    var articles = CategoryID != null ? _ArticleBLL.GetArticleByCategory(CategoryID.Value) : _ArticleBLL.GetArticleWithCategory(); 

    // Pass the list of articles to the view
    return View(articles);
  
    }

        public IActionResult Create()
    {
        var dropdwn = _categoryBLL.GetAll();
        return View(dropdwn);
    }

    [HttpPost]
    public IActionResult Create(ArticleCreateDTO articleCreate)
    {
        try
        {
            // articleCreate.CategoryID = Convert.ToInt32(articleCreate.CategoryID);
            // ArticleCreateDTO test = new ArticleCreateDTO();
            // test.Title = Title;
            // test.Details = Details;
            // test.CategoryID = Convert.ToInt32(CategoryID);
            _ArticleBLL.Insert(articleCreate);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
        }
        catch (Exception ex)
        {
           
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
        }
        return RedirectToAction("Index");
    }

        public IActionResult Edit(int id)
    {
        var model = _ArticleBLL.GetArticleById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleUpdateDTO articleUpdate)
    {
        try
        {
            _ArticleBLL.Update(articleUpdate);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(articleUpdate);
        }
        return RedirectToAction("Index");
    }

      public IActionResult Delete(int id)
    {
        // _ArticleBLL.QueryString
         _ArticleBLL.Delete(id);
        return RedirectToAction("Index");
    }

}
