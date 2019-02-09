using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RabbitMQ_Quartz_Xml.RabbitMQ.RabbitMQ_Recieve
{
    public class RabbitMQ_Recieve
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQ_Recieve(string queueName) // RabbitMQ'dan id alır.
        {
            _rabbitMQService = new RabbitMQService();
            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    // Received event'i sürekli listen modunda olacaktır.
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("{0} isimli queue üzerinden gelen mesaj: \"{1}\"", queueName, message);
                        GetReciver(message);
                    };
                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                }
            }
        }

        #region RabbitMQ ile alınan id ile işlemler yapılır.
        public void GetReciver(string id)
        {
            string FileSavePath = @""; // Dosyanın kaydedileceği konum
            int count = 0;
            if (count > 1)
            {
                //if (dbModel.XmlMergeOperationType == XmlMergeOperationType.SectionalXmlSingleNode)
                //{
                //    for (int i = 0; i < dbModel.XmlUrl.Count; i++) // Parçalı xml ler tek tek indilip dosya yolları kaydediliyor.
                //    {
                //        string name = "Parca" + (i + 1).ToString() + "_" + dbModel.FileName;
                //        string filePath = DownloadXML(dbModel.XmlUrl[i].Url, FileSavePath + name);
                //        dbModel.XmlUrl[i].XDocument = XDocument.Load(filePath);
                //        dbModel.FilePath = filePath;
                //    }

                //    string Path = XmlMerge(dbModel, FileSavePath);
                //    dbModel.FileName = "Merge_" + dbModel.FileName;
                //    dbModel.FilePath = Path;

                //}

            }
            else if (count == 1)
            {
                //string filePath = DownloadXML("dbModel.XmlUrl[0].Url", FileSavePath + "dbModel.FileName");
                //if (dbModel.XmlMergeOperationType == XmlMergeOperationType.SingleXmlSectionalNodes)
                //{
                //    dbModel.XmlUrl[0].XDocument = XDocument.Load(filePath);
                //    filePath = XmlMerge(dbModel, FileSavePath); //Dosya Birleştirme
                //    List<string> listElement = XmlNodeReceive(filePath);
                //    List<XmlUrunModel> productList = ProductList(filePath, listElement, dbModel.Rows);

                //}
                //else if (dbModel.XmlMergeOperationType == XmlMergeOperationType.SingleXmlSingleNode)
                //{
                //    List<string> listElement = XmlNodeReceive(filePath);
                //    List<XmlUrunModel> productList = ProductList(filePath, listElement, dbModel.Rows);
                //    commonidbconnector.Add<XmlUrunModel>(productList, collectionName: "XmlUrunModel");
                //}
            }
            else if (count == 0)
            {

            }
           
        }
        #endregion

        #region Xml İndirme
        public string DownloadXML(string XmlUrl, string path) // Xml İndirme
        {
            using (var client = new WebClientWithCookie())
            {
                Console.WriteLine(XmlUrl + " Xml İndiriliyor");
                client.Encoding = Encoding.UTF8;
                CredentialCache cc = new CredentialCache();
                cc.Add(
                    new Uri(XmlUrl),
                    "NTLM",
                    new NetworkCredential("", "", XmlUrl));
                client.Credentials = cc;
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36");
                client.DownloadFile(XmlUrl, path);
                Console.WriteLine("Xml İndirildi");
            }
            return path;
        }
        #endregion

        #region Parçalı Xml Birleştirme
        public string XmlMerge(CustomerXmlDataModel xmlListe, string XmlPath)
        {
            Console.WriteLine("Xml Birleştirme Başladı");
            #region Xml ler incelenerek yapılacak işlemlerin tipinin belirlenmesi
            //-----------------------------------------------------------------------------------
            // Xml ler incelenerek yapılacak işlemlerin tipinin belirlenmesi
            if (xmlListe.XmlUrl.Count == 1) // tek xml varsa
            {
                string topNode = xmlListe.XmlUrl[0].XDocument.Descendants().GroupBy(c => c.Name).Select(c => c.Key).Select(x => x.LocalName).FirstOrDefault();
                List<string> secondNodes = xmlListe.XmlUrl[0].XDocument.Element(topNode).Elements().Select(c => c.Name.LocalName).ToList().GroupBy(c => c).Select(c => c.Key).ToList();
                if (secondNodes.Count > 0 && secondNodes.Count == 1) // tek xml tek tip nodu varsa
                {
                    xmlListe.XmlUrl.FirstOrDefault().firstNode = topNode;
                    xmlListe.XmlUrl.FirstOrDefault().secondNodes.FirstOrDefault().NodeName = secondNodes.FirstOrDefault();
                    xmlListe.XmlUrl.FirstOrDefault().secondNodes.FirstOrDefault().ChildNodes = xmlListe.XmlUrl[0].XDocument.Element(topNode).Elements(secondNodes.FirstOrDefault()).Elements().Select(c => c.Name.LocalName).ToList().GroupBy(c => c).Select(c => c.Key).ToList();
                    xmlListe.XmlMergeOperationType = XmlMergeOperationType.SingleXmlSingleNode;
                }
                else // tek xml parçalı nodları varsa
                {
                    xmlListe.XmlUrl.FirstOrDefault().firstNode = topNode;
                    for (int i = 0; i < secondNodes.Count; i++)
                    {
                        List<string> insideNodes = xmlListe.XmlUrl[0].XDocument
                                                           .Element(topNode)
                                                           .Elements(secondNodes[i])
                                                           .Elements()
                                                           .Elements()
                                                           .Select(c => c.Name.LocalName)
                                                           .ToList()
                                                           .GroupBy(c => c)
                                                           .Select(c => c.Key)
                                                           .ToList();

                        xmlListe.XmlUrl.FirstOrDefault().secondNodes[i].NodeName = secondNodes[i];
                        xmlListe.XmlUrl.FirstOrDefault().secondNodes[i].ChildNodes = insideNodes;


                    }
                    xmlListe.XmlMergeOperationType = XmlMergeOperationType.SingleXmlSectionalNodes;
                }
            }
            else // parçalı xmller varsa
            {
                // xml modellerinin değerleri giriliyor
                for (int i = 0; i < xmlListe.XmlUrl.Count; i++)
                {
                    string firstNode = xmlListe.XmlUrl[i].XDocument.Descendants().GroupBy(c => c.Name).Select(c => c.Key).Select(x => x.LocalName).FirstOrDefault();
                    List<string> secondNodes = xmlListe.XmlUrl[i].XDocument.Element(firstNode).Elements().Select(c => c.Name.LocalName).ToList().GroupBy(c => c).Select(c => c.Key).ToList();

                    xmlListe.XmlUrl[i].firstNode = firstNode;
                    if (secondNodes.Count > 0 && secondNodes.Count == 1)
                    {

                        xmlListe.XmlUrl[i].secondNodes.FirstOrDefault().NodeName = secondNodes.FirstOrDefault();
                        xmlListe.XmlUrl[i].secondNodes.FirstOrDefault().ChildNodes = xmlListe.XmlUrl[i].XDocument.Element(firstNode)
                                                                        .Elements(secondNodes.FirstOrDefault())
                                                                        .Elements()
                                                                        .Select(c => c.Name.LocalName)
                                                                        .ToList()
                                                                        .GroupBy(c => c)
                                                                        .Select(c => c.Key)
                                                                        .ToList();
                    }
                    else
                    {
                        for (int k = 0; k < secondNodes.Count; k++)
                        {
                            List<string> insideNodes = xmlListe.XmlUrl[i].XDocument
                                                            .Element(firstNode)
                                                            .Elements(secondNodes[k])
                                                            .Elements()
                                                            .Select(c => c.Name.LocalName)
                                                            .ToList()
                                                            .GroupBy(c => c)
                                                            .Select(c => c.Key)
                                                            .ToList();

                            xmlListe.XmlUrl[i].secondNodes[k].NodeName = secondNodes[k];
                            xmlListe.XmlUrl[i].secondNodes[k].ChildNodes = insideNodes;
                        }
                    }
                }

                // xml lerin tiplerine göre operationtype lar veriliyor
                //-------------------------------------------------------------

                //parçalı xml lerden en az bir tanesinin birden fazla node u varsa
                if (xmlListe.XmlUrl.Any(c => c.secondNodes.Count > 1))
                {
                    List<string> allNodes = new List<string>();
                    for (int i = 0; i < xmlListe.XmlUrl.Count; i++) // xml ler dönülüyor
                    {
                        string sParameter = "";
                        // her xml in second node isimleri tek bir stringde birleştirilerek list e atılıyor
                        foreach (var item in xmlListe.XmlUrl[i].secondNodes)
                        {
                            sParameter += item.NodeName;
                        }
                        allNodes.Add(sParameter);

                    }
                    // oluşturulan list gruplanıyor, eğer count 1 den fazla ise operationtype dikey parçalı ve parçalı nodeları vardır
                    // eğer 1 e eşitse yatay parçalı ve parçalı nodeları vardır
                    var nName = allNodes.GroupBy(c => c).ToList();
                    xmlListe.XmlMergeOperationType = nName.Count > 1 ? XmlMergeOperationType.SectionalXmlSectionalNodes : XmlMergeOperationType.HorizantalSectionalXmlSectionalNode;
                }
                else
                {
                    List<string> allNodes = new List<string>();
                    // xml ler dönülüyor, node isimleri string olarak list e atılıyor
                    for (int i = 0; i < xmlListe.XmlUrl.Count; i++)
                    {
                        allNodes.Add(xmlListe.XmlUrl[i].secondNodes[0].NodeName);
                    }
                    // oluşturulan list gruplanıyor, eğer count 1 den fazla ise operationtype dikey parçalı tek node vardır
                    // eğer 1 e eşitse yatay parçalı ve tek node vardır
                    var nName = allNodes.GroupBy(c => c).ToList();
                    xmlListe.XmlMergeOperationType = nName.Count == 1 ? XmlMergeOperationType.HorizantalSectionalXmlSingleNode : XmlMergeOperationType.SectionalXmlSingleNode;
                }
            }
            #endregion

            #region Operasyon tipine göre xml işlemleri başlıyor
            string fileName = "";
            // Operasyon tipine göre xml işlemleri başlıyor
            if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.SingleXmlSingleNode)
            {
                // düz xml direk okunacak
            }
            else if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.SingleXmlSectionalNodes)
            {
                XDocument Xml = xmlListe.XmlUrl.FirstOrDefault().XDocument;
                // xml içindeki ana node ve içindeki xelementler
                NodeData mainNode = xmlListe.XmlUrl.FirstOrDefault().secondNodes.Where(c => c.mainNode).FirstOrDefault();
                List<XElement> mainNodeElements = Xml.Descendants().Where(c => c.Name.LocalName == mainNode.NodeName).Elements().ToList();
                // xml içindeki ana node olmayan node lar
                List<NodeData> notMainNode = xmlListe.XmlUrl.FirstOrDefault().secondNodes.Where(x => x.mainNode == false).ToList();
                foreach (var mainItem in mainNodeElements)
                {
                    var mainvalue = mainItem.Elements(mainNode.MainProperty).FirstOrDefault().Value;

                    foreach (NodeData notMainItem in notMainNode)
                    {
                        List<XElement> currentNodeElements = Xml.Descendants()
                            .Elements(notMainItem.NodeName)
                            .Elements()
                            .Elements(notMainItem.MainProperty)
                            .Where(c => c.Value == mainvalue)
                            .FirstOrDefault()
                            .Parent
                            .Elements()
                            .ToList();


                        var asd = currentNodeElements.Where(c => c.Name.LocalName != notMainItem.MainProperty).Select(c => c.Name.LocalName.ToLower()).ToList();
                        var qwe = mainNode.ChildNodes.Select(c => c.ToLower());
                        List<string> fark = asd.Select(c => c.ToLower()).Except(qwe.Select(c => c.ToLower())).ToList();
                        foreach (var farkItem in fark)
                        {
                            foreach (var cxElement in currentNodeElements.ToList())
                            {
                                if (fark.Any(c => c == cxElement.Name.LocalName.ToLower()) == false)
                                {
                                    currentNodeElements.Remove(cxElement);
                                }
                            }
                        }

                        // eksik nodeların alınması
                        foreach (var Reitem in currentNodeElements) { mainItem.Add(Reitem); }
                    }
                }
                foreach (var notMItem in notMainNode)
                {
                    Xml.Descendants().Elements().Where(c => c.Name == notMItem.NodeName).Remove();
                }
                List<XElement> elements = Xml.Descendants().Elements(mainNode.NodeName).Elements().ToList();
                var asdqxczc = Xml.Descendants().Elements();
                Xml.Descendants().Where(c => c.Name == mainNode.NodeName).Remove();
                foreach (var elem in elements)
                {
                    Xml.Element(xmlListe.XmlUrl.FirstOrDefault().firstNode).Add(elem);
                }
                fileName = "Merge_" + xmlListe.FileName;
                Xml.Save(XmlPath + fileName);

            } // yapıldı
            else if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.SectionalXmlSingleNode)
            {
                // ana xml in alt nodeları (diger xml lerin child nodelarında olup ana xml ın childnode larında olmayan xelement ler için kullanılacak)
                List<string> mainXmlNodeList = xmlListe.XmlUrl.Where(c => c.MainXml == true).FirstOrDefault().secondNodes.FirstOrDefault().ChildNodes;

                // ana xml in içindeki ana node ların listesi (<urun> xxxx <urun/> lerin listesi gibi)
                NodeList mainXml = xmlListe.XmlUrl.Where(c => c.MainXml == true).FirstOrDefault();
                XElement mainElement = mainXml.XDocument.Element(mainXml.firstNode);
                List<XElement> XelementList = mainXml.XDocument.Descendants().Where(c => c.Name == mainXml.secondNodes.FirstOrDefault().NodeName).ToList();
                foreach (var item in XelementList) //  ana xml baz alınarak foreach ile nodeları dönüyoruz
                {
                    List<NodeList> notMainXmlList = xmlListe.XmlUrl.Where(c => c.MainXml == false).ToList(); // anaxml haricindeki xml lerin listesi
                    for (int s = 0; s < notMainXmlList.Count(); s++)
                    {
                        // for dögüsündeki xml in döngüde kullanılacak olan node değeri (ürünid = 123 deki ürünid)
                        var mainProperty = notMainXmlList[s].secondNodes.FirstOrDefault().MainProperty;
                        // foreachdeki (ana xml) xml in şuandaki elemanının (item) temel alınan parametresinin değeri (ürünid = 123 deki 123)
                        var mainvalue = item.Elements(notMainXmlList[s].secondNodes.FirstOrDefault().MainProperty).FirstOrDefault().Value;
                        // for döngusundeki xml in ikinci ana node ismi
                        var xmlSecondNodeName = notMainXmlList[s].secondNodes.FirstOrDefault().NodeName;
                        //for dongusundeki xml in ikinci ana node ismindeki xelement leri
                        var secondNodeChildNodes = notMainXmlList[s].XDocument.Descendants(xmlSecondNodeName);

                        //List<XElement> asdqwe = secondNodeChildNodes.Elements(mainProperty).Where(c => c.Value == mainvalue).SingleOrDefault().Parent.Elements().Where(c => c.Name != mainProperty).ToList();
                        // ana xmldeki ana parametre ve degeri ile for dongusundeki ana parametre ve degeri ile eşleşen node un bütün elemanları
                        List<XElement> remainingNodes = secondNodeChildNodes.Elements(mainProperty).Where(c => c.Value == mainvalue).SingleOrDefault().Parent.Elements().ToList();
                        // remainingNodes daki node ların sadece isimlerinin listesi
                        List<string> currentNodeList = remainingNodes.Select(c => c.Name.LocalName).ToList();
                        // ana xmldeki ana parametre ve degerindeki nodeda olmayıp for dongusundeki ana parametre ve degerinde olan node ların isim listesi
                        List<string> fark = currentNodeList.Select(c => c.ToLower()).Except(mainXmlNodeList.Select(c => c.ToLower())).ToList();

                        //fark listesinde olup remainingNodes da olmayan nodeların remainingNodes listesinden silinmesi ve böylece bu döngüde bu elemandan alınacak nodelar belirlendi
                        foreach (var farkItem in fark)
                        {
                            foreach (var cxElement in remainingNodes.ToList())
                            {
                                if (fark.Any(c => c == cxElement.Name.LocalName.ToLower()) == false)
                                {
                                    remainingNodes.Remove(cxElement);
                                }
                            }
                        }

                        // foreach döngusundeki item ın (ana xml in elemanı) ana parametre (ürünid = 123 deki ürünid) sine ait değer (ürünid = 123 deki 123)
                        var ItemValue = item.Descendants(mainXml.secondNodes.FirstOrDefault().MainProperty).FirstOrDefault().Value;
                        // foreach deki item ın node xelement listesi
                        item.Elements(mainXml.secondNodes.FirstOrDefault().MainProperty)
                            .Where(c => c.Value == ItemValue)
                            .FirstOrDefault()
                            .Parent
                            .Elements()
                            .ToList();
                        // eksik nodeların alınması
                        foreach (var Reitem in remainingNodes) { item.Add(Reitem); }

                        // yeni xml oluşturma

                    }
                }
                mainXml.XDocument.Save(XmlPath + "Merge_" + xmlListe.FileName);
            } // yapıldı
            else if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.SectionalXmlSectionalNodes)
            {
                // xml örnekleri bekleniyor
            }
            else if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.HorizantalSectionalXmlSingleNode)
            {
                NodeList mainXml = xmlListe.XmlUrl.Where(c => c.MainXml == true).FirstOrDefault();

                XElement mainElement = mainXml.XDocument.Element(mainXml.firstNode);
                foreach (var item in xmlListe.XmlUrl.Where(c => c.MainXml == false).ToList())
                {
                    List<XElement> itemList = item.XDocument.Descendants().Elements().Where(x => x.Name == item.secondNodes.FirstOrDefault().ChildNodes.FirstOrDefault()).ToList();

                    foreach (var citem in itemList)
                    {
                        mainXml.XDocument.Element(mainXml.firstNode).Add(citem);
                    }
                }
                mainXml.XDocument.Save(XmlPath + "HorizantalSectionalXmlSingleNode.xml");
            } // yapıldı
            else if (xmlListe.XmlMergeOperationType == XmlMergeOperationType.HorizantalSectionalXmlSectionalNode)
            {

            }
            #endregion
            Console.WriteLine("Birleştirme İşlemi Tamamlandı");
            Console.WriteLine("-------------------------------------------------------");
            return XmlPath + fileName;
        }
        #endregion

        #region Xml Node ları alınıyor
        public List<string> XmlNodeReceive(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            List<string> listElement = new List<string>();
            xmlDoc.Descendants().ToList().ForEach(delegate (XElement element)
            {
                listElement.Add(element.Name.LocalName);
            });
            listElement = listElement.Distinct().ToList();
            return listElement;
        }
        #endregion

        #region Xmd den gelenleri eşleştirme ile bizim modele benzetme yapıyor.
        public List<XmlUrunModel> ProductList(string filePath, List<string> listElement, List<RabbitMQ.CDBXmlMappRows> Rows)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList xmlList = doc.SelectNodes(listElement[0] + @"/" + listElement[1]);
            List<XmlUrunModel> productList = XmlProductRowsFunc(xmlList, Rows);
            return productList;
        }

        public List<XmlUrunModel> XmlProductRowsFunc(XmlNodeList XmlList, List<RabbitMQ.CDBXmlMappRows> MatchingRow)
        {
            List<XmlUrunModel> productList = new List<XmlUrunModel>();

            foreach (XmlNode node in XmlList) // Xml ürünleri dönüyor.
            {
                XmlUrunModel urun = new XmlUrunModel();
                foreach (var row in MatchingRow) // Eşleştirme alanları dönüyor.
                {
                    if (!String.IsNullOrEmpty(row.ColonValue))
                    {
                        if (ProductPropType.StokId == row.ColonName)
                        {
                            urun.StokId = node.SelectSingleNode(row.ColonValue).InnerText;
                        }
                        else if (ProductPropType.Adi == row.ColonName)
                        {
                            urun.Adi = node.SelectSingleNode(row.ColonValue).InnerText;
                        }
                        else if (ProductPropType.Aciklama == row.ColonName)
                        {
                            urun.Aciklama = node.SelectSingleNode(row.ColonValue).InnerText;
                        }
                        else if (ProductPropType.Kategori == row.ColonName)
                        {
                            urun.Kategori = node.SelectSingleNode(row.ColonValue).InnerText;
                        }
                        else if (ProductPropType.Marka == row.ColonName)
                        {
                            urun.Marka = node.SelectSingleNode(row.ColonValue).InnerText;
                        }
                    }
                }
                productList.Add(urun);
            }
            return productList;
        }
        #endregion

        #region MD5 Şifreleme
        public string MD5Sifre(string name)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(name));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion
    }
}
