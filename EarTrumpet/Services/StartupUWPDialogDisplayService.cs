﻿using EarTrumpet.Extensions;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Foundation.Collections;

namespace EarTrumpet.Services
{
    class StartupUWPDialogDisplayService
    {
        private static string FirstRunKey = "hasShownFirstRun";
        private static string CurrentVersionKey = "currentVersion";
        
        private static IPropertySet LocalSettings => Windows.Storage.ApplicationData.Current.LocalSettings.Values;

        internal static void ShowIfAppropriate()
        {
            ShowWelcomeIfAppropriate();
            ShowWhatsNewIfAppropriate();

            App.Current.Exit += App_Exit;
        }

        private static void App_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            try
            {
                foreach (var proc in Process.GetProcessesByName("EarTrumpet.UWP"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        internal static void ShowWelcomeIfAppropriate()
        {
            if (App.Current.HasIdentity())
            {
                if (!LocalSettings.ContainsKey(FirstRunKey))
                {
                    LocalSettings[FirstRunKey] = true;
                    try
                    {
                        Process.Start("eartrumpet://welcome");
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
        }

        internal static void ShowWhatsNewIfAppropriate()
        {
            if (App.Current.HasIdentity())
            {
                var currentVersion = Package.Current.Id.Version.ToVersionString();
                var lastVersion = LocalSettings[CurrentVersionKey];
                if ((lastVersion == null || currentVersion != (string)lastVersion))
                {
                    LocalSettings[CurrentVersionKey] = currentVersion;

                    if (LocalSettings.ContainsKey(FirstRunKey))
                    {
                        try
                        {
                            Process.Start("eartrumpet:");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }
        }
    }
}