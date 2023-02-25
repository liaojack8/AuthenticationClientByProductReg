namespace AuthenticationClientByAvoidKeypro
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class LanguagePack
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal LanguagePack()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("AuthenticationClientByAvoidKeypro.LanguagePack", typeof(LanguagePack).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => 
                resourceCulture;
            set => 
                (resourceCulture = value);
        }

        internal static string AuthenticationFails =>
            ResourceManager.GetString("AuthenticationFails", resourceCulture);

        internal static string AuthenticationSuccessful =>
            ResourceManager.GetString("AuthenticationSuccessful", resourceCulture);

        internal static string AuthorizeNumber =>
            ResourceManager.GetString("AuthorizeNumber", resourceCulture);

        internal static string Enterprise =>
            ResourceManager.GetString("Enterprise", resourceCulture);

        internal static string GetDeviceInformationEmpty =>
            ResourceManager.GetString("GetDeviceInformationEmpty", resourceCulture);

        internal static string GetDeviceInformationError =>
            ResourceManager.GetString("GetDeviceInformationError", resourceCulture);

        internal static string MFP =>
            ResourceManager.GetString("MFP", resourceCulture);

        internal static string NotApplicable =>
            ResourceManager.GetString("NotApplicable", resourceCulture);

        internal static string OCR =>
            ResourceManager.GetString("OCR", resourceCulture);

        internal static string Professional =>
            ResourceManager.GetString("Professional", resourceCulture);

        internal static string ReleaseTime =>
            ResourceManager.GetString("ReleaseTime", resourceCulture);

        internal static string SNCover =>
            ResourceManager.GetString("SNCover", resourceCulture);

        internal static string SNExist =>
            ResourceManager.GetString("SNExist", resourceCulture);

        internal static string SNExpired =>
            ResourceManager.GetString("SNExpired", resourceCulture);

        internal static string SNIncorrectly =>
            ResourceManager.GetString("SNIncorrectly", resourceCulture);

        internal static string SNNew =>
            ResourceManager.GetString("SNNew", resourceCulture);

        internal static string SNNotMatch =>
            ResourceManager.GetString("SNNotMatch", resourceCulture);

        internal static string SNOld =>
            ResourceManager.GetString("SNOld", resourceCulture);

        internal static string SNSame =>
            ResourceManager.GetString("SNSame", resourceCulture);

        internal static string Standard =>
            ResourceManager.GetString("Standard", resourceCulture);

        internal static string TrialExpirationTime =>
            ResourceManager.GetString("TrialExpirationTime", resourceCulture);

        internal static string WithSmartMonitor =>
            ResourceManager.GetString("WithSmartMonitor", resourceCulture);
    }
}

