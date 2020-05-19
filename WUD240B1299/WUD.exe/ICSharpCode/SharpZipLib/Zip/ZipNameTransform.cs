namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.IO;
    using System.Text;

    public class ZipNameTransform : INameTransform
    {
        private static readonly char[] InvalidEntryChars;
        private static readonly char[] InvalidEntryCharsRelaxed;
        private string trimPrefix_;

        static ZipNameTransform()
        {
            char[] invalidPathChars = Path.GetInvalidPathChars();
            int num = invalidPathChars.Length + 2;
            InvalidEntryCharsRelaxed = new char[num];
            Array.Copy(invalidPathChars, 0, InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
            InvalidEntryCharsRelaxed[num - 1] = '*';
            InvalidEntryCharsRelaxed[num - 2] = '?';
            num = invalidPathChars.Length + 4;
            InvalidEntryChars = new char[num];
            Array.Copy(invalidPathChars, 0, InvalidEntryChars, 0, invalidPathChars.Length);
            InvalidEntryChars[num - 1] = ':';
            InvalidEntryChars[num - 2] = '\\';
            InvalidEntryChars[num - 3] = '*';
            InvalidEntryChars[num - 4] = '?';
        }

        public ZipNameTransform()
        {
        }

        public ZipNameTransform(string trimPrefix)
        {
            this.TrimPrefix = trimPrefix;
        }

        public static bool IsValidName(string name) => 
            (((name != null) && (name.IndexOfAny(InvalidEntryChars) < 0)) && (name.IndexOf('/') != 0));

        public static bool IsValidName(string name, bool relaxed)
        {
            bool flag = name != null;
            if (!flag)
            {
                return flag;
            }
            if (relaxed)
            {
                return (name.IndexOfAny(InvalidEntryCharsRelaxed) < 0);
            }
            return ((name.IndexOfAny(InvalidEntryChars) < 0) && (name.IndexOf('/') != 0));
        }

        private static string MakeValidName(string name, char replacement)
        {
            int num = name.IndexOfAny(InvalidEntryChars);
            if (num > 0)
            {
                StringBuilder builder = new StringBuilder(name);
                while (num >= 0)
                {
                    builder[num] = replacement;
                    if (num >= name.Length)
                    {
                        num = -1;
                    }
                    else
                    {
                        num = name.IndexOfAny(InvalidEntryChars, num + 1);
                    }
                }
                name = builder.ToString();
            }
            return name;
        }

        public string TransformDirectory(string name)
        {
            name = this.TransformFile(name);
            if (name.Length <= 0)
            {
                throw new ZipException("Cannot have an empty directory name");
            }
            if (!name.EndsWith("/"))
            {
                name = name + "/";
            }
            return name;
        }

        public string TransformFile(string name)
        {
            if (name != null)
            {
                string str = name.ToLower();
                if ((this.trimPrefix_ != null) && (str.IndexOf(this.trimPrefix_) == 0))
                {
                    name = name.Substring(this.trimPrefix_.Length);
                }
                if (Path.IsPathRooted(name))
                {
                    name = name.Substring(Path.GetPathRoot(name).Length);
                }
                name = name.Replace(@"\", "/");
                while ((name.Length > 0) && (name[0] == '/'))
                {
                    name = name.Remove(0, 1);
                }
                name = MakeValidName(name, '_');
                return name;
            }
            name = string.Empty;
            return name;
        }

        public string TrimPrefix
        {
            get => 
                this.trimPrefix_;
            set
            {
                this.trimPrefix_ = value;
                if (this.trimPrefix_ != null)
                {
                    this.trimPrefix_ = this.trimPrefix_.ToLower();
                }
            }
        }
    }
}

