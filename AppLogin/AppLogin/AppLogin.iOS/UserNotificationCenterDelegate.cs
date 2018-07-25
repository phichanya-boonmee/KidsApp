using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace AppLogin.iOS
{
    class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            //base.WillPresentNotification(center, notification, completionHandler);
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

    }
}