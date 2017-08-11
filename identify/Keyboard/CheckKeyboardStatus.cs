using System;
using System.Timers;
using identify.Logs;

namespace identify.Keyboard
{
    class CheckKeyboardStatus
    {
        public static int KeyPressCount { get; set; }

        public CheckKeyboardStatus()
        {
            var keyboardTimer = new System.Timers.Timer(60000);
            keyboardTimer.Elapsed += new ElapsedEventHandler(OnKeyboardCheck);
            keyboardTimer.Enabled = true;
        }
        
        private void OnKeyboardCheck(object sender, ElapsedEventArgs e)
        {
            if (KeyPressCount >= 1)
            {
                Logging.LogEntry(Notification.Event.EventType.Keyboard, String.Format("Keys Pressed: {0}", KeyPressCount));
            }

            KeyPressCount = 0;  // Reset Mouse counter
        }
    }
}
