using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONA_Clinics
{
  public static  class Patient
    {
        /// <summary>
        /// اسم المريض انكليزي
        /// </summary>
        public static string EName { get; set; }        
        /// <summary>
        /// اسم المريض عربي
        /// </summary>
        public static string AName { get; set; }
        /// <summary>
        /// رقم كارت المريض
        /// </summary>
        public static string Card_NO { get; set; }
        /// <summary>
        /// رقم شركة المريض
        /// </summary>
        public static string Comp_id { get; set; }
        /// <summary>
        /// رقم العقد لشركة المريض
        /// </summary>
        public static string Contract_NO { get; set; }
        /// <summary>
        /// كود فئة المريض
        /// </summary>
        public static string Class_Code { get; set; }
        /// <summary>
        /// اسم فئة المريض
        /// </summary>
        public static string Class_Name { get; set; }
        /// <summary>
        /// الحد الاقصى لكل الخدمات
        /// </summary>
        public static string Max_Amount { get; set; }
        /// <summary>
        /// استهلاكات المريض فى ال
        /// IRS  
        /// </summary>
        public static string Consumption_IRS { get; set; }
        /// <summary>
        /// استهلاكات المريض فى ال
        /// Online  
        /// </summary>
        public static string Consumption_Online { get; set; }
        /// <summary>
        /// مجموع استهلاكات المريض
        /// </summary>
        public static string Consumption_Total { get; set; }
        /// <summary>
        /// مجموع استهلاكات المريض
        /// </summary>
        public static string Available { get; set; }
                
    }
}
