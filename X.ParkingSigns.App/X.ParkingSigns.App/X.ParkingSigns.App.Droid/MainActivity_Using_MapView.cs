using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Geolocator.Plugin;
using System.Threading.Tasks;

namespace X.ParkingSigns.App.Droid
{
    [Activity(Label = "Parking Signs", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap googleMap;
        MapView mapView;
        //private MapFragment _mapFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            //InitMapFragment();
            mapView = FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(bundle);
            //mapView.Visibility = ViewStates.Invisible;
            mapView.GetMapAsync(this);
        }

        public async void OnMapReady(GoogleMap map)
        {
            googleMap = map;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.UiSettings.MyLocationButtonEnabled = true;
            googleMap.UiSettings.MapToolbarEnabled = true;
            googleMap.UiSettings.ZoomControlsEnabled = true;

            await RefreshData();
        }

        //async void is bad, but we are firing and forgetting here, so no worries
        async Task RefreshData()
        {
            if (googleMap == null)
                return;

            try
            {
                var ado = new JsonAdo();
                var signs = ado.GetSigns();

                LatLng position;
                MarkerOptions marker;

                foreach (var sign in signs)
                {
                    position = new LatLng(sign.Latitude, sign.Longitude);
                    marker = new MarkerOptions();
                    marker.SetPosition(position);
                    marker.SetTitle("NO PARKING " + sign.StartTime + " - " + sign.EndTime + " " + sign.DayOfWeek);
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.parkingsign));
                    googleMap.AddMarker(marker);
                }

                var center = new LatLng(40.728157, -74.077642); // Center of JC
                // If we know where you are.
                if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
                {
                    var myPosition = await CrossGeolocator.Current.GetPositionAsync(10000);
                    center = new LatLng(myPosition.Latitude, myPosition.Longitude); // Override
                }

                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(center, googleMap.MaxZoomLevel);
                googleMap.MoveCamera(cameraUpdate);
            }
            finally
            {
            }
        }

        //protected override async void OnResume()
        //{
        //    base.OnResume();
        //    await SetupMapIfNeeded();
        //}

        //private async Task SetupMapIfNeeded()
        //{
        //    if (googleMap == null)
        //    {
        //        googleMap = _mapFragment.Map;
        //        if (googleMap != null)
        //        {
        //            var ado = new JsonAdo();
        //            var signs = ado.GetSigns();

        //            LatLng position;
        //            MarkerOptions marker;

        //            foreach (var sign in signs)
        //            {
        //                position = new LatLng(sign.Latitude, sign.Longitude);
        //                marker = new MarkerOptions();
        //                marker.SetPosition(position);
        //                marker.SetTitle("NO PARKING " + sign.StartTime + " - " + sign.EndTime + " " + sign.DayOfWeek);
        //                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.parkingsign));
        //                googleMap.AddMarker(marker);
        //            }

        //            var center = new LatLng(40.728157, -74.077642); // Center of JC
        //            // If we know where you are.
        //            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
        //            {
        //                var myPosition = await CrossGeolocator.Current.GetPositionAsync(10000);
        //                center = new LatLng(myPosition.Latitude, myPosition.Longitude); // Override
        //            }

        //            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(center, googleMap.MaxZoomLevel);
        //            googleMap.MoveCamera(cameraUpdate);
        //        }
        //    }
        //}

        //private void InitMapFragment()
        //{
        //    _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
        //    if (_mapFragment == null)
        //    {
        //        GoogleMapOptions mapOptions = new GoogleMapOptions()
        //            .InvokeMapType(GoogleMap.MapTypeNormal)
        //            .InvokeZoomControlsEnabled(false)
        //            .InvokeCompassEnabled(true);

        //        // Make it lite google map. Would be faster
        //        mapOptions.InvokeLiteMode(true);

        //        FragmentTransaction fragTx = FragmentManager.BeginTransaction();
        //        _mapFragment = MapFragment.NewInstance(mapOptions);
        //        fragTx.Add(Resource.Id.map, _mapFragment, "map");
        //        fragTx.Commit();
        //    }
        //}
    }
}


