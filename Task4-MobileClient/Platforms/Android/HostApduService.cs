using Android.App;
using Android.Content;
using Android.Nfc.CardEmulators;
using Android.OS;
using Android.Runtime;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiNfcHceBootStrapExample.Platforms.Android
{
    [Service(Enabled = true, Exported = true, Permission = "android.permission.BIND_NFC_SERVICE")]
    [IntentFilter([ServiceInterface])]
    [MetaData(ServiceMetaData, Resource = "@xml/apduservice")]
    public class HostApduService : global::Android.Nfc.CardEmulators.HostApduService
    {
        static ApduDispatcher ApduDispatcher;
        Stopwatch m_timer;
        bool m_initialized;        

        // serialized data
        
        public static void FirstTimeAppStartInitialization()
        {
            ApduDispatcher = new ApduDispatcher();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            if (m_initialized == false)
            {
                Deseralize();
                m_initialized = true;
            }
        }

        private void Deseralize()
        {
            
        }

        public override void OnDeactivated([GeneratedEnum] DeactivationReason reason)
        {   
            
        }

        public override byte[] ProcessCommandApdu(byte[] commandApdu, Bundle extras)
        {
            string uiMessage = null;
            long startTime = 0, endTime = 0;
            List<byte> answer = null;

            try
            {
                if (m_timer == null)
                {
                    m_timer = new Stopwatch();
                }

                m_timer.Start();
                startTime = m_timer.ElapsedMilliseconds;                    

                if (Util.ByteArrayStartsWith(commandApdu, [0x00, 0xA4, 0x04]))
                {
                    m_timer.Reset();
                    m_timer.Start();
                    startTime = m_timer.ElapsedMilliseconds;
                }

                (answer, uiMessage) = ApduDispatcher.Process(commandApdu);                    
            }
            finally
            {
                m_timer.Stop();
                endTime = m_timer.ElapsedMilliseconds;

                long commandExecTime = endTime - startTime;
                WeakReferenceMessenger.Default.Send(new LoggedInStrChangedMessage(commandApdu, answer.ToArray(), commandExecTime, m_timer.ElapsedMilliseconds, uiMessage));
            }                               

            return answer.ToArray();
                        
        }
    }
    
}
