using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

//using Bulky.DataAcess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;


namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Category> objCategories = _unitOfWork.Category.GetAll().ToList();
            return View(objCategories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order cannot exactly match the Name");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is not a valid name");
            }
            if (ModelState.IsValid)
            {
               _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"]="Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }

      
        public IActionResult Edit(int? Id)
        {
            if(Id==null || Id==0){
                return NotFound();
            }
        Category categoryfromdb = _unitOfWork.Category.Get(u=>u.Id==Id);
            // Category categoryfromdb1 = _db.Categories.FirstOrDefault(u=>u.Id ==Id);// here only we can use the primary key
            // Category categoryfromdb2 = _db.Categories.Where(u=>u.Id ==Id).FirstOrDefault();
            if(categoryfromdb == null){

                return NotFound();
            }
            return View(categoryfromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order cannot exactly match the Name");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is not a valid name");
            }
            if (ModelState.IsValid)
            {
               _unitOfWork.Category.Update(obj);
               _unitOfWork.Save();
                TempData["success"]="Category edited successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }
         public IActionResult Delete(int? Id)
        {
            if(Id==null || Id==0){
                return NotFound();
            }
            Category categoryfromdb = _unitOfWork.Category.Get(u=>u.Id==Id);
            // Category categoryfromdb1 = _db.Categories.FirstOrDefault(u=>u.Id ==Id);// here only we can use the primary key
            // Category categoryfromdb2 = _db.Categories.Where(u=>u.Id ==Id).FirstOrDefault();
            if(categoryfromdb == null){

                return NotFound();
            }
            return View(categoryfromdb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
       
        {
             Category obj = _unitOfWork.Category.Get(u=>u.Id==Id);
           if(obj ==null){
            return NotFound();
           }
           
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["success"]="Category deleted successfully";
                return RedirectToAction("Index", "Category");
        }

    }
}