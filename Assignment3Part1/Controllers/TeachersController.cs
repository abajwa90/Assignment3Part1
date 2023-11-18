using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Assignment3Part1.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Teachers
        public ActionResult List(string SearchKey = null)
        {
            List<Teacher> teachersList = new List<Teachers> ();
            
            TeacherDataController controller = new TeacherDataController ();

            teachersList = (List<Teachers>)Controller.ListTeachers(SearchKey);

            return View(teachersList);
        }
        //GET: Teacher/Show/{TeacherID}]
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher selectedTeacher = Controller.FindTeacher(id);

            return View(selectedTeacher);
        }
    }
}