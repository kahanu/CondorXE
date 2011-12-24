using System;

namespace Condor.Tests.Context
{
    class TempZeusContext : Zeus.IZeusContext
    {
        #region IZeusContext Members
        public Zeus.IZeusContext Copy()
        {
            throw new NotImplementedException();
        }

        public string Describe(string varName, object obj)
        {
            throw new NotImplementedException();
        }

        public void Execute(string path, bool copyContext)
        {
            throw new NotImplementedException();
        }

        public void ExecuteTemplate(string path)
        {
            throw new NotImplementedException();
        }

        public Zeus.IZeusTemplateStub ExecutingTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int ExecutionDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Zeus.IZeusGuiControl Gui
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Zeus.IZeusInput Input
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Zeus.IZeusIntrinsicObject[] IntrinsicObjects
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Zeus.ILog Log
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public System.Collections.Hashtable Objects
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private Zeus.IZeusOutput _output;
        public Zeus.IZeusOutput Output
        {
            get
            {
                if (_output == null)
                {
                    _output = new TempZeusOutput();
                    return _output;
                }
                return _output;
            }
        }
        #endregion
    }
}