using CFAWebApi.Models;
using CFAWebApi.Repository.IRepo;
using CFAWebApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFAWebApi.Repository.Repo
{
    public class UserRepo : IUserRepo        
    {
        private UserContext _context;
        public UserRepo(UserContext context)
        {
            _context = context;
        }
        /// <summary>
        /// get list of all employees
        /// </summary>
        /// <returns></returns>
        public List<UserData> GetEmployeesList()
        {
            List<UserData> empList;
            try
            {
                empList = _context.Set<UserData>().ToList();
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
        public UserData GetEmployeeDetailsById(int empId)
        {
            UserData emp;
            try
            {
                emp = _context.Find<UserData>(empId);
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
        public ResponseModel SaveEmployee(UserData employeeModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                UserData _temp = GetEmployeeDetailsById(employeeModel.Id);
                if (_temp != null)
                {
                    _temp.Role = employeeModel.Role;
                    _temp.Name = employeeModel.Name;
                    _temp.EmailAddress = employeeModel.EmailAddress;
                    _temp.Password = employeeModel.Password;
                    _context.Update<UserData>(_temp);
                    model.Messsage = "User Update Successfully";
                }
                else
                {
                    _context.Add<UserData>(employeeModel);
                    model.Messsage = "User Inserted Successfully";
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
        public ResponseModel DeleteEmployee(int employeeId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                UserData _temp = GetEmployeeDetailsById(employeeId);
                if (_temp != null)
                {
                    _context.Remove<UserData>(_temp);
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

        public ResponseModel Login(UserData employeeModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                UserData _temp = GetEmployeeDetailsByEmail(employeeModel);
                if (_temp != null)
                {                    
                    model.Messsage = "Login Successfully";
                    model.IsSuccess = true;
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Invalid User";
                }               
                
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }

        public UserData GetEmployeeDetailsByEmail(UserData user)
        {
            UserData emp;
            try
            {
                var userlst = GetEmployeesList();
                emp = userlst.Find(u => u.EmailAddress == user.EmailAddress);
                    //Find<UserData>();
                if (emp.Password == user.Password)
                {                    
                    return emp;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return emp;
        }
    }
}
