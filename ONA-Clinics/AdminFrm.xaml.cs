using System;
using System.Windows;
using System.Windows.Input;



namespace ONA_Clinics
{
 
    /// <summary>
    /// Interaction logic for form1.xaml
    /// </summary>
    public partial class AdminFrm : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();

        public AdminFrm()
        {
            InitializeComponent();
            cbxServies3.ItemsSource = db.RunReader("select CODE,NAME from  USER_AUTHORITYS ").DefaultView;
            txt_code_user1.Text = db.RunReader("select nvl(Max(USER_ID),1)+1 from USERS_HOSPITAL").Rows[0][0].ToString();
            cbxServies2.ItemsSource = db.RunReader("select PRV_TYPE,TYP_ANAME from PROVIDER_TYP22 ").DefaultView;
            txt_occerty_code_main_user.Text = db.RunReader("select nvl(Max(CODE),0)+1 from USER_AUTHORITYS").Rows[0][0].ToString();

        }


        //Dentaltooth.subservies = cbxServies.Text;
        #region MainRegion

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.]+");
            e.Handled = reg.IsMatch(e.Text);

        }
        private void NumberOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = reg.IsMatch(e.Text);

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
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();

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

        private void AdminBTN_Click(object sender, RoutedEventArgs e)
        {
            new AdminFrm().Show();
            this.Close();
        }

        private void clinicBTN_Click(object sender, RoutedEventArgs e)
        {
            new ClinicsFrm().Show();
            this.Close();

        }

        private void hospitalBTN_Click(object sender, RoutedEventArgs e)
        {
            new form1().Show();
            this.Close();
        }

        private void SwitchBTN_Click(object sender, RoutedEventArgs e)
        {
          
                new MainWindow().Show();
            this.Close();
        }        
   
        private void refBTN_Click(object sender, RoutedEventArgs e)
        {
         
            Console.WriteLine("SAMPLE 2: Closing dialog with parameter:");

          
        }





        #endregion

        private void Button_Click_3(object sender, RoutedEventArgs e) {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           // DataTable ss = db.RunReader("select NAME ,DENTAL_FRM ,HOSPITAL_FRM ,PHYSICAL_THERAPY_FRM,DOCTOR_FRM  from USER_AUTHORITYS WHERE CODE ='" + User.AUTHORITY_CODE + "'");

          
            txt_occerty_name_main_user.Text = "";
            chb_main.IsChecked = false;
            chb_cacher.IsChecked = false;

            chb_katchen.IsChecked = false;
            chb_view.IsChecked = false;
            chb_report.IsChecked = false;

            txt_occerty_code_main_user.Text = db.RunReader("select nvl(Max(CODE),1)+1 from USER_AUTHORITYS").Rows[0][0].ToString();
            dg_user_main1.ItemsSource = null;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt_code_user1.Text = "";
            txtAName3.Text = "";
            txtAName4.Password = "";
            txtAName5.Text = "";
      
            txt_user_main_seach2.Text = "";
            cbxServies2.Text = "";
            cbx_osserty_user_Copy3.Text = "";
           
            cbxServies3.Text = "";
            txt_code_user1.Text = db.RunReader("select nvl(Max(USER_ID),1)+1 from USERS_HOSPITAL").Rows[0][0].ToString();
            dgTotal.ItemsSource = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

         

                  if (txt_occerty_name_main_user.Text.Trim() == string.Empty)
                MessageBox.Show("اكمل البيانات");
            else
            {
                string DENTAL_FRM = "N", HOSPITAL_FRM = "N", PHYSICAL_THERAPY_FRM = "N", DOCTOR_FRM = "N", ADMIN_FRM = "N";
                if (chb_katchen.IsChecked == true)
                    DENTAL_FRM = "Y";

                if (chb_cacher.IsChecked == true)
                    HOSPITAL_FRM = "Y";


                if (chb_view.IsChecked == true)
                    PHYSICAL_THERAPY_FRM = "Y";


                if (chb_report.IsChecked == true)
                    DOCTOR_FRM = "Y";


                if (chb_main.IsChecked == true)
                    ADMIN_FRM = "Y";

                if (db.RunReader(" select CODE  from USER_AUTHORITYS  where CODE ='" + txt_occerty_code_main_user.Text + "' ").Rows.Count == 0)
                    db.RunNonQuery(@"insert into USER_AUTHORITYS (CODE, NAME , DENTAL_FRM, HOSPITAL_FRM, PHYSICAL_THERAPY_FRM, DOCTOR_FRM, ADMIN_FRM,CREATEDBY ,CREATEDDATE) 
 VALUES ('" + txt_occerty_code_main_user.Text + "','" + txt_occerty_name_main_user.Text + "','" + DENTAL_FRM + "','" + HOSPITAL_FRM + "','" + PHYSICAL_THERAPY_FRM + "','" + DOCTOR_FRM + "','" + ADMIN_FRM + "','" + User.Name + "',sysdate)", "تم الحفظ");
                else
                    db.RunNonQuery("UPDATE USER_AUTHORITYS SET NAME ='" + txt_occerty_name_main_user.Text + "'  ,DENTAL_FRM = '" + DENTAL_FRM + "', HOSPITAL_FRM = '" + HOSPITAL_FRM + "', PHYSICAL_THERAPY_FRM ='" + PHYSICAL_THERAPY_FRM + "', DOCTOR_FRM = '" + DOCTOR_FRM + "', ADMIN_FRM = '" + ADMIN_FRM + "', UPDATEDBY = '" + User.Name + "', UPDATEDDATE = sysdate WHERE CODE='" + txt_occerty_code_main_user.Text + "' ", "تم التعديل");
            }
                  cbxServies3.ItemsSource = db.RunReader("select CODE,NAME from  USER_AUTHORITYS ").DefaultView;
      
        }

        private void txt_user_main_seach1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            dg_user_main1.ItemsSource = db.RunReader("select CODE, NAME , DENTAL_FRM, HOSPITAL_FRM, PHYSICAL_THERAPY_FRM, DOCTOR_FRM, ADMIN_FRM from  USER_AUTHORITYS where CODE like '%" + txt_user_main_seach1.Text.Trim() + "%' or NAME like '%" + txt_user_main_seach1.Text.Trim() + "%' ").DefaultView;
        }

        private void txt_user_main_seach2_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            dgTotal.ItemsSource = db.RunReader("select USER_ID, USER_NAME , PASSWORD, SHOW_NAME, PROVIDER_CODE, PROVIDER_ANAME, PROVIDER_ENAME,DEGREE_CODE ,DEGREE_NAME,PROVIDER_TYPE ,USER_AUTHORITY_CODE,CREATEDBY,CREATEDDATE from  USERS_HOSPITAL where USER_ID like '%" + txt_user_main_seach2.Text.Trim() + "%' or USER_NAME like '%" + txt_user_main_seach2.Text.Trim() + "%' ").DefaultView;
     
        }

        private void Cbx_osserty_user_Copy4_DropDownOpened(object sender, EventArgs e)
        {
        }

      
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
         
            try
            {
            

          
                cbx_osserty_user_Copy3.ItemsSource = db.RunReader("select DISTINCT PR_CODE, PR_ANAME, PR_ENAME, PROV_DEGREE from SERV_PROVIDERS  where (PR_CODE like '%" + cbx_osserty_user_Copy3.Text + "%' or upper(PR_ANAME) like '%" + cbx_osserty_user_Copy3.Text.ToUpper() + "%' or upper(PR_ENAME) like '%" + cbx_osserty_user_Copy3.Text.ToUpper() + "%') and TERMINATE_FLAG = 'N' and PRV_TYPE =" + cbxServies2.Text + " AND ROWNUM <= 50 order by PR_CODE").DefaultView;
                cbx_osserty_user_Copy3.IsDropDownOpen = true;
            }
            catch { }
        }

       

        private void CbxServies2_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                //if (cbxServies2.Text.Trim() != "")
                //    cbx_osserty_user_Copy3.ItemsSource = db.RunReader("   select s.PR_CODE,s.PR_ANAME from SERV_PROVIDERS s, PROVIDER_TYP22 p where s.PRV_TYPE = p.PRV_TYPE and p.TYP_ANAME = '" + cbxServies2.Text + "'").DefaultView;


                cbx_osserty_user_Copy3.ItemsSource = db.RunReader("SELECT DISTINCT PR_CODE,PR_ANAME, PR_ENAME, PROV_DEGREE from SERV_PROVIDERS where PRV_TYPE =" + cbxServies2.Text + " and TERMINATE_FLAG = 'N' AND ROWNUM <= 50 order by PR_CODE").DefaultView;
            }
            catch { }
        }

        private void Dg_user_main1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {  //      dg_user_main1.ItemsSource = db.RunReader("select CODE, NAME , DENTAL_FRM, HOSPITAL_FRM, PHYSICAL_THERAPY_FRM, DOCTOR_FRM, ADMIN_FRM from  USER_AUTHORITYS where CODE like '%" + txt_user_main_seach1.Text.Trim() + "%' or NAME like '%" + txt_user_main_seach1.Text.Trim() + "%' ").DefaultView;
            try
            {
                if (dg_user_main1.SelectedIndex > -1)
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)dg_user_main1.SelectedItem;
                    txt_occerty_code_main_user.Text = row[0].ToString();
                    txt_occerty_name_main_user.Text = row[1].ToString();
                    chb_katchen.IsChecked = (row[2].ToString() == "Y") ? true : false;
                    chb_cacher.IsChecked = (row[3].ToString() == "Y") ? true : false;
                    chb_view.IsChecked = (row[4].ToString() == "Y") ? true : false;
                    chb_report.IsChecked = (row[5].ToString() == "Y") ? true : false;
                    chb_main.IsChecked = (row[6].ToString() == "Y") ? true : false;

                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            string PROVIDER_CODE = cbx_osserty_user_Copy3.Text.Trim(), PROVIDER_ENAME, PROVIDER_ANAME, DEGREE_CODE, DEGREE_NAME;

            if(cbxServies3.Text.Trim()=="") {
                MessageBox.Show("برجاء اختيار صلاحية");

                return;
            }


            if (PROVIDER_CODE == "")
            {
                MessageBox.Show("ادخل مقدم الخدمة");
                return;
            }
            else
            {
                System.Data.DataTable ss = db.RunReader("select DISTINCT PR_CODE, PR_ANAME, PR_ENAME, PROV_DEGREE, BS_ANAME from SERV_PROVIDERS, dms_test.BASIC_DATA  where PR_CODE='"+ PROVIDER_CODE + "' AND SERV_PROVIDERS.PROV_DEGREE = BASIC_DATA.BS_CODE AND SOURCE_MOD = 'PRDEG' ");
                PROVIDER_ANAME = ss.Rows[0][1].ToString();
                PROVIDER_ENAME = ss.Rows[0][2].ToString();
                DEGREE_CODE = ss.Rows[0][3].ToString();
                DEGREE_NAME = ss.Rows[0][4].ToString();

            }

            if (txtAName3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("اكمل البيانات");
                return;
            }







            if (db.RunReader(" select USER_ID  from USERS_HOSPITAL  where USER_ID ='" + txt_code_user1.Text + "' ").Rows.Count == 0)
                db.RunNonQuery(@"insert into USERS_HOSPITAL (USER_ID, USER_NAME , PASSWORD, SHOW_NAME, PROVIDER_CODE, PROVIDER_ANAME, PROVIDER_ENAME,DEGREE_CODE ,DEGREE_NAME,PROVIDER_TYPE ,USER_AUTHORITY_CODE,CREATEDBY,CREATEDDATE)
VALUES ('" + txt_code_user1.Text + "','" + txtAName3.Text.Trim().ToUpper() + "','" + txtAName4.Password.Trim() + "','" + txtAName5.Text.Trim() + "','" +PROVIDER_CODE + "','" + PROVIDER_ANAME + "','" + PROVIDER_ENAME + "','" + DEGREE_CODE + "','"+ DEGREE_NAME + "','" + cbxServies2.Text + "','" + cbxServies3.Text + "','" + User.Name + "',sysdate)", "تم الحفظ");
            else
                db.RunNonQuery("UPDATE USERS_HOSPITAL SET USER_NAME ='" + txtAName3.Text.Trim().ToUpper() + "'  ,PASSWORD = '" + txtAName4.Password.Trim() + "', SHOW_NAME = '" + txtAName5.Text.Trim() 
                    + "', PROVIDER_CODE ='" + PROVIDER_CODE + "', PROVIDER_ANAME = '" + PROVIDER_ANAME + "', PROVIDER_ENAME = '" + PROVIDER_ENAME + "', DEGREE_CODE = '" + DEGREE_CODE + "', DEGREE_NAME = '"+ DEGREE_NAME + "', PROVIDER_TYPE = '" + cbxServies2.Text + "', USER_AUTHORITY_CODE = '" + cbxServies3.Text + "', UPDATEDBY  = '" + User.Name + "', UPDATEDDATE = sysdate WHERE USER_ID='" + txt_code_user1.Text + "' ", "تم التعديل");


        }

        private void DgTotal_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (dgTotal.SelectedIndex > -1)
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)dgTotal.SelectedItem;
                    txt_code_user1.Text = row[0].ToString();
                    txtAName3.Text = row[1].ToString();
                    txtAName4 .Password= row[2].ToString();
                    txtAName5.Text = row[3].ToString();
                    cbx_osserty_user_Copy3.Text = row[4].ToString();
                    cbxServies2.Text = row[9].ToString();
                    cbxServies3.Text = row[10].ToString();
              

                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

     
    }
}
