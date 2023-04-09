using System;

namespace Repository
{
    public class DbKey : Attribute
    {
        public string Value;
        
        public DbKey(string value)
        {
            Value = value;
        }
    }
}