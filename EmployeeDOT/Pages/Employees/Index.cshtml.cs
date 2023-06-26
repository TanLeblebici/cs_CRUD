using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeDOT.Pages.Employees
{
    public class IndexModel : PageModel
    {   
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>(); // list of EmployeeInfo
        // executed when we access to the page,
        // we need to access to the database to read data from employees table
        public void OnGet() 
        {

            try
            {
                // Connection to the database
                String connectionString = "Server=localhost;Database=Employees;Trusted_Connection=true;TrustServerCertificate=true;";

                using (SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open(); //opening connection
                    String sql = "SELECT * FROM employees"; // reading all the rows from employees table
                    using (SqlCommand command = new SqlCommand(sql, connection)) // allows us to execute sql query
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0); //converting int to str w/ ""
                                employeeInfo.name = reader.GetString(1);
                                employeeInfo.email = reader.GetString(2);
                                employeeInfo.phone = reader.GetString(3);
                                employeeInfo.department = reader.GetString(4);
                                employeeInfo.created_at = reader.GetDateTime(5).ToString();

                                listEmployees.Add(employeeInfo);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
    }

    public class EmployeeInfo
    {
        public String id = "";
        public String name = "";
        public String email = "";
        public String phone = "";
        public String department = "";
        public String created_at = "";
    }
}
