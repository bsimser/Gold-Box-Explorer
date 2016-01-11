using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GoldBoxExplorer.Lib.Exceptions
{
    public class UnhandledExceptionManager
    {
        private static bool _blnLogToFileOk;
        private static bool _blnLogToScreenshotOk;
        private static Assembly _objParentAssembly;
        private static string _strException;
        private static string _strLogFullPath;
        private static string _strScreenshotFullPath;

        private UnhandledExceptionManager()
        {
        }

        static UnhandledExceptionManager()
        {
            ScreenshotImageFormat = ImageFormat.Png;
        }

        public static void AddHandler()
        {
            LoadConfigSettings();
            ParentAssembly();
            Application.ThreadException -= ThreadExceptionHandler;
            Application.ThreadException += ThreadExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
        }

        private static DateTime AssemblyBuildDate(Assembly objAssembly, bool blnForceFileDate = false)
        {
            var objVersion = objAssembly.GetName().Version;
            
            if (blnForceFileDate)
            {
                return AssemblyFileTime(objAssembly);
            }
            
            var dtBuild = DateTime.Parse("01/01/2000").AddDays(objVersion.Build).AddSeconds(objVersion.Revision * 2);
            
            if (TimeZone.IsDaylightSavingTime(DateTime.Now, TimeZone.CurrentTimeZone.GetDaylightChanges(DateTime.Now.Year)))
            {
                dtBuild = dtBuild.AddHours(1.0);
            }
            
            if (((DateTime.Compare(dtBuild, DateTime.Now) > 0) | (objVersion.Build < 730)) | (objVersion.Revision == 0))
            {
                dtBuild = AssemblyFileTime(objAssembly);
            }
            
            return dtBuild;
        }

        private static DateTime AssemblyFileTime(Assembly objAssembly)
        {
            var assemblyFileTime = new DateTime();
            
            try
            {
                if (objAssembly != null && objAssembly.Location != null)
                        return File.GetLastWriteTime(objAssembly.Location);
            }
            catch (Exception)
            {
                assemblyFileTime = DateTime.MaxValue;
                return assemblyFileTime;
            }

            return assemblyFileTime;
        }

        [DllImport("gdi32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        private static extern int BitBlt(int hDestDc, int x, int y, int nWidth, int nHeight, int hSrcDc, int xSrc, int ySrc, int dwRop);

        private static void BitmapToJpeg(Image objBitmap, string strFilename, long lngCompression = 0x4bL)
        {
            var objEncoderParameters = new EncoderParameters(1);
            var objImageCodecInfo = GetEncoderInfo("image/jpeg");
            objEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, lngCompression);
            objBitmap.Save(strFilename, objImageCodecInfo, objEncoderParameters);
        }

        private static string CurrentEnvironmentIdentity()
        {
            string currentEnvironmentIdentity;

            try
            {
                currentEnvironmentIdentity = Environment.UserDomainName + @"\" + Environment.UserName;
            }
            catch (Exception)
            {
                return "";
            }

            return currentEnvironmentIdentity;
        }

        private static string CurrentWindowsIdentity()
        {
            try
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string EnhancedStackTrace()
        {
            var objStackTrace = new StackTrace(true);
            return EnhancedStackTrace(objStackTrace, "ExceptionManager");
        }

        private static string EnhancedStackTrace(Exception objException)
        {
            var objStackTrace = new StackTrace(objException, true);
            return EnhancedStackTrace(objStackTrace);
        }

        private static string EnhancedStackTrace(StackTrace objStackTrace, string strSkipClassName = "")
        {
            var sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);
            
            var objStackTraceFrameCount = objStackTrace.FrameCount - 1;
            
            for (var intFrame = 0; intFrame <= objStackTraceFrameCount; intFrame++)
            {
                var sf = objStackTrace.GetFrame(intFrame);
                MemberInfo mi = sf.GetMethod();
                
                if ((((strSkipClassName != "") && (mi.DeclaringType.Name.IndexOf(strSkipClassName) > -1)) ? 1 : 0) == 0)
                {
                    sb.Append(StackFrameToString(sf));
                }
            }
            
            sb.Append(Environment.NewLine);
            
            return sb.ToString();
        }

        private static void ExceptionToFile()
        {
            _strLogFullPath = GetApplicationPath() + "UnhandledExceptionLog.txt";

            try
            {
                var objStreamWriter = new StreamWriter(_strLogFullPath, true);
                objStreamWriter.Write(_strException);
                objStreamWriter.WriteLine();
                objStreamWriter.Close();
                _blnLogToFileOk = true;
            }
            catch (Exception)
            {
                _blnLogToFileOk = false;
            }
        }

        private static void ExceptionToScreenshot()
        {
            try
            {
                TakeScreenshotPrivate(GetApplicationPath() + "UnhandledException");
                _blnLogToScreenshotOk = true;
            }
            catch (Exception)
            {
                _blnLogToScreenshotOk = false;
            }
        }

        internal static string ExceptionToString(Exception objException)
        {
            var objStringBuilder = new StringBuilder();

            if (objException.InnerException != null)
            {
                objStringBuilder.Append("(Inner Exception)");
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append(ExceptionToString(objException.InnerException));
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append("(Outer Exception)");
                objStringBuilder.Append(Environment.NewLine);
            }
            objStringBuilder.Append(SysInfoToString());
            objStringBuilder.Append("Exception Source:      ");
            
            try
            {
                objStringBuilder.Append(objException.Source);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Exception Type:        ");
            try
            {
                objStringBuilder.Append(objException.GetType().FullName);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Exception Message:     ");
            try
            {
                objStringBuilder.Append(objException.Message);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Exception Target Site: ");
            try
            {
                objStringBuilder.Append(objException.TargetSite.Name);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            try
            {
                string x = EnhancedStackTrace(objException);
                objStringBuilder.Append(x);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            
            return objStringBuilder.ToString();
        }

        private static void ExceptionToUi()
        {
            string strHowUserAffected = KillAppOnException ? "When you click OK, (app) will close." : "The action you requested was not performed.";

            HandledExceptionManager.ShowDialog("There was an unexpected error in (app). This may be due to a programming bug.", 
                    strHowUserAffected, "Restart (app), and try repeating your last action. Try alternative methods of performing the same action.", 
                    FormatExceptionForUser(), MessageBoxButtons.OK, MessageBoxIcon.Hand, 
                    HandledExceptionManager.UserErrorDefaultButton.Default);
        }

        private static string FormatExceptionForUser()
        {
            var objStringBuilder = new StringBuilder();

            const string strBullet = "•";

            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("The following information about the error was automatically captured: ");
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            
            if (TakeScreenshot)
            {
                objStringBuilder.Append(" ");
                objStringBuilder.Append(strBullet);
                objStringBuilder.Append(" ");
                if (_blnLogToScreenshotOk)
                {
                    objStringBuilder.Append("a screenshot was taken of the desktop at:");
                    objStringBuilder.Append(Environment.NewLine);
                    objStringBuilder.Append("   ");
                    objStringBuilder.Append(_strScreenshotFullPath);
                }
                else
                {
                    objStringBuilder.Append("a screenshot could NOT be taken of the desktop.");
                }
                objStringBuilder.Append(Environment.NewLine);
            }

            if (LogToFile)
            {
                objStringBuilder.Append(" ");
                objStringBuilder.Append(strBullet);
                objStringBuilder.Append(" ");
                objStringBuilder.Append(_blnLogToFileOk
                                            ? "details were written to a text log at:"
                                            : "details could NOT be written to the text log at:");
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append("   ");
                objStringBuilder.Append(_strLogFullPath);
                objStringBuilder.Append(Environment.NewLine);
            }
            
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Detailed error information follows:");
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(_strException);

            return objStringBuilder.ToString();
        }

        private static void GenericExceptionHandler(Exception objException)
        {
            try
            {
                _strException = ExceptionToString(objException);
            }
            catch (Exception ex)
            {
                _strException = string.Format("Error '{0}' while generating exception string", ex.Message);
            }

            using (new WaitCursor())
            {
                try
                {
                    if (TakeScreenshot)
                    {
                        ExceptionToScreenshot();
                    }
                    if (LogToFile)
                    {
                        ExceptionToFile();
                    }
                }
                catch
                {
                }
            }

            if (DisplayDialog)
            {
                ExceptionToUi();
            }

            if (!KillAppOnException) return;
            
            KillApp();
            Application.Exit();
        }

        private static string GetApplicationPath()
        {
            if (ParentAssembly().CodeBase.StartsWith("http://"))
            {
                return (@"c:\" + Regex.Replace(ParentAssembly().CodeBase, "[\\/\\\\\\:\\*\\?\\\"\\<\\>\\|]", "_") + ".");
            }
            return (AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".");
        }

        private static bool GetConfigBoolean(string strKey, bool blnDefault = false)
        {
            string strTemp = null;
            
            try
            {
                if (ConfigurationManager.AppSettings != null)
                    strTemp = ConfigurationManager.AppSettings.Get("UnhandledExceptionManager/" + strKey);
            }
            catch (Exception)
            {
                if (!blnDefault)
                {
                    return false;
                }
                return true;
            }
            
            if (strTemp == null)
            {
                return blnDefault;
            }
            
            switch (strTemp.ToLower())
            {
                case "1":
                case "true":
                    return true;
            }
            
            return false;
        }

        private static string GetCurrentIp()
        {
            try
            {
                return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            }
            catch (Exception)
            {
                const string getCurrentIp = "127.0.0.1";
                return getCurrentIp;
            }
        }

        [DllImport("user32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        private static extern int GetDC(int hwnd);

        private static ImageCodecInfo GetEncoderInfo(string strMimeType)
        {
            var objImageCodecInfo = ImageCodecInfo.GetImageEncoders();
            return objImageCodecInfo.FirstOrDefault(t => t.MimeType == strMimeType);
        }

        private static void KillApp()
        {
            Process.GetCurrentProcess().Kill();
        }

        private static void LoadConfigSettings()
        {
            TakeScreenshot = GetConfigBoolean("TakeScreenshot");
            LogToFile = GetConfigBoolean("LogToFile");
            DisplayDialog = GetConfigBoolean("DisplayDialog", true);
            IgnoreDebugErrors = GetConfigBoolean("IgnoreDebug", true);
            KillAppOnException = GetConfigBoolean("KillAppOnException", true);
        }

        private static Assembly ParentAssembly()
        {
            return _objParentAssembly ??
                   (_objParentAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly());
        }

        [DllImport("user32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        private static extern int ReleaseDC(int hwnd, int hdc);

        private static string StackFrameToString(StackFrame sf)
        {
            var sb = new StringBuilder();
            MemberInfo mi = sf.GetMethod();

            sb.Append("   ");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);
            
            var objParameters = sf.GetMethod().GetParameters();
            sb.Append("(");
            
            var intParam = 0;
            
            foreach (var objParameter in objParameters)
            {
                intParam++;
                if (intParam > 1)
                {
                    sb.Append(", ");
                }
                sb.Append(objParameter.Name);
                sb.Append(" As ");
                sb.Append(objParameter.ParameterType.Name);
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("       ");
            if ((sf.GetFileName() == null) || (sf.GetFileName().Length == 0))
            {
                sb.Append(Path.GetFileName(ParentAssembly().CodeBase));
                sb.Append(": N ");
                sb.Append(string.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(string.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(string.Format("{0:#00}", sf.GetFileColumnNumber()));
                if (sf.GetILOffset() != -1)
                {
                    sb.Append(", IL ");
                    sb.Append(string.Format("{0:#0000}", sf.GetILOffset()));
                }
            }
            sb.Append(Environment.NewLine);
            
            return sb.ToString();
        }

        internal static string SysInfoToString(bool blnIncludeStackTrace = false)
        {
            var objStringBuilder = new StringBuilder();

            objStringBuilder.Append("Date and Time:         ");
            objStringBuilder.Append(DateTime.Now);
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Machine Name:          ");
            try
            {
                objStringBuilder.Append(Environment.MachineName);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("IP Address:            ");
            objStringBuilder.Append(GetCurrentIp());
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Current User:          ");
            objStringBuilder.Append(UserIdentity());
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Application Domain:    ");
            try
            {
                objStringBuilder.Append(AppDomain.CurrentDomain.FriendlyName);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Assembly Codebase:     ");
            try
            {
                objStringBuilder.Append(ParentAssembly().CodeBase);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Assembly Full Name:    ");
            try
            {
                objStringBuilder.Append(ParentAssembly().FullName);
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Assembly Version:      ");
            try
            {
                objStringBuilder.Append(ParentAssembly().GetName().Version.ToString());
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append("Assembly Build Date:   ");
            try
            {
                objStringBuilder.Append(AssemblyBuildDate(ParentAssembly()).ToString());
            }
            catch (Exception e)
            {
                objStringBuilder.Append(e.Message);
            }
            objStringBuilder.Append(Environment.NewLine);
            objStringBuilder.Append(Environment.NewLine);
            if (blnIncludeStackTrace)
            {
                objStringBuilder.Append(EnhancedStackTrace());
            }
            
            return objStringBuilder.ToString();
        }

        private static void TakeScreenshotPrivate(string strFilename)
        {
            var objRectangle = Screen.PrimaryScreen.Bounds;
            var objBitmap = new Bitmap(objRectangle.Right, objRectangle.Bottom);
            var objGraphics = System.Drawing.Graphics.FromImage(objBitmap);
            var hdcSrc = GetDC(0);
            var hdcDest = objGraphics.GetHdc();
            
            BitBlt(hdcDest.ToInt32(), 0, 0, objRectangle.Right, objRectangle.Bottom, hdcSrc, 0, 0, 0xcc0020);
            objGraphics.ReleaseHdc(hdcDest);
            ReleaseDC(0, hdcSrc);
            
            var strFormatExtension = ScreenshotImageFormat.ToString().ToLower();
            
            if (Path.GetExtension(strFilename) != ("." + strFormatExtension))
            {
                strFilename = strFilename + "." + strFormatExtension;
            }
            
            if (strFormatExtension == "jpeg")
            {
                BitmapToJpeg(objBitmap, strFilename, 80L);
            }
            else
            {
                objBitmap.Save(strFilename, ScreenshotImageFormat);
            }
            
            _strScreenshotFullPath = strFilename;
        }

        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            GenericExceptionHandler(e.Exception);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var objException = (Exception) args.ExceptionObject;
            GenericExceptionHandler(objException);
        }

        private static string UserIdentity()
        {
            var strTemp = CurrentWindowsIdentity();
            return strTemp != "" ? strTemp : CurrentEnvironmentIdentity();
        }

        public static bool DisplayDialog { get; set; }

        public static bool IgnoreDebugErrors { get; set; }

        public static bool KillAppOnException { get; set; }

        public static bool LogToFile { get; set; }

        public static ImageFormat ScreenshotImageFormat { get; set; }

        public static bool TakeScreenshot { get; set; }
    }
}