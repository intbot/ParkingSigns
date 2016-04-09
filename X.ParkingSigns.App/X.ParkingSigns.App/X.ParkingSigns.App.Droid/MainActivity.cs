using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Threading.Tasks;
using Plugin.Geolocator;

namespace X.ParkingSigns.App.Droid
{
	[Activity (Label = "Parking Signs", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap googleMap;
        private MapFragment _mapFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false);
                    //.InvokeMapToolbarEnabled(true)
                    //.InvokeCompassEnabled(true);

                // Make it lite google map. Would be faster
                //mapOptions.InvokeLiteMode(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
            }

            _mapFragment.GetMapAsync(this);
        }

        public async void OnMapReady(GoogleMap map)
        {
            googleMap = map;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MyLocationEnabled = true;
            googleMap.UiSettings.MyLocationButtonEnabled = true;
            //googleMap.UiSettings.MapToolbarEnabled = true;
            //googleMap.UiSettings.ZoomControlsEnabled = true;

            await RefreshData();
        }

        // TODO: This method should be called to refresh map when needed. E.g. Zoom/Pan etc
        async Task RefreshData()
        {
            var center = new LatLng(40.728157, -74.077642); // TODO: As of now center of JC - Have a search bar connects to google API/google map API geocoding

            var locator = CrossGeolocator.Current;
            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled) // If we know where you are.
            {
                locator.DesiredAccuracy = 100;
                var myPosition = await locator.GetPositionAsync(timeoutMilliseconds: 60000);
                center = new LatLng(myPosition.Latitude, myPosition.Longitude); // Override

                // TODO: This is not good as this gets zoomed along with map
                //var myPositionIndicator = new CircleOptions();
                //myPositionIndicator.InvokeCenter(center);
                //myPositionIndicator.InvokeRadius(16);
                //myPositionIndicator.InvokeStrokeWidth(0);
                //myPositionIndicator.InvokeFillColor(Resources.GetColor(Resource.Color.lightblue));
                //googleMap.AddCircle(myPositionIndicator);
            }

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(center, googleMap.MaxZoomLevel);
            googleMap.MoveCamera(cameraUpdate);

            // Get markers on viewport
            var ado = new JsonAdo();
            var signs = ado.GetSigns(); // TODO: Pass lefttop and right bottom to filter out only markers visible in the viewport

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
        }

        

        

        //private void SetupAnimateToButton()
        //{
        //    Button animateButton = FindViewById<Button>(Resource.Id.animateButton);
        //    animateButton.Click += (sender, e) => {
        //        // Move the camera to the Passchendaele Memorial in Belgium.
        //        CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
        //        builder.Target(Passchendaele);
        //        builder.Zoom(18);
        //        builder.Bearing(155);
        //        builder.Tilt(65);
        //        CameraPosition cameraPosition = builder.Build();

        //        // AnimateCamera provides a smooth, animation effect while moving
        //        // the camera to the the position.

        //        _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        //    };
        //}

        //private void SetupZoomInButton()
        //{
        //    Button zoomInButton = FindViewById<Button>(Resource.Id.zoomInButton);
        //    zoomInButton.Click += (sender, e) => { _map.AnimateCamera(CameraUpdateFactory.ZoomIn()); };
        //}

        //private void SetupZoomOutButton()
        //{
        //    Button zoomOutButton = FindViewById<Button>(Resource.Id.zoomOutButton);
        //    zoomOutButton.Click += (sender, e) => { _map.AnimateCamera(CameraUpdateFactory.ZoomOut()); };
        //}
    }

    //protected override void OnCreate (Bundle bundle)
    //{
    //	base.OnCreate (bundle);

    //          //var _myMapFragment = MapFragment.NewInstance();
    //          //FragmentTransaction tx = FragmentManager.BeginTransaction();
    //          //tx.Add(Resource.Id.my_mapfragment_container, _myMapFragment);
    //          //tx.Commit();

    //          // Set our view from the "main" layout resource
    //          SetContentView (Resource.Layout.Main);

    //          MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
    //          GoogleMap map = mapFrag.Map;
    //          map.SetOnMapLoadedCallback
    //          if (map != null)
    //          {
    //              // The GoogleMap object is ready to go.
    //              map.MapType = GoogleMap.MapTypeSatellite;
    //              map.UiSettings.ZoomControlsEnabled = true;
    //              map.UiSettings.CompassEnabled = true;
    //              map.MoveCamera(CameraUpdateFactory.ZoomIn());

    //              MarkerOptions markerOpt1 = new MarkerOptions();
    //              markerOpt1.SetPosition(new LatLng(50.379444, 2.773611));
    //              markerOpt1.SetTitle("Vimy Ridge");
    //              markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
    //              map.AddMarker(markerOpt1);
    //          }
    //      }
}


