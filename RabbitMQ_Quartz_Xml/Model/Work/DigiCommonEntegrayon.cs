using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.Model
{
    public class DigiCommonEntegrayon
    {
        public BotIslemTipi BotIslemTipi { get; set; }
        public BotCalisMaAralik BotCalismaAralik { get; set; }
    }

    public enum BotIslemTipi
    {
        IslemYok = 0,
        RabbitMQSender = 1,
        RabbitMQRecieve = 2,
    }

    public enum BotCalisMaAralik
    {
        yok = 0,
        _birSaat = 1,
        _ucSaat = 3,
        _besSaat = 5,
        _onSaat = 10,
        _birGun = 24,
        _birHafta = 168,
        _birAy = 720,
        _birYil = 8760
    }
}
