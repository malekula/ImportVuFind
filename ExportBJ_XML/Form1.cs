using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Xml.Linq;
using System.Diagnostics;

namespace ExportBJ_XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            label3.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            //XmlWriter objXmlWriter = XmlTextWriter.Create(new BufferedStream(new FileStream(@"F:\bjvvv.xml", FileMode.Create, System.Security.AccessControl.FileSystemRights.Write, FileShare.None, 1024, FileOptions.SequentialScan)), new XmlWriterSettings { Encoding = Encoding.Unicode, Indent = true, CloseOutput = true });
            XmlWriter objXmlWriter = XmlTextWriter.Create(@"F:\bjvvv.xml");
            using (objXmlWriter)
            {

                XmlDocument main = new XmlDocument();
                XmlNode docNode = main.CreateXmlDeclaration("1.0", "UTF-8", null);
                main.AppendChild(docNode);
                docNode.WriteTo(objXmlWriter);

                XmlNode root = main.CreateElement("add");
                main.AppendChild(root);
                objXmlWriter.WriteStartElement("add");
                //root.WriteTo(objXmlWriter);

                XmlNode doc = main.CreateElement("doc");
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BJVVV/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_BJVVV = GetMaxIDMAIN("BJVVV");
                for (int i = 1; i < MaxIDMAIN_BJVVV; i += 1)
                {
                    string q = GetQuery("BJVVV", i);
                    int check = CreateBJDocs("BJVVV", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");

                    label2.Text = "BJVVV_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////REDKOSTJ/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_REDKOSTJ = GetMaxIDMAIN("REDKOSTJ");
                for (int i = 1; i < MaxIDMAIN_REDKOSTJ; i += 1)
                {
                    string q = GetQuery("REDKOSTJ", i);
                    int check = CreateBJDocs("REDKOSTJ", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "REDKOSTJ_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BJACC/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_ACC = GetMaxIDMAIN("BJACC");

                for (int i = 1; i < MaxIDMAIN_ACC; i += 1)
                {
                    string q = GetQuery("BJACC", i);
                    int check = CreateBJDocs("BJACC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "BJACC_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BJFCC/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_FCC = GetMaxIDMAIN("BJFCC");

                for (int i = 1; i < MaxIDMAIN_FCC; i += 1)
                {
                    string q = GetQuery("BJFCC", i);
                    int check = CreateBJDocs("BJFCC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "BJFCC_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BRIT_SOVET/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_BRIT = GetMaxIDMAIN("BRIT_SOVET");

                for (int i = 1; i < MaxIDMAIN_BRIT; i += 1)
                {
                    string q = GetQuery("BRIT_SOVET", i);
                    int check = CreateBJDocs("BRIT_SOVET", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "BRIT_SOVET_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BJSCC/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_SCC = GetMaxIDMAIN("BJSCC");

                for (int i = 1; i < MaxIDMAIN_SCC; i+=1)
                {
                    string q = GetQuery("BJSCC", i);
                    int check = CreateBJDocs("BJSCC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "BJSCC_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////*/
                //////////////////////////////////LITRES/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                string litresFile = @"f:\csv_litres_exp_873.csv";

                DataTable Litres = ConvertCSVtoDataTable(litresFile);
                string allFields = "";

                foreach (DataRow row in Litres.Rows)
                {

                    foreach (DataColumn col in Litres.Columns)
                    {
                        switch (col.ColumnName)
                        {
                            case "ID книги":
                                

                                //allFields = allFields + " "+row[col.ColumnName].ToString();
                                break;
                            case "Название":
                                AddField(main, doc, "title", row[col.ColumnName].ToString());
                                AddField(main, doc, "title_short", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();
                                break;
                            case "Авторы":
                                AddField(main, doc, "author", row[col.ColumnName].ToString());
                                AddField(main, doc, "author_sort", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();

                                break;
                            case "ISBN":
                                AddField(main, doc, "isbn", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();

                                break;
                            case "Издательство":
                                AddField(main, doc, "publisher", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();
                                break;
                            case "Год":
                                AddField(main, doc, "publishDate", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();
                                break;
                            case "Жанры":
                                AddField(main, doc, "genre", row[col.ColumnName].ToString());
                                allFields = allFields + " " + row[col.ColumnName].ToString();
                                break;

                        }
                    }
                    AddField(main, doc, "id", "litres_" + row["ID книги"].ToString());
                    AddField(main, doc, "fund", "litres");
                    AddField(main, doc, "allfields", allFields);
                    allFields = "";
                    
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "litres_" + row["ID книги"].ToString();
                    Application.DoEvents();

                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////PERIOD/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection();
                da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=PERIOD;Persist Security Info=True;User ID=sasha;Password=Corpse536";
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 120";
                DataSet ds = new DataSet();
                int cnt = da.Fill(ds, "t");
                DataTable val = new DataTable();
                foreach (DataRow row in ds.Tables["t"].Rows)
                {
                    allFields = "";
                    string pin = row["POLE"].ToString();
                    if (pin == "")
                        continue;
                    if ((Convert.ToInt32(row["POLE"]) < 2208) || (Convert.ToInt32(row["POLE"]) > 2308))
                    {
                        //continue;
                    }
                    //title
                    da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 121 and VVERH = "+row["IDZ"].ToString();
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        AddField(main, doc, "title", val.Rows[0]["POLE"].ToString());
                        AddField(main, doc, "title_short", val.Rows[0]["POLE"].ToString());
                    }
                    //вид издания
                    da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 124 and VVERH = " + row["IDZ"].ToString();
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        AddField(main, doc, "period_ModeOfPublication", val.Rows[0]["POLE"].ToString());//добавить в индекс
                    }
                    //язык
                    da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 128 and VVERH = " + row["IDZ"];
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        AddField(main, doc, "period_Language", val.Rows[0]["POLE"].ToString());//добавить в индекс
                    }
                    //периодичность
                    da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 130 and VVERH = " + row["IDZ"];
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        AddField(main, doc, "period_Periodicity", val.Rows[0]["POLE"].ToString());//добавить в индекс
                    }
                    //года 
                    da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 131 and VVERH = " + row["IDZ"];
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        string result = "";
                        foreach (DataRow r in val.Rows)
                        {
                            result = result + r["POLE"].ToString() + "; ";

                        }
                        AddField(main, doc, "period_Years", result);//добавить в индекс
                    }
                    //гиперссылки 
                    da.SelectCommand.CommandText = "select E.POLE " +
                                                   "  from PERIOD..PI A "+
                                                   "  left join PERIOD..PI B on A.IDZ = B.VVERH "+
                                                   "  left join PERIOD..PI C on B.IDZ = C.VVERH "+
                                                   "  left join PERIOD..PI D on C.IDZ = D.VVERH "+
                                                   "  left join PERIOD..PI E on C.IDZ = E.VVERH "+
                                                   "  where A.IDF = 120 and A.POLE = '"+row["POLE"]+"' "+
                                                   " and D.IDF = 363  "+
                                                   " and D.POLE = 'e-book' "+
                                                   "  and E.IDF = 219";
                    val = new DataTable();
                    cnt = da.Fill(val);
                    if (cnt != 0)
                    {
                        string result = "";
                        foreach (DataRow r in val.Rows)
                        {
                            result = result + r["POLE"].ToString() + "; ";
                        }
                        AddField(main, doc, "period_HyperLink", result);//добавить в индекс
                    }



                    AddField(main, doc, "id", "period_" + row["POLE"]);
                    AddField(main, doc, "fund", "period");
                    AddField(main, doc, "allfields", allFields);
                    allFields = "";
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "period_" + row["POLE"];
                    Application.DoEvents();


                }


                sw.Stop();
                label1.Text = sw.Elapsed.Minutes.ToString();
                label4.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //main.Save(@"f:\bjvvv.xml");
                //Close();
            }
        }
        private string GetQuery(string baza, int count)
        {
           return "select A.IDMAIN,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, "+
                   " case when C.IDLEVEL = 1 then 'Монография'  " +
                   "  when C.IDLEVEL = -100 then 'Коллекция'  " +
                   "  when C.IDLEVEL = -2 then 'Сводный уровень многотомного издания'  " +
                   "  when C.IDLEVEL = 2 then 'Том (выпуск) многотомного издания'  " +
                   "  when C.IDLEVEL = -3 then 'Сводный уровень сериального издания'  " +
                   "  when C.IDLEVEL = -33 then 'Сводный уровень подсерии, входящей в серию'  " +
                   "  when C.IDLEVEL = 3 then 'Выпуск сериального издания'  " +
                   "  when C.IDLEVEL = -4 then 'Сводный уровень сборника'  " +
                   "  when C.IDLEVEL = 4 then 'Часть сборника'  " +
                   "  when C.IDLEVEL = -5 then 'Сводный уровень продолжающегося издания'  " +
                   "  when C.IDLEVEL = 5 then 'Выпуск продолжающегося издания' else '' end level " +
                   " from " + baza + "..DATAEXT A" +
                   " left join "+baza+"..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                   " left join " +baza+"..MAIN C on A.IDMAIN = C.ID "+
                   //" where A.IDMAIN between " + count + " and " + (count + 999).ToString() +//типа для ускорения, но можно явно пин указать же. не помню.
                   " where A.IDMAIN = " + count + 
                //" and exists (select 1 from BRIT_SOVET..DATAEXT C where A.IDMAIN = C.IDMAIN and C.MNFIELD = 899 and C.MSFIELD = '$p')" +
                   " order by IDMAIN";
        }
        private int GetMaxIDMAIN(string p)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = "select max(ID) from "+p+"..MAIN";
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            return int.Parse(ds.Tables["t"].Rows[0][0].ToString());
        }
        public void AddField(XmlDocument main,XmlNode doc_, string name,string val)
        {
            
            XmlNode field = main.CreateElement("field");
            XmlAttribute att = main.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            //field.InnerText = SecurityElement.Escape(val);
            if (val.Contains("Tendances dans l'economie mondiale;"))
            {
                field.InnerText = SecurityElement.Escape(val);
                //field.Value = SecurityElement.Escape(val);
                //val = XmlConvert.EncodeName(val);

            }
            //XElement xe = new XElement("dummy", val);
            //field.InnerText = xe.Value;
            //val = val.Replace("\\0", "").Replace("/","");
            field.InnerText = SecurityElement.Escape(val);
            //field.Value = SecurityElement.Escape(val);
            //val = XmlConvert.EncodeName(val);
            val = XmlCharacterWhitelist(val);
            field.InnerText = val;
            //XNode xn = 

            //field.InnerText = val.Replace("&", "").Replace("\v", "").Replace("#","");
            //XmlElement xe = new XmlElement();
            
            doc_.AppendChild(field);
        }
        public static string XmlCharacterWhitelist(string in_string)
        {
            if (in_string == null) return null;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            for (int i = 0; i < in_string.Length; i++)
            {
                ch = in_string[i];
                if ((ch >= 0x0020 && ch <= 0xD7FF) ||
                    (ch >= 0xE000 && ch <= 0xFFFD) ||
                    ch == 0x0009 ||
                    ch == 0x000A ||
                    ch == 0x000D)
                {
                    sbOutput.Append(ch);
                }
            }
            return sbOutput.ToString();
        }
        public int CreateBJDocs(string fund, string query,XmlDocument main,XmlNode doc,XmlNode root, XmlWriter objXmlWriter)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = query;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            if (i == 0) return 1;
            string CurrentIDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();
            string Level = ds.Tables["t"].Rows[0]["Level"].ToString();
            string allFields = "";
            bool wasTitle = false;//встречается ошибка: два заглавия в одном пине
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                if (r["IDMAIN"].ToString() != CurrentIDMAIN)
                {
                    AddField(main, doc, "id", fund + "_" + CurrentIDMAIN);
                    AddField(main, doc, "fund", fund);
                    AddField(main, doc, "allfields", allFields);
                    AddField(main, doc, "Level", r["level"].ToString());
                    allFields = "";
                    //root.AppendChild(doc);
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    CurrentIDMAIN = r["IDMAIN"].ToString();
                }
                //else
                {
                    allFields += " " + r["PLAIN"].ToString();
                    switch (r["code"].ToString())
                    {
                        //=======================================================================Родные поля вуфайнд=======================
                        case "200$a":
                            if (wasTitle) break;
                            AddField(main, doc, "title", r["PLAIN"].ToString());
                            AddField(main, doc, "title_short", r["PLAIN"].ToString());
                            wasTitle = true;
                            break;
                        case "700$a":
                            AddField(main, doc, "author", r["PLAIN"].ToString());
                            AddField(main, doc, "author_sort", r["SORT"].ToString());
                            //забрать все варианты написания автора из авторитетного файла и вставить в скрытое, но поисковое поле
                            break;
                        case "701$a":
                            AddField(main, doc, "author2", r["PLAIN"].ToString());
                            break;
                        case "710$a":
                            AddField(main, doc, "author_corporate", r["PLAIN"].ToString());
                            break;
                        case "710$4":
                            AddField(main, doc, "author_corporate_role", r["PLAIN"].ToString());
                            break;
                        case "700$4":
                            AddField(main, doc, "author_role", r["PLAIN"].ToString());
                            break;
                        case "701$4":
                            AddField(main, doc, "author2_role", r["PLAIN"].ToString());
                            break;
                        case "330$a":
                            AddField(main, doc, "description", r["PLAIN"].ToString());
                            break;
                        case "205$a":
                            AddField(main, doc, "edition", r["PLAIN"].ToString());
                            break;
                        case "921$a":
                            AddField(main, doc, "format", r["PLAIN"].ToString());
                            break;
                        case "922$e":
                            AddField(main, doc, "genre", r["PLAIN"].ToString());
                            break;
                        case "10$a":
                            AddField(main, doc, "isbn", r["PLAIN"].ToString());
                            break;
                        case "11$a":
                            AddField(main, doc, "issn", r["PLAIN"].ToString());
                            break;
                        case "101$a":
                            AddField(main, doc, "language", r["PLAIN"].ToString());
                            break;
                        case "2100$d":
                            AddField(main, doc, "publishDate", r["PLAIN"].ToString());
                            break;
                        case "210$c":
                            AddField(main, doc, "publisher", r["PLAIN"].ToString());
                            break;
                        case "517$a":
                            da.SelectCommand.CommandText = " select B.PLAIN from " + fund + "..DATAEXT A "+
                                                           " left join "+fund+"..DATAEXTPLAIN B on A.ID = B.IDDATAEXT "+ 
                                                           " where MNFIELD = 517 and MSFIELD = '$b' and IDDATA = " + r["IDDATA"].ToString();
                            int j = da.Fill(ds, "clarify");
                            if (j != 0)
                            {
                                AddField(main, doc, "title_alt", "(" + ds.Tables["clarify"].Rows[0]["PLAIN"].ToString() + ")" + r["PLAIN"].ToString());
                            }
                            else
                            {
                                AddField(main, doc, "title_alt", r["PLAIN"].ToString());
                            }
                            //нужно специальным образом обрабатывать
                            break;
                        //=======================================================================добавленные в индекс=======================
                        case "210$a":
                            AddField(main, doc, "PlaceOfPublication", r["PLAIN"].ToString());
                            break;
                        case "215$a":
                            AddField(main, doc, "Pagination", r["PLAIN"].ToString());
                            break;
                        case "200$6":
                            AddField(main, doc, "Title_another_chart", r["PLAIN"].ToString());
                            break;
                        case "200$b":
                            AddField(main, doc, "Title_same_author", r["PLAIN"].ToString());
                            break;
                        case "200$d":
                            AddField(main, doc, "Parallel_title", r["PLAIN"].ToString());
                            break;
                        case "200$e":
                            AddField(main, doc, "Info_pertaining_title", r["PLAIN"].ToString());
                            break;
                        case "200$f":
                            AddField(main, doc, "Responsibility_statement", r["PLAIN"].ToString());
                            break;
                        case "200$h":
                            AddField(main, doc, "Part_number", r["PLAIN"].ToString());
                            break;
                        case "200$i":
                            AddField(main, doc, "Part_title", r["PLAIN"].ToString());
                            break;
                        case "200$z":
                            AddField(main, doc, "Language_title_alt", r["PLAIN"].ToString());
                            break;
                        case "500$a":
                            AddField(main, doc, "Title_unified", r["PLAIN"].ToString());
                            break;
                        case "517$e":
                            AddField(main, doc, "Info_title_alt", r["PLAIN"].ToString());
                            break;
                        case "517$z":
                            AddField(main, doc, "Language_title_alt", r["PLAIN"].ToString());
                            break;
                        case "700$3":
                            da.SelectCommand.CommandText = " select PLAIN from " + fund + "..AFNAMESVAR A " +
                                                           " where IDAF " + r["AFLINKID"].ToString();
                            ds.Tables["clarify"].Rows.Clear();
                            da.Fill(ds, "clarify");
                            string allVariantsOfAuthor = "";
                            foreach (DataRow rr in ds.Tables["clarify"].Rows)
                            {
                                allVariantsOfAuthor += rr["PLAIN"].ToString() + " ";
                            }
                            AddField(main, doc, "Author_AF_all", allVariantsOfAuthor);//хранить но не отображать
                            break;
                        //case "$":
                        //    AddField(main, doc, "", r["PLAIN"].ToString());
                        //    break;
                        //case "$":
                        //    AddField(main, doc, "", r["PLAIN"].ToString());
                        //    break;

                    }
                }
            }
            AddField(main, doc, "id", fund + "_" + CurrentIDMAIN);
            AddField(main, doc, "fund", fund);
            AddField(main, doc, "allfields", allFields);
            AddField(main, doc, "Level", Level);
            //root.AppendChild(doc);
            doc.WriteTo(objXmlWriter);
            return 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "Tendances dans l'economie mondiale; &#x0; 23/ Conseil scientifique hongrois d'economie mondaile";
            XElement x = new XElement("bb",text);
            string xmlText = SecurityElement.Escape(text);
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath, Encoding.Default))
            {
                
                string[] headers = sr.ReadLine().Split(';');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Trim('"'));
                }
                int g = 1;
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    for (int i = 0; i < rows.Count(); i++ )
                    {
                        rows[i] = rows[i].Trim('"');
                    }

                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                    if (g++ > 1000) break;
                }

            }


            return dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 230";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            da.SelectCommand.CommandText = "select * from BJVVV..DATAEXTPLAIN where ID = 3";
            i = da.Fill(ds, "t");
            

        }
    }
}
