using System.Collections.Generic;
using Android.Net;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using RV_PhotoAlbum.Models;

namespace RV_PhotoAlbum
{
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private readonly int _itemCount;
        private readonly List<PhotoModel> photoModels;

        public RecyclerAdapter(List<PhotoModel> photoModels)
        {
            this.photoModels = photoModels;
        }

        public override int ItemCount
        {
            get { return photoModels.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var myHolder = holder as MyView;
            var uri = Uri.WithAppendedPath(Uri.Empty, photoModels[position].Path);
            myHolder.image.SetImageURI(uri);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row, parent, false);
            var image = row.FindViewById<ImageView>(Resource.Id.myImage);
            var view = new MyView(row) {image = image};
            return view;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public MyView(View itemView) : base(itemView)
            {
                mMainView = itemView;
            }

            public View mMainView { get; set; }
            public ImageView image { get; set; }
        }
    }
}