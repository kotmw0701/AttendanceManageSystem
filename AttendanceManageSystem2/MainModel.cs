using AttendanceManageSystem2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManageSystem2 {
    class MainModel {

        #region アカウント

        public string Email {
            get {
                return FirebaseManager.Instance.Email;
            }
            set {
                if(value != null)
                    FirebaseManager.Instance.Email = value;
            }
        }

        public SecureString Password {
            get {
                return FirebaseManager.Instance.Password;
            }
            set {
                if (value != null)
                    FirebaseManager.Instance.Password = value;
            }
        }

        #endregion

        private RelayCommand _signInCommand;
        public RelayCommand SignInCommand {
            get {
                return this._signInCommand = this._signInCommand ?? new RelayCommand(async () => {
                    await FirebaseManager.Instance.SignInAsync();
                });
            }
        }

        private RelayCommand _signUpCommand;
        public RelayCommand SignUpCommand {
            get {   
                return this._signUpCommand = this._signUpCommand ?? new RelayCommand(async () => {
                    await FirebaseManager.Instance.SignUpAsync();
                });
            }
        }
    }
}
