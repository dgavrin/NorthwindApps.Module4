using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Northwind.DataAccess.Employees;

namespace Northwind.DataAccess.SqlServer.Employees
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject
    {
        private SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public EmployeeSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public bool DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            const string commandText =
                @"DELETE FROM dbo.Employees WHERE EmployeeID = @employeeId
                  SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string employeeIdParameter = @"employeeId";
                command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
                command.Parameters[employeeIdParameter].Value = employeeId;

                this.OpenSqlConnectionIfItClose();
                var result = command.ExecuteScalar();
                return ((int)result) > 0;
            }
        }

        /// <inheritdoc/>
        public EmployeeTransferObject FindEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            const string commandText =
                @"SELECT e.EmployeeId, e.LastName, e.FirstName, e.Title, e.TitleOfCountesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath, e.Salary FROM dbo.Employees as e
                  WHERE e.EmployeeID = @employeeId";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                const string employeeIdParameter = "@employeeId";
                command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
                command.Parameters[employeeIdParameter].Value = employeeId;

                this.OpenSqlConnectionIfItClose();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new ArgumentException($"Employee with id {employeeId} not found.", nameof(employeeId));
                    }

                    return CreateEmployee(reader);
                }
            }
        }

        /// <inheritdoc/>
        public int InsertEmployee(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            const string commandText =
                @"INSERT INTO dbo.Employees (LastName, FirstName, Title, TitleOfCountesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath, Salary) OUTPUT Inserted.EmployeeID
                VALUES (@lastName, @firstName, @title, @titleOfCountesy, @birthDate, @hireDate, @address, @city, @region, @postalCode, @country, @homePhone, @extension, @photo, @notes, @reportsTo, @photoPath, @salary)";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                AddSqlParameters(employee, command);

                this.OpenSqlConnectionIfItClose();
                var id = command.ExecuteScalar();
                return (int)id;
            }
        }

        /// <inheritdoc/>
        public IList<EmployeeTransferObject> SelectEmployees(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(limit));
            }

            const string commandTemplate =
                @"SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCountesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath, e.Salary FROM dbo.Employees as e
                  ORDER BY e.EmployeeID
                  OFFSET {0} ROWS
                  FETCH FIRST {1} ROWS ONLY";

            string commandText = string.Format(CultureInfo.CurrentCulture, commandTemplate, offset, limit);
            this.OpenSqlConnectionIfItClose();
            return this.ExecuteReader(commandText);
        }

        /// <inheritdoc/>
        public bool UpdateEmployee(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            const string commandText =
                @"UPDATE dbo.Employees SET LastName = @lastName, FirstName = @firstName, Title = @title, TitleOfCountesy = @titleOfCountesy, BirthDate = @birthDate, HireDate = @hireDate, Address = @address, City = @city, Region = @region, PostalCode = @postalCode, Country = @country, HomePhone = @homePhone, Extension = @extension, Photo = @photo, Notes = @notes, ReportsTo = @reportsTo, PhotoPath = @photoPath, Salary = @salary
                  WHERE EmployeeID = @employeeId
                  SELECT @@ROWCOUNT";

            using (var command = new SqlCommand(commandText, this.connection))
            {
                AddSqlParameters(employee, command);

                const string employeeId = "@employeeId";
                command.Parameters.Add(employeeId, SqlDbType.Int);
                command.Parameters[employeeId].Value = employee.Id;

                this.OpenSqlConnectionIfItClose();
                var result = command.ExecuteScalar();
                return ((int)result) > 0;
            }
        }

        private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            var id = (int)reader["EmployeeID"];
            var lastName = (string)reader["LastName"];
            var firstName = (string)reader["FirstName"];
            var title = (string)reader["Title"];
            var titleOfCountesy = (string)reader["TitleOfCountesy"];
            var birthDate = (DateTime)reader["BirthDate"];
            var hireDate = (DateTime)reader["HireDate"];
            var address = (string)reader["Address"];
            var city = (string)reader["City"];
            var region = (string)reader["Region"];
            var postalCode = (string)reader["PostalCode"];
            var country = (string)reader["Country"];
            var homePhone = (string)reader["HomePhone"];
            var extension = (string)reader["Extension"];

            const string PhotoColumnName = "Photo";
            byte[] photo = null;
            if (reader[PhotoColumnName] != DBNull.Value)
            {
                photo = (byte[])reader[PhotoColumnName];
            }

            var notes = (string)reader["Notes"];
            var reportsTo = (int)reader["ReportsTo"];
            var photoPath = (string)reader["PhotoPath"];
            var salary = (double)reader["Salary"];

            return new EmployeeTransferObject
            {
                Id = id,
                LastName = lastName,
                FirstName = firstName,
                Title = title,
                TitleOfCountesy = titleOfCountesy,
                BirthDate = birthDate,
                HireDate = hireDate,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country,
                HomePhone = homePhone,
                Extension = extension,
                Photo = photo,
                Notes = notes,
                ReportsTo = reportsTo,
                PhotoPath = photoPath,
                Salary = salary,
            };
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string employeeLastNameParameter = "@lastName";
            command.Parameters.Add(employeeLastNameParameter, SqlDbType.NVarChar, 20);
            command.Parameters[employeeLastNameParameter].IsNullable = true;

            if (employee.LastName != null)
            {
                command.Parameters[employeeLastNameParameter].Value = employee.LastName;
            }
            else
            {
                command.Parameters[employeeLastNameParameter].Value = DBNull.Value;
            }

            const string employeeFirstNameParameter = "@firstName";
            command.Parameters.Add(employeeFirstNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[employeeFirstNameParameter].IsNullable = true;

            if (employee.FirstName != null)
            {
                command.Parameters[employeeFirstNameParameter].Value = employee.FirstName;
            }
            else
            {
                command.Parameters[employeeFirstNameParameter].Value = DBNull.Value;
            }

            const string employeeTitleParameter = "@title";
            command.Parameters.Add(employeeTitleParameter, SqlDbType.NVarChar, 30);
            command.Parameters[employeeTitleParameter].IsNullable = true;

            if (employee.Title != null)
            {
                command.Parameters[employeeTitleParameter].Value = employee.Title;
            }
            else
            {
                command.Parameters[employeeTitleParameter].Value = DBNull.Value;
            }

            const string employeeTitleOfCountesyParameter = "@titleOfCountesy";
            command.Parameters.Add(employeeTitleOfCountesyParameter, SqlDbType.NVarChar, 25);
            command.Parameters[employeeTitleOfCountesyParameter].IsNullable = true;

            if (employee.TitleOfCountesy != null)
            {
                command.Parameters[employeeTitleOfCountesyParameter].Value = employee.TitleOfCountesy;
            }
            else
            {
                command.Parameters[employeeTitleOfCountesyParameter].Value = DBNull.Value;
            }

            const string employeeBirthDateParameter = "@birthDate";
            command.Parameters.Add(employeeBirthDateParameter, SqlDbType.Date);
            command.Parameters[employeeBirthDateParameter].Value = employee.BirthDate;

            const string employeeHireDateParameter = "@hireDate";
            command.Parameters.Add(employeeHireDateParameter, SqlDbType.Date);
            command.Parameters[employeeHireDateParameter].Value = employee.HireDate;

            const string employeeAddressParameter = "@address";
            command.Parameters.Add(employeeAddressParameter, SqlDbType.NVarChar, 60);
            command.Parameters[employeeAddressParameter].Value = employee.Address;

            const string employeeCityParameter = "@city";
            command.Parameters.Add(employeeCityParameter, SqlDbType.NVarChar, 15);
            command.Parameters[employeeCityParameter].Value = employee.City;

            const string employeeRegionParameter = "@region";
            command.Parameters.Add(employeeRegionParameter, SqlDbType.NVarChar, 15);
            command.Parameters[employeeRegionParameter].Value = employee.Region;

            const string employeePostalCodeParameter = "@postalCode";
            command.Parameters.Add(employeePostalCodeParameter, SqlDbType.NVarChar, 10);
            command.Parameters[employeePostalCodeParameter].Value = employee.PostalCode;

            const string employeeCountryParameter = "@country";
            command.Parameters.Add(employeeCountryParameter, SqlDbType.NVarChar, 15);
            command.Parameters[employeeCountryParameter].Value = employee.Country;

            const string employeeHomePhoneParameter = "@homePhone";
            command.Parameters.Add(employeeHomePhoneParameter, SqlDbType.NVarChar, 24);
            command.Parameters[employeeHomePhoneParameter].Value = employee.HomePhone;

            const string employeeExtensionParameter = "@extension";
            command.Parameters.Add(employeeExtensionParameter, SqlDbType.NVarChar, 4);
            command.Parameters[employeeExtensionParameter].Value = employee.Extension;

            const string employeePhotoParameter = "@photo";
            command.Parameters.Add(employeePhotoParameter, SqlDbType.Image);
            command.Parameters[employeePhotoParameter].IsNullable = true;

            if (employee.Photo != null)
            {
                command.Parameters[employeePhotoParameter].Value = employee.Photo;
            }
            else
            {
                command.Parameters[employeePhotoParameter].Value = DBNull.Value;
            }

            const string employeeNotesParameter = "@notes";
            command.Parameters.Add(employeeNotesParameter, SqlDbType.Text);
            command.Parameters[employeeNotesParameter].Value = employee.Notes;

            const string employeeReportsToParameter = "@reportsTo";
            command.Parameters.Add(employeeReportsToParameter, SqlDbType.Int);
            command.Parameters[employeeReportsToParameter].Value = employee.ReportsTo;

            const string employeePhotoPathParameter = "@photoPath";
            command.Parameters.Add(employeePhotoPathParameter, SqlDbType.NVarChar, 255);
            command.Parameters[employeePhotoPathParameter].Value = employee.PhotoPath;

            const string employeeSalaryParameter = "@salary";
            command.Parameters.Add(employeeSalaryParameter, SqlDbType.Float);
            command.Parameters[employeeSalaryParameter].Value = employee.Salary;
        }

        private IList<EmployeeTransferObject> ExecuteReader(string commandText)
        {
            var productCategories = new List<EmployeeTransferObject>();

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using (var command = new SqlCommand(commandText, this.connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    productCategories.Add(CreateEmployee(reader));
                }
            }

            return productCategories;
        }

        private void OpenSqlConnectionIfItClose()
        {
            if (this.connection is not null && this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
        }
    }
}
