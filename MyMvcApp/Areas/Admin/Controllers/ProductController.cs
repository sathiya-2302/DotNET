using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

//using Bulky.DataAcess.Data;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProducts);
        }

        public IActionResult Upsert(int? id)//update and insert
        {
            IEnumerable<SelectListItem> CategoryList;
            // ViewBag.CategoryList = CategoryList;
            // ViewData["CategoryList"]=CategoryList;
            ProductVM productVM = new()
            {

                CategoryList = _unitOfWork.Category.GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {//createion
                return View(productVM);
            }
            else
            {//updation

                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);

            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");
                     //var extension = Path.GetExtension(file.FileName);
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName ), FileMode.Create))
                    {

                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"/images/product/" + fileName ;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll()
           .Select(u => new SelectListItem
           {
               Text = u.Name,
               Value = u.Id.ToString()
           });

            }
            return View(productVM);


        }


        // public IActionResult Edit(int? Id)
        // {
        //     if (Id == null || Id == 0)
        //     {
        //         return NotFound();
        //     }
        //     Product productfromdb = _unitOfWork.Product.Get(u => u.Id == Id);

        //     if (productfromdb == null)
        //     {

        //         return NotFound();
        //     }
        //     return View(productfromdb);
        // }
        // [HttpPost]
        // public IActionResult Edit(Product obj)
        // {

        //     if (ModelState.IsValid)
        //     {
        //         _unitOfWork.Product.Update(obj);
        //         _unitOfWork.Save();
        //         TempData["success"] = "Product edited successfully";
        //         return RedirectToAction("Index", "Product");
        //     }
        //     return View();


        // }
        // public IActionResult Delete(int? Id)
        // {
        //     if (Id == null || Id == 0)
        //     {
        //         return NotFound();
        //     }
        //     Product productfromdb = _unitOfWork.Product.Get(u => u.Id == Id);


        //     if (productfromdb == null)
        //     {

        //         return NotFound();
        //     }
        //     return View(productfromdb);
        // }
        // [HttpPost, ActionName("Delete")]
        // public IActionResult DeletePOST(int? Id)

        // {
        //     Product obj = _unitOfWork.Product.Get(u => u.Id == Id);
        //     if (obj == null)
        //     {
        //         return NotFound();
        //     }

        //     _unitOfWork.Product.Remove(obj);
        //     _unitOfWork.Save();
        //     TempData["success"] = "Product deleted successfully";
        //     return RedirectToAction("Index", "Product");
        // }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProducts });
        }

        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            Product productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == Id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error wile deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                   productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}