//using DomainModels.Entities;
//using DomainModels.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Repository;
//using System;
//using System.IO;

//namespace UI.Areas.Admin.Controllers
//{
//    public class ProductController : Controller
//    {
//        IHostingEnvironment env;
//        public ProductController() : base(_uow)
//        {
//            env = _env;
//        }

//        void BindCategory()
//        {
//            ViewBag.CategoryList = uow.CategoryRepo.GetAll();
//        }

//        // GET: Admin/Product
//        public ActionResult Index()
//        {
//            return View(uow.ProductRepo.GetAll());
//        }

//        public ActionResult Create()
//        {
//            BindCategory();
//            return View();
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public ActionResult Create(ProductModel model)
//        {
//            try
//            {
//                var uploads = Path.Combine(env.WebRootPath, "uploads");

//                bool exists = Directory.Exists(uploads);
//                if (!exists)
//                    Directory.CreateDirectory(uploads);

//                //saving file
//                var fileName = Path.GetFileName(model.file.FileName);
//                var fileStream = new FileStream(Path.Combine(uploads, model.file.FileName), FileMode.Create);
//                model.file.CopyToAsync(fileStream);

//                model.ImageName = fileName;
//                model.ImagePath = uploads + fileName;// "/Uploads/" + fileName;

//                Product data = new Product
//                {
//                    ProductId = model.ProductId,
//                    Name = model.Name,
//                    UnitPrice = model.UnitPrice,
//                    CategoryId = model.CategoryId,
//                    Description = model.Description,
//                    ImagePath = model.ImagePath,
//                    ImageName = model.ImageName
//                };

//                uow.ProductRepo.Add(data);
//                uow.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {

//            }
//            BindCategory();
//            return View();
//        }

//        public ActionResult Edit(int id)
//        {
//            BindCategory();
//            Product data = uow.ProductRepo.GetById(id);
//            ProductModel model = new ProductModel();
//            if (data != null)
//            {
//                model.ProductId = data.ProductId;
//                model.Name = data.Name;
//                model.UnitPrice = data.UnitPrice;
//                model.CategoryId = data.CategoryId;
//                model.Description = data.Description;
//                model.ImagePath = data.ImagePath;
//            }
//            return View(model);
//        }
//    }
//}