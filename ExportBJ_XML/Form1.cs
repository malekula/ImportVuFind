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
            label3.Text = "Начато в "+DateTime.Now.ToLongTimeString();//.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
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

                for (int i = 1; i < MaxIDMAIN_SCC; i += 1)
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
                    AddField(main, doc, "fund", GetRusFund("litres"));
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
                    if ((Convert.ToInt32(row["POLE"]) < 2258) || (Convert.ToInt32(row["POLE"]) > 2308))
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
                    AddField(main, doc, "fund", GetRusFund("period"));
                    AddField(main, doc, "allfields", allFields);
                    allFields = "";
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "period_" + row["POLE"];
                    Application.DoEvents();


                }


                sw.Stop();
                label1.Text = "Закончено. Потрачено: " + sw.Elapsed.Days.ToString() + " дней " + sw.Elapsed.Hours.ToString() + " часов " + sw.Elapsed.Minutes.ToString() + " минут " + sw.Elapsed.Seconds.ToString() + " секунд ";
                label4.Text = "Закочено в "+DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //main.Save(@"f:\bjvvv.xml");
                //Close();
            }
        }
        private string GetQuery(string baza, int count)
        {
           return "select A.*,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, "+
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
            //if (val.Contains("Tendances dans l'economie mondiale;"))
            //{
            //    field.InnerText = SecurityElement.Escape(val);
            //    //field.Value = SecurityElement.Escape(val);
            //    //val = XmlConvert.EncodeName(val);

            //}
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
            ds.Tables.Add("clarify");
            int i = da.Fill(ds, "t");
            if (i == 0) return 1;
            string CurrentIDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();
            string Level = ds.Tables["t"].Rows[0]["Level"].ToString();
            string allFields = "";
            string AF_all = "";
            bool wasTitle = false;//встречается ошибка: два заглавия в одном пине
            string description = "";//все 3хх поля
            //doc = main.CreateElement("doc");
            foreach (DataRow r in ds.Tables["t"].Rows)
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
                    case "921$a":
                        AddField(main, doc, "format", r["PLAIN"].ToString());
                        break;
                    case "922$e":
                        AddField(main, doc, "genre", r["PLAIN"].ToString());
                        break;
                    case "10$a":
                        da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                           " left join " + fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                           " where A.MNFIELD = 10 and A.MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        int j = da.Fill(ds, "clarify");
                        string add = r["PLAIN"].ToString();
                        if (j != 0)
                        {
                            add = r["PLAIN"].ToString() + " (" +ds.Tables["clarify"].Rows[0]["PLAIN"].ToString()+")";
                        }
                        AddField(main, doc, "isbn", add);
                        break;
                    case "11$a":
                        AddField(main, doc, "issn", r["PLAIN"].ToString());
                        break;
                    case "101$a":
                        da.SelectCommand.CommandText = " select NAME from " + fund + "..LIST_1 " +
                           " where ID = " + r["IDINLIST"].ToString();
                        ds.Tables["clarify"].Rows.Clear();
                        j = da.Fill(ds, "clarify");
                        if (j == 0)
                        {
                            AddField(main, doc, "language", r["PLAIN"].ToString());
                        }
                        else
                        {
                            AddField(main, doc, "language", ds.Tables["clarify"].Rows[0]["NAME"].ToString());
                        }
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
                                                       " where MNFIELD = 517 and MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
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
                    case "500$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField(main, doc, "Title_unified", AF_all);
                        break;
                    case "517$e":
                        AddField(main, doc, "Info_title_alt", r["PLAIN"].ToString());
                        break;
                    case "517$z":
                        AddField(main, doc, "Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "700$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField(main, doc, "author_variant", AF_all);//хранить но не отображать
                        break;
                    case "701$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(),"AFNAMESVAR");
                        AddField(main, doc, "Another_author_AF_all", AF_all);//хранить но не отображать
                        break;
                    case "501$a":
                        AddField(main, doc, "Another_title", r["PLAIN"].ToString());
                        break;
                    case "501$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField(main, doc, "Another_title_AF_All", AF_all);
                        break;
                    case "503$a":
                        AddField(main, doc, "Unified_Caption", r["PLAIN"].ToString());
                        break;
                    case "503$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(),"AFHEADERVAR");
                        AddField(main, doc, "Unified_Caption_AF_All", AF_all);
                        break;
                    case "700$6":
                        AddField(main, doc, "Author_another_chart", r["PLAIN"].ToString());
                        break;
                    case "702$a":
                        AddField(main, doc, "Editor", r["PLAIN"].ToString());
                        break;
                    case "702$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField(main, doc, "Editor_AF_all", AF_all);
                        break;
                    case "702$4":
                        AddField(main, doc, "Editor_role", AF_all);
                        break;
                    case "710$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(),"AFORGSVAR");
                        AddField(main, doc, "Collective_author_all", AF_all);
                        break;
                    case "710$9":
                        AddField(main, doc, "Organization_nature", r["PLAIN"].ToString());
                        break;
                    case "11$9":
                        AddField(main, doc, "Printing", r["PLAIN"].ToString());
                        break;
                    case "205$a":
                        string PublicationInfo = r["PLAIN"].ToString();

                        // 205$b
                        da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                                                       " left join "+fund+"..DATAEXTPLAIN B on A.ID = B.IDDATAEXT "+ 
                                                       " where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDDATA = "+r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
                        foreach (DataRow rr in ds.Tables["clarify"].Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        // 205$f
                        da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                                                       " left join " + fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                                       " where A.MNFIELD = 205 and A.MSFIELD = '$f' and A.IDDATA = " + r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
                        foreach (DataRow rr in ds.Tables["clarify"].Rows)
                        {
                            PublicationInfo += " / " + rr["PLAIN"].ToString();
                        }
                        // 205$g
                        da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                                                       " left join " + fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                                       " where A.MNFIELD = 205 and A.MSFIELD = '$g' and A.IDDATA = " + r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
                        foreach (DataRow rr in ds.Tables["clarify"].Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        AddField(main, doc, "Publication_info", r["PLAIN"].ToString());
                        break;
                    case "921$b":
                        AddField(main, doc, "EditionType", r["PLAIN"].ToString());
                        break;
                    case "102$a":
                        AddField(main, doc, "Country", r["PLAIN"].ToString());
                        break;
                    case "210$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(),"AFORGSVAR");
                        AddField(main, doc, "PlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2110$g":
                        AddField(main, doc, "PrintingHouse", r["PLAIN"].ToString());
                        break;
                    case "2110$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField(main, doc, "PrintingHouse_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2111$e":
                        AddField(main, doc, "GeoNamePlaceOfPublication", r["PLAIN"].ToString());
                        break;
                    case "2111$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFGEOVAR");
                        AddField(main, doc, "GeoNamePlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "10$z":
                        AddField(main, doc, "IncorrectISBN", r["PLAIN"].ToString());
                        break;
                    case "11$z":
                        AddField(main, doc, "IncorrectISSN", r["PLAIN"].ToString());
                        break;
                    case "11$y":
                        AddField(main, doc, "CanceledISSN", r["PLAIN"].ToString());
                        break;
                    case "101$b":
                        AddField(main, doc, "IntermediateTranslateLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$d":
                        AddField(main, doc, "SummaryLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$e":
                        AddField(main, doc, "TableOfContentsLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$f":
                        AddField(main, doc, "TitlePageLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$g":
                        AddField(main, doc, "BasicTitleLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$i":
                        AddField(main, doc, "AccompayingMaterialLanguage", r["PLAIN"].ToString());
                        break;
                    case "215$a":
                        AddField(main, doc, "Volume", r["PLAIN"].ToString());
                        break;
                    case "215$b":
                        AddField(main, doc, "Illustrations", r["PLAIN"].ToString());
                        break;
                    case "215$c":
                        AddField(main, doc, "Dimensions", r["PLAIN"].ToString());
                        break;
                    case "215$d":
                        AddField(main, doc, "AccompayingMaterial", r["PLAIN"].ToString());
                        break;
                    case "225$a":
                        if (r["PLAIN"].ToString() == "") break;
                        AddHierarchyFields(r["PLAIN"].ToString(), main, fund, doc, r["IDMAIN"].ToString());
                        //AddField(main, doc, "", r["PLAIN"].ToString());
                        break;
                    case "225$h":
                        AddField(main, doc, "NumberInSeries", r["PLAIN"].ToString());
                        break;
                    case "225$v":
                        AddField(main, doc, "NumberInSubseries", r["PLAIN"].ToString());
                        break;
                    case "300$a":
                    case "301$a":
                    case "316$a":
                    case "320$a":
                    case "326$a":
                    case "327$a":
                    case "336$a":
                    case "337$a":
                    case "330$a":
                        description += r["PLAIN"].ToString() + " ; " ;
                        break;
                    case "830$a":
                        AddField(main, doc, "CatalogerNote", r["PLAIN"].ToString());
                        break;
                    case "831$a":
                        AddField(main, doc, "DirectoryNote", r["PLAIN"].ToString());
                        break;
                    case "924$a":
                        AddField(main, doc, "AdditionalBibRecord", r["PLAIN"].ToString());
                        break;
                    case "899$a":
                        AddExemplarFields(r["IDDATA"].ToString(), main, fund, doc);
                        AddField(main, doc, "Location", r["PLAIN"].ToString());
                        break;
                    case "940$a":
                        AddField(main, doc, "HyperLink", r["PLAIN"].ToString());
                        AddField(main, doc, "Location", "Удалённый доступ");
                        break;
                    case "606$a"://"""""" • """"""
                        da.SelectCommand.CommandText = "select * " +
                                                       " from BJVVV..TPR_CHAIN A " +
                                                       " left join BJVVV..TPR_TES B on A.IDTES = B.ID " +
                                                       " where A.IDCHAIN = " + r["SORT"].ToString()+
                                                       " order by IDORDER";
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
                        if (j == 0) break;
                        string TPR = "";
                        foreach (DataRow rr in ds.Tables["clarify"].Rows)
                        {
                            TPR += rr["VALUE"].ToString() + "•";
                        }
                        TPR = TPR.Substring(0, TPR.Length - 1);
                        AddField(main, doc, "topic", TPR);
                        break;
                    case "3000$a":
                        AddField(main, doc, "OwnerPerson", r["PLAIN"].ToString());
                        break;
                    case "3000$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField(main, doc, "OwnerPerson_AF_All", AF_all);
                        break;
                    case "3001$a":
                        AddField(main, doc, "OwnerOrganization", r["PLAIN"].ToString());
                        break;
                    case "3001$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField(main, doc, "OwnerOrganization_AF_All", AF_all);
                        break;
                    case "3002$a":
                        AddField(main, doc, "Ownership", r["PLAIN"].ToString());
                        break;
                    case "3003$a":
                        AddField(main, doc, "OwnerExemplar", r["PLAIN"].ToString());
                        break;
                    case "3200$a":
                        AddField(main, doc, "IllustrationMaterial", r["PLAIN"].ToString());
                        break;
                }
                
            }
            AddField(main, doc, "id", fund + "_" + CurrentIDMAIN);

            string rusFund = GetRusFund(fund);

            AddField(main, doc, "fund", rusFund);
            AddField(main, doc, "allfields", allFields);
            AddField(main, doc, "Level", Level);
            if (description != "")
            {
                AddField(main, doc, "description", description);
            }
            description = "";

            allFields = "";
            doc.WriteTo(objXmlWriter);
            return 0;

        }
        string GetRusFund(string fund)
        {
            switch (fund)
            {
                case "BJVVV":
                    return "Основной фонд";
                case "REDKOSTJ":
                    return "Фонд редкой книги";
                case "BJACC":
                    return "Фонд центра Американской культуры";
                case "BJFCC":
                    return "Фонд Французского культурного центра";
                case "BJSCC":
                    return "Фонд Центра Славянских культур";
                case "BRIT_SOVET":
                    return "Фонд Британского совета";
                case "litres":
                    return "Литрес";
                case "period":
                    return "Периодика";
                case "ДЕК":
                    return "Дом еврейской книги";
            }
            return "<неизвестный фонд>";
        }
        private void AddExemplarFields(string iddata, XmlDocument main, string fund, XmlNode doc)
        {
            string Exemplar = "";
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A" +
                                " left join "+fund+"..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                               " where A.IDDATA = " + iddata;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            string IDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                string code = r["MNFIELD"].ToString()+r["MSFIELD"].ToString();
                switch (code)
                {
                    case "899$a":
                        Exemplar += "Местонахождение:"+r["PLAIN"].ToString()+"#";
                        break;
                    case "482$a":
                        Exemplar += "Приплетено к:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$b":
                        string fnd = r["PLAIN"].ToString();
                        if ((fnd == "ОФ") || (fnd == "Фонд редкой книги") || (fnd == "Фонд Редкой книги") || (fnd == "ОФ - Восток"))//надо определить что коллекция, что фонд, а что на дом.
                        {
                            AddField(main, doc, "MethodOfAccess", "В помещении библиотеки");
                            //break;//тут надо оставить только коллекции
                            Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                        } else
                        {
                            AddField(main, doc, "collection", r["PLAIN"].ToString());
                            Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                        }
                        if (fnd == "Абонемент")
                        {
                            AddField(main, doc, "MethodOfAccess", "На дом");
                            //break;//тут надо оставить только коллекции
                            Exemplar += "Доступ: Заказать через личный кабинет на дом#";
                        }
                        if ((fund == "BJFCC") || (fund == "BJACC") || (fund == "BRIT_SOVET") || (fund == "BJSCC"))
                        {
                            AddField(main, doc, "MethodOfAccess", "На дом");
                            //break;//тут надо оставить только коллекции
                            Exemplar += "Доступ: Проследовать в указанный зал для получения на дом#";
                        }

                        Exemplar += "Наименование фонда или коллекции:" + r["PLAIN"].ToString() + "#";

                        break;
                    case "899$c":
                        Exemplar += "Местонахождение стеллажа:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$d":
                        Exemplar += "Направление временного хранения:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$j":
                        Exemplar += "Расстановочный шифр:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$p":
                        Exemplar += "Инвентарный номер:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$w":
                        Exemplar += "Штрихкод:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "899$x":
                        Exemplar += "Примечание к инвентарному номеру:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "921$a":
                        Exemplar += "Носитель:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "921$c":
                        Exemplar += "Класс издания:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "922$b":
                        Exemplar += "Трофей\\Принадлежность к:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$a":
                        Exemplar += "Вид переплёта:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$b":
                        Exemplar += "Век переплёта:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$c":
                        Exemplar += "Тип переплёта:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$d":
                        Exemplar += "Материал крышек:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$e":
                        Exemplar += "Материал на крышках:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$f":
                        Exemplar += "Материал корешка:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$g":
                        Exemplar += "Бинты:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$h":
                        Exemplar += "Тиснение на крышках:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$i":
                        Exemplar += "Тиснение на корешке:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$j":
                        Exemplar += "Фурнитура:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$k":
                        Exemplar += "Жуковины:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$l":
                        Exemplar += "Форзац:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$m":
                        Exemplar += "Обрез:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$n":
                        Exemplar += "Состояние:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$o":
                        Exemplar += "Футляр:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$p":
                        Exemplar += "Тиснение на канте:" + r["PLAIN"].ToString() + "#";
                        break;
                    case "3300$r":
                        Exemplar += "Примечание о переплете:" + r["PLAIN"].ToString() + "#";
                        break;                    
                }
            }
            
            AddField(main, doc, "Exemplar", Exemplar);


            Exemplar = "";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A" +
                    " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.MNFIELD = 940 and A.MSFIELD = '$a' and A.IDMAIN = " + IDMAIN;
            ds = new DataSet();
            i = da.Fill(ds, "t");
            if (i != 0)
            {
                Exemplar += "Электронная копия: есть#";
                Exemplar += "Гиперссылка: " + ds.Tables["t"].Rows[0]["PLAIN"].ToString() + " #";
            }
            if (fund == "BJVVV")
            {
                da.SelectCommand.CommandText = " select * from BookAddInf..ScanInfo A" +
                " where A.IDBase = 1 and A.IDBook = " + IDMAIN;
            }
            if (fund == "REDKOSTJ")
            {
                da.SelectCommand.CommandText = " select * from BookAddInf..ScanInfo A" +
                " where A.IDBase = 2 and A.IDBook = " + IDMAIN;
            }
            ds = new DataSet();
            i = da.Fill(ds, "t");
            if (i == 0) return;

            Exemplar += "Авторское право: " + ((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть");
            Exemplar += "Ветхий оригинал: " + ((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет");
            Exemplar += "Наличие PDF версии: " + ((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет");
            Exemplar += "Доступ: Заказать через личный кабинет";




        }

        private void AddHierarchyFields(string ParentPIN, XmlDocument main, string fund, XmlNode doc, string CurrentPIN)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT " +
                               " where IDMAIN = " +ParentPIN;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");

            string TopHierarchyId = GetTopId(ParentPIN, fund);

            AddField(main, doc, "hierarchy_top_id", fund+"_"+TopHierarchyId);

            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                               " left join "+fund+"..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                               " where A.IDMAIN = " +TopHierarchyId +" and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");
            string hierarchy_top_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
            AddField(main, doc, "hierarchy_top_title", hierarchy_top_title);

            AddField(main, doc, "hierarchy_parent_id", fund + "_" + ParentPIN);

            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                   " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.IDMAIN = " + ParentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");
            string hierarchy_parent_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
            AddField(main, doc, "hierarchy_parent_title", hierarchy_parent_title);

            bool metka = false;
            foreach (XmlNode n in doc.ChildNodes)
            {
                if (n.Attributes["name"].Value == "is_hierarchy_id")
                {
                    metka = true;
                }
            }
            if (!metka) AddField(main, doc, "is_hierarchy_id", fund + "_" + CurrentPIN);

           



            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                   " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.IDMAIN = " + CurrentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");

            string is_hierarchy_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
            
            metka = false;
            foreach (XmlNode n in doc.ChildNodes)
            {
                if (n.Attributes["name"].Value == "is_hierarchy_id")
                {
                    metka = true;
                }
            }
            if (!metka) AddField(main, doc, "is_hierarchy_title", is_hierarchy_title);

        }

        private string GetTopId(string ParentPIN, string fund)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT " +
                               " where MNFIELD = 225 and MSFIELD = '$a' and IDMAIN = " + ParentPIN;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            if (i == 0) return ParentPIN;
            ParentPIN = ds.Tables["t"].Rows[0]["SORT"].ToString();
            return GetTopId(ParentPIN, fund);
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

        public string GetAFAll(string fund, string AFLinkId, string AFTable)
        {
            //NAMES: (1) Персоналии
            //ORGS: (2) Организации
            //HEADER: (3) Унифицированное заглавие
            //DEL: (5) Источник списания
            //GEO: (6) Географическое название

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = " select PLAIN from " + fund + ".."+AFTable+" A " +
                               " where IDAF = " + AFLinkId;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            string Another_author_AF_all = "";
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                Another_author_AF_all += r["PLAIN"].ToString() + " ";
            }
            return Another_author_AF_all;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = new SqlCommand();
            //da.SelectCommand.Connection = new SqlConnection();
            //da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            //da.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 230";
            //DataSet ds = new DataSet();
            //int i = da.Fill(ds, "t");
            //da.SelectCommand.CommandText = "select * from BJVVV..DATAEXTPLAIN where ID = 3";
            //i = da.Fill(ds, "t");

            da = new SqlDataAdapter();
            da.InsertCommand = new SqlCommand();
            da.InsertCommand.Connection = new SqlConnection();
            da.InsertCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.InsertCommand.Connection.Open();
            StreamReader sr = new StreamReader(@"f:\Lib_100S25642.txt");
            string account;
            while (sr.Peek() >= 0)
            {
                account = sr.ReadLine();
                da.InsertCommand.Parameters.Clear();
                da.InsertCommand.Parameters.AddWithValue("login",account.Split(',')[0]);
                da.InsertCommand.Parameters.AddWithValue("pwd",account.Split(',')[1]);
                da.InsertCommand.CommandText = "insert into LITRES..ACCOUNTS (LRLOGIN,LRPWD,CREATED) values (@login, @pwd, getdate())";
                da.InsertCommand.ExecuteNonQuery();
            }
            da.InsertCommand.Connection.Close();

            



        }
    }
}
