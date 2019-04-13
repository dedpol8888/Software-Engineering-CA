using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsFileApplication
{
    public class comboboxItems
    {
        public string _Name;
        public int _Id;

      public comboboxItems(string name, int id)
        {
            _Name = name;
            _Id = id;
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
    }
}
