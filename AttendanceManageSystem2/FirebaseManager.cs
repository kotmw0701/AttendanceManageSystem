using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceManageSystem2 {
    public class FirebaseManager {

        private FirebaseAuthLink _authLink;

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

                await Test();

                /*MainWindow mainWindow = new MainWindow();
                mainWindow.Show();*/
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

        public ChildQuery GetDatabaseQuery(String path = null) {
            if (_authLink == null)
                throw new NullReferenceException();
            return new FirebaseClient(
                "https://is12-a4cef.firebaseio.com",
                new FirebaseOptions {
                    AuthTokenAsyncFactory = () => Task.FromResult(this._authLink.FirebaseToken),
                })
                .Child(path ?? "Test");
        }

        public async Task UploadTextToDatabaseAsync() {
            try {
                var query = GetDatabaseQuery("学籍番号じゃね");
                await query.PostAsync(new TestData() { Value = "データベースさん聞こえてますかー" });

                MessageBox.Show("データベースに投げることに成功しました", "成功", MessageBoxButton.OK);
            } catch (FirebaseException e) {
                MessageBox.Show(e.ResponseData, "エラー", MessageBoxButton.OK);
            } catch {
                MessageBox.Show("例外が発生しました。", "エラー", MessageBoxButton.OK);
            }
        }

        public async Task Test() {
            FirestoreDb db = FirestoreDb.Create("is12-a4cef");
            CollectionReference collection = db.Collection("users");
            DocumentReference document = await collection.AddAsync(new { Name = new { First = "Ada", Last = "Lovelace" }, Born = 1815 });

            DocumentSnapshot snapshot = await document.GetSnapshotAsync();

            Console.WriteLine(snapshot.GetValue<string>("Name.First"));
            Console.WriteLine(snapshot.GetValue<string>("Name.Last"));
            Console.WriteLine(snapshot.GetValue<int>("Born"));

            Dictionary<string, object> data = snapshot.ToDictionary();
            Dictionary<String, object> name = (Dictionary<string, object>) data["name"];
            Console.WriteLine(name["First"]);
            Console.WriteLine(name["Last"]);

            Query query = collection.WhereLessThan("Born", 1900);
            QuerySnapshot querySnapShot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot queryResult in querySnapShot.Documents) {
                string firstName = queryResult.GetValue<string>("Name.First");
                string lastName = queryResult.GetValue<string>("Name.Last");
                int born = queryResult.GetValue<int>("Born");
                Console.WriteLine($"{firstName} {lastName}; born {born}");
            }
        }
    }
}
