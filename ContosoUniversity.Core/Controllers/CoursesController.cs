using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class CoursesController : BaseController
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;
    private readonly IWebHostEnvironment _environment;

    public CoursesController(SchoolContext context, INotificationService notificationService, IWebHostEnvironment environment)
        : base(context, notificationService)
    {
        _environment = environment;
    }

    // GET: Courses
    public IActionResult Index()
    {
        var courses = db.Courses.Include(c => c.Department);
        return View(courses.ToList());
    }

    // GET: Courses/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        PopulateDepartmentsDropDownList();
        return View(new Course());
    }

    // POST: Courses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
    {
        if (ModelState.IsValid)
        {
            if (teachingMaterialImage is { Length: > 0 })
            {
                var uploadResult = await TrySaveTeachingMaterialImageAsync(course.CourseID, teachingMaterialImage);
                if (!uploadResult.Success)
                {
                    ModelState.AddModelError("teachingMaterialImage", uploadResult.ErrorMessage);
                    PopulateDepartmentsDropDownList(course.DepartmentID);
                    return View(course);
                }

                course.TeachingMaterialImagePath = uploadResult.ApplicationPath;
            }

            db.Courses.Add(course);
            db.SaveChanges();

            await SendEntityNotificationAsync("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);

            return RedirectToAction(nameof(Index));
        }

        PopulateDepartmentsDropDownList(course.DepartmentID);
        return View(course);
    }

    // GET: Courses/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var course = db.Courses.Find(id);
        if (course == null)
        {
            return NotFound();
        }

        PopulateDepartmentsDropDownList(course.DepartmentID);
        return View(course);
    }

    // POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
    {
        if (ModelState.IsValid)
        {
            if (teachingMaterialImage is { Length: > 0 })
            {
                var oldImagePath = course.TeachingMaterialImagePath;
                var uploadResult = await TrySaveTeachingMaterialImageAsync(course.CourseID, teachingMaterialImage);
                if (!uploadResult.Success)
                {
                    ModelState.AddModelError("teachingMaterialImage", uploadResult.ErrorMessage);
                    PopulateDepartmentsDropDownList(course.DepartmentID);
                    return View(course);
                }

                DeleteTeachingMaterialImage(oldImagePath);
                course.TeachingMaterialImagePath = uploadResult.ApplicationPath;
            }

            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();

            await SendEntityNotificationAsync("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);

            return RedirectToAction(nameof(Index));
        }

        PopulateDepartmentsDropDownList(course.DepartmentID);
        return View(course);
    }

    // GET: Courses/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = db.Courses.Find(id);
        if (course == null)
        {
            return NotFound();
        }

        var courseTitle = course.Title;
        DeleteTeachingMaterialImage(course.TeachingMaterialImagePath);

        db.Courses.Remove(course);
        db.SaveChanges();

        await SendEntityNotificationAsync("Course", id.ToString(), courseTitle, EntityOperation.DELETE);

        return RedirectToAction(nameof(Index));
    }

    private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
    {
        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", selectedDepartment);
    }

    private async Task<(bool Success, string ApplicationPath, string ErrorMessage)> TrySaveTeachingMaterialImageAsync(int courseId, IFormFile teachingMaterialImage)
    {
        var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();

        if (!AllowedImageExtensions.Contains(fileExtension))
        {
            return (false, null, "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
        }

        if (teachingMaterialImage.Length > MaxImageSizeBytes)
        {
            return (false, null, "File size must be less than 5MB.");
        }

        try
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "Uploads", "TeachingMaterials");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"course_{courseId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await teachingMaterialImage.CopyToAsync(stream);

            return (true, $"~/Uploads/TeachingMaterials/{fileName}", null);
        }
        catch (Exception ex)
        {
            return (false, null, "Error uploading file: " + ex.Message);
        }
    }

    private void DeleteTeachingMaterialImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return;
        }

        var relativePath = imagePath.TrimStart('~', '/', '\\')
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
        var filePath = Path.Combine(_environment.WebRootPath, relativePath);

        if (!System.IO.File.Exists(filePath))
        {
            return;
        }

        try
        {
            System.IO.File.Delete(filePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
        }
    }
}
