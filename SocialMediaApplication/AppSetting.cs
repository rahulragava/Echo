//using SocialMediaApplication.Database.DatabaseAdapter.Contract;
//using SocialMediaApplication.Database.DatabaseAdapter;
//using SocialMediaApplication.Database.DatabaseHandler;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Storage;
//using SocialMediaApplication.Models.BusinessModels;

//namespace SocialMediaApplication
//{
//    public class AppSetting
//    {
//        private static AppSetting Instance { get; set; }
//        private static readonly object PadLock = new object();
//        //private static readonly _localSettings = Windows.Storage.ApplicationData.

//        private AppSetting() { }

//        public static AppSetting GetInstance
//        {
//            get
//            {
//                if (Instance == null)
//                {
//                    lock (PadLock)
//                    {
//                        if (Instance == null)
//                        {
//                            Instance = new AppSetting();
//                        }
//                    }
//                }
//                return Instance;
//            }
//        }

//        public UserBObj GetLoggedUserBObj()
//        {

//        }

//        public void Logout()
//        {

//        }

//        public void SignIn()
//        {

//        }

//        public void SignUp()
//        {

//        }

//    }
//}
