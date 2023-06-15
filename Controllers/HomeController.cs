using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext db;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        List<Dish> allDishes = db.Dishes.ToList();
        return View("AllDishes", allDishes);
    }

    [HttpGet("dishes/new")]
    public IActionResult NewDish()
    {
        return View("New");
    }

    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (!ModelState.IsValid)
        {
            return View("New");
        }

        db.Dishes.Add(newDish);

        db.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet("dishes/{id}")]
    public IActionResult ViewDish(int id)
    {
        Dish? dish = db.Dishes.FirstOrDefault(dish => dish.DishId == id);

        if (dish == null)
        {
            return RedirectToAction("Index");
        }

        return View("Details", dish);
    }

    [HttpPost("dishes/{id}/delete")]
    public IActionResult DeleteDish(int id)
    {
        Dish? dish = db.Dishes.FirstOrDefault(dish => dish.DishId == id);

        if (dish != null)
        {
            db.Dishes.Remove(dish);
            db.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpGet("dishes/{id}/edit")]
    public IActionResult EditDish(int id)
    {
        Dish? dish = db.Dishes.FirstOrDefault(dish => dish.DishId == id);

        if (dish == null)
        {
            return RedirectToAction("Index");
        }

        return View("Edit", dish);
    }

    [HttpPost("dishes/{id}/edit")]
    public IActionResult UpdateDish(int id, Dish updatedDish)
    {
        if (!ModelState.IsValid)
        {
            return EditDish(id);
        }

        Dish? dbDish = db.Dishes.FirstOrDefault(dish => dish.DishId == id);

        if (dbDish == null)
        {
            return RedirectToAction("Index");
        }

        dbDish.Name = updatedDish.Name;
        dbDish.Chef = updatedDish.Chef;
        dbDish.Tastiness = updatedDish.Tastiness;
        dbDish.Calories = updatedDish.Calories;
        dbDish.Description = updatedDish.Description;
        dbDish.UpdatedAt = DateTime.Now;

        db.SaveChanges();

        return RedirectToAction("ViewDish", new { id = dbDish.DishId });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
