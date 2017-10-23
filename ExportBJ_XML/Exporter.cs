using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Net;
using System.Security.Cryptography;

namespace ExportBJ_XML
{
    class Exporter
    {

        XmlWriter _objXmlWriter;
        XmlDocument _exportDocument;
        XmlNode _doc;
        XmlNode _root;
        Form1 _f1;
        int _lastID = 1;

        public Exporter(Form1 f1)
        {
            this._f1 = f1;
        }

        private void StartExportFrom(string fund, int previous)
        {
            int step = 1;
            int MaxIDMAIN = GetMaxIDMAIN(fund);
            for (int i = previous; i < MaxIDMAIN; i += step)
            {
                _lastID = i; 
                string q = GetQuery(fund, i);
                try
                {
                    int check = CreateBJDoc(fund, q);
                    if (check == 1) continue;
                }
                catch (Exception ex)
                {
                    _f1.textBox1.Text += DateTime.Now.ToShortTimeString() + " - " + ex.Message + " ;\r\n ";

                    Thread.Sleep(5000);
                    _doc = _exportDocument.CreateElement("doc");
                    this.StartExportFrom(fund, _lastID);
                }


                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");

                _f1.label2.Text = fund + "_" + i;
                Application.DoEvents();
            }

            _objXmlWriter.Flush();
            _objXmlWriter.Close();

        }

        public void ExportSingleRecord(string fund, int idmain)
        {
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\" + fund + idmain + ".xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////TEST/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            string q = GetQuery(fund, idmain);
            int check = CreateBJDoc(fund, q);
            if (check == 1) return;
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
            _objXmlWriter.Flush();
            _objXmlWriter.Close();

        }

        public void BJVVV()
        {
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\bjvvvTest.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJVVV/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1449310;
            this.StartExportFrom("BJVVV", _lastID);

            //1449336 - экземпляры не выгружаются.
        }

        public void REDKOSTJ()
        {
            
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\redkostj.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////REDKOSTJ/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom("REDKOSTJ", _lastID);
        }

        public void BRIT_SOVET()
        {
            
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\brit_sovet.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BRIT_SOVET/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom("BRIT_SOVET", _lastID);
        }
        public void BJACC( )
        {
            
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\bjacc.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJACC/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom("BJACC", _lastID);
        }
        public void BJFCC( )
        {
            
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\bjfcc.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJFCC/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom("BJFCC", _lastID);
        }
        public void BJSCC( )
        {
            
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\bjscc.xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJSCC/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom("BJSCC", _lastID);
        }

