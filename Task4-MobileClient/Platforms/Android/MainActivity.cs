using Android.App;
using Android.Nfc;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.NFC;

namespace EquipmentWatcherMAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    //[IntentFilter([NfcAdapter.ActionNdefDiscovered], Categories = [Intent.CategoryDefault], DataMimeType = MainPage.MIME_TYPE)]
    public class MainActivity : MauiAppCompatActivity
    {
        /*private NfcAdapter _nfcAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CrossNFC.Init(this);

        }

        protected override void OnResume()
        {
            base.OnResume();

            CrossNFC.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            CrossNFC.OnNewIntent(intent);
        }*/
    }
}
