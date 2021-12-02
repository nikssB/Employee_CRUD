using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using System.Data.Common;
 

namespace Employee_CRUD.Models
{
    public class DbServicesContext:DbContext
    {
        public DbSet<Employees> tbl_emp { get; set; }

    }
}