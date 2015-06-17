using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassaK.Rnums
{
    public enum Error
    {
        [Description("Связь с весами не установлена")]
        ConnectionNotEstablished = 1,
        [Description("Ошибка обмена с весами")]
        DataCommunicationEror    = 2,
        [Description("Весы не готовы к передаче данных")]
        ScalesAreNotReady        = 3,
        [Description("Параметр не поддерживается весами")]
        ParameterDoesNotSupplied = 4,
        [Description("Установка параметра невозможна")]
        ParameterCannotBeSet     = 5
    }
}
