using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataContext.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DataContext;
using ServiceLayer;

namespace BlobStorage_File_Upload.Controllers
{
    public class ProductsController : Controller
    {
        private UnitOfWork _context;
        IHostingEnvironment env;
        private NumberToWord objNumberToWord;

        public ProductsController(IHostingEnvironment _env)
        {
            this._context = new UnitOfWork();
            env = _env;
            this.objNumberToWord = new NumberToWord();
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_context.ProductRepo.GetAll());
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.ProductRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {

                #region Read File Content

                var uploads = Path.Combine(env.WebRootPath, "uploads\\");
                bool exists = Directory.Exists(uploads);
                if (!exists)
                    Directory.CreateDirectory(uploads);

                // https://www.c-sharpcorner.com/article/upload-files-in-azure-blob-storage-using-asp-net-core/
                // Code has been copied from the above link but there was a bug in it - seen below
                var fileName = Path.GetFileName(product.File.FileName);
                string mimeType = product.File.ContentType;
                byte[] fileData = System.IO.File.ReadAllBytes(@"C:\\Users\\robjs\\Desktop\\" + product.File.FileName);

                BlobStorageService objBlobService = new BlobStorageService();

                product.ImagePath = objBlobService.UploadFileToBlob(product.File.FileName, fileData, mimeType);
                #endregion

                _context.ProductRepo.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.ProductRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,Name,UnitPrice,Description,ImageName,ImagePath,CreatedDate,UpdatedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ProductRepo.Modify(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.ProductRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.ProductRepo.GetById(id);
            BlobStorageService objBlob = new BlobStorageService();
            objBlob.DeleteBlobData(product.ImagePath);
            _context.ProductRepo.Delete(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.ProductRepo.GetAll().Any(e => e.ProductId == id);
        }
    }
}
