using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using ExportBJ_XML.classes.BJ;

/// <summary>
/// Сводное описание для BookInfo
/// </summary>
namespace ExportBJ_XML.ValueObjects
{
    public class BJBookInfo
    {
        public BJBookInfo()
        {
        }

        public List<BJField> Fields = new List<BJField>();
        
        //public string Title { get; set; }
        //public BJField Author = new BJField();//700a,701a
        //public BJField Annotation = new BJField();//

        public string ID { get; set; }


        #region Экземпляры книги

        public List<ExemplarInfo> Exemplars = new List<ExemplarInfo>();

        #endregion




        internal BJField AddField(string fieldValue, int mNFIELD, string mSFIELD)
        {
            BJField search = Fields.FirstOrDefault(code => code.MNFIELD == mNFIELD && code.MSFIELD == mSFIELD);
            if (search == null)
            {
                search = new BJField(mNFIELD, mSFIELD);
                search.Add(fieldValue);
                Fields.Add(search);
            }
            else
            {
                search.Add(fieldValue);
            }
            
            return search;
        }
        internal BJField AddField(string fieldValue, int mNFIELD, string mSFIELD, AuthoritativeFile af)
        {
            BJField search = AddField(fieldValue, mNFIELD, mSFIELD);
            search.AFData = af;
            Fields.Add(search);
            return search;
        }
    }
}
