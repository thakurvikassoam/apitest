using CFAWebApi.Models;
using CFAWebApi.ViewModels;
using System.Collections.Generic;

namespace CFAWebApi.Repository.IRepo
{
    public interface ITicketRepo
    {
        /// <summary>
        /// get list of all employees
        /// </summary>
        /// <returns></returns>
        List<Tickets> GetTicketList();

        /// <summary>
        /// get employee details by employee id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Tickets GetTicketDetailsById(int empId);

        /// <summary>
        ///  add edit employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        ResponseModel SaveTicket(Tickets employeeModel);


        /// <summary>
        /// delete employees
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ResponseModel DeleteTicket(int employeeId);
    }
}
