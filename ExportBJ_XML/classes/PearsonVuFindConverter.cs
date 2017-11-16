using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ExportBJ_XML.classes
{
    public class PearsonVuFindConverter : VuFindConverter
    {
        public override void Export()
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
                AddField("title", token["catalog"]["title"]["default"].ToString());
                AddField("title_short", token["catalog"]["title"]["default"].ToString());
                AddField("title_sort", token["catalog"]["title"]["default"].ToString());
                AddField("author", token["catalog"]["options"]["Authors"].ToString());
                AddField("author_sort", token["catalog"]["options"]["Authors"].ToString());
                AddField("Country", token["catalog"]["options"]["Country of publication"].ToString());
                AddField("publisher", token["catalog"]["options"]["Publisher"].ToString());
                AddField("publishDate", token["catalog"]["options"]["Publishing date"].ToString().Split('.')[2]);
                AddField("isbn", token["catalog"]["options"]["ISBN"].ToString());
                AddField("Volume", token["catalog"]["options"]["Number of pages"].ToString());
                AddField("Annotation", token["catalog"]["options"]["Desk"].ToString() + " ; " +
                                              token["catalog"]["description"]["default"].ToString());
                AddField("genre", token["catalog"]["options"]["Subject"].ToString());
                AddField("genre_facet", token["catalog"]["options"]["Subject"].ToString());
                AddField("topic", token["catalog"]["options"]["Catalogue section"].ToString());
                AddField("topic_facet", token["catalog"]["options"]["Catalogue section"].ToString());
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
                writer.WriteValue("https://ebooks.libfl.ru/product/" + token["id"].ToString());
                writer.WritePropertyName("exemplar_copyright");
                writer.WriteValue("Да");
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue("ebook");

                writer.WriteEndObject();
                writer.WriteEndObject();


                AddField("MethodOfAccess", "Удалённый доступ");
                AddField("Location", "internet");
                AddField("Exemplar", sb.ToString());
                AddField("id", "Pearson_" + token["id"].ToString());
                AddField("HyperLink", "https://ebooks.libfl.ru/product/" + token["id"].ToString() );
                AddField("fund", "Pearson");
                AddField("Level", "Монография");


                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");

                //OnRecordExported
                cnt++;
                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "Pearson_" + token["id"].ToString();
                OnRecordExported(args);
            }
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }

    }
}
