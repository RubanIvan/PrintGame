using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PrintGame.Proc
{
    public static class Slug
    {
        public static string Create(string gameName)
        {
            // make the url lowercase
            string slugUrl = (gameName ?? "").ToLower();

            // replace & with and
            slugUrl = Regex.Replace(slugUrl, @"\&+", "and");

            // remove characters
            slugUrl = slugUrl.Replace("'", "");

            // remove invalid characters
            slugUrl = Regex.Replace(slugUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            slugUrl = Regex.Replace(slugUrl, @"-+", "-");

            // trim leading & trailing characters
            slugUrl = slugUrl.Trim('-');

            return slugUrl;
        }
    }
}