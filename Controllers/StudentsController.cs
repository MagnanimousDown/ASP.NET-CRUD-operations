using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoAPI.Data;
using DemoAPI.Model;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private ApplicationDbContext _db;


        //Adding a logger
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(ApplicationDbContext context, ILogger<StudentsController> logger)
        {
            _db = context;
            _logger = logger;
        }

        [HttpGet]
        public List<StudentEntity> GetAllStudents()
        {
            _logger.LogInformation("Fetching All Student List");
            return _db.StudentRegister.ToList();
        }

        [HttpGet("GetStudentsById")]
        public ActionResult<StudentEntity> GetStudentDetails(Int32 Id)
        {

            if (Id == 0)
            {
                _logger.LogError("Student Id was not passed");
                return BadRequest();
            }

            var StudentDetails=_db.StudentRegister.FirstOrDefault(
                x => x.Id == Id);

            if (StudentDetails == null)
            {
                return NotFound();
            }
            return StudentDetails;
        }
        /* [HttpGet("GetStudentsName")]
         public string GetAllStudentsName()
         {
             return "Hello Student";
         }*/

        [HttpPost]
        public ActionResult<StudentEntity> AddStudent([FromBody] StudentEntity studentDetails)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.StudentRegister.Add(studentDetails);
            _db.SaveChanges();

            return Ok(studentDetails);
        }

        [HttpPost("UpdateStudentDetails")]
        public ActionResult<StudentEntity> UpdateStudent(Int32 Id,
            [FromBody] StudentEntity studentDetails)
        {

            if (studentDetails==null)
            {
                return BadRequest(studentDetails);
            }

            var StudentDetails = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);
            if (StudentDetails == null)
            {
                return NotFound(); 
            }
            
            StudentDetails.Name = studentDetails.Name;
            StudentDetails.Age = studentDetails.Age;
            StudentDetails.Standard = studentDetails.Standard;
            StudentDetails.EmailAddress = studentDetails.EmailAddress;

            _db.SaveChanges();

            return Ok(studentDetails);
        }


        [HttpPut("DeleteStudent")]
        public ActionResult<StudentEntity> Delete(Int32 Id)
        {

            var StudentDetails = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);
            if (StudentDetails == null)
            {
                return NotFound();
            }
            _db.Remove(StudentDetails);

            _db.SaveChanges();

            return NoContent();
        }
    }
}
