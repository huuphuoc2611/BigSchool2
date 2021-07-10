using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
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
            objCourse.ListCategory = context.Categories.ToList();

            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {

            BigSchoolContext1 context = new BigSchoolContext1();
            ModelState.Remove("LectureId");
            //if (!ModelState.IsValid)
            //    {
            //    course.ListCategory = context.Categories.ToList();
            //    return View("Create", course);
            //}

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LecturerId = user.Id;
            context.Courses.Add(course);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Attending()
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach(Attendances temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext1 context = new BigSchoolContext1();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach(Course i in courses)
            {
                i.LectureName = currentUser.Name;
            }
            return View(courses);
        }
        public ActionResult ListCourse()
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            var listCourse = context.Courses.ToList();
            return View(listCourse);

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            Course c = context.Courses.SingleOrDefault(p => p.Id == id);
            c.ListCategory = context.Categories.ToList();
            return View(c);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Course c)
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            Course edit = context.Courses.SingleOrDefault(p => p.Id == c.Id);

            if (edit != null)
            {
                context.Courses.AddOrUpdate(c);
                context.SaveChanges();

            }
            return RedirectToAction("Mine");

        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            BigSchoolContext1 context = new BigSchoolContext1();
            Course delete = context.Courses.SingleOrDefault(p => p.Id == id);
            return View(delete);
        }
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {


            BigSchoolContext1 context = new BigSchoolContext1();
            Course delete = context.Courses.SingleOrDefault(p => p.Id == id);
            if (delete != null)
            {
                context.Courses.Remove(delete);
                context.SaveChanges();

            }
            return RedirectToAction("Mine");
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext1 context = new BigSchoolContext1();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId ==currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }

    }
}
//public ActionResult Edit(int? id)
//{
//    BigSchoolContext1 context = new BigSchoolContext1();
//    ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Course course = context.Courses.Where(p => p.Id == id && p.LecturerId == currentUser.Id).FirstOrDefault();
//    if (course == null)
//    {
//        return HttpNotFound();
//    }
//    Course objCourse = new Course();
//    objCourse.ListCategory = context.Categories.ToList();


//    ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
//    //return View(course);
//    return View(objCourse);
//}
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "Id,LectureId,Place,DateTime,CategoryId")] Course course) //ai lam cai nay v
//{
//    ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
//    BigSchoolContext1 context = new BigSchoolContext1();
//    course.LecturerId = currentUser.Id;
//    ModelState.Remove("LectureId");
//    if (ModelState.IsValid)
//    {
//        context.Entry(course).State = EntityState.Modified;
//        context.SaveChanges();
//        return RedirectToAction("Mine");
//    }
//    ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
//    return View(course);
//}
//public ActionResult Delete(int? id)
//{
//    BigSchoolContext1 context = new BigSchoolContext1();
//    ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Course course = context.Courses.Where(c => c.Id == id && c.LecturerId == currentUser.Id).FirstOrDefault();
//    if (course == null)
//    {
//        return HttpNotFound();
//    }
//    return View(course);
//}


//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteCourse(int id)
//{
//    BigSchoolContext1 context = new BigSchoolContext1();
//    ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
//    Course course = context.Courses.Where(c => c.Id == id && c.LecturerId == currentUser.Id).FirstOrDefault();
//    Attendance attendance = context.Attendances.Where(a => a.CourseId == id).FirstOrDefault();
//    context.Courses.Remove(course);
//    if (attendance != null)
//    {
//        context.Attendances.Remove(attendance);

//    }
//    context.SaveChanges();
//    return RedirectToAction("Mine");
//}