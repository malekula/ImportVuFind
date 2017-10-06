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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ExportBJ_XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Stopwatch sw;
        private void button1_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            label3.Text = "Начато в "+DateTime.Now.ToLongTimeString();//.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            //XmlWriter objXmlWriter = XmlTextWriter.Create(new BufferedStream(new FileStream(@"F:\bjvvv.xml", FileMode.Create, System.Security.AccessControl.FileSystemRights.Write, FileShare.None, 1024, FileOptions.SequentialScan)), new XmlWriterSettings { Encoding = Encoding.Unicode, Indent = true, CloseOutput = true });
            XmlWriter objXmlWriter = XmlTextWriter.Create(@"F:\bjvvv_current.xml");
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
                int step = 100;
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////BJVVV/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_BJVVV = GetMaxIDMAIN("BJVVV");
                for (int i = 1; i < MaxIDMAIN_BJVVV; i += step)
                {
                    string q = GetQuery("BJVVV", i);
                    int check = CreateBJDoc("BJVVV", q, main, doc, root, objXmlWriter);
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
                for (int i = 1; i < MaxIDMAIN_REDKOSTJ; i += step)
                {
                    string q = GetQuery("REDKOSTJ", i);
                    int check = CreateBJDoc("REDKOSTJ", q, main, doc, root, objXmlWriter);
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

                for (int i = 1; i < MaxIDMAIN_ACC; i += step)
                {
                    string q = GetQuery("BJACC", i);
                    int check = CreateBJDoc("BJACC", q, main, doc, root, objXmlWriter);
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

                for (int i = 1; i < MaxIDMAIN_FCC; i += step)
                {
                    string q = GetQuery("BJFCC", i);
                    int check = CreateBJDoc("BJFCC", q, main, doc, root, objXmlWriter);
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

                for (int i = 1; i < MaxIDMAIN_BRIT; i += step)
                {
                    string q = GetQuery("BRIT_SOVET", i);
                    int check = CreateBJDoc("BRIT_SOVET", q, main, doc, root, objXmlWriter);
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

                for (int i = 1; i < MaxIDMAIN_SCC; i += step)
                {
                    string q = GetQuery("BJSCC", i);
                    int check = CreateBJDoc("BJSCC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "BJSCC_" + i;
                    Application.DoEvents();

                }
                /////////////////////////////////////////////////////////////////////////////////////////////*/
                //////////////////////////////////LITRES/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                XmlDocument litres = new XmlDocument();
                //litres.Load(@"f:\api_litres.xml");
                //int y = litres.ChildNodes.Count;
#region PERIOD
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////PERIOD/////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                string allFields = "";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection();
                da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=PERIOD;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
                    if ((Convert.ToInt32(row["POLE"]) < 2305) || (Convert.ToInt32(row["POLE"]) > 2308))
                    {
                        continue;
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
#endregion
                /////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////Pearson////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////

                string Pearson = File.ReadAllText(@"f:/pearson.json");

                JArray desPearson = (JArray)JsonConvert.DeserializeObject(Pearson);

                string tmp = desPearson.First["licensePackage"].ToString();
                tmp = desPearson.First["catalog"]["options"]["Supported platforms"].ToString();
                cnt = 1;
                foreach (JToken token in desPearson)
                {
                    AddField(main, doc, "id", token["id"].ToString());
                    AddField(main, doc, "title", token["catalog"]["title"]["default"].ToString());
                    AddField(main, doc, "title_short", token["catalog"]["title"]["default"].ToString());
                    AddField(main, doc, "author", token["catalog"]["options"]["Authors"].ToString());
                    AddField(main, doc, "Country", token["catalog"]["options"]["Country of publication"].ToString());
                    AddField(main, doc, "publisher", token["catalog"]["options"]["Publisher"].ToString());
                    AddField(main, doc, "publishDate", token["catalog"]["options"]["Publishing date"].ToString().Split('.')[2]);
                    AddField(main, doc, "isbn", token["catalog"]["options"]["ISBN"].ToString());
                    AddField(main, doc, "Volume", token["catalog"]["options"]["Number of pages"].ToString());
                    AddField(main, doc, "description", token["catalog"]["options"]["Desk"].ToString() + " ; " +
                                                  token["catalog"]["description"]["default"].ToString());
                    AddField(main, doc, "genre", token["catalog"]["options"]["Subject"].ToString());
                    AddField(main, doc, "topic", token["catalog"]["options"]["Catalogue section"].ToString());
                    AddField(main, doc, "collection", token["catalog"]["options"]["Collection"].ToString());
                    AddField(main, doc, "language", token["catalog"]["options"]["Language"].ToString());


                    //описание экземпляра Пирсон
                    StringBuilder sb = new StringBuilder();
                    StringWriter strwriter = new StringWriter(sb);
                    JsonWriter writer = new JsonTextWriter(strwriter);

                    writer.WriteStartObject();
                    writer.WritePropertyName("1");
                    writer.WriteStartObject();

                    writer.WritePropertyName("exemplar_carrier");
                    writer.WriteValue("Электронная книга");
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue("Для прочтения онлайн необходимо перейти по ссылке");
                    writer.WritePropertyName("exemplar_hyperlink");
                    writer.WriteValue("http://не сформирован");
                    writer.WritePropertyName("exemplar_copyright");
                    writer.WriteValue("Да");
                    
                    writer.WriteEndObject();
                    writer.WriteEndObject();


                    AddField(main, doc, "MethodOfAccess", "Удалённый доступ");
                    AddField(main, doc, "Exemplar", sb.ToString());
                    AddField(main, doc, "id", "Pearson_"+token["id"].ToString());
                    AddField(main, doc, "HyperLink", "http://не сформирован");
                    AddField(main, doc, "fund", "Pearson");


                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    label2.Text = "Pearson_" + cnt++;
                    Application.DoEvents();
                }




                /////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////
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
                   " order by IDMAIN, IDDATA";
        }
        private int GetMaxIDMAIN(string p)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
            field.InnerText = SecurityElement.Escape(val);
            val = XmlCharacterWhitelist(val);
            field.InnerText = val;           
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
        public int CreateBJDoc(string fund, string query,XmlDocument main,XmlNode doc,XmlNode root, XmlWriter objXmlWriter)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
                        if (r["PLAIN"].ToString() == "-1") break;
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
                    case "899$a":
                        
                        AddField(main, doc, "Location", r["PLAIN"].ToString());
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

            AddExemplarFields(CurrentIDMAIN, main, fund, doc);
            
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

        private void AddExemplarFields(string idmain, XmlDocument main, string fund, XmlNode doc)
        {

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText =  " select * from " + fund + "..DATAEXT A" +
                                            " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                                            " where A.IDMAIN = " + idmain + " and A.MNFIELD = 899 and A.MSFIELD = '$p' "+
                                            " and not exists (select 1 from BJVVV..DATAEXT C where C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' and C.SORT = 'Списано')";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");

            if (i == 0) return;
            string IDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            
            //{"1":
            //    {
            //     "exemplar_location":"Абонемент",
            //     "exemplar_collection":"ОФ",
            //     "exemplar_placing_cipher":"08.B 4650",
            //     "exemplar_carrier":"Бумага",
            //     "exemplar_inventory_number":"2494125",
            //     "exemplar_class_edition":"Для выдачи"
            //    },
            // "2":
            //    {
            //        "exemplar_location":"Абонемент",
            //        "exemplar_collection":"ОФ",
            //        "exemplar_placing_cipher":"08.B 4651",
            //        "exemplar_carrier":"Бумага",
            //        "exemplar_inventory_number":"2494126",
            //        "exemplar_class_edition":"Для выдачи"
            //    }
            //}


            writer.WriteStartObject();

            ds.Tables.Add("exemplar");
            int cnt = 1;
            //ser.Serialize(
            foreach(DataRow iddata in ds.Tables["t"].Rows)
            {
                da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A" +
                                                " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                                                " where A.IDDATA = " + iddata["IDDATA"];// + 
                ds.Tables["exemplar"].Rows.Clear();
                ds.Tables["exemplar"].Clear();
                i = da.Fill(ds, "exemplar");

                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();


                
                foreach (DataRow r in ds.Tables["exemplar"].Rows)
                {
                    string code = r["MNFIELD"].ToString()+r["MSFIELD"].ToString();
                    switch (code)
                    {
                        case "899$a":
                            writer.WritePropertyName("exemplar_location");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "482$a":
                            //Exemplar += "Приплетено к:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_interlaced_to");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$b"://тут надо оставить только коллекции
                            string fnd = r["PLAIN"].ToString();
                            if ((fnd == "ОФ") || (fnd == "Фонд редкой книги") || (fnd == "Фонд Редкой книги") || (fnd == "ОФ - Восток"))//надо определить что коллекция, что фонд, а что на дом.
                            {
                                AddField(main, doc, "MethodOfAccess", "В помещении библиотеки");
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            else
                            {
                                AddField(main, doc, "collection", r["PLAIN"].ToString());
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            if (fnd == "Абонемент")
                            {
                                AddField(main, doc, "MethodOfAccess", "На дом");
                                //Exemplar += "Доступ: Заказать через личный кабинет на дом#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет на дом");
                            }
                            if ((fund == "BJFCC") || (fund == "BJACC") || (fund == "BRIT_SOVET") || (fund == "BJSCC"))
                            {
                                AddField(main, doc, "MethodOfAccess", "На дом");
                                //break;//тут надо оставить только коллекции
                                //Exemplar += "Доступ: Проследовать в указанный зал для получения на дом#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Проследовать в указанный зал для получения на дом");
                            }

                            //Exemplar += "Наименование фонда или коллекции:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_collection");
                            writer.WriteValue(r["PLAIN"].ToString());

                            break;
                        case "899$c":
                            //Exemplar += "Местонахождение стеллажа:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_rack_location");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$d":
                            //Exemplar += "Направление временного хранения:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_direction_temporary_storage");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$j":
                            //Exemplar += "Расстановочный шифр:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_placing_cipher");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$p":
                            //Exemplar += "Инвентарный номер:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inventory_number");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$w":
                            //Exemplar += "Штрихкод:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_barcode");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$x":
                            //Exemplar += "Примечание к инвентарному номеру:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inv_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$a":
                            //Exemplar += "Носитель:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_carrier");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$c":
                            //Exemplar += "Класс издания:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_class_edition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "922$b":
                            //Exemplar += "Трофей\\Принадлежность к:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_trophy");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$a":
                            //Exemplar += "Вид переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_kind");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$b":
                            //Exemplar += "Век переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_age");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$c":
                            //Exemplar += "Тип переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_type");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$d":
                            //Exemplar += "Материал крышек:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cover_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$e":
                            //Exemplar += "Материал на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_material_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$f":
                            //Exemplar += "Материал корешка:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_spine_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$g":
                            //Exemplar += "Бинты:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bandages");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$h":
                            //Exemplar += "Тиснение на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_stamping_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$i":
                            //Exemplar += "Тиснение на корешке:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_spine");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$j":
                            //Exemplar += "Фурнитура:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_fittings");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$k":
                            //Exemplar += "Жуковины:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bugs");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$l":
                            //Exemplar += "Форзац:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_forth");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$m":
                            //Exemplar += "Обрез:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cutoff");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$n":
                            //Exemplar += "Состояние:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_condition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$o":
                            //Exemplar += "Футляр:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_case");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$p":
                            //Exemplar += "Тиснение на канте:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_edge");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$r":
                            //Exemplar += "Примечание о переплете:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;                    
                    }

                }
                //Exemplar += "exemplar_id:" + ds.Tables["t"].Rows[0]["IDDATA"].ToString() + "#";
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue(iddata["IDDATA"].ToString());
                writer.WriteEndObject();
            }

            //смотрим есть ли гиперссылка
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A" +
                    " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.MNFIELD = 940 and A.MSFIELD = '$a' and A.IDMAIN = " + IDMAIN;
            ds = new DataSet();
            i = da.Fill(ds, "t");
            if (i != 0)//если есть - вставляем отдельным экземпляром.
            {
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();


                //Exemplar += "Электронная копия: есть#";
                writer.WritePropertyName("exemplar_electronic_copy");
                writer.WriteValue("да");
                //Exemplar += "Гиперссылка: " + ds.Tables["t"].Rows[0]["PLAIN"].ToString() + " #";
                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue(ds.Tables["t"].Rows[0]["PLAIN"].ToString());
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
                if (i != 0)
                {
                    //Exemplar += "Авторское право: " + ((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть");
                    writer.WritePropertyName("exemplar_copyright");
                    writer.WriteValue(((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть"));
                    //Exemplar += "Ветхий оригинал: " + ((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_old_original");
                    writer.WriteValue(((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Наличие PDF версии: " + ((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_PDF_exists");
                    writer.WriteValue(((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Доступ: Заказать через личный кабинет";
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue(
                        (ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? 
                        "Для прочтения онлайн необходимо перейти по ссылке" :
                        "Для прочтения онлайн необходимо положить в корзину и заказать через личный кабинет");
                    writer.WritePropertyName("exemplar_carrier");
                    writer.WriteValue("Электронная книга");

                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
            writer.Flush();
            writer.Close();

            AddField(main, doc, "MethodOfAccess", "Удалённый доступ");

            AddField(main, doc, "Exemplar", sb.ToString());


        }

        private void AddHierarchyFields(string ParentPIN, XmlDocument main, string fund, XmlNode doc, string CurrentPIN)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
            if (i != 0)
            {
                string hierarchy_top_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
                AddField(main, doc, "hierarchy_top_title", hierarchy_top_title);
            }

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
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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
            da.InsertCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sw == null) return;
            Application.DoEvents();
            label1.Text = "Потрачено: " + sw.Elapsed.Days.ToString() + " дней " + sw.Elapsed.Hours.ToString() + " часов " + sw.Elapsed.Minutes.ToString() + " минут " + sw.Elapsed.Seconds.ToString() + " секунд ";
        }
    }
}
//string litresFile = @"f:\csv_litres_exp_873.csv";

//DataTable Litres = ConvertCSVtoDataTable(litresFile);
//string allFields = "";

//foreach (DataRow row in Litres.Rows)
//{

//    foreach (DataColumn col in Litres.Columns)
//    {
//        switch (col.ColumnName)
//        {
//            case "ID книги":


//                //allFields = allFields + " "+row[col.ColumnName].ToString();
//                break;
//            case "Название":
//                AddField(main, doc, "title", row[col.ColumnName].ToString());
//                AddField(main, doc, "title_short", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();
//                break;
//            case "Авторы":
//                AddField(main, doc, "author", row[col.ColumnName].ToString());
//                AddField(main, doc, "author_sort", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();

//                break;
//            case "ISBN":
//                AddField(main, doc, "isbn", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();

//                break;
//            case "Издательство":
//                AddField(main, doc, "publisher", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();
//                break;
//            case "Год":
//                AddField(main, doc, "publishDate", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();
//                break;
//            case "Жанры":
//                AddField(main, doc, "genre", row[col.ColumnName].ToString());
//                allFields = allFields + " " + row[col.ColumnName].ToString();
//                break;

//        }
//    }
//    AddField(main, doc, "id", "litres_" + row["ID книги"].ToString());
//    AddField(main, doc, "fund", GetRusFund("litres"));
//    AddField(main, doc, "allfields", allFields);
//    allFields = "";

//    doc.WriteTo(objXmlWriter);
//    doc = main.CreateElement("doc");
//    label2.Text = "litres_" + row["ID книги"].ToString();
//    Application.DoEvents();

//}