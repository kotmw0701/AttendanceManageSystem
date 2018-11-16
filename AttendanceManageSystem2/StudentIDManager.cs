using FelicaLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManageSystem2 {
    class StudentIDManager : INotifyPropertyChanged{
        #region 変数

        public string StudentID {
            get {
                return _studentId;
            }
            set {
                _studentId = value;
                onPropertyChanged("StudentID");
            }
        }

        public string Time {
            get {
                return _time;
            }
            set {
                _time = value;
                onPropertyChanged("Time");
            }
        }

        private string _studentId, _time;
        #endregion

        /// <summary>
        /// 定周期処理
        /// </summary>
        public async Task Start() {
            string prevId = "";
            //スケジューラーをキャンセルするToken(？)
            var token = CancellationToken.None;
            using (Felica felica = new Felica()) {
                while (!token.IsCancellationRequested) {
                //引数1 : 遅延処理
                //引数2 : 非同期したい処理の内容
                    await Task.WhenAll(Task.Delay(50), Task.Run(async () => {
                        string studentId = felica.GetStudentID();
                        if (!prevId.Equals(studentId)) {
                            if (studentId == "") {
                                StudentID = "";
                                Time = "";
                            } else {
                                StudentID = studentId;
                                DateTime dateTime = DateTime.Now;
                                Time = dateTime.ToString("HH時mm分ss秒");
                                await FirebaseManager.Instance.PushDataBase(studentId, dateTime);
                                Console.WriteLine(StudentID + " : " + dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                        prevId = studentId;
                    }, token));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string propertyValue) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyValue));
        }
    }
}
