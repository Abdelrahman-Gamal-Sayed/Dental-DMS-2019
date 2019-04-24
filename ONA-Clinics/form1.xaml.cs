using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Globalization;
using System.Threading;

namespace ONA_Clinics
{
    /// <summary>
    /// Interaction logic for form1.xaml
    /// </summary>
    public partial class form1 : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();

        public form1()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }
        
        private void LogOutBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenueBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenMenueBTN.Visibility = Visibility.Collapsed;
            CloseMenueBTN.Visibility = Visibility.Visible;
        }

        private void CloseMenueBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenMenueBTN.Visibility = Visibility.Visible;
            CloseMenueBTN.Visibility = Visibility.Collapsed;

        }

     

        private void PackIcon_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void PackIcon_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

   

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
         
                this.WindowState = WindowState.Maximized;
            btnMax.Visibility = Visibility.Collapsed;
            btnRestore.Visibility = Visibility.Visible;

        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            btnMax.Visibility = Visibility.Visible;
            btnRestore.Visibility = Visibility.Collapsed;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void txtCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataTable dtinformation = db.RunReader("select  to_char(BIRTH_DATE,'DD-MM-YYYY'),C_COMP_ID,CLASS_CODE,NVL(to_char(SPECIFIC_DATE,'DD-MM-YYYY'),to_char(INS_START_DATE,'DD-MM-YYYY')),to_char(INS_END_DATE,'DD-MM-YYYY'),TERMINATE_FLAG ,EMP_ANAME_ST ,EMP_ANAME_SC,EMP_ANAME_TH ,EMP_ENAME_ST ,EMP_ENAME_SC,EMP_ENAME_TH, to_char(INS_START_DATE,'DD-MM-YYYY'), to_char(TERMINATE_DATE,'DD-MM-YYYY') from COMP_EMPLOYEESS where CARD_ID='" + txtCard.Text + "' order by ins_start_date DESC");

                if (dtinformation.Rows.Count > 0 && Convert.ToDateTime(dtinformation.Rows[0][4]).Date < DateTime.Now.Date)
                    MessageBox.Show("خارج التغطية التامينية");
                else if (dtinformation.Rows.Count > 0 && dtinformation.Rows[0][5].ToString() == "Y" && Convert.ToDateTime(dtinformation.Rows[0][13]).Date < DateTime.Now.Date)
                {
                    MessageBox.Show("أنتهى التعاقد مع هذا الموظف بتاريخ " + dtinformation.Rows[0][13].ToString());
                }
                else if (dtinformation.Rows.Count > 0)
                {
                    string comp = "", cont = "", cls = "", maxamt = "", conirs = "", conon = "", cononothr = "", cononmed = "", conuse = "", avilabl = "";
                    //comp = txtCard.Text.Substring(0, txtCard.Text.IndexOf('-'));

                    txtBirthDate.Text = dtinformation.Rows[0][0].ToString();
                    Patient.Comp_id = dtinformation.Rows[0][1].ToString();
                    txtCompName.Text = db.RunReader(@"select C_ENAME from V_COMPANIES  WHERE  C_COMP_ID  = '" + comp + "'").Rows[0][0].ToString();
                    txtAge.Text = (DateTime.Now.Year - Convert.ToDateTime(dtinformation.Rows[0][0]).Year).ToString();
                    Patient.Class_Code = dtinformation.Rows[0][2].ToString();
                    txtContractStartDate.Text = dtinformation.Rows[0][3].ToString();
                    txtContractEndtDate.Text = dtinformation.Rows[0][4].ToString();
                    Patient.AName = dtinformation.Rows[0][6].ToString() + " " + dtinformation.Rows[0][7].ToString() + " " + dtinformation.Rows[0][8].ToString();
                    Patient.EName = dtinformation.Rows[0][9].ToString() + " " + dtinformation.Rows[0][10].ToString() + " " + dtinformation.Rows[0][11].ToString();


                    Patient.Contract_NO = db.RunReader("select max(contract_no) from COMP_EMPLOYEESS where C_COMP_ID='" + comp + "'").Rows[0][0].ToString();
                    Patient.Max_Amount = db.RunReader("select MAX_AMOUNT from V_P_COMP_CONTRACT_CLASS where C_COMP_ID='" + comp + "' and CONTRACT_NO='" + cont + "' and CLASS_CODE='" + cls + "'").Rows[0][0].ToString();

                    DataTable dtirs = db.RunReader("select TOT_NET_AMT from IRS_CONSUMPTION where CARD_NO = '" + comp + "' and CONTRACT_NO = (select max(CONTRACT_NO) FROM IRS_CONSUMPTION where CARD_NO = '" + txtCard.Text + "') AND to_date(CON_END_DATE) > to_date(sysdate) ");
                    // System.Data.DataTable dton = db.RunReader("select TOT_AMOUNT from ONLINE_CONS_02 where CARD_NO = '" + cbxindtyp1_Copy1.Text + "'").Result;

                    

                    System.Data.DataTable dton1 = tdb.getonlinevalue(txtCard.Text, Convert.ToDateTime(dtinformation.Rows[0][12].ToString()), Convert.ToDateTime(dtinformation.Rows[0][4].ToString()), 1);
                    System.Data.DataTable dton2 = tdb.getonlinevalue(txtCard.Text, Convert.ToDateTime(dtinformation.Rows[0][12].ToString()), Convert.ToDateTime(dtinformation.Rows[0][4].ToString()), 2);

                    if (dtirs.Rows.Count == 0 || dtirs.Rows[0][0].ToString() == string.Empty)
                        Patient.Consumption_IRS = "0";
                    else
                        Patient.Consumption_IRS = dtirs.Rows[0][0].ToString();
                                        
                    try
                    {
                        cononmed = dton1.Rows[0][0].ToString();
                        cononothr = dton2.Rows[0][0].ToString();

                        Patient.Consumption_Online = (double.Parse(dton1.Rows[0][0].ToString()) + double.Parse(dton2.Rows[0][0].ToString())).ToString();

                        Patient.Consumption_Total = (Convert.ToDouble(Patient.Consumption_IRS) + Convert.ToDouble(Patient.Consumption_Online)).ToString();

                        Patient.Available = (Convert.ToDouble(Patient.Max_Amount) - Convert.ToDouble(Patient.Consumption_Total)).ToString();
                    }
                    catch { }
                }
                else
                    MessageBox.Show("هذا الكارت غير موجود من فضلك تأكد من رقم الكارت");
            }
        }



        private void SwitchBTN_Click(object sender, RoutedEventArgs e)
        {
          
                new MainWindow().Show();
            this.Close();
        }        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        
        }
    }
}
