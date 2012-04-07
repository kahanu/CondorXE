using System;
using System.Text;

namespace Condor.Fakes
{
    public class TempZeusOutput : Zeus.IZeusOutput
    {
        private StringBuilder sb;
        private int _tablevel;

        public TempZeusOutput()
        {
            sb = new StringBuilder();
        }

        #region IZeusOutput Members

        public System.Collections.ICollection SavedFiles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void append(string path)
        {
            sb.Append(path);
        }

        public void autoTab(string text)
        {
            for (int i = 0; i < this._tablevel; i++)
            {
                this.sb.Append("\t");
            }
            this.sb.Append(text);
        }

        public void autoTabLn(string text)
        {
            this.autoTab(text);
            sb.Append("\r\n");
        }

        public void clear()
        {
            sb = new StringBuilder();
        }

        public void decTab()
        {
            throw new NotImplementedException();
        }

        public string getPreserveBlock(string key)
        {
            throw new NotImplementedException();
        }

        public string getPreservedData(string key)
        {
            throw new NotImplementedException();
        }

        public void incTab()
        {
            throw new NotImplementedException();
        }

        public void preserve(string key)
        {
            throw new NotImplementedException();
        }

        public void save(string path, object action)
        {
            throw new NotImplementedException();
        }

        public void saveEnc(string path, object action, object encoding)
        {
            throw new NotImplementedException();
        }

        public void setPreserveSource(string path, string prefix, string suffix)
        {
            throw new NotImplementedException();
        }

        public int tabLevel
        {
            get
            {
                return this._tablevel;
            }
            set
            {
                this._tablevel = value;
            }
        }

        public string text
        {
            get
            {
                return sb.ToString();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void write(string text)
        {
            this.sb.Append(text);
        }

        public void writeln(string text)
        {
            sb.Append(text);
            sb.Append("\r\n");
        }

        #endregion

        public void rollback(int num)
        {
            throw new NotImplementedException();
        }
    }
}