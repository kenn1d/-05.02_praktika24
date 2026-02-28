using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using praktika22.Data.Interfaces;
using praktika22.Data.Models;
using praktika22.Data.ViewModell;
using System.Collections.Generic;

namespace praktika22.Controllers
{
    public class ItemsController : Controller
    {
        private IItems IAllItems;
        private ICategorys IAllCategorys;
        VMItems VMItems = new VMItems();

        private readonly IHostingEnvironment hostingEnvironment;

        public ItemsController(IItems IAllItems, ICategorys IAllCategorys, IHostingEnvironment hostingEnvironment)
        {
            this.IAllItems = IAllItems;
            this.IAllCategorys = IAllCategorys;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult List(int id = 0)
        {
            ViewBag.Title = "Страница с предметами";
            VMItems.Items = IAllItems.AllItems;
            VMItems.Categorys = IAllCategorys.AllCategorys;
            VMItems.SelectCategory = id;

            return View(VMItems);
        }

        [HttpGet]
        public ViewResult Add()
        {
            IEnumerable<Data.Models.Categorys> Categorys = IAllCategorys.AllCategorys;
            return View(Categorys);
        }

        [HttpGet]
        public ViewResult Delete()
        {
            IEnumerable<Data.Models.Items> Items = IAllItems.AllItems;
            return View(Items);
        }

        [HttpPost]
        public RedirectResult Add(string name, string description, IFormFile files, float price, int idCategory)
        {
            if (files != null)
            {
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                var filePath = Path.Combine(uploads, files.FileName);
                files.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            Items newItems = new Items();
            newItems.Name = name;
            newItems.Description = description;
            newItems.Img = files.FileName;
            newItems.Price = (int)price;
            newItems.Category = new Categorys() { Id = idCategory };

            int id = IAllItems.Add(newItems);
            return Redirect("/Items/Update?id=" + id);
        }
    }
}
