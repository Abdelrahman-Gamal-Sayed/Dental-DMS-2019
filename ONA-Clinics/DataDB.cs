using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows;


namespace ONA_Clinics
{
    class DataDB
    {



        OracleConnection con = new OracleConnection(@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
                                            (HOST=171.0.1.96)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                                            (SERVICE_NAME=ora11g)));User Id=app;Password=12369");

        OracleCommand cmd = new OracleCommand();

        OracleDataAdapter da;
        DataTable dd;

        public DataTable getonlinevalue(string crd, DateTime dat1, DateTime dat2, int flg)
        {

            //            OracleConnection con;
            //            OracleCommand cmd = new OracleCommand();
            //            OracleDataAdapter da;
            //            con = new OracleConnection(@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
            //                                            (HOST=171.0.1.96 )(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
            //                                            (SERVICE_NAME=ora11g)));User Id=app;Password=12369");

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

                da.Fill(dd); con.Close();
                return dd;
            }
            catch { return dd; }
        }

    }
}
