using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentDemo.helper
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Patiean = "Patiean";
        public static string Doctor = "Doctor";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value= Helper.Admin, Text=Helper.Admin},
                new SelectListItem{Value= Helper.Patiean, Text=Helper.Patiean},
                new SelectListItem{Value= Helper.Doctor, Text=Helper.Doctor}
            };
        }
    }
}
