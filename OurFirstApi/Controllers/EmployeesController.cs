using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.Models;

namespace OurFirstApi.Controllers
{
    //api/employees
    public class EmployeesController : ApiController
    {
        //api/employees
        public HttpResponseMessage Get()
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    var result = connection.Query<EmployeeListResult>("select * " +
                                                                      "from Employee");


                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
                }
            }
        }
        

        //api/employees/3000
        public HttpResponseMessage Get(int id)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    var result =
                        connection.Query<EmployeeListResult>("Select * From Employee where EmployeeId = @id",
                            new {id = id}).FirstOrDefault();

                    if (result == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,$"Employee with the Id {id} was not found");
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
        public HttpResponseMessage Post(EmployeeListResult employee)
        {
            using (var connection = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    connection.Execute("Insert into Employee(FirstName, LastName) " +
                                       "Values(@firstName, @lastName)",
                                       new { FirstName = employee.FirstName, LastName = employee.LastName });

                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }

        // PUT api/values/99
        public HttpResponseMessage Put(int id, EmployeeListResult employee)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    connection.Execute("update Employee " +
                                       "set LastName = @changedLastName " +
                                       "where EmployeeId = @selectedID",
                                       new { changedLastName = employee.LastName, selectedID = id });

                    return Request.CreateResponse(HttpStatusCode.Accepted);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }


        // DELETE api/values/99
        public HttpResponseMessage Delete(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    connection.Execute("delete from Employee where EmployeeId = @thisEmployee",
                                        new { thisEmployee = id });

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }
    }
}
