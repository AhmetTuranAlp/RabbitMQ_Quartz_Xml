using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RabbitMQ_Quartz_Xml.RabbitMQ
{
    public class CustomerXmlDataModel
    {
        public string _id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool isSectional { get; set; }
        public List<NodeList> XmlUrl { get; set; }
        public List<CDBXmlMappRows> Rows { get; set; }
        public bool isNamePasswordRequired { get; set; }
        public List<CustomerXmlSettings> CustomerXmlSettings { get; set; }
        public XmlMergeOperationType XmlMergeOperationType { get; set; }
    }

    public class CDBXmlMappRows
    {
        public ProductPropType ColonName { get; set; }
        public string ColonValue { get; set; }
    }

    public class CustomerXmlSettings 
    {
        public string _id { get; set; }
        public string DbName { get; set; }
        public List<IDBXmlMappRowReplace> ReplaceRows { get; set; }
    }

    public class IDBXmlMappRowReplace 
    {
        public string _id { get; set; }
        public string ColonName { get; set; }
        public string OldColonValue { get; set; }
        public string NewColonValue { get; set; }
    }

    public enum XmlMergeOperationType
    {
        [Description("Tip Yok")]
        NoType = 0,
        [Description("Tek Xml ve Tek Node")]
        SingleXmlSingleNode = 1,
        [Description("Tek Xml ve Parçalı Node")]
        SingleXmlSectionalNodes = 2,
        [Description("Dikey Parçalı Xml ve Tek Node")]
        SectionalXmlSingleNode = 3,
        [Description("Dikey Parçalı Xml ve Parçalı Node")]
        SectionalXmlSectionalNodes = 4,
        [Description("Yatay Parçalı Xml ve Tek Node")]
        HorizantalSectionalXmlSingleNode = 5,
        [Description("Yatay Parçalı Xml ve Parçalı Node")]
        HorizantalSectionalXmlSectionalNode = 6,
    }

    public class NodeList
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Url { get; set; }
        public bool MainXml { get; set; } // Parça xml de ana parçayı belirleme
        public bool isWork { get; set; } // Xml de istenmeyen alanları dahil etmeme
        public string firstNode { get; set; }
        public List<NodeData> secondNodes { get; set; } // Parçalı xml de ortak alanlar
        public List<string> AllNodes { get; set; }
        public XDocument XDocument { get; set; }
    }

    public class NodeData
    {
        public bool mainNode { get; set; }
        public string NodeName { get; set; }
        public string MainProperty { get; set; }
        public bool isWork { get; set; }
        public List<string> ChildNodes { get; set; }
    }

    public enum ProductPropType
    {
        [Description("StokId")]
        StokId = 0,
        [Description("Adi")]
        Adi = 1,
        [Description("Aciklama")]
        Aciklama = 2,
        [Description("Marka")]
        Marka = 3,
        [Description("Kategori")]
        Kategori = 4,
        [Description("Resim")]
        Resim = 5,
    }
}
