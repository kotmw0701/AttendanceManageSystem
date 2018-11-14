using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManageSystem2 {
    class TestData {
        public string Value {
            get {
                return _testData;
            } set {
                if (value != null) _testData = value;
            }
        }
        private string _testData;
    }
}
