using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Category.ToList();

            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {

            BigSchoolContext1 context = new BigSchoolContext1();
            ModelState.Remove("LectureId");
            if (!ModelState.IsValid)
                {
                course. ListCategory = context.Category.ToList();
                return View("Create", course);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LectureId = user.Id;
            context.Course.Add(course);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Attending ()
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendance = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var course = new List<Course>();
            foreach (Attendance temp in listAttendance)
            {
                Course objCourse = temp.Course;
                objCourse.LectureId = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LectureId).Name;
                course.Add(objCourse);
            }
            return View(course);
        }
        public ActionResult Mine ()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext1 context = new BigSchoolContext1();
            var course = context.Course.Where(c => c.LectureId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in course)
            {
                i.LectureName = currentUser.Name;
            }
            return View(course);
        }
    }
}