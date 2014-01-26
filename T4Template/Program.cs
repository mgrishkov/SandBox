using System;
namespace T4Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = new System.Text.StringBuilder();
            
            string[] cultures = { "en-GB", "ru-RU" };

            foreach (var culture in cultures)
            {
                var ci = System.Globalization.CultureInfo.GetCultureInfo(culture);
                var resource = System.String.Format("Resources.{0}", ci.TwoLetterISOLanguageName.ToUpper());
                sb.AppendLine(System.String.Format("{0} = {{}};", resource));

                var resourceSet = Resources.Errors.ResourceManager.GetResourceSet(ci, true, true);
                foreach (System.Collections.DictionaryEntry entry in resourceSet)
                {
                    sb.AppendLine(System.String.Format("{0}.{1} = \'{2}\';", resource, entry.Key, entry.Value));
                }
            };

            Console.Write(sb.ToString());
            
            Console.ReadKey();
        }
    }
}
