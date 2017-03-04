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
    class MeterHistoryAdapter : BaseAdapter<Data>
    {

        Activity context;
        List<Data> items;
        public MeterHistoryAdapter(Activity context, List<Data> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override Data this[int position]
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
        public void Insert(int index, Data item)
        {
            items.Insert(index, item);
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.meterHistoryLayout, null);
            view.FindViewById<TextView>(Resource.Id.dateText).Text = items[position].date_published.ToLongDateString();
            view.FindViewById<TextView>(Resource.Id.valueText).Text = items[position].meter_data.ToString()+" киловатт/час";

            //         view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(items[position].meter_description.ImageResourceId);
            //    view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = ;
            return view;
        }

        internal void InsertRange(List<Data> meterList)
        {
            items.InsertRange(0, meterList);
        }
    }
}