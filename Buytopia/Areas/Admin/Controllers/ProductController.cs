using Buytopia.Data;
using Buytopia.Models;
using Buytopia.Models.ViewModels;
using Buytopia.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Buytopia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SF.ManagerUser)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductItemViewModel ProductItemVM { get; set; }

        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            ProductItemVM = new ProductItemViewModel()
            {
                Category = _db.Category,
                Product = new Models.Product()
            };
        }
        public async Task<IActionResult> Index()
        {
            var ProductItems = await _db.Product.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return View(ProductItems);
        }
        //GET Create
        public IActionResult Create()
        {
            return View(ProductItemVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            ProductItemVM.Product.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if(!ModelState.IsValid)
            {
                return View(ProductItemVM);
            }

            _db.Product.Add(ProductItemVM.Product);
            await _db.SaveChangesAsync();

            //work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var productItemFromDb = await _db.Product.FindAsync(ProductItemVM.Product.Id);
            if (files.Count>0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, ProductItemVM.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                productItemFromDb.Image = @"\images\" + ProductItemVM.Product.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SF.DefaultProudctImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + ProductItemVM.Product.Id + ".png");
                productItemFromDb.Image = @"\images\" + ProductItemVM.Product.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ProductItemVM.Product = await _db.Product.Include(m=>m.Category).Include(m=>m.SubCategory).SingleOrDefaultAsync(m=>m.Id==id);
            ProductItemVM.SubCategory = await _db.SubCategorie.Where(s=>s.CategoryId==ProductItemVM.Product.CategoryId).ToListAsync();
            if (ProductItemVM.Product == null)
            {
                return NotFound();
            }
            return View(ProductItemVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ProductItemVM.Product.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                ProductItemVM.SubCategory = await _db.SubCategorie.Where(s => s.CategoryId == ProductItemVM.Product.CategoryId).ToListAsync();
                return View(ProductItemVM);
            }

            //work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var productItemFromDb = await _db.Product.FindAsync(ProductItemVM.Product.Id);
            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath,productItemFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                //Upload the new file and save to database
                using (var filesStream = new FileStream(Path.Combine(uploads, ProductItemVM.Product.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                productItemFromDb.Image = @"\images\" + ProductItemVM.Product.Id + extension_new;
            }
           
            productItemFromDb.Name = ProductItemVM.Product.Name;
            productItemFromDb.Description = ProductItemVM.Product.Description;
            productItemFromDb.Price = ProductItemVM.Product.Price;
            productItemFromDb.Ratings = ProductItemVM.Product.Ratings;
            productItemFromDb.CategoryId = ProductItemVM.Product.CategoryId;
            productItemFromDb.SubCategoryId = ProductItemVM.Product.SubCategoryId;
            
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //GET : Details Product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           ProductItemVM.Product = await _db.Product.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductItemVM.Product == null)
            {
                return NotFound();
            }

            return View(ProductItemVM);
        }

        //GET : Delete Product
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductItemVM.Product = await _db.Product.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductItemVM.Product == null)
            {
                return NotFound();
            }

            return View(ProductItemVM);
        }

        //POST Delete Product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Product product = await _db.Product.FindAsync(id);

            if (product != null)
            {
                var imagePath = Path.Combine(webRootPath, product.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.Product.Remove(product);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }
    }
}