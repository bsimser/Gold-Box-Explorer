using System;
using System.Collections;
using System.Windows.Forms;

namespace GoldBoxExplorer
{
    public class ListViewSorter : IComparer
    {
        public ListViewSorter()
        {
            LastSort = 0;
            ByColumn = 0;
        }

        public int ByColumn { get; set; }

        public int LastSort { get; set; }

        #region IComparer Members

        public int Compare(object o1, object o2)
        {
            if ((!(o1 is ListViewItem)))
                return (0);

            if (!(o2 is ListViewItem))
                return (0);


            var lvi1 = (ListViewItem) o2;

            if (ByColumn > lvi1.SubItems.Count)
                return 0;
            
            var str1 = lvi1.SubItems[ByColumn].Text;
            var lvi2 = (ListViewItem) o1;

            if (ByColumn > lvi2.SubItems.Count)
                return 0;

            var str2 = lvi2.SubItems[ByColumn].Text;

            var result = 0;

            switch (ByColumn)
            {
                case 0:
                    result = lvi1.ListView.Sorting == SortOrder.Ascending
                                 ? String.Compare(str1, str2)
                                 : String.Compare(str2, str1);
                    break;

                case 1:
                    result = lvi1.ListView.Sorting == SortOrder.Ascending
                                 ? DateTime.Compare(DateTime.Parse(str1), DateTime.Parse(str2))
                                 : DateTime.Compare(DateTime.Parse(str2), DateTime.Parse(str1));
                    break;

                case 2:
                    result = lvi1.ListView.Sorting == SortOrder.Ascending
                                 ? String.Compare(str1, str2)
                                 : String.Compare(str2, str1);
                    break;
            }

            LastSort = ByColumn;

            return (result);
        }

        #endregion
    }
}