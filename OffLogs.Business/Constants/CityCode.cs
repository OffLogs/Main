namespace OffLogs.Business.Constants
{
    public class CityCode : AConstant<CityCode>
    {
        public static readonly CityCode Zp = new CityCode("zp", "zp");
        public static readonly CityCode Kv = new CityCode("kv", "kv");
        public static readonly CityCode Dp = new CityCode("dp", "dp");
        public static readonly CityCode Nk = new CityCode("nk", "nk");
        public static readonly CityCode Sv = new CityCode("sv", "sv");
        public static readonly CityCode KrRg = new CityCode("kr","kr");
        public CityCode() { }
        
        private CityCode(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string Value)
        {
            var value = Value.Trim().ToLower();
            return value == Zp.ToString() 
                   || value == Kv.ToString() 
                   || value == Dp.ToString() 
                   || value == Nk.ToString()
                   || value == Sv.ToString()
                   || value == KrRg.ToString();
        }

        public override CityCode FromString(string Value)
        {
            if (Zp.GetValue().Equals(Value))
                return Zp;
            if (Kv.GetValue().Equals(Value))
                return Kv;
            if (Dp.GetValue().Equals(Value))
                return Dp;
            if (Nk.GetValue().Equals(Value))
                return Nk;
            if (Sv.GetValue().Equals(Value))
                return Sv;
            if (KrRg.GetValue().Equals(Value))
                return KrRg;
            return null;
        }
    }
}