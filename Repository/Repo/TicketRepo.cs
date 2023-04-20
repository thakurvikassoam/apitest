
using CFAWebApi.Models;
using CFAWebApi.Repository.IRepo;
using CFAWebApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFAWebApi.Repository.Repo
{
    public class TicketRepo : ITicketRepo        
    {
        private UserContext _context;
        public TicketRepo(UserContext context)
        {
            _context = context;
        }
        /// <summary>
        /// get list of all employees
        /// </summary>
        /// <returns></returns>
        public List<Tickets> GetTicketList()
        {
            List<Tickets> empList;
            try
            {
                empList = _context.Set<Tickets>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return empList;
        }
        /// <summary>
        /// get employee details by employee id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public Tickets GetTicketDetailsById(int empId)
        {
            Tickets emp;
            try
            {
                emp = _context.Find<Tickets>(empId);
            }
            catch (Exception)
            {
                throw;
            }
            return emp;
        }
        /// <summary>
        ///  add edit employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        public ResponseModel SaveTicket(Tickets employeeModel)
        {
            Tickets _temp = GetTicketDetailsById(employeeModel.Id);
            ResponseModel model = new ResponseModel();
            try
            {
               // Tickets _temp = GetTicketDetailsById(employeeModel.Id);
                if (_temp != null)
                {
                    _temp.Title = employeeModel.Title;
                    _temp.Description = employeeModel.Description;
                    _temp.status = "Under-Process";
                    _temp.Updatedby= employeeModel.Updatedby;
                    _temp.Updatedate = DateTime.Now.ToString();
                    _temp.Reply = employeeModel.Reply;
                    _temp.ReplyBy = employeeModel.ReplyBy;
                    _context.Update<Tickets>(_temp);
                    model.Messsage = "Ticket Update Successfully";
                }
                else
                {

                    employeeModel.status = "Pending";
                    employeeModel.Createdate= DateTime.Now.ToString();
                    _context.Add<Tickets>(employeeModel);
                    model.Messsage = "Ticket Inserted Successfully";
                }
                _context.SaveChanges();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }

        /// <summary>
        /// delete employees
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ResponseModel DeleteTicket(int employeeId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Tickets _temp = GetTicketDetailsById(employeeId);
                if (_temp != null)
                {
                    _context.Remove<Tickets>(_temp);
                    _context.SaveChanges();
                    model.IsSuccess = true;
                    model.Messsage = "Employee Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Employee Not Found";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }
    }
}
