//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.UI.Xaml.Data;
//using SocialMediaApplication.Models.BusinessModels;

//namespace SocialMediaApplication.Util
//{
//    //public class IncrementalLoadingCollection
//    //{
//    //}
//    public class IncrementalLoadingCollection : ObservableCollection<PostBObj>, ISupportIncrementalLoading
//    {
//        uint x = 0; //just for the example
//        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
//        {
//            throw new NotImplementedException();
//        }

//        public bool HasMoreItems => x < 10000; //maximum

//        //the count is the number requested
//        //public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
//        //{
//        //    return AsyncInfo.Run(async cancelToken =>
//        //    {
//        //        //here you need to do your loading
//        //        for (var c = x; c < x + count; c++)
//        //        {
//        //            //add your newly loaded item to the collection
//        //            Add(new TextPostBObj()
//        //            {
//        //                Text = c.ToString()
//        //            });
//        //        }

//        //        x += count;
//        //        //return the actual number of items loaded (here it's just maxed)
//        //        return new LoadMoreItemsResult { Count = count };
//        //    });
//        //}
//    }

//}
