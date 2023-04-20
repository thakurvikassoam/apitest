using CFAWebApi.JWT.Services;
using CFAWebApi.MailSetting;
using CFAWebApi.MailSetting.Services;
using CFAWebApi.Models;
using CFAWebApi.Repository.IRepo;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeFirstApproachWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IUserRepo _employeeService;
        ITicketRepo _ticketRepository;
        private readonly CFAWebApi.MailSetting.Services.IMailService _mailService;
        private readonly IIdentityService _identityService;
        public HomeController(IUserRepo service, ITicketRepo ticketRepository, CFAWebApi.MailSetting.Services.IMailService mail,IIdentityService identityService)
        {
            _employeeService = service;
            _ticketRepository = ticketRepository;
            _mailService = mail;
            _identityService=identityService;

        }
        public enum Priority{
            Low,
            medium

            }
        /// <summary>
        /// get all employess
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeeService.GetEmployeesList();
                if (employees == null) return NotFound();
                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// get employee details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserById/Id")]
        public IActionResult GetEmployeesById(int id)
        {
            try
            {
                var employees = _employeeService.GetEmployeeDetailsById(id);
                if (employees == null) return NotFound();
                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// save employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveUser")]
        public IActionResult SaveEmployees(UserData employeeModel)
        {
            try
            {
                var model = _employeeService.SaveEmployee(employeeModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// delete employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteEmployee")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var model = _employeeService.DeleteEmployee(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// get all employess
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllTicket")]
        public IActionResult GetAllTickets()
        {
            try
            {
                var employees = _ticketRepository.GetTicketList();
                if (employees == null) return NotFound();
                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// get employee details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTicketById/Id")]
        public IActionResult GetTicketsById(int id)
        {
            try
            {
                var employees = _ticketRepository.GetTicketDetailsById(id);
                if (employees == null) return NotFound();
                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// save employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTicket")]
        public IActionResult SaveTickets(Tickets employeeModel)
        {
            try
            {
                if (employeeModel.Priority =="1")
                {
                    employeeModel.Priority = "Low";
                }                
                if (employeeModel.Priority =="2")
                {
                    employeeModel.Priority = "Medium";
                }                
                if (employeeModel.Priority =="3")
                {
                    employeeModel.Priority = "High";
                }
                MailRequest er = new MailRequest();
                er.ToEmail = employeeModel.CreatedBy;
                er.Subject = employeeModel.Title;
                er.Body = employeeModel.Description;
                SendMail(er);
                var model = _ticketRepository.SaveTicket(employeeModel);               
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                   // return Ok(new { dbPath });
                }
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// delete employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTicket/Id")]
        public IActionResult DeleteTicket(int id)
        {
            try
            {
                var model = _ticketRepository.DeleteTicket(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost()]
        [Route("SendMail")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //[HttpPost]
        //public async Task<IActionResult> Login(IFormFile file)
        //{
        //    // Perform file upload logic here, e.g. save the file to the server
        //}
        [HttpPost]
        [Route("Login")]
        public IActionResult login(UserData user)
        {
            try
            {
               // var model = _employeeService.Login(user);
                var result=_identityService.LoginAsync(user);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
