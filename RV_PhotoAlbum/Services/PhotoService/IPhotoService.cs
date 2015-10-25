using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RV_PhotoAlbum.Models;

namespace RV_PhotoAlbum.Services.PhotoService
{
    public interface IPhotoService
    {
        void AddPhoto(PhotoModel photo);
        List<PhotoModel> GetPhotos();
        List<string> GetPhotosPaths();
    }
}