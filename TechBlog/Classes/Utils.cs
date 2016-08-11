using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechBlog.Classes
{
    public class Utils
    {
        public static string CutText (string text, int maxLength = 150)
        {
            if (text == null || text.Length <= maxLength)
            {
                return text;
            }
            var shortText = text.Substring(0, maxLength) + "...";
            return shortText;
        }
    }
}