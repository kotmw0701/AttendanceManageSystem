using AttendanceManageSystem2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManageSystem2 {
    class MainModel {

        private readonly FirebaseManager _firebaseManager = new FirebaseManager();

        #region アカウント

        public string Email {
            get {
                return _firebaseManager.Email;
            }
            set {
                if(value != null)
                    _firebaseManager.Email = value;
            }
        }

        public SecureString Password {
            get {
                return _firebaseManager.Password;
            }
            set {
                if (value != null)
                    _firebaseManager.Password = value;
            }
        }

        #endregion

        private RelayCommand _signInCommand;
        public RelayCommand SignInCommand {
            get {
                return this._signInCommand = this._signInCommand ?? new RelayCommand(async () => {
                    await _firebaseManager.SignInAsync();
                });
            }
        }

        private RelayCommand _signUpCommand;
        public RelayCommand SignUpCommand {
            get {   
                return this._signUpCommand = this._signUpCommand ?? new RelayCommand(async () => {
                    await _firebaseManager.SignUpAsync();
                });
            }
        }
    }
}
