using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Employee_CRUD.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Employee_CRUD.Controllers
{
    public class EmployeeController : Controller
    {
        DbServicesContext db = new DbServicesContext();
        // GET: Employee
        public ActionResult Index() //=> View(db.tbl_emp.ToList());
        {
            if (Session["UserLogin"] != null)
            {
                return View(db.tbl_emp.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Employees emp)
        {
            db.tbl_emp.Add(emp);
           int a = db.SaveChanges();
            if(a>0)
            {
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.SubmitMsg = ("<JavaScript>alert('Something went wrong..')</JavaScript>");            
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
           var a= db.tbl_emp.Where(model => model.Id == id).FirstOrDefault();
             
            return View(a);
        }
         
        [HttpPost]
        public ActionResult Edit(Employees emp)
        {
            db.Entry(emp).State = EntityState.Modified;
            int a = db.SaveChanges();

            if (a > 0)
            {
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.EditMsg = ("<Script>alert('Something went wrong..')</Script>");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var a = db.tbl_emp.Where(model => model.Id == id).FirstOrDefault();

            return View(a);
        }

        [HttpPost]
        public ActionResult Delete(Employees emp)
        {
            db.Entry(emp).State = EntityState.Deleted;
            int a = db.SaveChanges();

            if (a > 0)
            {
                ModelState.Clear();
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.EditMsg = ("<Script>alert('Something went wrong..')</Script>");
            }

            return View();
        }


        public ActionResult LogIn()
        {
            if (Session["UserLogin"] != null)
            {
                return View("Welcome",db.tbl_emp.ToList());
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LogIn(Employees emp)
        {
            var row = db.tbl_emp.Where(model => model.Email == emp.Email && model.Password == emp.Password).FirstOrDefault();   
           
            if (row != null)
            {
                Session["UserLogin"] = "1";
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.LogInMsg = ("<Script>alert('please enter Valid Credentials')</Script>");
                ModelState.Clear();
            }

            return View();
        }

        public ActionResult Welcome()
        {
            if (Session["UserLogin"] != null)
            {
                return View(db.tbl_emp.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session["UserLogin"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult DashBoardount()
        {
            try
            {
                
                string[] DashBoardcount = new string[2];
                string connectionString = ConfigurationManager.ConnectionStrings["DbServicesContext"].ConnectionString; ;
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(Gender) as male, (select count(Gender) from Employees where Gender = 'female') as female from Employees  where Gender = 'male'", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);
                cmd1.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DashBoardcount[0] = "0";
                    DashBoardcount[1] = "1";

                }
                else
                {
                    DashBoardcount[0] = dt.Rows[0]["male"].ToString();
                    DashBoardcount[1] = dt.Rows[0]["female"].ToString();

                }
                return Json(new { DashBoardcount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }


}