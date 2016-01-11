using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GoldBoxExplorer.Lib.Exceptions;
using GoldBoxExplorer.Lib.Plugins;

namespace GoldBoxExplorer
{
    public class MainViewPresenter
    {
        
        private readonly IMainView _view;
        private IPlugin _plugin;

        public IPlugin getPlugin()
        {
            return _plugin;
        }
   
        public MainViewPresenter(IMainView view)
        {
            _view = view;


        }

        public void InitView()
        {
            ResetZoom();
        }

        public string CurrentFile { get; set; }

        public string CurrentDirectory { get; private set; }

        public float ZoomLevel { get; private set; }

        public void LoadDirectory(string directory)
        {
            const string fileSpec = "*.*";
            var dir = new DirectoryInfo(directory);
            var files = new List<FileInfo>();

            CurrentDirectory = dir.FullName;

            files.AddRange(dir.GetFiles(fileSpec));

            var fileDtoList = files.Select(fileInfo => new FileDto(fileInfo)).ToList();

            _view.DisplayFileList(fileDtoList);
        }

        public void IncreaseZoomLevel()
        {
            ZoomLevel += 0.10f;
            UpdateZoom();
        }

        public void DecreaseZoomLevel()
        {
            var oldZoom = ZoomLevel;
            ZoomLevel -= 0.10f;
            if (ZoomLevel <= 0.10f)
            {
                ZoomLevel = oldZoom;
            }
            UpdateZoom();
        }

        public void ResetZoom()
        {
            ZoomLevel = 1;
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            if (_plugin != null)
            {
                _plugin.Viewer.Zoom = ZoomLevel;
            }
            _view.RefreshView();
            _view.SetZoomMessage(GetZoomMessage());
        }

        private string GetZoomMessage()
        {
            return String.Format("{0:p0}", ZoomLevel);
        }

 
        public IPlugin ProcessFile()
        {
            if (String.IsNullOrEmpty(CurrentDirectory) || String.IsNullOrEmpty(CurrentFile))
                return null;

            try
            {
                var filename = String.Format(@"{0}\{1}", CurrentDirectory, CurrentFile);
                _plugin = PluginFactory.CreateUsing(new PluginParameter
                                            {
                                                Zoom = ZoomLevel,
                                                ContainerWidth = _view.GetViewerWidth(),
                                                Filename = filename,
                                            });
                if (_plugin == null) return null;
                _view.DisplayBlock(_plugin.Viewer.GetControl());
                return _plugin;

            }
            catch (Exception ex)
            {
                var whatHappened = String.Format("We're sorry but Gold Box Explorer encountered a problem with the file named \"{0}\".", CurrentFile);
                var howUserAffected = "Unfortunately Gold Box Explorer is having some difficulty in displaying this file.";
                var whatUserCanDo = "If the error persists, please consider logging an issue at http://goldbox.codeplex.com.";
                HandledExceptionManager.ShowDialog(whatHappened, howUserAffected, whatUserCanDo,
                    ex, MessageBoxButtons.OK, MessageBoxIcon.Hand, HandledExceptionManager.UserErrorDefaultButton.Default);
            }
            return null;
        }

        public void ResizeBlockViewer(Size size)
        {
            if (_plugin != null)
            {
                _plugin.Viewer.ContainerWidth = size.Width - SystemInformation.VerticalScrollBarWidth;
            }
            _view.RefreshView();
        }

        public void SetZoom(double size)
        {
            ZoomLevel = (float) size;
            UpdateZoom();
        }
    }
}