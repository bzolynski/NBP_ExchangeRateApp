using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBP_ExchangeRateApp.Library.Enums
{
    /// <summary>
    /// Table type: a - common currency, b - uncommon currency, c - buy and sell prices.
    /// </summary>
    public enum TableType
    {
        [Description("Common")]
        a,
        [Description("Unommon")]
        b,
        [Description("Buy and sell prices")]
        c
    }
}
