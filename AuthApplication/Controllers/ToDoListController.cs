using Microsoft.AspNetCore.Mvc;
using AuthApplication.Data;
using AuthApplication.Models;

namespace AuthApplication.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ToDoListController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<ToDoListViewModel> ToDoList = _db.ToDoList.ToList();
            return View(ToDoList);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ToDoListViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var item = new ToDoListViewModel
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    DateofCreation = DateTime.Now,
                    IsCompleted = obj.IsCompleted,

                };
                _db.ToDoList.Add(item);
                _db.SaveChanges();
                return Json(item);
            }

            return BadRequest();
        }

        [HttpPut]
        public IActionResult Edit([FromBody] ToDoListViewModel obj)
        {
            var Check = _db.ToDoList.Find(obj.Id);
            if (Check != null)
            {
                Check.Name = obj.Name;
                Check.Description = obj.Description;
                Check.DateofCreation = obj.DateofCreation;
                Check.IsCompleted = obj.IsCompleted;

                _db.SaveChanges();

                return Json(Check);
            }

            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        { 
            var check = _db.ToDoList.Find(id);
            if (check != null)
            {
                _db.ToDoList.Remove(check);
                _db.SaveChanges();
                return Json(check);
            }
            return NotFound();
        }
    }
}
