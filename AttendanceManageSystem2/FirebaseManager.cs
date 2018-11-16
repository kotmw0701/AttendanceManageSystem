using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceManageSystem2 {
    public class FirebaseManager {

        #region Singleton

        public static FirebaseManager Instance { get; } = new FirebaseManager();

        private FirebaseManager() { }

        #endregion

        private FirebaseAuthLink _authLink;

        private FirestoreDb db;
        private CollectionReference collection;

        #region アカウント

        public string Email { get; set; }

        public SecureString Password { get; set; }

        #endregion



        #region 認証

        public async Task SignInAsync() {
            try {
                if(Email == null || Password == null) {
                    MessageBox.Show("値を入れてください", "エラー", MessageBoxButton.OK);
                    return;
                }
                var auth = new FirebaseAuthProvider(new FirebaseConfig(Properties.Settings.Default.Token));

                this._authLink = await auth.SignInWithEmailAndPasswordAsync(this.Email, Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(this.Password)));

                MessageBox.Show("ログインしました(/・ω・)/", "いんふぉめーそん", MessageBoxButton.OK);

                //雑だけどちょっとここ置いとく
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Properties.Settings.Default.JsonPath);
                
                db = FirestoreDb.Create(Properties.Settings.Default.ProjectID);
                collection = db.Collection("students");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            } catch (FirebaseAuthException e) {
                Console.WriteLine(e.Reason);
                string errorMessage = "";
                switch (e.Reason.ToString()) {
                    case "UnknownEmailAddress":
                        errorMessage = "そのメールアドレスは登録されていません。";
                        break;
                    case "InvalidEmailAddress":
                        errorMessage = "無効なメールアドレスです。";
                        break;
                    case "WrongPassword":
                        errorMessage = "パスワードが間違っています。";
                        break;
                    default:
                        errorMessage = "不明なエラーです。";
                        break;
                }
                MessageBox.Show(errorMessage, "ログインエラー", MessageBoxButton.OK);
            }
        }

        public async Task SignUpAsync() {
            try {
                if (Email == null || Password == null) {
                    MessageBox.Show("値を入れてください", "エラー", MessageBoxButton.OK);
                    return;
                }
                var auth = new FirebaseAuthProvider(new FirebaseConfig(Properties.Settings.Default.Token));

                this._authLink = await auth.CreateUserWithEmailAndPasswordAsync(this.Email, Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(this.Password)));


                MessageBox.Show("アカウント作成に成功しました", "", MessageBoxButton.OK);

            } catch (FirebaseAuthException e) {
                MessageBox.Show("アカウント作成が出来ません\r\n" + e.Reason, "エラー", MessageBoxButton.OK);
            }
        }

        #endregion

        public async Task PushDataBase(string studentID, DateTime time) {
            await collection.AddAsync(new { StudentID = studentID, Time = time.ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }
}
