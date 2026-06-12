using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class InstructorsController : BaseController
{
    public InstructorsController(SchoolContext context, INotificationService notificationService)
        : base(context, notificationService)
    {
    }

    // GET: Instructors - All roles can view
    public IActionResult Index(int? id, int? courseID)
    {
        var viewModel = new InstructorIndexData
        {
            Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(d => d.Department)
                .OrderBy(i => i.LastName)
                .ToList()
        };

        if (id != null)
        {
            ViewBag.InstructorID = id.Value;
            viewModel.Courses = viewModel.Instructors
                .Single(i => i.ID == id.Value)
                .CourseAssignments
                .Select(s => s.Course)
                .ToList();
        }

        if (courseID != null && viewModel.Courses != null)
        {
            ViewBag.CourseID = courseID.Value;
            viewModel.Enrollments = viewModel.Courses
                .Single(x => x.CourseID == courseID)
                .Enrollments;
        }

        return View(viewModel);
    }

    // GET: Instructors/Details/5 - All roles can view details
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var instructor = db.Instructors.Find(id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // GET: Instructors/Create
    public IActionResult Create()
    {
        var instructor = new Instructor
        {
            CourseAssignments = new List<CourseAssignment>()
        };
        PopulateAssignedCourseData(instructor);
        return View(instructor);
    }

    // POST: Instructors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
    {
        if (selectedCourses != null)
        {
            instructor.CourseAssignments = [];
            foreach (var course in selectedCourses)
            {
                var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = int.Parse(course) };
                instructor.CourseAssignments.Add(courseToAdd);
            }
        }

        if (ModelState.IsValid)
        {
            db.Instructors.Add(instructor);
            db.SaveChanges();

            await SendEntityNotificationAsync("Instructor", instructor.ID.ToString(), EntityOperation.CREATE);

            return RedirectToAction(nameof(Index));
        }

        PopulateAssignedCourseData(instructor);
        return View(instructor);
    }

    // GET: Instructors/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var instructor = db.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
                .ThenInclude(c => c.Course)
            .SingleOrDefault(i => i.ID == id);

        if (instructor == null)
        {
            return NotFound();
        }

        PopulateAssignedCourseData(instructor);
        return View(instructor);
    }

    private void PopulateAssignedCourseData(Instructor instructor)
    {
        var allCourses = db.Courses;
        var instructorCourses = new HashSet<int>(instructor.CourseAssignments?.Select(c => c.CourseID) ?? Enumerable.Empty<int>());
        var viewModel = new List<AssignedCourseData>();
        foreach (var course in allCourses)
        {
            viewModel.Add(new AssignedCourseData
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.CourseID)
            });
        }
        ViewBag.Courses = viewModel;
    }

    // POST: Instructors/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var instructorToUpdate = db.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
                .ThenInclude(c => c.Course)
            .SingleOrDefault(i => i.ID == id);

        if (instructorToUpdate == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            instructorToUpdate.LastName = instructor.LastName;
            instructorToUpdate.FirstMidName = instructor.FirstMidName;
            instructorToUpdate.HireDate = instructor.HireDate;
            instructorToUpdate.OfficeAssignment = instructor.OfficeAssignment;

            try
            {
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                db.SaveChanges();

                await SendEntityNotificationAsync("Instructor", instructorToUpdate.ID.ToString(), EntityOperation.UPDATE);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        PopulateAssignedCourseData(instructorToUpdate);
        return View(instructorToUpdate);
    }

    private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
    {
        if (selectedCourses == null)
        {
            instructorToUpdate.CourseAssignments = [];
            return;
        }

        var selectedCoursesHS = new HashSet<string>(selectedCourses);
        var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
        foreach (var course in db.Courses)
        {
            if (selectedCoursesHS.Contains(course.CourseID.ToString()))
            {
                if (!instructorCourses.Contains(course.CourseID))
                {
                    instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                }
            }
            else if (instructorCourses.Contains(course.CourseID))
            {
                var courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == course.CourseID);
                if (courseToRemove != null)
                {
                    db.Entry(courseToRemove).State = EntityState.Deleted;
                }
            }
        }
    }

    // GET: Instructors/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var instructor = db.Instructors.Find(id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // POST: Instructors/Delete/5 - Only admins can delete instructors
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var instructor = db.Instructors
            .Include(i => i.OfficeAssignment)
            .SingleOrDefault(i => i.ID == id);

        if (instructor == null)
        {
            return NotFound();
        }

        db.Instructors.Remove(instructor);

        var department = db.Departments.SingleOrDefault(d => d.InstructorID == id);
        if (department != null)
        {
            department.InstructorID = null;
        }

        db.SaveChanges();

        await SendEntityNotificationAsync("Instructor", id.ToString(), EntityOperation.DELETE);

        return RedirectToAction(nameof(Index));
    }
}
