using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using ToDoList.Models;

namespace TodoList.Controllers
{
    public class TodoItemsController : Controller
    {
        private static List<TodoItem> _todoItems = new List<TodoItem>();

        public IActionResult Index()
        {
            string userName = Request.Cookies["UserName"];
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Welcome");
            }

            ViewBag.UserName = userName;
            return View(_todoItems);
        }

        public IActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetName(string name)
        {
            Response.Cookies.Append("UserName", name, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            });

            TempData["Message"] = "Welcome, " + name + "!";
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                item.Id = _todoItems.Count + 1;
                _todoItems.Add(item);
                TempData["Message"] = "Todo item created successfully!";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _todoItems.FirstOrDefault(x => x.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Title = item.Title;
                    existingItem.Description = item.Description;
                    existingItem.Deadline = item.Deadline;
                    TempData["Message"] = "Todo item updated successfully!";
                }
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _todoItems.Remove(item);
                TempData["Message"] = "Todo item deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}