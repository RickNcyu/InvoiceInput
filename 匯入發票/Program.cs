using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
namespace 匯入發票
{
    class Program
    {
        static int Main(string[] args)
        {

            /////////////////////////////////////////////////////////////
            string connectionString = @"Persist Security Info=False;Integrated Security=true;  Server=localhost\SQLEXPRESS;Initial Catalog=TEST;User ID=;Password=;";
            /////////////////////////////////////////////////////////////


            string Title = null;
            string Number = null;
            SqlConnection connet = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM StatSets", connet);
            connet.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Title = reader.GetString(0);
                Number = reader.GetString(3);
            }


            connet.Close();
            //Console.WriteLine(Number);

            /////////////////////////////////////////////////////////////
            string fileName = @"./invnoapply.csv";
            /////////////////////////////////////////////////////////////

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            /////////////////////////////////////////////////////////////
            string outfileName = @"./Invoice.txt";
            /////////////////////////////////////////////////////////////

            FileStream fs2 = new FileStream(outfileName, FileMode.Create, FileAccess.Write);

            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                using (StreamWriter sw = new StreamWriter(fs2, Encoding.Default))
                {
                    string temp = "";
                    string[] arrTemp;
                    int row = 0;
                    string[] yearandmon;
                    string[] year2andmon;
                    string con, con2;
                    string output;
                    int yeartoin, yeartoin2;

                    while ((temp = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(Number);

                        if (row > 0)
                        {
                            arrTemp = temp.Split(',', '~');
                            if (arrTemp[0] != Number) throw new System.ArgumentException("請檢查第" + row.ToString() + "筆客戶編號");
                            //Console.WriteLine(arrTemp[0]);
                            //Console.WriteLine(arrTemp[0]+" "+ arrTemp[1] + " "+arrTemp[2]+ " "+ arrTemp[3]+" "+ arrTemp[4] +" "+ arrTemp[5] +" "+ arrTemp[6]+" "+arrTemp[7]);
                            //Console.WriteLine(arrTemp[3] + " " + arrTemp[4]);
                            yearandmon = arrTemp[3].Split('/');
                            year2andmon = arrTemp[4].Split('/');
                            //Console.WriteLine(yearandmon[0] + year2andmon[0]);
                            //Console.WriteLine(yearandmon[1] + year2andmon[1]);
                            yeartoin = Int32.Parse(yearandmon[0]) + 1911;
                            con = yeartoin.ToString() + yearandmon[1];
                            //Console.WriteLine(con);
                            yeartoin2 = Int32.Parse(year2andmon[0]) + 1911;
                            con2 = yeartoin2.ToString() + year2andmon[1];
                            //Console.WriteLine(con2);
                            output = Title + " " + con + con2 + " " + arrTemp[5] + " " + arrTemp[6] + " " + arrTemp[7] + " " + "N";

                            Console.WriteLine(output);

                            sw.WriteLine(output);
                        }
                        row++;
                        //Console.WriteLine(row);
                    }
                    Console.WriteLine("請再次確認即將匯入的資料是否正確完畢後輸入Y/N");

                }

            }



            connet.Close();

            string ret;
            ret = Console.ReadLine();
            if (ret == "Y" || ret == "y") return 1;
            else return 0;
        }
    }
}
