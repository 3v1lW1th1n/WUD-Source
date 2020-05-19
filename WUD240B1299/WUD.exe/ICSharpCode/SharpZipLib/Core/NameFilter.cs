namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;

    public class NameFilter : IScanFilter
    {
        private ArrayList exclusions_;
        private string filter_;
        private ArrayList inclusions_;

        public NameFilter(string filter)
        {
            this.filter_ = filter;
            this.inclusions_ = new ArrayList();
            this.exclusions_ = new ArrayList();
            this.Compile();
        }

        private void Compile()
        {
            if (this.filter_ != null)
            {
                string[] strArray = this.filter_.Split(new char[] { ';' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i] != null) && (strArray[i].Length > 0))
                    {
                        string str;
                        bool flag = strArray[i][0] != '-';
                        if (strArray[i][0] == '+')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else if (strArray[i][0] == '-')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else
                        {
                            str = strArray[i];
                        }
                        if (flag)
                        {
                            this.inclusions_.Add(new Regex(str, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase));
                        }
                        else
                        {
                            this.exclusions_.Add(new Regex(str, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase));
                        }
                    }
                }
            }
        }

        public bool IsExcluded(string name)
        {
            foreach (Regex regex in this.exclusions_)
            {
                if (regex.IsMatch(name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsIncluded(string name)
        {
            if (this.inclusions_.Count == 0)
            {
                return true;
            }
            foreach (Regex regex in this.inclusions_)
            {
                if (regex.IsMatch(name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMatch(string name) => 
            (this.IsIncluded(name) && !this.IsExcluded(name));

        public static bool IsValidExpression(string expression)
        {
            bool flag = true;
            try
            {
                new Regex(expression, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool IsValidFilterExpression(string toTest)
        {
            if (toTest == null)
            {
                throw new ArgumentNullException("toTest");
            }
            bool flag = true;
            try
            {
                string[] strArray = toTest.Split(new char[] { ';' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i] != null) && (strArray[i].Length > 0))
                    {
                        string str;
                        if (strArray[i][0] == '+')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else if (strArray[i][0] == '-')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else
                        {
                            str = strArray[i];
                        }
                        new Regex(str, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public override string ToString() => 
            this.filter_;
    }
}

