using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    public class Dummy
    {
        public class DummyEventArgs : EventArgs
        {
            public string Operation;
        }


        public event EventHandler ValRead;
        public event EventHandler<DummyEventArgs> ValChanged;

        private int val;

        public int Val 
        { 
            get
            {
                OnValRead();
                return val;
            }
            set 
            {
                if (val != value)
                {
                    OnValChanged(new DummyEventArgs() { Operation = "WRITE" });
                    val = value;
                }
                
            }
        }

        protected virtual void OnValRead()
        {
            if (ValRead != null)
            {
                ValRead(this, new EventArgs());
            };
        }
        protected virtual void OnValChanged(DummyEventArgs e)
        {
            if (ValChanged != null)
            {
                ValChanged(this, e);
            };
        }
    }
}
