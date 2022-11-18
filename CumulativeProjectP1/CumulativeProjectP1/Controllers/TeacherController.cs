using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CumulativeProjectP1.Models;
using System.Diagnostics;

namespace CumulativeProjectP1.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher/List?search_key={value}
        public ActionResult List(string search_key)
        {
            //data from the user typing in the search bar
            
            Debug.WriteLine("The search key is "+ search_key);
            
            
            // I want to recive all teachers in the system
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<teacher> Myteacher = MyController.listTeachers(search_key);

            //Debug.WriteLine("i have accesed " + Myteacher.Count());

            return View(Myteacher);
        }


        //GET: /teacher/show/{teacherid}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}