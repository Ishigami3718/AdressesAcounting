using AdressAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Utils
{
    public static class Db
    {
        public static AdressAccountingContext Context => new AdressAccountingContext();
    }
}
