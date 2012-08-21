using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Data.Linq;
using wp7_sdk_unitTests.Models;
using System.Linq;
using wp7_sdk_unitTests.Helpers.Mock;

namespace wp7_sdk_unitTests.Tests
{
    [TestClass]
    public class MobeelizerDatabaseTest
    {
        ManualResetEvent loginEvent = new ManualResetEvent(false);


        [TestMethod]
        public void SimpleTest()
        {
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = db.GetModelSet<Department>();
                Department department = new Department();
                department.Name = "Dep1";
                department.InternalNumber = 333;
                departments.InsertOnSubmit(department);
                Department department2 = new Department();
                department.Name = "Dep2";
                department.InternalNumber = 333;
                departments.InsertOnSubmit(department2);
                Department department3 = new Department();
                department.Name = "Dep3";
                department.InternalNumber = 333;
                departments.InsertOnSubmit(department3);
                db.SubmitChanges();
            }

            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var query = from d in transaction.GetModelSet<Department>() select d;
                foreach (Department dep in query)
                {

                }
            }
        }

        [TestMethod]
        public void _Init()
        {
            Mobeelizer.OnLaunching();
            UTWebRequest.SyncData = "firstSync.zip";
            Mobeelizer.Login("user", "password", (s) =>
            {
                loginEvent.Set();
            });
            loginEvent.WaitOne();
        }

        [TestMethod]
        public void GetModels()
        {
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                Assert.IsInstanceOfType(new Employee(), typeof(MobeelizerWp7Model));
                Assert.IsNotNull(db.GetModelSet<Employee>());
                Assert.IsInstanceOfType(db.GetModelSet<Employee>(), typeof(ITable<Employee>));

                Assert.IsInstanceOfType(new Department(), typeof(MobeelizerWp7Model));
                Assert.IsNotNull(db.GetModelSet<Department>());
                Assert.IsInstanceOfType(db.GetModelSet<Department>(), typeof(ITable<Department>));
            }
        }

        [TestMethod]
        public void Commit_Validation01()
        {
            String guid;
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = db.GetModelSet<Department>();
                Department department = new Department();
                department.Name = "Dep1";
                department.InternalNumber = 333;
                departments.InsertOnSubmit(department);
                db.SubmitChanges();
                guid = department.Guid;
            }
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = db.GetModelSet<Employee>();
                Employee employee = new Employee();
                employee.Name = "NameNameNameNameNameNameNameNameNameNameName";
                employee.Surname = "Surname";
                employee.Position = "Position";
                employee.Department = guid;
                employees.InsertOnSubmit(employee);
                String exceptionMessage = string.Empty;
                bool thrown = false;
                try
                {
                    db.SubmitChanges();
                }

                catch (ArgumentException e)
                {
                    thrown = true;
                    exceptionMessage = e.Message;
                }

                Assert.IsTrue(thrown);
                Assert.IsTrue(exceptionMessage.Contains("Name"));
            }
        }


        [TestMethod]
        public void Commit_Validation02()
        {
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = db.GetModelSet<Department>();
                Department department = new Department();
                department.Name = "Dep1";
                department.InternalNumber = 333;
                departments.InsertOnSubmit(department);
                db.SubmitChanges();
            }

            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = db.GetModelSet<Employee>();
                Employee employee = new Employee();
                employee.Name = "Name";
                employee.Surname = "Surname";
                employee.Position = "Position";
                employee.Department = "wrong guid";
                employees.InsertOnSubmit(employee);
                String exceptionMessage = string.Empty;
                bool thrown = false;
                try
                {
                    db.SubmitChanges();
                }
                catch (ArgumentException e)
                {
                    thrown = true;
                    exceptionMessage = e.Message;
                }
                Assert.IsTrue(thrown);
                Assert.IsTrue(exceptionMessage.Contains("Department"));
            }
        }

        [TestMethod]
        public void Commit_Delete()
        {
            String justAddedGuid = string.Empty;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModelSet<Department>();
                Department department = new Department()
                {
                    Name = "department",
                    InternalNumber = 13
                };
                departments.InsertOnSubmit(department);
                transaction.SubmitChanges();
                justAddedGuid = department.Guid;
            }

            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModelSet<Department>();
                departments.DeleteOnSubmit((from d in departments where d.Guid == justAddedGuid select d).Single());
                transaction.SubmitChanges();
            }

            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModelSet<Department>();
                var query = from d in departments where d.Guid == justAddedGuid select d;
                
                bool thrown = false;
                try
                {
                    Department department = query.Single();
                }
                catch
                {
                    thrown = true;
                }
                Assert.IsTrue(thrown);
            }
        }


        [TestMethod]
        public void Querys()
        {
            String justAddEntityGuid = string.Empty;
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departmentTable = db.GetModelSet<Department>();
                Department de = new Department();
                de.InternalNumber = 1;
                de.Name = "ddd";
                departmentTable.InsertOnSubmit(de);
                db.SubmitChanges();
                justAddEntityGuid = de.Guid;
            }

            using (IMobeelizerTransaction transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = transaction.GetModelSet<Employee>();
                Employee employee = new Employee() { Department = justAddEntityGuid, Name = "name", Position = "position", Surname = "surname", Salary = 13 };
                employees.InsertOnSubmit(employee);
                transaction.SubmitChanges();
            }

            using (IMobeelizerTransaction transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = transaction.GetModelSet<Employee>();
                var departments = transaction.GetModelSet<Department>();

                var query = from e in employees join d in departments on e.Department equals d.Guid select new { eName = e.Name, dName = d.Name };
                int found = 0;
                foreach (var result in query)
                {
                    ++found;
                }
                Assert.IsTrue(found > 0);
            }
        }
    }
}
