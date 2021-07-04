
using System.Windows.Controls;
namespace ONA_Clinics
{
   public static class User
    {
        public static string Name { get; set; }
        public static string Provider_Code { get; set; }
        public static string Provider_Type { get; set; }
        public static string Provider_CC { get; set; }
        public static string Degree_Code { get; set; }
        public static string Degree_Name { get; set; }
        public static string ID { get; set; }
        public static string Name_Show { get; set; }
        public static string Provider_Arabic_Name { get; set; }
        public static string Provider_English_Name { get; set; }
        public static string AUTHORITY_CODE { get; set; }
        public static string AUTHORITY_NAME { get; set; }
        public static string DENTAL_FRM { get; set; }
        public static string HOSPITAL_FRM { get; set; }
        public static string PHYSICAL_THERAPY_FRM { get; set; }
        public static string DOCTOR_FRM { get; set; }
        public static string ADMIN_FRM { get; set; }
        public static string DOCTOR_SPE { get; set; }
        public static void CleanAll(this Panel s)
        {

            foreach (System.Windows.UIElement child in s.Children)
            {
                GroupBox gb = child as GroupBox;
                if (gb != null)
                    ((Grid)gb.Content).CleanAll();

                Grid dg = child as Grid;
                if (dg != null)
                    dg.CleanAll();
                CheckBox chb = child as CheckBox;
                if (chb != null)
                    chb.IsChecked = false;
                TextBox txt = child as TextBox;
                if (txt != null)
                    txt.Text = "";
                ComboBox cbx = child as ComboBox;
                if (cbx != null)
                    cbx.Text = "";
                DatePicker dp = child as DatePicker;
                if (dp != null)
                    dp.Text = "";
                StackPanel sp = child as StackPanel;
                if (sp != null)
                    ((StackPanel)sp).CleanAll();

                DataGrid DG = child as DataGrid;
                if (DG != null)
                    DG.ItemsSource = null;
            }

        }

    }
}
