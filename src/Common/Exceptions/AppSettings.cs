using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace GoldBoxExplorer.Lib.Exceptions
{
    public class AppSettings
    {
        private static NameValueCollection _objAssemblyAttribs;
        private static NameValueCollection _objCommandLineArgs;
        private static string _strAppBase;
        private static string _strConfigPath;
        private static string _strRuntimeVersion;
        private static string _strSecurityZone;

        private AppSettings()
        {
        }

        private static DateTime AssemblyBuildDate(Assembly objAssembly, bool blnForceFileDate = false)
        {
            var objVersion = objAssembly.GetName().Version;
            
            if (blnForceFileDate)
            {
                return AssemblyFileTime(objAssembly);
            }
            
            var dtBuild = DateTime.Parse("01/01/2000").AddDays(objVersion.Build).AddSeconds(objVersion.Revision * 2);
            
            if (TimeZone.IsDaylightSavingTime(dtBuild, TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuild.Year)))
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
                    assemblyFileTime = File.GetLastWriteTime(objAssembly.Location);
            }
            catch (Exception)
            {
                assemblyFileTime = DateTime.MaxValue;
                return assemblyFileTime;
            }

            return assemblyFileTime;
        }

        private static NameValueCollection GetAssemblyAttribs()
        {
            var objNameValueCollection = new NameValueCollection();
            var objAssembly = GetEntryAssembly();
            var customAttributes = objAssembly.GetCustomAttributes(false);
            
            foreach (var attribute in customAttributes)
            {
                var objAttribute = RuntimeHelpers.GetObjectValue(attribute);
                var strAttribName = objAttribute.GetType().ToString();
                var strAttribValue = "";
                
                switch (strAttribName)
                {
                    case "System.Reflection.AssemblyTrademarkAttribute":
                        strAttribName = "Trademark";
                        strAttribValue = ((AssemblyTrademarkAttribute) objAttribute).Trademark;
                        break;

                    case "System.Reflection.AssemblyProductAttribute":
                        strAttribName = "Product";
                        strAttribValue = ((AssemblyProductAttribute) objAttribute).Product;
                        break;

                    case "System.Reflection.AssemblyCopyrightAttribute":
                        strAttribName = "Copyright";
                        strAttribValue = ((AssemblyCopyrightAttribute) objAttribute).Copyright;
                        break;

                    case "System.Reflection.AssemblyCompanyAttribute":
                        strAttribName = "Company";
                        strAttribValue = ((AssemblyCompanyAttribute) objAttribute).Company;
                        break;

                    case "System.Reflection.AssemblyTitleAttribute":
                        strAttribName = "Title";
                        strAttribValue = ((AssemblyTitleAttribute) objAttribute).Title;
                        break;

                    case "System.Reflection.AssemblyDescriptionAttribute":
                        strAttribName = "Description";
                        strAttribValue = ((AssemblyDescriptionAttribute) objAttribute).Description;
                        break;
                }

                if ((strAttribValue != "") && (objNameValueCollection[strAttribName] == null))
                {
                    objNameValueCollection.Add(strAttribName, strAttribValue);
                }
            }
            
            objNameValueCollection.Add("CodeBase", objAssembly.CodeBase.Replace("file:///", ""));
            objNameValueCollection.Add("BuildDate", AssemblyBuildDate(objAssembly).ToString());
            objNameValueCollection.Add("Version", objAssembly.GetName().Version.ToString());
            objNameValueCollection.Add("FullName", objAssembly.FullName);
            
            if (objNameValueCollection["Product"] == null)
            {
                throw new MissingFieldException("The AssemblyInfo file for the assembly " + objAssembly.GetName().Name + " must have the <Assembly:AssemblyProduct()> key populated.");
            }
            
            if (objNameValueCollection["Company"] == null)
            {
                throw new MissingFieldException("The AssemblyInfo file for the assembly " + objAssembly.GetName().Name + " must have the <Assembly: AssemblyCompany()>  key populated.");
            }
            
            return objNameValueCollection;
        }

        public static bool GetBoolean(string key)
        {
            var strTemp = ConfigurationManager.AppSettings.Get(key);

            if (strTemp != null)
            {
                switch (strTemp.ToLower())
                {
                    case "1":
                    case "true":
                        return true;
                }
            }
            
            return false;
        }

        private static NameValueCollection GetCommandLineArgs()
        {
            var strArgs = Environment.GetCommandLineArgs();
            var objNameValueCollection = new NameValueCollection();

            if (strArgs.Length > 0)
            {
                var intArg = 0;
                foreach (var strArg in strArgs)
                {
                    if (IsUrl(strArg))
                    {
                        GetUrlCommandLineArgs(strArg, ref objNameValueCollection);
                    }
                    else if (!GetKeyValueCommandLineArg(strArg, ref objNameValueCollection))
                    {
                        objNameValueCollection.Add("arg" + intArg, RemoveArgPrefix(strArg));
                        intArg++;
                    }
                }
            }
            
            return objNameValueCollection;
        }

        private static Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }

        public static int GetInteger(string key)
        {
            var intTemp = Convert.ToInt32(ConfigurationManager.AppSettings.Get(key));
            return intTemp == 0 ? 0 : intTemp;
        }

        private static bool GetKeyValueCommandLineArg(string strArg, ref NameValueCollection objNameValueCollection)
        {
            IEnumerator enumerator = null;
            var objMatchCollection = Regex.Matches(strArg, "(?<Key>^[^=]+)=(?<Value>[^= ]*$)");

            if (objMatchCollection.Count == 0)
            {
                return false;
            }
            try
            {
                enumerator = objMatchCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var objMatch = (Match) enumerator.Current;
                    objNameValueCollection.Add(RemoveArgPrefix(objMatch.Groups["Key"].ToString()), objMatch.Groups["Value"].ToString());
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            
            return true;
        }

        public static string GetString(string key)
        {
            return ConfigurationManager.AppSettings.Get(key) ?? "";
        }

        private static void GetUrlCommandLineArgs(string strUrl, ref NameValueCollection objNameValueCollection)
        {
            IEnumerator enumerator = null;
            var objMatchCollection = Regex.Matches(strUrl, "(?<Key>[^=#&?]+)=(?<Value>[^=#&]*)");

            try
            {
                enumerator = objMatchCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var objMatch = (Match) enumerator.Current;
                    objNameValueCollection.Add(objMatch.Groups["Key"].ToString(), objMatch.Groups["Value"].ToString());
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
        }

        private static bool IsUrl(string strAny)
        {
            return (((strAny.IndexOf("&") > -1) || strAny.StartsWith("?")) || strAny.ToLower().StartsWith("http://"));
        }

        private static string RemoveArgPrefix(string strArg)
        {
            if (strArg.StartsWith("-") | strArg.StartsWith("/"))
            {
                return strArg.Substring(1);
            }
            return strArg;
        }

        public static string AppBase
        {
            get {
                return _strAppBase ??
                       (_strAppBase =
                        Convert.ToString(RuntimeHelpers.GetObjectValue(AppDomain.CurrentDomain.GetData("APPBASE"))));
            }
        }

        public static string AppBuildDate
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["BuildDate"];
            }
        }

        public static string AppCompany
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Company"];
            }
        }

        public static string AppCopyright
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Copyright"];
            }
        }

        public static string AppDescription
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Description"];
            }
        }

        public static string AppFileName
        {
            get
            {
                return Regex.Match(AppPath, "[^/]*.(exe|dll)", RegexOptions.IgnoreCase).ToString();
            }
        }

        public static string AppFullName
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["FullName"];
            }
        }

        public static string AppPath
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["CodeBase"];
            }
        }

        public static string AppProduct
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Product"];
            }
        }

        public static string AppTitle
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Title"];
            }
        }

        public static string AppVersion
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Version"];
            }
        }

        public static NameValueCollection CommandLineArgs
        {
            get { return _objCommandLineArgs ?? (_objCommandLineArgs = GetCommandLineArgs()); }
        }

        public static bool CommandLineHelpRequested
        {
            get
            {
                if (_objCommandLineArgs == null)
                {
                    _objCommandLineArgs = GetCommandLineArgs();
                }
                if (_objCommandLineArgs.HasKeys())
                {
                    foreach (string strKey in _objCommandLineArgs.AllKeys)
                    {
                        if (Regex.IsMatch(strKey, @"^(help|\?)", RegexOptions.IgnoreCase))
                        {
                            return true;
                        }
                        if (Regex.IsMatch(_objCommandLineArgs[strKey], @"^(help|\?)", RegexOptions.IgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public static string ConfigPath
        {
            get {
                return _strConfigPath ??
                       (_strConfigPath =
                        Convert.ToString(
                            RuntimeHelpers.GetObjectValue(AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE"))));
            }
        }

        public static bool DebugMode
        {
            get
            {
                return CommandLineArgs["debug"] != null || Debugger.IsAttached;
            }
        }

        public static string RuntimeVersion
        {
            get {
                return _strRuntimeVersion ??
                       (_strRuntimeVersion = Regex.Match(Environment.Version.ToString(), @"\d+.\d+.\d+").ToString());
            }
        }

        public static string SecurityZone
        {
            get { return _strSecurityZone ?? (_strSecurityZone = Zone.CreateFromUrl(AppBase).SecurityZone.ToString()); }
        }
    }
}