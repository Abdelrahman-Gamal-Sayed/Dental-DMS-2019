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
using CrystalDecisions.Shared;
using Microsoft.Win32;

namespace ONA_Clinics
{
    /// <summary>
    /// Interaction logic for form1.xaml
    /// </summary>
    public partial class frmPhysicalTherapy : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();


        public frmPhysicalTherapy()
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
               string cardnom= txtCard.Text ;
                gr_main.CleanAll();
                Patient.Card_NO = "";
                txtCard.Text = cardnom;
                grbox_newser.Visibility = Visibility.Hidden;
                grbox_madserv.Visibility = Visibility.Hidden;
                grbox_serv_old.Visibility = Visibility.Hidden;
                User.Provider_Code = "1336";
                int D_DD=0 ,USERDEGRY=0;
                
                //try {
                //    string DEGREY = db.RunReader(@"SELECT COVER_RELATION, BS_ANAME FROM dms_test.COMP_CONTRACT_CLASS, dms_test.BASIC_DATA 
                //                                                 WHERE COMP_CONTRACT_CLASS.COVER_RELATION = BASIC_DATA.BS_CODE AND SOURCE_MOD = 'PRDEG' 
                //                                                    AND C_COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "'").Rows[0][1].ToString();

                //    switch (DEGREY)
                //    {
                //        case "A++" :
                //            D_DD = 5;
                //            break;
                //        case "A+":
                //            D_DD = 4;
                //            break;

                //        case "A" :
                //            D_DD = 3;
                //            break;
                //        case "B":
                //            D_DD = 2;
                //            break;

                //        case  "C":

                //            D_DD = 1;

                //            break;

                //        default:
                //            break;
                //    }

                //    switch (User.Degree)
                //    {
                //        case "A++":
                //            USERDEGRY = 5;
                //            break;
                //        case "A+":
                //            USERDEGRY = 4;
                //            break;

                //        case "A":
                //            USERDEGRY = 3;
                //            break;
                //        case "B":
                //            USERDEGRY = 2;
                //            break;

                //        case "C":

                //            USERDEGRY = 1;

                //            break;

                //        default:
                //            break;
                //    }

                //}
                //catch
                //{
                //    return;
                //}
           

                txt_amount_new.Text = "";
                DataTable dtinformation = db.RunReader("select  to_char(BIRTH_DATE,'DD-MM-YYYY'),C_COMP_ID,CLASS_CODE,NVL(to_char(SPECIFIC_DATE,'DD-MM-YYYY'),to_char(INS_START_DATE,'DD-MM-YYYY')),to_char(INS_END_DATE,'DD-MM-YYYY'),TERMINATE_FLAG ,EMP_ANAME_ST ,EMP_ANAME_SC,EMP_ANAME_TH ,EMP_ENAME_ST ,EMP_ENAME_SC,EMP_ENAME_TH, to_char(INS_START_DATE,'DD-MM-YYYY'), to_char(TERMINATE_DATE,'DD-MM-YYYY') from  dms_test.COMP_EMPLOYEES where CARD_ID='" + txtCard.Text + "' order by ins_start_date DESC");

                if (D_DD < USERDEGRY)
                {
                    MessageBox.Show("هذا الكارت علي فئة اقل ");
                    gr_main.CleanAll();
                    Patient.Card_NO = "";
                    txtCard.Text = "";
                }

