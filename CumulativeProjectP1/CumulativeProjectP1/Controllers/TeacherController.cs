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

        //GET: /teacher/add

        public ActionResult Add()
        {
            return View();
        }

        //POST: /teacher/create
        public ActionResult Create(string teacherfname, string teacherlname, string employeenumber, decimal salary, DateTime hiredate)//,ADD the rest from add
        {
            Debug.WriteLine("trying to add teacher with name " + teacherfname);
            
            teacher NewTeacher = new teacher();
            NewTeacher.teacherfname = teacherfname;
            NewTeacher.teacherlname = teacherlname;
            NewTeacher.employeenumber = employeenumber;
            NewTeacher.salary = salary;
            NewTeacher.hiredate = hiredate;




            TeacherDataController MyController = new TeacherDataController();

            MyController.AddTeacher(NewTeacher);
            
            //returns back to list of teachers
            return RedirectToAction("list");
        }

        //GET: /Teacher/deleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //POST: /teacher/delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            MyController.DeleteTeacher(id);

            return RedirectToAction("list");
        }
        //GET: /teacher/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {

            TeacherDataController MyController = new TeacherDataController();
            teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //POST: /teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string teacherfname, string teacherlname, string employeenumber, decimal salary, DateTime hiredate) // ADD THE REST OF THE FEILDS
        {
            //recive info about the teacher
            Debug.WriteLine("teacher info");
            Debug.WriteLine(teacherfname);
            Debug.WriteLine(teacherlname);

            teacher updatedteacher = new teacher();
            //add updated teacher information
            updatedteacher.teacherfname = teacherfname;
            updatedteacher.teacherlname = teacherlname;
            updatedteacher.employeenumber = employeenumber;
            updatedteacher.salary = salary;
            updatedteacher.hiredate = hiredate;

            TeacherDataController MyController = new TeacherDataController();
            MyController.UpdateTeacher(id, updatedteacher);
            //return back to show page to confirm the updates made
            return RedirectToAction("Show/" + id);
        }







    }
}