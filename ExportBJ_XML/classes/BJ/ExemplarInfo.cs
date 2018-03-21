using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExportBJ_XML.classes;
using ExportBJ_XML.classes.BJ;


namespace ExportBJ_XML.ValueObjects
{
    /// <summary>
    /// Сводное описание для ExemplarInfo
    /// </summary>
    public class ExemplarInfo
    {
        public ExemplarInfo(int idData)
        {
            this._iddata = idData;
        }

        private int _iddata;
        public int IdData
        {
            get
            {
                return _iddata;
            }
        }


        public List<BJField> Fields = new List<BJField>();



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


        public ExemplarAccessInfo ExemplarAccess = new ExemplarAccessInfo(); 







        //public string InventoryNumber { get; set; }//899p
        //public string EditionClass { get; set; }//921c
        //public string Location { get; set; }//899a
        //public string FundOrCollectionName { get; set; }//899b
        //public string Barcode { get; set; } //899w
        //public string PlacingCipher { get; set; }//899j
        //public string InventoryNumberNote { get; set; }//899x



        //для электронных экземпдяров
        //public bool IsElectronicCopy = false;


        //public ElectronicExemplarInfo ElectronicCopyInfo;


    }
   
}