        #region PERIOD
        public void PERIOD()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////PERIOD/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            string allFields = "";
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\period.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
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
                    //continue;
                }
                //title
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 121 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("title", val.Rows[0]["POLE"].ToString());
                    AddField("title_short", val.Rows[0]["POLE"].ToString());
                }
                //вид издания
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 124 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_ModeOfPublication", val.Rows[0]["POLE"].ToString());//добавить в индекс
                }
                //язык
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 128 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_Language", val.Rows[0]["POLE"].ToString());//добавить в индекс
                }
                //периодичность
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 130 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_Periodicity", val.Rows[0]["POLE"].ToString());//добавить в индекс
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
                    AddField("period_Years", result);//добавить в индекс
                }
                //гиперссылки 
                da.SelectCommand.CommandText = "select E.POLE " +
                                               "  from PERIOD..PI A " +
                                               "  left join PERIOD..PI B on A.IDZ = B.VVERH " +
                                               "  left join PERIOD..PI C on B.IDZ = C.VVERH " +
                                               "  left join PERIOD..PI D on C.IDZ = D.VVERH " +
                                               "  left join PERIOD..PI E on C.IDZ = E.VVERH " +
                                               "  where A.IDF = 120 and A.POLE = '" + row["POLE"] + "' " +
                                               " and D.IDF = 363  " +
                                               " and D.POLE = 'e-book' " +
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
                    AddField("period_HyperLink", result);//добавить в индекс
                }



                AddField("id", "period_" + row["POLE"]);
                AddField("fund", GetRusFund("period"));
                AddField("allfields", allFields);
                allFields = "";
                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");
                _f1.label2.Text = "period_" + row["POLE"];
                Application.DoEvents();
            }
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }
        #endregion



        #region Pearson
        public void Pearson()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////Pearson////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\pearson.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);
            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");
            _doc = _exportDocument.CreateElement("doc");

            string Pearson = File.ReadAllText(@"f:/pearson.json");

            JArray desPearson = (JArray)JsonConvert.DeserializeObject(Pearson);

            string tmp = desPearson.First["licensePackage"].ToString();
            tmp = desPearson.First["catalog"]["options"]["Supported platforms"].ToString();
            int cnt = 1;
            foreach (JToken token in desPearson)
            {
                //AddField( "id", token["id"].ToString());
                AddField("title", token["catalog"]["title"]["default"].ToString());
                AddField("title_short", token["catalog"]["title"]["default"].ToString());
                AddField("author", token["catalog"]["options"]["Authors"].ToString());
                AddField("Country", token["catalog"]["options"]["Country of publication"].ToString());
                AddField("publisher", token["catalog"]["options"]["Publisher"].ToString());
                AddField("publishDate", token["catalog"]["options"]["Publishing date"].ToString().Split('.')[2]);
                AddField("isbn", token["catalog"]["options"]["ISBN"].ToString());
                AddField("Volume", token["catalog"]["options"]["Number of pages"].ToString());
                AddField("description", token["catalog"]["options"]["Desk"].ToString() + " ; " +
                                              token["catalog"]["description"]["default"].ToString());
                AddField("genre", token["catalog"]["options"]["Subject"].ToString());
                AddField("topic", token["catalog"]["options"]["Catalogue section"].ToString());
                AddField("collection", token["catalog"]["options"]["Collection"].ToString());
                AddField("language", token["catalog"]["options"]["Language"].ToString());


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


                AddField("MethodOfAccess", "Удалённый доступ");
                AddField("Exemplar", sb.ToString());
                AddField("id", "Pearson_" + token["id"].ToString());
                AddField("HyperLink", "http://не сформирован");
                AddField("fund", "Pearson");
                AddField("Level", "Монография");


                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");
                _f1.label2.Text = "Pearson_" + cnt++;
                Application.DoEvents();
            }
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }
        #endregion


        
        
        public void Litres()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////*/
            //////////////////////////////////LITRES/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\litres.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);
            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");
            _doc = _exportDocument.CreateElement("doc");




            //внимание! Timestamp нужен от текущего времени, а не от чекпоинта! SHA генерируется тоже от текущего времени, а не от чекпоинта


            
            string stamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();
            string key = "geqop45m))AZvb23zerhgjj76cvc##PFbbfqorptqskj";
            DateTime checkpointDate = new DateTime(2013, 1, 1, 0, 0, 0);
            string checkpoint= checkpointDate.ToString("yyyy-MM-dd HH:mm:ss");//"2017-01-01 00:00:00";
            string endpoint = checkpointDate.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");

            string inputString = stamp + ":" + key + ":" + checkpoint;
            string sha256 = Exporter.sha256(inputString);


            Uri apiUrl =
            new Uri("http://partnersdnld.litres.ru/get_fresh_book/?checkpoint=" + checkpoint +
                                                                 "&place=GTCTL" +
                                                                 "&timestamp=" + stamp +
                                                                 "&sha=" + sha256);
                                                                 //"&enpoint="+endpoint);






            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(apiUrl) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            XDocument xdoc = XDocument.Load(new StreamReader(response.GetResponseStream()));


            //читать простым текстом по частям
            //using (Stream output = File.OpenWrite("litres.dat"))
            //using (Stream input = response.GetResponseStream())
            //{
            //    byte[] buffer = new byte[8192];
            //    int bytesRead;
            //    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        output.Write(buffer, 0, bytesRead);
            //    }
            //}


          
            
            
            XmlWriter writ = XmlTextWriter.Create(@"F:\litres_source.xml");
            xdoc.WriteTo(writ);
            writ.Flush();
            writ.Close();


            

            xdoc = XDocument.Load(@"f:\fresh.litres.ru.xml");


            var books = xdoc.Descendants("updated-book");
            int cnt = 1;

            foreach (XElement elt in books)
            {
                AddField("title", elt.Element("title-info").Element("book-title").Value);
                AddField("HyperLink", elt.Element("document-info").Element("src-url").Value);
                AddField("author", elt.Element("title-info").Element("author").Element("last-name").Value + " " +
                        elt.Element("title-info").Element("author").Element("first-name").Value
                        //+ " " +
                        //elt.Element("title-info").Element("author").Element("middle-name").Value
                        );
                
                
                
                //описание экземпляра Litres
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
                writer.WriteValue(elt.Element("document-info").Element("src-url").Value);
                writer.WritePropertyName("exemplar_copyright");
                writer.WriteValue("Да");

                writer.WriteEndObject();
                writer.WriteEndObject();


                AddField("MethodOfAccess", "Удалённый доступ");
                AddField("Exemplar", sb.ToString());
                AddField("id", "Litres_" + elt.Attribute("id").Value);
                AddField("fund", "Литрес");
                AddField("Level", "Монография");


                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");
                _f1.label2.Text = "Litres_" + cnt++;
                Application.DoEvents();
            }

             
        }
        static string sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private string GetQuery(string baza, int count)
        {
            return "select A.*,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, " +
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
                    " left join " + baza + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                    " left join " + baza + "..MAIN C on A.IDMAIN = C.ID " +
                //" where A.IDMAIN between " + count + " and " + (count + 999).ToString() +//типа для ускорения, но можно явно пин указать же. не помню.
                    " where A.IDMAIN = " + count +
                //" and exists (select 1 from BRIT_SOVET..DATAEXT C where A.IDMAIN = C.IDMAIN and C.MNFIELD = 899 and C.MSFIELD = '$p')" +
                    " order by IDMAIN, IDDATA";
        }

        public int CreateBJDoc(string fund, string query)
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
            foreach (DataRow r in ds.Tables["t"].Rows)
            {

                allFields += " " + r["PLAIN"].ToString();
                switch (r["code"].ToString())
                {
                    //=======================================================================Родные поля вуфайнд=======================
                    case "200$a":
                        if (wasTitle) break;
                        AddField( "title", r["PLAIN"].ToString());
                        AddField( "title_short", r["PLAIN"].ToString());
                        wasTitle = true;
                        break;
                    case "700$a":
                        AddField( "author", r["PLAIN"].ToString());
                        AddField( "author_sort", r["SORT"].ToString());
                        //забрать все варианты написания автора из авторитетного файла и вставить в скрытое, но поисковое поле
                        break;
                    case "701$a":
                        AddField( "author2", r["PLAIN"].ToString());
                        break;
                    case "710$a":
                        AddField( "author_corporate", r["PLAIN"].ToString());
                        break;
                    case "710$4":
                        AddField( "author_corporate_role", r["PLAIN"].ToString());
                        break;
                    case "700$4":
                        AddField( "author_role", r["PLAIN"].ToString());
                        break;
                    case "701$4":
                        AddField( "author2_role", r["PLAIN"].ToString());
                        break;
                    case "921$a":
                        AddField( "format", r["PLAIN"].ToString());
                        break;
                    case "922$e":
                        AddField( "genre", r["PLAIN"].ToString());
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
                            add = r["PLAIN"].ToString() + " (" + ds.Tables["clarify"].Rows[0]["PLAIN"].ToString() + ")";
                        }
                        AddField( "isbn", add);
                        break;
                    case "11$a":
                        AddField( "issn", r["PLAIN"].ToString());
                        break;
                    case "101$a":
                        da.SelectCommand.CommandText = " select NAME from " + fund + "..LIST_1 " +
                           " where ID = " + r["IDINLIST"].ToString();
                        ds.Tables["clarify"].Rows.Clear();
                        j = da.Fill(ds, "clarify");
                        if (j == 0)
                        {
                            AddField( "language", r["PLAIN"].ToString());
                        }
                        else
                        {
                            AddField( "language", ds.Tables["clarify"].Rows[0]["NAME"].ToString());
                        }
                        break;
                    case "2100$d":
                        AddField( "publishDate", r["PLAIN"].ToString());
                        break;
                    case "210$c":
                        AddField( "publisher", r["PLAIN"].ToString());
                        break;
                    case "517$a":
                        da.SelectCommand.CommandText = " select B.PLAIN from " + fund + "..DATAEXT A " +
                                                       " left join " + fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                                       " where MNFIELD = 517 and MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        ds.Tables["clarify"].Clear();
                        j = da.Fill(ds, "clarify");
                        if (j != 0)
                        {
                            AddField( "title_alt", "(" + ds.Tables["clarify"].Rows[0]["PLAIN"].ToString() + ")" + r["PLAIN"].ToString());
                        }
                        else
                        {
                            AddField( "title_alt", r["PLAIN"].ToString());
                        }
                        //нужно специальным образом обрабатывать
                        break;
                    //=======================================================================добавленные в индекс=======================
                    case "210$a":
                        AddField( "PlaceOfPublication", r["PLAIN"].ToString());
                        break;
                    case "200$6":
                        AddField( "Title_another_chart", r["PLAIN"].ToString());
                        break;
                    case "200$b":
                        AddField( "Title_same_author", r["PLAIN"].ToString());
                        break;
                    case "200$d":
                        AddField( "Parallel_title", r["PLAIN"].ToString());
                        break;
                    case "200$e":
                        AddField( "Info_pertaining_title", r["PLAIN"].ToString());
                        break;
                    case "200$f":
                        AddField( "Responsibility_statement", r["PLAIN"].ToString());
                        break;
                    case "200$h":
                        AddField( "Part_number", r["PLAIN"].ToString());
                        break;
                    case "200$i":
                        AddField( "Part_title", r["PLAIN"].ToString());
                        break;
                    case "200$z":
                        AddField( "Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "500$a":
                        AddField( "Title_unified", r["PLAIN"].ToString());
                        break;
                    case "500$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField( "Title_unified", AF_all);
                        break;
                    case "517$e":
                        AddField( "Info_title_alt", r["PLAIN"].ToString());
                        break;
                    case "517$z":
                        AddField( "Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "700$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField( "author_variant", AF_all);//хранить но не отображать
                        break;
                    case "701$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField( "Another_author_AF_all", AF_all);//хранить но не отображать
                        break;
                    case "501$a":
                        AddField( "Another_title", r["PLAIN"].ToString());
                        break;
                    case "501$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField( "Another_title_AF_All", AF_all);
                        break;
                    case "503$a":
                        AddField( "Unified_Caption", r["PLAIN"].ToString());
                        break;
                    case "503$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField( "Unified_Caption_AF_All", AF_all);
                        break;
                    case "700$6":
                        AddField( "Author_another_chart", r["PLAIN"].ToString());
                        break;
                    case "702$a":
                        AddField( "Editor", r["PLAIN"].ToString());
                        break;
                    case "702$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField( "Editor_AF_all", AF_all);
                        break;
                    case "702$4":
                        AddField( "Editor_role", AF_all);
                        break;
                    case "710$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField( "Collective_author_all", AF_all);
                        break;
                    case "710$9":
                        AddField( "Organization_nature", r["PLAIN"].ToString());
                        break;
                    case "11$9":
                        AddField( "Printing", r["PLAIN"].ToString());
                        break;
                    case "205$a":
                        string PublicationInfo = r["PLAIN"].ToString();

                        // 205$b
                        da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                                                       " left join " + fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                                       " where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
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
                        AddField( "Publication_info", r["PLAIN"].ToString());
                        break;
                    case "921$b":
                        AddField( "EditionType", r["PLAIN"].ToString());
                        break;
                    case "102$a":
                        AddField( "Country", r["PLAIN"].ToString());
                        break;
                    case "210$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField( "PlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2110$g":
                        AddField( "PrintingHouse", r["PLAIN"].ToString());
                        break;
                    case "2110$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField( "PrintingHouse_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2111$e":
                        AddField( "GeoNamePlaceOfPublication", r["PLAIN"].ToString());
                        break;
                    case "2111$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFGEOVAR");
                        AddField( "GeoNamePlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "10$z":
                        AddField( "IncorrectISBN", r["PLAIN"].ToString());
                        break;
                    case "11$z":
                        AddField( "IncorrectISSN", r["PLAIN"].ToString());
                        break;
                    case "11$y":
                        AddField( "CanceledISSN", r["PLAIN"].ToString());
                        break;
                    case "101$b":
                        AddField( "IntermediateTranslateLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$d":
                        AddField( "SummaryLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$e":
                        AddField( "TableOfContentsLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$f":
                        AddField( "TitlePageLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$g":
                        AddField( "BasicTitleLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$i":
                        AddField( "AccompayingMaterialLanguage", r["PLAIN"].ToString());
                        break;
                    case "215$a":
                        AddField( "Volume", r["PLAIN"].ToString());
                        break;
                    case "215$b":
                        AddField( "Illustrations", r["PLAIN"].ToString());
                        break;
                    case "215$c":
                        AddField( "Dimensions", r["PLAIN"].ToString());
                        break;
                    case "215$d":
                        AddField( "AccompayingMaterial", r["PLAIN"].ToString());
                        break;
                    case "225$a":
                        if (r["PLAIN"].ToString() == "") break;
                        if (r["PLAIN"].ToString() == "-1") break;
                        AddHierarchyFields(r["PLAIN"].ToString(), _exportDocument, fund, _doc, r["IDMAIN"].ToString());
                        //AddField( "", r["PLAIN"].ToString());
                        break;
                    case "225$h":
                        AddField( "NumberInSeries", r["PLAIN"].ToString());
                        break;
                    case "225$v":
                        AddField( "NumberInSubseries", r["PLAIN"].ToString());
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
                        description += r["PLAIN"].ToString() + " ; ";
                        break;
                    case "830$a":
                        AddField( "CatalogerNote", r["PLAIN"].ToString());
                        break;
                    case "831$a":
                        AddField( "DirectoryNote", r["PLAIN"].ToString());
                        break;
                    case "924$a":
                        AddField( "AdditionalBibRecord", r["PLAIN"].ToString());
                        break;
                    case "940$a":
                        AddField( "HyperLink", r["PLAIN"].ToString());
                        AddField( "Location", "Удалённый доступ");
                        break;
                    case "606$a"://"""""" • """"""
                        da.SelectCommand.CommandText = "select * " +
                                                       " from BJVVV..TPR_CHAIN A " +
                                                       " left join BJVVV..TPR_TES B on A.IDTES = B.ID " +
                                                       " where A.IDCHAIN = " + r["SORT"].ToString() +
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
                        AddField( "topic", TPR);
                        break;
                    case "3000$a":
                        AddField( "OwnerPerson", r["PLAIN"].ToString());
                        break;
                    case "3000$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField( "OwnerPerson_AF_All", AF_all);
                        break;
                    case "3001$a":
                        AddField( "OwnerOrganization", r["PLAIN"].ToString());
                        break;
                    case "3001$3":
                        AF_all = GetAFAll(fund, r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField( "OwnerOrganization_AF_All", AF_all);
                        break;
                    case "3002$a":
                        AddField( "Ownership", r["PLAIN"].ToString());
                        break;
                    case "3003$a":
                        AddField( "OwnerExemplar", r["PLAIN"].ToString());
                        break;
                    case "3200$a":
                        AddField( "IllustrationMaterial", r["PLAIN"].ToString());
                        break;
                    case "899$a":
                        AddField( "Location", r["PLAIN"].ToString());
                        break;
                }

            }
            AddField( "id", fund + "_" + CurrentIDMAIN);

            string rusFund = GetRusFund(fund);

            AddField( "fund", rusFund);
            AddField( "allfields", allFields);
            AddField( "Level", Level);

            if (description != "")
            {
                AddField( "description", description);
            }

            AddExemplarFields(CurrentIDMAIN, _exportDocument, fund);

            description = "";

            allFields = "";
            return 0;
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
            da.SelectCommand.CommandText = " select PLAIN from " + fund + ".." + AFTable + " A " +
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

        private void AddExemplarFields(string idmain, XmlDocument _exportDocument, string fund)
        {

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A" +
                                            " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                                            " where A.IDMAIN = " + idmain + " and A.MNFIELD = 899 and A.MSFIELD = '$p' " +
                                            " and not exists (select 1 from BJVVV..DATAEXT C where C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' and C.SORT = 'Списано')";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");

            if (i == 0) return;
            string IDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            //3901 - проверить, почему у этого пина удаленный доступ

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
            foreach (DataRow iddata in ds.Tables["t"].Rows)
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
                    string code = r["MNFIELD"].ToString() + r["MSFIELD"].ToString();
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
                                AddField( "MethodOfAccess", "В помещении библиотеки");
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            else
                            {
                                AddField( "collection", r["PLAIN"].ToString());
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            if (fnd == "Абонемент")
                            {
                                AddField( "MethodOfAccess", "На дом");
                                //Exemplar += "Доступ: Заказать через личный кабинет на дом#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет на дом");
                            }
                            if ((fund == "BJFCC") || (fund == "BJACC") || (fund == "BRIT_SOVET") || (fund == "BJSCC"))
                            {
                                AddField( "MethodOfAccess", "На дом");
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
                AddField( "MethodOfAccess", "Удалённый доступ");
            }
            writer.WriteEndObject();
            writer.Flush();
            writer.Close();


            AddField( "Exemplar", sb.ToString());


        }

        private void AddHierarchyFields(string ParentPIN, XmlDocument _exportDocument, string fund, XmlNode _doc, string CurrentPIN)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT " +
                               " where IDMAIN = " + ParentPIN;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");

            string TopHierarchyId = GetTopId(ParentPIN, fund);

            AddField( "hierarchy_top_id", fund + "_" + TopHierarchyId);

            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                               " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                               " where A.IDMAIN = " + TopHierarchyId + " and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");
            if (i != 0)
            {
                string hierarchy_top_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
                AddField( "hierarchy_top_title", hierarchy_top_title);
            }

            AddField( "hierarchy_parent_id", fund + "_" + ParentPIN);

            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                   " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.IDMAIN = " + ParentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");
            string hierarchy_parent_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();
            AddField( "hierarchy_parent_title", hierarchy_parent_title);

            bool metka = false;
            foreach (XmlNode n in _doc.ChildNodes)
            {
                if (n.Attributes["name"].Value == "is_hierarchy_id")
                {
                    metka = true;
                }
            }
            if (!metka) AddField( "is_hierarchy_id", fund + "_" + CurrentPIN);





            da.SelectCommand.CommandText = " select * from " + fund + "..DATAEXT A " +
                   " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                   " where A.IDMAIN = " + CurrentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            ds = new DataSet();
            i = da.Fill(ds, "t");

            string is_hierarchy_title = ds.Tables["t"].Rows[0]["PLAIN"].ToString();

            metka = false;
            foreach (XmlNode n in _doc.ChildNodes)
            {
                if (n.Attributes["name"].Value == "is_hierarchy_id")
                {
                    metka = true;
                }
            }
            if (!metka) AddField( "is_hierarchy_title", is_hierarchy_title);

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


        private int GetMaxIDMAIN(string p)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = "select max(ID) from " + p + "..MAIN";
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            return int.Parse(ds.Tables["t"].Rows[0][0].ToString());
        }
        public void AddField(string name, string val)
        {
            XmlNode field = _exportDocument.CreateElement("field");
            XmlAttribute att = _exportDocument.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            field.InnerText = SecurityElement.Escape(val);
            val = XmlCharacterWhitelist(val);
            field.InnerText = val;
            _doc.AppendChild(field);
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
        public static string GetRusFund(string fund)
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
                case "HJB":
                    return "Дом еврейской книги";
                case "pearson":
                    return "Pearson";
            }
            return "<неизвестный фонд>";
        }
        public string GetLitresLanguageRus(string lng)
        {
            return "Русский";
//            o	ru – Русский;
//o	uk – Украинский;
//o	en – Английский;
//o	de – Немецкий;
//o	fr – Французский;
//o	ab – Абхазский;
//o	az – Азербайджанский;
//o	ay – Аймара;
//o	sq – Албанский;
//o	ar – Арабский;
//o	hy – Армянский;
//o	as – Ассамский;
//o	af – Африкаанс;
//o	ts – Банту;
//o	eu – Баскский;
//o	ba – Башкирский;
//o	be – Белорусский;
//o	bn – Бенгальский;
//o	my – Бирманский;
//o	bh – Бихарский;
//o	bg – Болгарский;
//o	br – Бретонский;
//o	cy – Валлийский;
//o	hu – Венгерский;
//o	wo – Волоф;
//o	vi – Вьетнамский;
//o	gd – Гаэльский;
//o	nl – Голландский;
//o	el – Греческий;
//o	ka – Грузинский;
//o	gn – Гуарани;
//o	da – Датский;
//o	gr – Древнегреческий;
//o	iw – Древнееврейский;
//o	dr – Древнерусский;
//o	zu – Зулу;
//o	he – Иврит;
//o	yi – Идиш;
//o	in – Индонезийский;
//o	ia – Интерлингва;
//o	ga – Ирландский;
//o	is – Исландский;
//o	es – Испанский;
//o	it – Итальянский;
//o	kk – Казахский;
//o	kn – Каннада;
//o	ca – Каталанский;
//o	ks – Кашмири;
//o	qu – Кечуа;
//o	ky – Киргизский;
//o	zh – Китайский;
//o	ko – Корейский;
//o	kw – Корнский;
//o	co – Корсиканский;
//o	ku – Курдский;
//o	km – Кхмерский;
//o	xh – Кхоса;
//o	la – Латинский;
//o	lv – Латышский;
//o	lt – Литовский;
//o	mk – Македонский;
//o	mg – Малагасийский;
//o	ms – Малайский;
//o	mt – Мальтийский;
//o	mi – Маори;
//o	mr – Маратхи;
//o	mo – Молдавский;
//o	mn – Монгольский;
//o	na – Науру;
//o	ne – Непали;
//o	no – Норвежский;
//o	pa – Панджаби;
//o	fa – Персидский;
//o	pl – Польский;
//o	pt – Португальский;
//o	ps – Пушту;
//o	rm – Ретороманский;
//o	ro – Румынский;
//o	rn – Рунди;
//o	sm – Самоанский;
//o	sa – Санскрит;
//o	sr – Сербский;
//o	si – Сингальский;
//o	sd – Синдхи;
//o	sk – Словацкий;
//o	sl – Словенский;
//o	so – Сомали;
//o	st – Сото;
//o	sw – Суахили;
//o	su – Сунданский;
//o	tl – Тагальский;
//o	tg – Таджикский;
//o	th – Тайский;
//o	ta – Тамильский;
//o	tt – Татарский;
//o	te – Телугу;
//o	bo – Тибетский;
//o	tr – Турецкий;
//o	tk – Туркменский;
//o	uz – Узбекский;
//o	ug – Уйгурский;
//o	ur – Урду;
//o	fo – Фарерский;
//o	fj – Фиджи;
//o	fi – Финский;
//o	fy – Фризский;
//o	ha – Хауса;
//o	hi – Хинди;
//o	hr – Хорватскосербский;
//o	cs – Чешский;
//o	sv – Шведский;
//o	sn – Шона;
//o	eo – Эсперанто;
//o	et – Эстонский;
//o	jv – Яванский;
//o	ja – Японский.

        }

    }
}
