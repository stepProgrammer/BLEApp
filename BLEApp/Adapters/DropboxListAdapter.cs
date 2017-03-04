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
using BLEApp.Models;
namespace BLEApp.Adapters
{
    class DropboxListAdapter : BaseAdapter<string>
    {

        Activity context;
        List<string> items;
        public DropboxListAdapter(Activity context, List<string> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override string this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public void Clear()
        {
            items.Clear();
            this.NotifyDataSetChanged();
        }
        public void Insert(int index, string item)
        {
            items.Insert(index, item);
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];

            //         view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(items[position].meter_description.ImageResourceId);
            //    view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = ;
            return view;
        }

        internal void InsertRange(List<string> meterList)
        {
            items.InsertRange(0, meterList);
        }
    }
}