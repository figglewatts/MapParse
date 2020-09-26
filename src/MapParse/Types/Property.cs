namespace MapParse.Types
{
    public class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Property()
        {
            Key = "";
            Value = "";
        }

        public Property(string k, string v)
        {
            Key = k;
            Value = v;
        }
    }
}
