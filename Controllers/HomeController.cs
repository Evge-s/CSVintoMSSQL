using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSV_Base.Models;
using CSV_Base.Models.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using CSV_Base.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace CSV_Base.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private FileParser fileParser = new FileParser();        
        private DataContext dataContext;
        private IWebHostEnvironment appEnvironment;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext, IWebHostEnvironment appEnvironment)
        {
            this.logger = logger;
            this.dataContext = dataContext;
            this.appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(dataContext.People);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                string fullPath = appEnvironment.WebRootPath + path;

                // save file in wwwroot/Files/
                using (var fileSrtream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileSrtream);
                }
                FileCsv csv = new FileCsv { Name = uploadedFile.FileName, Path = path, UploadDate = DateTime.Now };

                dataContext.Files.Add(csv);
                dataContext.People.AddRange(fileParser.CsvFileToModel(fullPath));
                dataContext.SaveChanges();
            }            

            return RedirectToAction("Index");
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await dataContext.People.FindAsync(id);

            dataContext.People.Remove(person);
            await dataContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await dataContext.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataContext.Update(person);
                    await dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }
        private bool PersonExists(int id)
        {
            return dataContext.People.Any(e => e.Id == id);
        }
    }
}
