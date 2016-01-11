using System.Collections.Generic;
using System.Windows.Forms;

namespace GoldBoxExplorer
{
    public interface IMainView
    {
        void DisplayFileList(IList<FileDto> files);
        void SetZoomMessage(string message);
        void DisplayBlock(Control control);
        int GetViewerWidth();
        void RefreshView();
        void DisplayMode(string mode);
    }
}