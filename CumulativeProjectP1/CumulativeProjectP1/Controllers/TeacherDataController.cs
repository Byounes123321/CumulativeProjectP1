using CumulativeProjectP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Renci.SshNet.Security.Cryptography;
using Org.BouncyCastle.Asn1.Cmp;

namespace CumulativeProjectP1.Controllers
{
    public class TeacherDataController : ApiController
    {
        // the context class allows us to comunicate with the SQL DB
        private SchoolDbContext School = new SchoolDbContext();
        
        //This Controller Will access the authors table of our blog database.
        /// <summary>
        /// Returns a list of Teachers in the system. if a search= key is included we filter where the teacher infromation is like the key
        /// </summary>
        /// <param name="search_key">An optional search key for teacher name information</param>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <example>GET api/TeacherData/ListTeachers?search_key</example>
        /// <returns>
        /// ({teacherId,teacherId, teacherfname,teacherHireDate, teacherSalary},{teacherId,teacherId, teacherfname,teacherHireDate, teacherSalary})
        /// </returns>
        [HttpGet]
        [Route("api/teacher/list/{search_key}")]
        public IEnumerable<teacher> listTeachers(string search_key)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatatbase();

            //open connection between the server and the database
            Conn.Open();

            //sql query to be sent to the database
            string query = "select * from teachers where teacherfname like @key or teacherlname like @key or employeenumber like @key or hiredate like @key or salary like @key or teacherid like @key";

            //output the query to the debug console
            Debug.WriteLine("the query is " + query);

            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query will be the query defined above
            cmd.CommandText =query;

            //fill the parameters of the query to keep it safe from SQL injection
            cmd.Parameters.AddWithValue("@key","%"+ search_key +"%");
           
            cmd.Prepare();

            //gather resultset of the query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            List<teacher> teachers = new List<teacher> ();

            
            while (ResultSet.Read())
            {
                //access column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherfname = ResultSet["teacherfname"].ToString();
                string teacherlname = ResultSet["teacherlname"].ToString();
                string employeenumber = ResultSet["employeenumber"].ToString();
                DateTime hiredate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal salary = Convert.ToDecimal(ResultSet["salary"].ToString());
                
                

            
                teacher Newteacher = new teacher();

                Newteacher.teacherid = teacherId;
                Newteacher.teacherfname = teacherfname;
                Newteacher.teacherlname = teacherlname;
                Newteacher.employeenumber = employeenumber;
                Newteacher.hiredate = hiredate;
                Newteacher.salary = salary;
                teachers.Add(Newteacher);
            }

            //close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //display the list of teachers to the user
            return teachers;

        }

        /// <summary>
        /// returns a teachers information when provided their ID
        /// </summary>
        /// <param name="TeacherId"> the primary key of teachers in the database</param>
        /// <returns>
        /// {teacherfname,teacherHireDate, teacherSalary}
        /// </returns>
        [HttpGet]
        [Route("api/teacherdata/FindTeacher/{TeacherId}")]
        
        public teacher FindTeacher(int TeacherId)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatatbase();
            
            //Open the connection to the database
            Conn.Open();

            //SQL query to be sent to the database
            string query = "Select * from teachers where teacherId= @key";

            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            //clean up the input from the user to prevent SQL injection
            cmd.Parameters.AddWithValue("key",TeacherId);

            //gather resultset of the query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            teacher SelectedTeacher = new teacher();

            while (ResultSet.Read())
            {
                //access column information by the DB column name as an index
                SelectedTeacher.teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.teacherfname = ResultSet["teacherfname"].ToString();
                SelectedTeacher.teacherlname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.employeenumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.hiredate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.salary = Convert.ToDecimal(ResultSet["salary"].ToString());

            }
            //close database connection
            Conn.Close();
            //display the list of teachers to the user
            return SelectedTeacher;
        }

    }
}
