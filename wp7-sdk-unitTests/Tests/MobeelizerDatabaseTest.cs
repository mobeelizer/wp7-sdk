﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Data.Linq;
using wp7_sdk_unitTests.Models;
using System.Linq;

namespace wp7_sdk_unitTests.Tests
{
    [TestClass]
    public class MobeelizerDatabaseTest
    {
        ManualResetEvent loginEvent = new ManualResetEvent(false);

        [TestMethod]
        public void _Init()
        {
            Mobeelizer.OnLaunching();

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
                Assert.IsNotNull(db.GetModels<Employee>());
                Assert.IsInstanceOfType(db.GetModels<Employee>(), typeof(ITable<Employee>));

                Assert.IsInstanceOfType(new Department(), typeof(MobeelizerWp7Model));
                Assert.IsNotNull(db.GetModels<Department>());
                Assert.IsInstanceOfType(db.GetModels<Department>(), typeof(ITable<Department>));
            }
        }

        [TestMethod]
        public void Commit_Validation01()
        {
            String guid;
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = db.GetModels<Department>();
                Department department = new Department();
                department.name = "Dep1";
                department.internalNumber = 333;
                departments.InsertOnSubmit(department);
                db.Commit();
                guid = department.guid;
            }
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {    
                var employees = db.GetModels<Employee>();
                Employee employee = new Employee();
                employee.name = "NameNameNameNameNameNameNameNameNameNameName";
                employee.surname = "Surname";
                employee.position = "Position";
                employee.department = guid;
                employees.InsertOnSubmit(employee);
                String exceptionMessage = string.Empty;
                bool thrown = false;
                try
                {
                    db.Commit();
                }

                catch (InvalidOperationException e)
                {
                    thrown = true;
                    exceptionMessage = e.Message;
                }

                Assert.IsTrue(thrown);
                Assert.IsTrue(exceptionMessage.Contains("name"));
            }
        }


        [TestMethod]
        public void Commit_Validation02()
        {
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = db.GetModels<Department>();
                Department department = new Department();
                department.name = "Dep1";
                department.internalNumber = 333;
                departments.InsertOnSubmit(department);
                db.Commit();
            }

            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = db.GetModels<Employee>();
                Employee employee = new Employee();
                employee.name = "Name";
                employee.surname = "Surname";
                employee.position = "Position";
                employee.department = "wrong guid";
                employees.InsertOnSubmit(employee);
                String exceptionMessage = string.Empty;
                bool thrown = false;
                try
                {
                    db.Commit();
                }
                catch (InvalidOperationException e)
                {
                    thrown = true;
                    exceptionMessage = e.Message;
                }
                Assert.IsTrue(thrown);
                Assert.IsTrue(exceptionMessage.Contains("department"));
            }
        }

        [TestMethod]
        public void Commit_Delete()
        {
            String justAddedGuid = string.Empty;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModels<Department>();
                Department department = new Department()
                {
                    name = "department",
                    internalNumber = 13
                };
                departments.InsertOnSubmit(department);
                transaction.Commit();
                justAddedGuid = department.guid;
            }

            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModels<Department>();
                departments.DeleteOnSubmit((from d in departments where d.guid == justAddedGuid select d).Single());
                transaction.Commit();
            }

            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departments = transaction.GetModels<Department>();
                var query = from d in departments where d.guid == justAddedGuid select d;
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
                var departmentTable = db.GetModels<Department>();
                Department de = new Department();
                de.internalNumber = 1;
                de.name = "ddd";
                departmentTable.InsertOnSubmit(de);
                db.Commit();
                justAddEntityGuid = de.guid;
            }

            using (IMobeelizerTransaction transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = transaction.GetModels<Employee>();
                Employee employee = new Employee() { department = justAddEntityGuid, name = "name", position = "position", surname = "surname", salary = 13 };
                employees.InsertOnSubmit(employee);
                transaction.Commit();
            }

            using (IMobeelizerTransaction transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var employees = transaction.GetModels<Employee>();
                var departments = transaction.GetModels<Department>();

                var query = from e in employees join d in departments on e.department equals d.guid select new { eName = e.name, dName = d.name };
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