                else if (dtinformation.Rows.Count > 0 && Convert.ToDateTime(dtinformation.Rows[0][4]).Date < DateTime.Now.Date)
                {
                    MessageBox.Show("خارج التغطية التامينية");
                    gr_main.CleanAll();
                    Patient.Card_NO = "";
                    txtCard.Text = "";
                }
                else if (dtinformation.Rows.Count > 0 && dtinformation.Rows[0][5].ToString() == "Y" && Convert.ToDateTime(dtinformation.Rows[0][13]).Date < DateTime.Now.Date)
                {
                    MessageBox.Show("أنتهى التعاقد مع هذا الموظف بتاريخ " + dtinformation.Rows[0][13].ToString());
                    gr_main.CleanAll();
                    Patient.Card_NO = "";
                    txtCard.Text = "";
                }
                else if (dtinformation.Rows.Count > 0)
                {
                    //string comp = "", cont = "", cls = "", maxamt = "", conirs = "", conon = "", cononothr = "", cononmed = "", conuse = "", avilabl = "";
                    //comp = txtCard.Text.Substring(0, txtCard.Text.IndexOf('-'));

                    string cononmed = "", cononothr = "";
                    Patient.Card_NO = txtCard.Text;
                    txtBirthDate.Text = dtinformation.Rows[0][0].ToString();
                    Patient.Comp_id = dtinformation.Rows[0][1].ToString();
                    txtCompName.Text = db.RunReader(@"select C_ENAME from dms_test.CONTRACT_COMP  WHERE  C_COMP_ID  = '" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    txtAge.Text = (DateTime.Now.Year - Convert.ToDateTime(dtinformation.Rows[0][0]).Year).ToString();
                    Patient.Class_Code = dtinformation.Rows[0][2].ToString();
                    txtContractStartDate.Text = dtinformation.Rows[0][3].ToString();
                    txtContractEndtDate.Text = dtinformation.Rows[0][4].ToString();
                    Patient.AName = dtinformation.Rows[0][6].ToString() + " " + dtinformation.Rows[0][7].ToString() + " " + dtinformation.Rows[0][8].ToString();
                    Patient.EName = dtinformation.Rows[0][9].ToString() + " " + dtinformation.Rows[0][10].ToString() + " " + dtinformation.Rows[0][11].ToString();

                    txtEName.Text = Patient.EName;
                    txtAName.Text = Patient.AName;

                    Patient.Contract_NO = db.RunReader("select max(contract_no) from  dms_test.COMP_EMPLOYEES  where C_COMP_ID='" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    Patient.Max_Amount = db.RunReader("select MAX_AMOUNT from DMS_TEST.COMP_CONTRACT_CLASS where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "'").Rows[0][0].ToString();

                    DataTable dtirs = db.RunReader("select TOT_NET_AMT from IRS_CONSUMPTION where CARD_NO = '" + Patient.Comp_id + "' and CONTRACT_NO = (select max(CONTRACT_NO) FROM IRS_CONSUMPTION where CARD_NO = '" + Patient.Card_NO + "') AND to_date(CON_END_DATE) > to_date(sysdate) ");
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


                    DataTable aaa = new DataTable();
                    aaa = db.RunReader(@"SELECT COPAY_PERC, MAX_AMOUNT, COPAY_AMT FROM COMP_CONTRACT_CLASS_PROVIDER 
                                         WHERE COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND PR_CODE = '" + User.Provider_Code + "' AND SERV_CODE = '11204'");
                    if (aaa.Rows.Count == 0)
                        aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204' AND CARD_ID = '" + txtCard.Text + "'");
                    if (aaa.Rows.Count == 0)
                        aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204'");
                    //   else if (aaa.Rows.Count == 0)
                    // aaa = db.RunReader("select CEILING_PERT,CEILING_AMT, CARR_AMT from V_P_COMP_CUSTOMIZED_D where C_COMP_ID='" + Patient.Comp_id  + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204'");

                    if (aaa.Rows.Count == 0)
                    {
                        MessageBox.Show("هذا الكارت غير متاح له هذه الخدمة");

                        gr_main.CleanAll();
                        Patient.Card_NO = "";
                        txtCard.Text = "";
                    }
                    else
                    {
                        txt_comp_prce.Text = aaa.Rows[0][0].ToString() + "%";
                        txt_pationt_prce.Text = (100 - Convert.ToDouble(aaa.Rows[0][0].ToString())).ToString() + "%";
                        Patient.Patient_carrying_ratio = 100 - Convert.ToDouble(aaa.Rows[0][0].ToString());
                        Patient.CEILING_AMT = aaa.Rows[0][1].ToString();
                        Patient.CARR_AMT = aaa.Rows[0][2].ToString();
                    }
                }
                else
                { MessageBox.Show("هذا الكارت غير موجود من فضلك تأكد من رقم الكارت");
                    gr_main.CleanAll();
                    Patient.Card_NO = "";
                    txtCard.Text = "";
                }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbxmagornew.ItemsSource == null)
            {
                cbxmagornew.ItemsSource = db.RunReader("select BS_CODE,BS_ANAME from BASIC_DATA_NEW where ACTIVE='Y'  and SOURCE_MOD='DTYP' and BS_TYPE='DOC' ").DefaultView;
            }
        }



        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grbox_newser.Visibility = Visibility.Hidden;
            grbox_madserv.Visibility = Visibility.Hidden;
            grbox_serv_old.Visibility = Visibility.Hidden;
            refBTN_save_new.IsEnabled = true;
            switch (ListView.SelectedIndex)
            {
                case 0:
                    grbox_newser.Visibility = Visibility.Visible;
                    gr_add_new.CleanAll();
                    refBTN_save_new.IsEnabled = true;
                    break;
                case 1:
                    grbox_madserv.Visibility = Visibility.Visible;
                    dg_mat_mainsous.ItemsSource = rf_mad_main_grad().DefaultView;
                    gr_add_new3.CleanAll();
                    break;

                case 2:
                    grbox_serv_old.Visibility = Visibility.Visible;
                    dataGrid3.ItemsSource = rf_mad_main_grad().DefaultView;
                    break;
                case 3:
                    break;

                case 4:

                    gr_main.CleanAll();
                    Patient.Card_NO = "";
                    txtCard.Text = "";

                    break;

                default:
                    break;
            }

        }
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
        private void refBTN_Copy2_Click(object sender, RoutedEventArgs e)
        {
            gr_add_new.CleanAll();
            refBTN_save_new.IsEnabled = true;

        }

        private void refBTN_save_new_Click(object sender, RoutedEventArgs e)
        {

            if (txt_name_dector_new.Text == string.Empty || cbxmagornew.Text == string.Empty || txtCard.Text == string.Empty || txt_nom_gls_new.Text == string.Empty || txt_nom_part_new1.Text == string.Empty || txt_nom_gls_new2.Text == string.Empty || txt_amount_new.Text == string.Empty)
                MessageBox.Show("اكمل البيانات");
            else if (Convert.ToInt64(txt_nom_gls_new.Text) == 0 || Convert.ToInt64(txt_nom_part_new1.Text) == 0 || Convert.ToInt64(txt_nom_gls_new2.Text) == 0)
            {
                MessageBox.Show("يجب ان يكون العدد  اكبر من الصفر");
            }
            else
            {
                string activ = "N";
                if (Convert.ToInt64(txt_nom_gls_new.Text) == 1)
                    activ = "Y";
                string code = db.RunReader("select nvl( max(code) + 1,1) from PHYSICAL_THERAPY").Rows[0][0].ToString();
                if (db.RunNonQuery(@"INSERT INTO PHYSICAL_THERAPY( CODE           ,CARD_ID ,PROVIDER_CODE                                                         ,DOCTOR_NAME ,DOCTOR_SPECIALTY                               ,NUM_SESSIONS                 ,NUM_SERV                      ,NUM_MEMBERS ,AMOUNT ,ACTIVE ,CONTRACT_NO,CLASS_CODE  )
                                                       VALUES('" + code + "','" + txtCard.Text + "','" + User.Provider_Code + "','" + txt_name_dector_new.Text + "','" + cbxmagornew.Text + "','" + txt_nom_gls_new.Text + "','" + txt_nom_gls_new2.Text + "','" + txt_nom_part_new1.Text + "','" + txt_amount_new.Text + "','" + activ + "','" + Patient.Contract_NO + "','" + Patient.Class_Code + "')", "تم الحفظ"))
                {
                    refBTN_save_new.IsEnabled = false;
                    db.RunNonQuery("INSERT INTO FOLLOW_SESSIONS (SESSIONS_CODE,SESSIONS_DATE) VALUES('" + code + "',SYSDATE )");

                    if (activ == "Y")
                    {
                        try
                        {
                            prentnumserv repo = new prentnumserv();

                            Window1 showreport = new Window1();

                            repo.SetDatabaseLogon("APP", "12369");

                            repo.SetParameterValue("p_code", txtAge2.Text);
                            repo.SetParameterValue("EMP_NAME", txtEName.Text);


                            showreport.crystalReportsViewer111.ViewerCore.ReportSource = repo;
                            showreport.ShowDialog();

                            ExportOptions exp = new ExportOptions();
                            DiskFileDestinationOptions dis = new DiskFileDestinationOptions();

                            PdfFormatOptions expdf = new PdfFormatOptions();
                            string sa = "";

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "Pdf file|*.pdf";
                            sfd.FileName = txtEName.Text + DateTime.Now.ToString("ddd, dd MMMM yyyy HH mm ss");
                            if (sfd.ShowDialog() == true)
                                sa = sfd.FileName;

                            dis.DiskFileName = sa;
                            exp = repo.ExportOptions;
                            exp.ExportDestinationType = ExportDestinationType.DiskFile;
                            exp.ExportFormatType = ExportFormatType.PortableDocFormat;
                            exp.ExportFormatOptions = expdf;
                            exp.ExportDestinationOptions = dis;
                            repo.Export();

                            MessageBox.Show("Successfull Export to Pdf");

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            }
        }

        private void txt_amount_new_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double test, max_amount = 0, add_catrd = 0;
                if (double.TryParse(txt_amount_new.Text, out test))
                {
                    txt_tota_amount.Text = txt_amount_new.Text;
                    if (Patient.CEILING_AMT == string.Empty || Convert.ToDouble(Patient.CEILING_AMT) > Convert.ToDouble(Patient.Available))
                        max_amount = Convert.ToDouble(Patient.Available);
                    else
                        max_amount = Convert.ToDouble(Patient.CEILING_AMT);
                    if (Patient.CARR_AMT != string.Empty)
                        add_catrd = Convert.ToDouble(Patient.CARR_AMT);
                    txt_add_card.Text = add_catrd.ToString();

                    if (max_amount < Convert.ToDouble(txt_amount_new.Text))
                    {
                        txt_over_seling.Text = (Convert.ToDouble(txt_amount_new.Text) - max_amount).ToString();
                        txt_pay_amount.Text = ((Convert.ToDouble(txt_amount_new.Text) - max_amount) + max_amount * Patient.Patient_carrying_ratio / 100 + add_catrd).ToString();
                    }
                    else
                    {
                        txt_over_seling.Text = "0";
                        txt_pay_amount.Text = (Convert.ToDouble(txt_amount_new.Text) * Patient.Patient_carrying_ratio / 100 + add_catrd).ToString();

                    }
                }
            }
            catch { }
        }
        DataTable rf_mad_main_grad()
        {

            return db.RunReader("SELECT CODE ,DOCTOR_NAME ,DOCTOR_SPECIALTY ,NUM_SESSIONS ,NUM_SERV ,NUM_MEMBERS ,AMOUNT  FROM PHYSICAL_THERAPY  where CARD_ID='" + Patient.Card_NO + "' and ACTIVE='N' and PROVIDER_CODE='" + User.Provider_Code + "'");
        }
        private void refBTN_Copy5_Click(object sender, RoutedEventArgs e)
        {
            gr_add_mad.CleanAll();
            refBTN_Copy6.IsEnabled = true;
            dg_mat_mainsous.ItemsSource = rf_mad_main_grad().DefaultView;
        }

        private void refBTN_Copy6_Click(object sender, RoutedEventArgs e)
        {//if()
            try
            {
                if (txt_nom_glsat_mad.Text != string.Empty && dg_mat_mainsous.SelectedIndex > -1)
                {

                    DataRowView row = (DataRowView)dg_mat_mainsous.SelectedItem;
                    if (Convert.ToInt64(txt_nom_glsat_mad.Text) > Convert.ToInt64(row[3].ToString()))
                    {
                        db.RunNonQuery("UPDATE PHYSICAL_THERAPY SET NUM_SESSIONS= '" + txt_nom_glsat_mad.Text + "' WHERE CODE='" + txt_amount_mad1.Text + "'", "تم الحفظ");
                    }
                    else
                    {
                        MessageBox.Show("يجب ذيادة عدد الجلسات");
                    }

                }


            }
            catch { }
        }

        private void dg_mat_mainsous_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dg_mat_mainsous.SelectedItem;
                txt_amount_mad1.Text = row[0].ToString();
                txt_doctor_mad.Text = row[1].ToString();
                txt_spatialty_mad.Text = row[2].ToString();
                txt_nom_glsat_mad.Text = row[3].ToString();
                txt_amount_mad.Text = row[6].ToString();
            }
            catch
            {

            }
        }

        private void refBTN_Copy8_Click(object sender, RoutedEventArgs e)
        {
            if (txtAge2.Text == string.Empty || txtAge11.Text == string.Empty)
            {
                MessageBox.Show("اختار العملية");
            }
            else if (db.RunNonQuery("INSERT INTO FOLLOW_SESSIONS (SESSIONS_CODE,SESSIONS_DATE) VALUES('" + txtAge2.Text + "',SYSDATE )", "تم الحفظ"))
            {


                dataGrid1_Copy2.ItemsSource = db.RunReader(" select SESSIONS_DATE from  FOLLOW_SESSIONS where SESSIONS_CODE='" + txtAge2.Text + "' ").DefaultView;

                if (dataGrid1_Copy2.Items.Count >= Convert.ToInt64(txtAge11.Text))
                {
                    db.RunNonQuery("UPDATE PHYSICAL_THERAPY SET ACTIVE= 'Y' WHERE CODE='" + txtAge2.Text + "' ");

                    try
                    {
                        prentnumserv repo = new prentnumserv();

                        Window1 showreport = new Window1();

                        repo.SetDatabaseLogon("APP", "12369");

                        repo.SetParameterValue("p_code", txtAge2.Text);
                        repo.SetParameterValue("EMP_NAME", txtEName.Text);


                        showreport.crystalReportsViewer111.ViewerCore.ReportSource = repo;
                        showreport.ShowDialog();

                        ExportOptions exp = new ExportOptions();
                        DiskFileDestinationOptions dis = new DiskFileDestinationOptions();

                        PdfFormatOptions expdf = new PdfFormatOptions();
                        string sa = "";

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Pdf file|*.pdf";
                        sfd.FileName = txtEName.Text + DateTime.Now.ToString("ddd, dd MMMM yyyy HH mm ss");
                        if (sfd.ShowDialog() == true)
                            sa = sfd.FileName;

                        dis.DiskFileName = sa;
                        exp = repo.ExportOptions;
                        exp.ExportDestinationType = ExportDestinationType.DiskFile;
                        exp.ExportFormatType = ExportFormatType.PortableDocFormat;
                        exp.ExportFormatOptions = expdf;
                        exp.ExportDestinationOptions = dis;
                        repo.Export();

                        MessageBox.Show("Successfull Export to Pdf");

                    }
                    catch { }
                }
                dataGrid1_Copy2.ItemsSource = null;
                gr_add_new3.CleanAll();
                dataGrid3.ItemsSource = rf_mad_main_grad().DefaultView;


            }
        }

        private void refBTN_Copy7_Click(object sender, RoutedEventArgs e)
        {
            dataGrid3.ItemsSource = rf_mad_main_grad().DefaultView;
            refBTN_Copy8.IsEnabled = true;
        }

        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGrid3.SelectedIndex > -1)
                {
                    DataRowView row = (DataRowView)dataGrid3.SelectedItem;
                    txtAge2.Text = row[0].ToString();
                    txtAge14.Text = row[1].ToString();
                    txtAge15.Text = row[2].ToString();
                    txtAge11.Text = row[3].ToString();
                    txtAge12.Text = row[6].ToString();

                    try
                    {
                        double test, max_amount = 0, add_catrd = 0;
                        if (double.TryParse(txt_amount_new.Text, out test))
                        {
                            txt_tota_amount.Text = txt_amount_new.Text;
                            if (Patient.CEILING_AMT == string.Empty || Convert.ToDouble(Patient.CEILING_AMT) > Convert.ToDouble(Patient.Available))
                                max_amount = Convert.ToDouble(Patient.Available);
                            else
                                max_amount = Convert.ToDouble(Patient.CEILING_AMT);
                            if (Patient.CARR_AMT != string.Empty)
                                add_catrd = Convert.ToDouble(Patient.CARR_AMT);
                            txt_add_card.Text = add_catrd.ToString();

                            if (max_amount < Convert.ToDouble(txt_amount_new.Text))
                            {
                                txt_over_seling.Text = (Convert.ToDouble(txt_amount_new.Text) - max_amount).ToString();
                                txt_pay_amount.Text = ((Convert.ToDouble(txt_amount_new.Text) - max_amount) + max_amount * Patient.Patient_carrying_ratio / 100 + add_catrd).ToString();
                            }
                            else
                            {
                                txt_over_seling.Text = "0";
                                txt_pay_amount.Text = (Convert.ToDouble(txt_amount_new.Text) * Patient.Patient_carrying_ratio / 100 + add_catrd).ToString();

                            }
                        }
                    }
                    catch { }
                    dataGrid1_Copy2.ItemsSource = db.RunReader(" select SESSIONS_DATE from  FOLLOW_SESSIONS where SESSIONS_CODE='" + row[0].ToString() + "' ").DefaultView;

                }
            }
            catch { }
        }

        private void refBTN_Copy9_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want save report to pdf file", "Save pdf file", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    prentnumserv repo = new prentnumserv();

                    Window1 showreport = new Window1();

                    repo.SetDatabaseLogon("APP", "12369");

                    repo.SetParameterValue("p_code", txtAge2.Text);
                    repo.SetParameterValue("EMP_NAME", txtEName.Text);

                   
                    showreport.crystalReportsViewer111.ViewerCore.ReportSource = repo;
                    showreport.ShowDialog();

                    ExportOptions exp = new ExportOptions();
                    DiskFileDestinationOptions dis = new DiskFileDestinationOptions();

                    PdfFormatOptions expdf = new PdfFormatOptions();
                    string sa = "";

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Pdf file|*.pdf";
                    sfd.FileName = txtEName.Text + DateTime.Now.ToString("ddd, dd MMMM yyyy HH mm ss");
                    if (sfd.ShowDialog() == true)
                        sa = sfd.FileName;

                    dis.DiskFileName = sa;
                    exp = repo.ExportOptions;
                    exp.ExportDestinationType = ExportDestinationType.DiskFile;
                    exp.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exp.ExportFormatOptions = expdf;
                    exp.ExportDestinationOptions = dis;
                    repo.Export();

                    MessageBox.Show("Successfull Export to Pdf");

                }
                catch { }
            }
        }

      
    }
}
