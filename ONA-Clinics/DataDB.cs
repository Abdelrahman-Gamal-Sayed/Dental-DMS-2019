using System;
using System.Data;
using System.Data.OracleClient;
using Microsoft.Win32;
using System.Windows;



namespace ONA_Clinics
{
    class DataDB
    {

        string constring = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
                                            (HOST=**********)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                                            (SERVICE_NAME=ora11g)));User Id=app;Password=******";

        
//    @"INSERT INTO ONLINE_DATA (CODE, COMP_ID, CONTRACT_NO, CLASS_CODE, CARD_ID, EMP_ANAME, EMP_ENAME, BIRTHDAY, TYPE, PROVIDER_ID, PROVIDER_NAME, APROVAL_NO, CHECKUP_TYP, LAST_CHECKUP, NEXT_CONSULT, PERCENT, NOTES, ALL_AMT, CREDIT, CASH, CREATED_BY, CREATED_DATE) 
//        VALUE(:cod, :cmp, :contr, :cls, :crd, :empe, :empa, :birth, :typ, :prvidno, :prvidnam, :aprovno, :chktyp, :lstchk, :nxtcon, :perc, :nts, :amt, :crdt, :csh, :crby, CREATED_DATE)"


        public DataTable getonlinevalue(string crd, DateTime dat1, DateTime dat2, int flg)
        {

            OracleConnection con = new OracleConnection(constring);

            OracleCommand cmd = new OracleCommand();

            OracleDataAdapter da;
            //            OracleConnection con;
            //            OracleCommand cmd = new OracleCommand();
            //            OracleDataAdapter da;
            //            con = new OracleConnection(@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
            //                                            (HOST=********** )(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
            //                                            (SERVICE_NAME=ora11g)));User Id=app;Password=******");

            DataTable dd = new DataTable();
            try
            {
                if (flg == 1)
                    cmd = new OracleCommand(@" SELECT NVL(SUM(CLAIM_AMOUNT),0) FROM ONLINE_CONS_01 WHERE card_no =:crd and claim_date BETWEEN :dat1 AND :dat2 AND group_no = 116", con);
                else
                    cmd = new OracleCommand(@" SELECT NVL(SUM(CLAIM_AMOUNT),0) FROM ONLINE_CONS_01 WHERE card_no =:crd and claim_date BETWEEN :dat1 AND :dat2 AND group_no != 116", con);

                cmd.Parameters.Clear();
                cmd.Parameters.Add(":crd", OracleType.VarChar).Value = crd;
                cmd.Parameters.Add(":dat1", OracleType.DateTime).Value = dat1;
                cmd.Parameters.Add(":dat2", OracleType.DateTime).Value = dat2;

                da = new OracleDataAdapter(cmd);

                da.Fill(dd);
                                
                return dd;
            }
            catch { return dd; }
            finally
            {
                con.Dispose();
                con.Close();

                OracleConnection.ClearAllPools();
            }
        }

        public int savedoctor(int typ, string amt, string crdt, string csh)
        {
            OracleConnection con = new OracleConnection(constring);
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da;
          
            try
            {

                cmd = new OracleCommand(@"INSERT INTO D_DETECTION (CODE, COMP_ID, CONTRACT_NO, CLASS_CODE, CARD_ID, PROVIDER_TYPE, PROVIDER_CODE, PROVIDER_NAME, TOTAL_AMOUNT, TOTAL_CREDIT, TOTAL_CASH, CREATED_BY, CREATED_DATE, SERV_CODE) 
                                                           VALUES ((SELECT NVL(MAX(CODE), 0) + 1 FROM D_DETECTION), :cmp, :contr, :cls, :crd, :typ, :prvidno, :prvidnam, :amt, :crdt, :csh, :crby, SYSDATE, 11205)", con);

                con.Open();
                cmd.Parameters.Clear();
                
                //(:, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :,

                cmd.Parameters.Add(":cmp", OracleType.Number).Value = Convert.ToInt32(Patient.Comp_id);
                cmd.Parameters.Add(":contr", OracleType.Number).Value = Convert.ToInt32(Patient.Contract_NO);
                cmd.Parameters.Add(":cls", OracleType.VarChar).Value = Patient.Class_Code;
                cmd.Parameters.Add(":crd", OracleType.VarChar).Value = Patient.Card_NO;
               
                cmd.Parameters.Add(":typ", OracleType.Number).Value = typ;

                if (User.Provider_Code != null &&  User.Provider_Code != string.Empty)
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = Convert.ToInt64(User.Provider_Code);
                else
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = DBNull.Value;

                if (User.Provider_English_Name != null && User.Provider_English_Name != string.Empty)
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = User.Provider_English_Name;// "Doctor";// User.Provider_English_Name.ToString();
                else
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = DBNull.Value; 

                if (amt != string.Empty)
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = Convert.ToDouble(amt);
                else
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = DBNull.Value;

                if (crdt != string.Empty)
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = Convert.ToDouble(crdt);
                else
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = DBNull.Value;

                if (csh != string.Empty)
                    cmd.Parameters.Add(":csh", OracleType.Number).Value = Convert.ToDouble(csh);
                else
                    cmd.Parameters.Add(":csh", OracleType.Number).Value = DBNull.Value;

                if (User.Name != null && User.Name != string.Empty)
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = User.Name;// User.Name.ToString();
                else
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = DBNull.Value;

                cmd.ExecuteNonQuery();

                //MessageBox.Show("تم الحفظ بنجاح");
                return 1;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); return 0; }
            finally
            {
                con.Dispose();
                con.Close();

                OracleConnection.ClearAllPools();
                
            }
        }
        public void savedoctor2(string cod, string birth, int typ, int chktyp, string lstchk, string nxtcon, int perc, string nts, string amt, string crdt, string csh)
        {
            OracleConnection con = new OracleConnection(constring);
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da;
            
            try
            {
                cmd = new OracleCommand(@"INSERT INTO D_ONLINE_DATA (CODE, COMP_ID, CONTRACT_NO, CLASS_CODE, CARD_ID, EMP_ANAME, EMP_ENAME, BIRTHDAY, TYPE, PROVIDER_ID, PROVIDER_NAME, CHECKUP_TYP, LAST_CHECKUP, NEXT_CONSULT, PERCENT, NOTES, ALL_AMT, CREDIT, CASH, CREATED_BY, CREATED_DATE) 
                                                            VALUES (:cod, :cmp, :contr, :cls, :crd, :empe, :empa, :birth, :typ, :prvidno, :prvidnam, :chktyp, :lstchk, :nxtcon, :perc, :nts, :amt, :crdt, :csh, :crby, SYSDATE)", con);

                con.Open();
                cmd.Parameters.Clear();

                //(:, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :,
                cmd.Parameters.Add(":cod", OracleType.Number).Value = Convert.ToInt32(cod);
                cmd.Parameters.Add(":cmp", OracleType.Number).Value = Convert.ToInt32(Patient.Comp_id);
                cmd.Parameters.Add(":contr", OracleType.Number).Value = Convert.ToInt32(Patient.Contract_NO);
                cmd.Parameters.Add(":cls", OracleType.VarChar).Value = Patient.Class_Code;
                cmd.Parameters.Add(":crd", OracleType.VarChar).Value = Patient.Card_NO;
                cmd.Parameters.Add(":empe", OracleType.VarChar).Value = Patient.EName;
                cmd.Parameters.Add(":empa", OracleType.VarChar).Value = Patient.AName;

                if (birth == string.Empty)
                    cmd.Parameters.Add(":birth", OracleType.DateTime).Value = DBNull.Value;
                else
                    cmd.Parameters.Add(":birth", OracleType.DateTime).Value = Convert.ToDateTime(birth);

                cmd.Parameters.Add(":typ", OracleType.Number).Value = typ;

                if (User.Provider_Code != null && User.Provider_Code != string.Empty)
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = Convert.ToInt64(User.Provider_Code);
                else
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = DBNull.Value;

                if (User.Provider_English_Name != null && User.Provider_English_Name != string.Empty)
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = User.Provider_English_Name;// "Doctor";// User.Provider_English_Name.ToString();
                else
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = DBNull.Value;

                cmd.Parameters.Add(":chktyp", OracleType.Number).Value = chktyp;

                if (lstchk == string.Empty)
                    cmd.Parameters.Add(":lstchk", OracleType.DateTime).Value = DBNull.Value;
                else
                    cmd.Parameters.Add(":lstchk", OracleType.DateTime).Value = Convert.ToDateTime(lstchk);

                if (nxtcon == string.Empty)
                    cmd.Parameters.Add(":nxtcon", OracleType.DateTime).Value = DBNull.Value;
                else
                    cmd.Parameters.Add(":nxtcon", OracleType.DateTime).Value = Convert.ToDateTime(nxtcon);

                cmd.Parameters.Add(":perc", OracleType.Number).Value = perc;

                cmd.Parameters.Add(":nts", OracleType.VarChar).Value = nts;
                if (amt != string.Empty)
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = Convert.ToDouble(amt);
                else
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = DBNull.Value;

                if (crdt != string.Empty)
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = Convert.ToDouble(crdt);
                else
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = DBNull.Value;

                if (csh != string.Empty)
                    cmd.Parameters.Add(":csh", OracleType.Number).Value = Convert.ToDouble(csh);
                else
                    cmd.Parameters.Add(":csh", OracleType.Number).Value = DBNull.Value;

                if (User.Name != null && User.Name != string.Empty)
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = User.Name;// User.Name.ToString();
                else
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = DBNull.Value;

                cmd.ExecuteNonQuery();

               // MessageBox.Show("تم الحفظ بنجاح");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                con.Dispose();
                con.Close();

                OracleConnection.ClearAllPools();
            }
        }

        public int saveapproval(string noaprov, string vlue)
        {
            OracleConnection con = new OracleConnection(constring);
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da;

            try
            {

                cmd = new OracleCommand(@"INSERT INTO D_DETECTION (CODE, COMP_ID, CONTRACT_NO, CLASS_CODE, CARD_ID, PROVIDER_TYPE, PROVIDER_CODE, PROVIDER_NAME, TOTAL_AMOUNT, TOTAL_CREDIT, TOTAL_CASH, CREATED_BY, CREATED_DATE, SERV_CODE, APPROVAL_NO) 
                                                           VALUES ((SELECT NVL(MAX(CODE), 0) + 1 FROM D_DETECTION), :cmp, :contr, :cls, :crd, :typ, :prvidno, :prvidnam, :amt, :crdt, :csh, :crby, SYSDATE, 11205, :noaprov)", con);

                con.Open();
                cmd.Parameters.Clear();

                //(:, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :, :,

                cmd.Parameters.Add(":cmp", OracleType.Number).Value = Convert.ToInt32(Patient.Comp_id);
                cmd.Parameters.Add(":contr", OracleType.Number).Value = Convert.ToInt32(Patient.Contract_NO);
                cmd.Parameters.Add(":cls", OracleType.VarChar).Value = Patient.Class_Code;
                cmd.Parameters.Add(":crd", OracleType.VarChar).Value = Patient.Card_NO;

                cmd.Parameters.Add(":typ", OracleType.Number).Value = 1;

                if (User.Provider_Code != null && User.Provider_Code != string.Empty)
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = Convert.ToInt64(User.Provider_Code);
                else
                    cmd.Parameters.Add(":prvidno", OracleType.Number).Value = DBNull.Value;

                if (User.Provider_English_Name != null && User.Provider_English_Name != string.Empty)
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = User.Provider_English_Name;// "Doctor";// User.Provider_English_Name.ToString();
                else
                    cmd.Parameters.Add(":prvidnam", OracleType.VarChar).Value = DBNull.Value;

                if (vlue != string.Empty)
                {
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = Convert.ToDouble(vlue);
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = Convert.ToDouble(vlue);
                }
                else
                {
                    cmd.Parameters.Add(":amt", OracleType.Number).Value = DBNull.Value;
                    cmd.Parameters.Add(":crdt", OracleType.Number).Value = DBNull.Value;
                }
                
                    cmd.Parameters.Add(":csh", OracleType.Number).Value = 0;

                if (User.Name != null && User.Name != string.Empty)
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = User.Name;// User.Name.ToString();
                else
                    cmd.Parameters.Add(":crby", OracleType.VarChar).Value = DBNull.Value;

                cmd.Parameters.Add(":noaprov", OracleType.VarChar).Value = noaprov;

                cmd.ExecuteNonQuery();

                //MessageBox.Show("تم الحفظ بنجاح");
                return 1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
            finally
            {
                con.Dispose();
                con.Close();

                OracleConnection.ClearAllPools();

            }
        }

    }
}
