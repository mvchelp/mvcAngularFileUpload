using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJSDemo5.Models;
using System.Web;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
namespace AngularJSDemo5.Controllers
{
    public class StudentsController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET: api/Students
        [HttpGet]
        public HttpResponseMessage GetStudents()
        {
            var students = db.Students;

            if (students != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, students);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        // GET: api/Students/5
        [HttpGet]
        public HttpResponseMessage GetStudentById(int id)
        {
            var student = db.Students.Where(i => i.StudentId == id).Single();
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        // PUT: api/Students/5
        [HttpPut]
        public HttpResponseMessage PutStudent()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var model = HttpContext.Current.Request.Params;
            if (model != null)
            {
                int id = Convert.ToInt32(model["StudentId"]);

                var studentToUpdate = db.Students
                    .Where(i => i.StudentId == id).Single();

                if (studentToUpdate != null)
                {
                    var files = HttpContext.Current.Request.Files;
                    if (files != null && files.Count > 0)
                    {
                        var file = files[0];
                        string actualfilename = file.FileName;
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + fileName));
                        File.Delete(HttpContext.Current.Server.MapPath("~" + studentToUpdate.ImagePath));
                        studentToUpdate.ImagePath = "/Uploads/" + fileName;
                    }

                    string actualDate = model["BirthDate"];
                    string date = actualDate.Substring(4, 11);
                    string s = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                    studentToUpdate.StudentId = id;
                    studentToUpdate.FirstName = model["FirstName"];
                    studentToUpdate.LastName = model["LastName"];
                    studentToUpdate.Gender = model["Gender"];
                    studentToUpdate.BirthDate = Convert.ToDateTime(s);
                    studentToUpdate.EnrollmentNo = model["EnrollmentNo"];
                    studentToUpdate.EmailId = model["EmailId"];

                    db.Entry(studentToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "success!");
                }
                else
                {
                    HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }
            }
            else
            {

                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }

        }

        // POST: api/Students
        [HttpPost]
        public HttpResponseMessage PostStudent()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var model = HttpContext.Current.Request.Params;
            if (model != null)
            {
                var file = HttpContext.Current.Request.Files[0];

                string actualfilename = file.FileName;
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string subPath = "~/Uploads/";

                bool IsExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!IsExists)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                file.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + fileName));

                string actualDate = model["BirthDate"];
                string date = actualDate.Substring(4, 11);
                string s = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                Student student = new Student();
                student.FirstName = model["FirstName"];
                student.LastName = model["LastName"];
                student.Gender = model["Gender"];
                student.BirthDate = Convert.ToDateTime(s);
                student.EnrollmentNo = model["EnrollmentNo"];
                student.EmailId = model["EmailId"];
                student.ImagePath = "/Uploads/" + fileName;

                db.Students.Add(student);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "success!");

            }else{

                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }
            
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}