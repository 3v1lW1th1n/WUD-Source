namespace Supremus.Configuration
{
    using System;
    using System.IO;
    using System.Xml;

    public class Settings
    {
        private XmlDocument doc;
        private string fileName;
        private string rootName;

        public Settings(string fileName) : this(fileName, "Settings")
        {
        }

        public Settings(string fileName, string rootName)
        {
            this.fileName = fileName;
            this.rootName = rootName;
            this.doc = new XmlDocument();
            try
            {
                this.doc.Load(fileName);
            }
            catch (FileNotFoundException)
            {
                this.CreateSettingsDocument();
            }
        }

        public void ClearSection(string section)
        {
            XmlNode node2 = this.doc.DocumentElement.SelectSingleNode(string.Concat(new object[] { '/', this.rootName, '/', section }));
            if (node2 != null)
            {
                node2.RemoveAll();
            }
        }

        protected void CreateSettingsDocument()
        {
            this.doc.AppendChild(this.doc.CreateXmlDeclaration("1.0", null, null));
            this.doc.AppendChild(this.doc.CreateElement(this.rootName));
        }

        public void Flush()
        {
            try
            {
                this.doc.Save(this.fileName);
            }
            catch (Exception)
            {
            }
        }

        public int GetSectionCount(string section)
        {
            XmlNode node2 = this.doc.DocumentElement.SelectSingleNode(string.Concat(new object[] { '/', this.rootName, '/', section }));
            return node2?.ChildNodes.Count;
        }

        public bool ReadBool(string section, string name, bool defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == bool.TrueString)
            {
                return true;
            }
            if (str == bool.FalseString)
            {
                return false;
            }
            return defaultValue;
        }

        public DateTime ReadDateTime(string section, string name, DateTime defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToDateTime(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public double ReadDouble(string section, string name, double defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToDouble(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public float ReadFloat(string section, string name, float defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToSingle(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public int ReadInt(string section, string name, int defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public long ReadLong(string section, string name, long defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt64(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public short ReadShort(string section, string name, short defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt16(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public string ReadString(string section, string name, string defaultValue)
        {
            XmlNode node2 = this.doc.DocumentElement.SelectSingleNode(string.Concat(new object[] { '/', this.rootName, '/', section }));
            if (node2 != null)
            {
                XmlNode node3 = node2.SelectSingleNode(name);
                if (node3 == null)
                {
                    return defaultValue;
                }
                foreach (XmlAttribute attribute in node3.Attributes)
                {
                    if (attribute.Name == "value")
                    {
                        return attribute.Value;
                    }
                }
            }
            return defaultValue;
        }

        public uint ReadUInt(string section, string name, uint defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToUInt32(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public ulong ReadULong(string section, string name, ulong defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToUInt64(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public ushort ReadUShort(string section, string name, ushort defaultValue)
        {
            string str = this.ReadString(section, name, "");
            if (str == "")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToUInt16(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public void RemoveSection(string section)
        {
            XmlNode documentElement = this.doc.DocumentElement;
            XmlNode oldChild = documentElement.SelectSingleNode(string.Concat(new object[] { '/', this.rootName, '/', section }));
            if (oldChild != null)
            {
                documentElement.RemoveChild(oldChild);
            }
        }

        public void Save()
        {
            this.Flush();
        }

        public void WriteBool(string section, string name, bool value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteDateTime(string section, string name, DateTime value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteDouble(string section, string name, double value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteFloat(string section, string name, float value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteInt(string section, string name, int value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteLong(string section, string name, long value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteShort(string section, string name, short value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteString(string section, string name, string value)
        {
            XmlNode documentElement = this.doc.DocumentElement;
            XmlNode node2 = documentElement.SelectSingleNode(string.Concat(new object[] { '/', this.rootName, '/', section }));
            if (node2 == null)
            {
                node2 = documentElement.AppendChild(this.doc.CreateElement(section));
            }
            XmlNode node3 = node2.SelectSingleNode(name);
            if (node3 == null)
            {
                node3 = node2.AppendChild(this.doc.CreateElement(name));
            }
            ((XmlElement) node3).SetAttributeNode("value", "").Value = value;
        }

        public void WriteUInt(string section, string name, uint value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteULong(string section, string name, ulong value)
        {
            this.WriteString(section, name, value.ToString());
        }

        public void WriteUShort(string section, string name, ushort value)
        {
            this.WriteString(section, name, value.ToString());
        }
    }
}

