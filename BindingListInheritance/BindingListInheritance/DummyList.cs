using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BindingListInheritance
{
    public class DummyList : BindingList<Dummy>
    {
        public void Fill()
        {
            this.Add(new Dummy() { SomeProperty = "A" });
            this.Add(new Dummy() { SomeProperty = "B" });
            this.Add(new Dummy() { SomeProperty = "C" });
            this.Add(new Dummy() { SomeProperty = "D" });
            this.Add(new Dummy() { SomeProperty = "E" });
            this.Add(new Dummy() { SomeProperty = "F" });
        }
    }
}
