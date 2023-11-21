using LanchesMac.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminImagesController : Controller
    {
        private readonly ConfigurationImages _myConfig;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminImagesController(
            IWebHostEnvironment hostingEnvironment,
            IOptions<ConfigurationImages> myConfiguration)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _myConfig = myConfiguration?.Value ?? throw new ArgumentNullException(nameof(myConfiguration));
        }

        /// <summary>
        /// Displays the index view.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Uploads files to the server.
        /// </summary>
        /// <param name="files">List of files to upload.</param>
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ViewData["Error"] = "Error: File(s) not selected";
                return View(ViewData);
            }

            if (files.Count > 10)
            {
                ViewData["Error"] = "Error: Quantity of files exceeds the limit";
                return View(ViewData);
            }

            long size = files.Sum(f => f.Length);
            var filePathsName = new List<string>();
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.ProductsImagesFolderName);

            foreach (var formFile in files)
            {
                if (formFile.FileName.EndsWith(".jpg") || formFile.FileName.EndsWith(".gif") || formFile.FileName.EndsWith(".png"))
                {
                    var fileNameWithPath = Path.Combine(filePath, formFile.FileName);
                    filePathsName.Add(fileNameWithPath);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            ViewData["Result"] = $"{files.Count} files sent to server, with a total of {size} bytes";
            ViewBag.Files = filePathsName;

            return View(ViewData);
        }

        /// <summary>
        /// Retrieves images from the server.
        /// </summary>
        public IActionResult GetImages()
        {
            FileManagerModel model = new FileManagerModel();
            var userImagesPath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.ProductsImagesFolderName);

            DirectoryInfo dir = new DirectoryInfo(userImagesPath);
            FileInfo[] files = dir.GetFiles();

            model.PathImagesProduct = _myConfig.ProductsImagesFolderName;

            if (files.Length == 0)
            {
                ViewData["Error"] = $"There are no files in {userImagesPath}";
            }

            model.Files = files;
            return View(model);
        }

        /// <summary>
        /// Deletes a file from the server.
        /// </summary>
        /// <param name="fName">File name to be deleted.</param>
        public IActionResult DeleteFile(string fName)
        {
            string deleteImage = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.ProductsImagesFolderName, fName);

            if (System.IO.File.Exists(deleteImage))
            {
                System.IO.File.Delete(deleteImage);
                ViewData["Deleted"] = $"File(s) deleted: {deleteImage}";
            }

            return View("Index");
        }
    }
}
