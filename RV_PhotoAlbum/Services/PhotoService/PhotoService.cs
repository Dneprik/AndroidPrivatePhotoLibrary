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
using RV_PhotoAlbum.Services.SQLite;
using SQLite;

namespace RV_PhotoAlbum.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        public readonly SQLiteConnection _connection;

        public PhotoService()
        {
            SQLite_Android sql = new SQLite_Android();;
            _connection = sql.GetConnection();// DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<PhotoModel>();
        }

        public void AddPhoto(PhotoModel photo)
        {
            _connection.Insert(new PhotoModel { Path = photo.Path, DateCreated = photo.DateCreated });
        }

        public List<PhotoModel> GetPhotos()
        {
            //   _connection.DeleteAll<PhotoModel>();
            return _connection.Table<PhotoModel>().ToList();
        }

        public List<string> GetPhotosPaths()
        {
            return _connection.Table<PhotoModel>().ToList().Select(t => t.Path).ToList();
        }
    }
}