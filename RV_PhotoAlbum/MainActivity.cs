using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Support.V7.Widget;
using Android.Views;
using Java.IO;
using RV_PhotoAlbum.Models;
using RV_PhotoAlbum.Services.PhotoService;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace RV_PhotoAlbum
{
    [Activity(Label = "Private photos", MainLauncher = true, Icon = "@drawable/playboy")]
    public class MainActivity : Activity
    {
        public static File _file;
        public static File _dir;
        private IPhotoService _photoService;
        private RecyclerView.Adapter myAdapter;
        private StaggeredGridLayoutManager mygridLayoutManager;
        private RecyclerView.LayoutManager myLayoutManager;
        private RecyclerView myRecyclerView;
        private List<PhotoModel> photos;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _photoService = new PhotoService();
            photos = _photoService.GetPhotos();

            #region IsAvialableCamera?

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
            }

            #endregion

            #region ResouleViewInitialization

            myRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            //Create layout manager
            //myLayoutManager = new LinearLayoutManager(this);
            mygridLayoutManager = new StaggeredGridLayoutManager(3, StaggeredGridLayoutManager.Vertical);
            // Attach the layout manager to the recycler view
            myRecyclerView.SetLayoutManager(mygridLayoutManager);
            myAdapter = new RecyclerAdapter(photos);
            myRecyclerView.SetItemAnimator(new DefaultItemAnimator {MoveDuration = 501, AddDuration = 1000});
            myRecyclerView.SetAdapter(myAdapter);

            #endregion
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                var contentUri = Uri.FromFile(_file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);
                var pm = new PhotoModel {DateCreated = DateTime.Now, Path = contentUri.Path};
                photos.Add(pm);
                myAdapter.NotifyItemInserted(photos.Count - 1);
                _photoService.AddPhoto(pm);
            }
            GC.Collect();
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "PrivatePhotos");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            var availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            _file = new File(_dir, string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add:
                    //Add button clicked
                    if (IsThereAnAppToTakePictures())
                    {
                        TakeAPicture();
                    }
                    return true;

                case Resource.Id.discard:
                    //Discard button clicked
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}