
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.DataAccess;
using OurFirstApi.Models;


namespace OurFirstApi.Controllers
{
    //api/employees
    [RoutePrefix("api/employee")]
    public class EmployeesController : ApiController
    {
        //api/employees (defined above in RoutePrefix)
        public HttpResponseMessage Get()
        {
            try
            {
                var employeeData = new EmployeeDataAccess();
                var employees = employeeData.GetAll();

                return Request.CreateResponse(HttpStatusCode.OK, employees);
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
            }
        // The commented code has been moved to EmployeeDataAccess.cs / no Console.WriteLines / only interacting w database
        //    using (var connection =
        //        new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
        //  {
        //       try
        //       {
        //           connection.Open();

        //            var result = connection.Query<EmployeeListResult>("select * " +
        //                                                              "from Employee");


        //           return Request.CreateResponse(HttpStatusCode.OK, result);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            Console.WriteLine(ex.StackTrace);
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
        //        }
        //    }
        }


        //api/employees/3000
        //[HttpGet, Route("api/employee/{id}")]
        // identifying RoutePrefix above ::
        [HttpGet, Route("{id}")] // RoutePrefix "api/employee" defined above
        public HttpResponseMessage Get(int id)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    var employeeData = new EmployeeDataAccess();
                    var result = employeeData.Get(id);

                    if (result == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't find that employee");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }


        //api/employees/3000
        //[HttpGet, Route("api/employee/name/{firstname}")]
        [HttpGet, Route("name/{firstname}")]
        public HttpResponseMessage Get(string firstName)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    // The commented code moved to EmployeeDataAccess.cs
                    //connection.Open();

                    //var result =
                    //    connection.Query<EmployeeListResult>("Select * From Employee where FirstName = @firstname",
                    //        new { firstName }).FirstOrDefault();]

                    var employeeData = new EmployeeDataAccess();
                    var result = employeeData.Get(firstName);

                    if (result == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with the Name {firstName} was not found");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }


        // POST //api/employees/employee
        [HttpPost, Route("add")]
        public HttpResponseMessage Post(EmployeeListResult employee)
        {
            using (var connection = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    // The commented code moved to EmployeeDataAccess.cs
                    //connection.Open();

                    //connection.Execute("Insert into Employee(FirstName, LastName) " +
                    //                   "Values(@firstName, @lastName)",
                    //                  new { FirstName = employee.FirstName, LastName = employee.LastName });

                    var employeeData = new EmployeeDataAccess();
                    var rowsAdded = employeeData.AddEmployee(employee);

                    return Request.CreateResponse(HttpStatusCode.Created, $"{rowsAdded} rows added");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }

        // PUT api/employee (RoutePrefix)
        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage Put(int id, EmployeeListResult employee)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    // The commented code moved to EmployeeDataAccess.cs
                    //connection.Open();

                    //connection.Execute("update Employee " +
                    //                   "set LastName = @changedLastName " +
                    //                   "where EmployeeId = @selectedID",
                    //                   new { changedLastName = employee.LastName, selectedID = id });

                    var employeeData = new EmployeeDataAccess();
                    var affectedRows = employeeData.Update(id, employee);

                    if (affectedRows == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, 
                                        $"Employee with {id} not found");
                    }
                

                    return Request.CreateResponse(HttpStatusCode.OK, $"{affectedRows} rows updated");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }


        // DELETE api/values/99
        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    //connection.Open();

                    //connection.Execute("delete from Employee where EmployeeId = @thisEmployee",
                    //                    new { thisEmployee = id });

                    var employeeData = new EmployeeDataAccess();
                    var rowsDeleted = employeeData.Delete(id);

                    return Request.CreateResponse(HttpStatusCode.OK, $"{rowsDeleted} rows deleted");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }
    }
}
