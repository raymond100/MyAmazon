using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers 
{
    //[Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            categoryVM.categories = await _categoryService.GetAllAsync();
            return View(categoryVM);

        }
        [HttpGet]
        public async Task<IActionResult> CreateUpdate(int id)
        {
            CategoryViewModel vm = new CategoryViewModel();
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Category= await _categoryService.GetByIdAsync(id);
                if (vm.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpdate(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Category.Id == 0)
                {
                    await _categoryService.CreateAsync(vm.Category);
                    TempData["success"] = "Category Created Done!";
                }
                else
                {
                    await _categoryService.UpdateAsync(vm.Category);
                    TempData["success"] = "Category Updated Done!";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var category = await _categoryRepository.GetByIdAsync(id);

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}


        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var category = await _categoryService.GetByIdAsync(Id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteAsync(category);
            TempData["success"] = "Category Deleted!";

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category =  await _categoryService.GetByIdAsync((int)id);
            if (category != null)
            {
                await _categoryService.DeleteAsync(category);
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteData(int id)
        {
            var category = await _categoryService.GetByIdAsync(id); ;

            if (category == null)
            {
                return NotFound();
            }
            await _categoryService.DeleteAsync(category);

            TempData["success"] = "Category Deleted Done!";
            return RedirectToAction("Index");
        }
    }
}
