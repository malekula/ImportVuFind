using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security;

namespace ExportBJ_XML.classes
{

    public abstract class VuFindConverter
    {
        public string Fund
        { 
            get 
            {
                return this._fund; 
            }
            protected set
            {
                this._fund = value;
            }
        }
        private string _fund = "unknown";
        protected XmlWriter _objXmlWriter;
        protected XmlDocument _exportDocument;
        protected XmlNode _doc;
        protected XmlNode _root;

        public event EventHandler RecordExported;
        //public event EventHandler<VuFindConverterEventArgs> OnDatabaseTimeout;
        //event EventHandler<VuFindConverterEventArgs> OnConvertError;

        public VuFindConverter() 
        {
            //Fund = "unknown";
        }

        public abstract void Export();
        public abstract void ExportSingleRecord(int idmain);
        public abstract void ExportCovers();

        protected virtual void OnRecordExported(EventArgs e)
        {
            EventHandler handler = RecordExported;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void AddField(string name, string val)
        {
            XmlNode field = _exportDocument.CreateElement("field");
            XmlAttribute att = _exportDocument.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            field.InnerText = SecurityElement.Escape(val);
            val = Extensions.XmlCharacterWhitelist(val);
            field.InnerText = val;
            _doc.AppendChild(field);
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
    }
}
