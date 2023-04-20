using CFAWebApi.Models;
using CFAWebApi.ViewModels;
using System.Collections.Generic;

namespace CFAWebApi.Repository.IRepo
{
    public interface IUserRepo
    {
        /// <summary>
        /// get list of all employees
        /// </summary>
        /// <returns></returns>
        List<UserData> GetEmployeesList();

        /// <summary>
        /// get employee details by employee id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        UserData GetEmployeeDetailsById(int empId);

        /// <summary>
        ///  add edit employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        ResponseModel SaveEmployee(UserData employeeModel);

        


        /// <summary>
        /// delete employees
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ResponseModel DeleteEmployee(int employeeId);
        ResponseModel Login(UserData employeeModel);
    }

   

}
