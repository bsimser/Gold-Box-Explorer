using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System;
using System.IO;
using GoldBoxExplorer.Lib.Plugins.Dax;

namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    class FpViewPos : Object
    {
        public int cursorX;
        public int cursorY;
        public string facing;

        public FpViewPos(int x = 8, int y = 8, string f = "s")
        {
            cursorX = x;
            cursorY = y;
            facing = f;
        }
        public void rotateCursorLeft()
        {
            var cardinals = "nwse";
            var d = cardinals.IndexOf(facing);
            d++;
            d %= 4;
            facing = cardinals.Substring(d, 1);
        }
        public void rotateCursorRight()
        {
            var cardinals = "nwse";
            var d = cardinals.IndexOf(facing);
            d--;
            if (d == -1) d = 3;
            facing = cardinals.Substring(d, 1);
        }
        private void sanityCheck()
        {
            if (cursorX < 0) cursorX = 0;
            if (cursorX > 15) cursorX = 15;
            if (cursorY < 0) cursorY = 0;
            if (cursorY > 15) cursorY = 15;

        }
        public void moveCursorForwards()
        {
            if (facing == "n") cursorY--;
            if (facing == "s") cursorY++;
            if (facing == "w") cursorX--;
            if (facing == "e") cursorX++;
            sanityCheck();
        }
        public void moveCursorBackwards()
        {
            if (facing == "s") cursorY--;
            if (facing == "n") cursorY++;
            if (facing == "e") cursorX--;
            if (facing == "w") cursorX++;
            sanityCheck();
        }

    }
    public class GeoDaxFileViewer : IGoldBoxViewer
    {
        private const int RoomSize = 32;
        private const int WallThickness = 2;
        private const int BaseMapSize = 256 * 2;
        private const int GutterSize = 32;
        private const int FullMapWidth = BaseMapSize + GutterSize;
        private const int FullMapHeight = BaseMapSize + GutterSize;
        private const int FPViewWidth = 350;
        private const int FPViewHeight = 310;
        private readonly GeoDaxFile _goldBoxFile;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private TabControl tab;
        private int selected_dax_id = -1;
        private Dictionary<int, FpViewPos> mapCursors = new Dictionary<int, FpViewPos>();
        public GeoDaxFileViewer(GeoDaxFile goldBoxFile, int containerWidth, float zoom)
        {
            ContainerWidth = containerWidth;
            _goldBoxFile = goldBoxFile;
            Zoom = zoom;
            
        }

        private Rectangle getPictureBoxOffset(PictureBox pb)
        {
            
            // It seems the only way of correctly adjusting for the picturebox position after it has been resized is to access private data from the picturebox class
            System.Reflection.PropertyInfo pInfo = pb.GetType().GetProperty("ImageRectangle",
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

            return (Rectangle)pInfo.GetValue(pb, null);
            //return new Point(rectangle.X, rectangle.Y);
        }
        public void mouseClickOnMap(object sender, MouseEventArgs e)
        {

            var panel = tab.SelectedTab.Controls["mapPanel"];
            var pictureBox = panel.Controls["map"] as PictureBox;
            var mousePos = new Point(e.X, e.Y); // Cursor.Position;
            var offset = getPictureBoxOffset(pictureBox);
            
            float x = mousePos.X - offset.X;
            float y = mousePos.Y - offset.Y;

            x /= ((float)offset.Width / FullMapWidth);
            y /= ((float) offset.Height / FullMapHeight);

            int row = (int)((y - GutterSize) / (RoomSize));
            int col = (int) ((x - GutterSize) / (RoomSize));
       
            EventHandler<ChangeFileEventArgs> handler = ChangeSelectedFile;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                ChangeFileEventArgs ev = new ChangeFileEventArgs();
                ev.daxId = -1;
                var mapRecord = _goldBoxFile.GetMaps()[tab.SelectedIndex];
                foreach (var w in mapRecord.Walls)
                {
                    if (w.Row == row && w.Column == col)
                        ev.place = String.Format("event {0}", w.Event & 127);
                }

                var diskNumber = Convert.ToInt32(Path.GetFileName(_goldBoxFile.FileName).Replace("GEO", "").Replace(".DAX", ""));
                ev.filename = String.Format("ECL{0}.DAX", diskNumber);

                // choose an ecl we hope corresponds to this map

                // first choose the ecl with the same id as the map, if such an ecl block exists
                if (_goldBoxFile._daxEclFile.eclDumps.Find(ecldump => ecldump._blockId == mapRecord.DaxId) != null)
                {
                    ev.daxId = mapRecord.DaxId;                 
                }

                // have any of the ecls referenced this map in code? if so, pick one of those
                if (_goldBoxFile.referencedByEcl.Count > 0)                 
                {
                    // do we not have a valid ecl block yet? if not, default to the first ecl block that references this map id
                    if (ev.daxId == -1)
                        ev.daxId = _goldBoxFile.referencedByEcl[0];
                   
                    // see if there's an ecl with the same number of events, that will be the most likely candidate
                    foreach (var id in _goldBoxFile.referencedByEcl)
                    {
                        var eclBlk = _goldBoxFile._daxEclFile.eclDumps.Find(ecldump => ecldump._blockId == id); // find the ecl block which matches id
                        if ( eclBlk != null) {
                            if ((_goldBoxFile.highestEvent[mapRecord.DaxId] == eclBlk.eventCount
                                || (_goldBoxFile.highestEvent[mapRecord.DaxId] & 31) == eclBlk.eventCount)
                                && eclBlk.eventCount > 0)
                            {
                                ev.daxId = id;

                            }
                        }
                    }
                }
                // if we still haven't found an ecl, just go with the first one in the ecl file for this disk
                if (ev.daxId == -1)
                    ev.daxId = _goldBoxFile._daxEclFile.eclDumps[0]._blockId;
                // Use the () operator to raise the event.
                if (ev.place!= null)
                    handler(this, ev);
            }
        }
        
        public void rotateCursorLeft(object sender, MouseEventArgs e)
        {
            var fpViewPos = (FpViewPos) tab.SelectedTab.Tag;
            fpViewPos.rotateCursorLeft();
            moveCursor();
        }
        public void rotateCursorRight(object sender, MouseEventArgs e)
        {
            var fpViewPos = (FpViewPos)tab.SelectedTab.Tag;
            fpViewPos.rotateCursorRight();
            moveCursor();
        }
        public void moveCursorForwards(object sender, MouseEventArgs e)
        {
            var fpViewPos = (FpViewPos)tab.SelectedTab.Tag;
            fpViewPos.moveCursorForwards();
            moveCursor();
        }
        public void moveCursorBackwards(object sender, MouseEventArgs e)
        {
            var fpViewPos = (FpViewPos)tab.SelectedTab.Tag;
            fpViewPos.moveCursorBackwards();
            moveCursor();
        }

        private void moveCursor()
        {
            var panel = tab.SelectedTab.Controls["mapPanel"];
            var pictureBox = panel.Controls["map"] as PictureBox;
            var fpViewBox = panel.Controls["FPViewPanel"].Controls["3d view"] as PictureBox;

            var surface = Graphics.FromImage(pictureBox.Image);
            foreach (var m in _goldBoxFile.GetMaps())
            {
                if (m.Name == tab.SelectedTab.Text)
                {
                    Drawfpview(fpViewBox.Image, m.Walls, (FpViewPos) tab.SelectedTab.Tag);
                    fpViewBox.Invalidate();
                    DrawMap(pictureBox.Image, m.Walls, (FpViewPos)tab.SelectedTab.Tag);
                    pictureBox.Invalidate();
                }
            }
        }

        public void geoImageExportForm(object sender, MouseEventArgs e)
        {
            var panel = tab.SelectedTab.Controls["mapPanel"];
            var pictureBox = panel.Controls["map"] as PictureBox;
            using (var form = new GeoExportForm(pictureBox,tab.SelectedTab.Text))
            {
                form.ShowDialog();
            }

        }
        public Control GetControl()
        {
            var maps = _goldBoxFile.GetMaps();

            tab = new TabControl {Dock = DockStyle.Fill};
            tab.Invalidated += InvalidateMap;

            foreach (var geoMapRecord in maps)
            {
                var newFPViewPos = new FpViewPos();
                //System.Diagnostics.Debug.WriteLine("name='"+ geoMapRecord.Name+"'");
                //mapCursors.Add(geoMapRecord.DaxId, newFPViewPos);
                selected_dax_id = geoMapRecord.DaxId;
                var page = new TabPage(geoMapRecord.Name) {Size = new Size(FullMapWidth + FPViewWidth, Math.Max(FPViewHeight, FullMapHeight))};
                page.Tag = newFPViewPos;
                var panel = ViewerHelper.CreatePanel();
                panel.Name = "mapPanel";
                var exportButton = ViewerHelper.CreateButton();
                exportButton.Text = "export map to image";
                exportButton.MouseClick += geoImageExportForm;

                var movementButtonPanel = ViewerHelper.CreatePanel();
                movementButtonPanel.Width = 50;
                movementButtonPanel.Name = "Movement Button Panel";
                movementButtonPanel.Dock = DockStyle.Bottom;
                var forwardButton = ViewerHelper.CreateButton();
                forwardButton.Text = "Forward";
                forwardButton.Width = 40;
                forwardButton.MouseClick += moveCursorForwards;
                forwardButton.Dock = DockStyle.Top;
                var movementRow3 = ViewerHelper.CreateRow();
                movementRow3.Controls.Add(forwardButton);

                var backwardButton = ViewerHelper.CreateButton();
                backwardButton.Text = "Back";
                backwardButton.Width = 40;
                backwardButton.MouseClick += moveCursorBackwards;
                backwardButton.Dock = DockStyle.Bottom;
                var movementRow1 = ViewerHelper.CreateRow();
                movementRow1.Controls.Add(backwardButton);

                var leftButton = ViewerHelper.CreateButton();
                leftButton.Text = "Left";
                backwardButton.Width = 40;
                leftButton.MouseClick += rotateCursorLeft;
                leftButton.Dock = DockStyle.Left;
                

                var rightButton = ViewerHelper.CreateButton();
                rightButton.Text = "Right";
                rightButton.Width = 40;
                rightButton.MouseClick += rotateCursorRight;
                rightButton.Dock = DockStyle.Fill;

                var movementRow2 = ViewerHelper.CreateRow();
                movementRow2.Controls.Add(rightButton);
                movementRow2.Controls.Add(leftButton);
 
                var fpViewPanel = ViewerHelper.CreatePanel();
                fpViewPanel.Dock = DockStyle.Left;
                fpViewPanel.Width = FPViewWidth;
                fpViewPanel.Height = FPViewHeight;
                fpViewPanel.Name = "FPViewPanel";
           //     var mapPanel = ViewerHelper.CreatePanel();
                var fpViewBox = new PictureBox();
                fpViewBox.Dock = DockStyle.Fill;
                fpViewBox.BorderStyle = BorderStyle.Fixed3D;

                var pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Left ;
                pictureBox.MouseClick += mouseClickOnMap;

                movementButtonPanel.Controls.Add(movementRow1);
                movementButtonPanel.Controls.Add(movementRow2);
                movementButtonPanel.Controls.Add(movementRow3);


                var bitmap = new Bitmap(FullMapWidth, FullMapHeight);
                DrawMap(bitmap, geoMapRecord.Walls, newFPViewPos);

                pictureBox.Name = "map";
                pictureBox.Image = bitmap;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                pictureBox.Size = new Size(FullMapWidth, FullMapHeight);

                var fpvbitmap = new Bitmap(FPViewWidth, FPViewHeight);
                Drawfpview(fpvbitmap, geoMapRecord.Walls, newFPViewPos);

                fpViewBox.Name = "3d view";
                fpViewBox.Image = fpvbitmap;
                fpViewBox.SizeMode = PictureBoxSizeMode.Zoom;

                fpViewBox.Size = new Size(FPViewWidth, FPViewHeight+60);

                
                fpViewPanel.Controls.Add(fpViewBox);
                fpViewPanel.Controls.Add(movementButtonPanel);
                panel.Controls.Add(fpViewPanel);
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(exportButton);

                page.AutoScroll = true;
                page.Controls.Add(panel);

                tab.TabPages.Add(page);
            }

            return tab;
        }

        void InvalidateMap(object sender, InvalidateEventArgs e)
        {
            var tab = sender as TabControl;
            if (tab == null) return;
            foreach (TabPage page in tab.TabPages)
            {
                var panel = page.Controls["mapPanel"];
                var pictureBox = panel.Controls["map"] as PictureBox;
                pictureBox.Width = (int)(FullMapWidth * Zoom);
                pictureBox.Height = (int)(FullMapHeight * Zoom);
                var fpViewBox = panel.Controls["FPViewPanel"].Controls["3d view"] as PictureBox;
                fpViewBox.Width = (int)(FPViewWidth* Zoom);
                fpViewBox.Height = (int)(FPViewHeight * Zoom);


            }
            

        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }

        /// <summary>
        /// Render the town map to a bitmap image for rendering onto the surface
        /// </summary>
        /// <param name="town"></param>
        /// <param name="wallRecords"></param>
        private void Drawfpview(System.Drawing.Image fpview, IEnumerable<GeoWallRecord> wallRecords, FpViewPos currentFPViewPos)
        {
            //var surface = Graphics.FromImage(fpview);
            int eight = 8;
            int viewPortWidth = (7 * eight)* 2 - (eight*3) ;
            int viewPortHeight = (11 * eight);
            int xpos = currentFPViewPos.cursorX;
            int ypos = currentFPViewPos.cursorY;
            string facing = currentFPViewPos.facing;

            var cache_bm = new Bitmap(fpview, viewPortWidth, viewPortHeight);
            var surface = Graphics.FromImage(cache_bm);
            surface.Clear(Color.White);
//            surface.
//            var xpos = cursorX;
//            var ypos = cursorY;
            List<GeoWallRecord> fpsNearMapView = new List<GeoWallRecord>();
            List<GeoWallRecord> fpsMidMapView = new List<GeoWallRecord>();
            List<GeoWallRecord> fpsFarMapView = new List<GeoWallRecord>();
            if (facing == "s")
            {
                fpsNearMapView = getMapRow(xpos - 1, ypos, xpos + 1, wallRecords);
                fpsNearMapView.Reverse();
                fpsMidMapView = getMapRow(xpos - 2, ypos + 1, xpos + 2, wallRecords);
                fpsMidMapView.Reverse();
                fpsFarMapView = getMapRow(xpos - 3, ypos + 2, xpos + 3, wallRecords);
                fpsFarMapView.Reverse();
            }
            if (facing == "n")
            {
                fpsNearMapView = getMapRow(xpos - 1, ypos, xpos + 1, wallRecords);
                fpsMidMapView = getMapRow(xpos - 2, ypos - 1, xpos + 2, wallRecords);
                fpsFarMapView = getMapRow(xpos - 3, ypos - 2, xpos + 3, wallRecords);
            }
            if (facing == "e")
            {
                fpsNearMapView = getMapCol(ypos - 1, xpos, ypos + 1, wallRecords);
                fpsMidMapView = getMapCol(ypos - 2, xpos + 1, ypos + 2, wallRecords);
                fpsFarMapView = getMapCol(ypos - 3, xpos + 2, ypos + 3, wallRecords);
            }
            if (facing == "w")
            {
                fpsNearMapView = getMapCol(ypos - 1, xpos, ypos + 1, wallRecords);
                fpsNearMapView.Reverse();
                fpsMidMapView = getMapCol(ypos - 2, xpos - 1, ypos + 2, wallRecords);
                fpsMidMapView.Reverse();
                fpsFarMapView = getMapCol(ypos - 3, xpos - 2, ypos + 3, wallRecords);
                fpsFarMapView.Reverse();
            }
            var c = 0;
            int lastWall = 0; int lastRightWall = 0;
            foreach (var w in fpsFarMapView)
            {
                var wallType = getOppositeWall(w, facing);
                if (wallType > 0)
                {
                    DrawFarFacingWall(surface, c, 8, w.Event & 127, wallType);

                    if (c <= 3 && c > 0)
                    {
                        if (getLeftWall(w, facing) > 0 || lastWall > 0)
                        {
                            DrawFarFacingWallAdj(surface, c, 8, w.Event & 127, wallType);

                        }
                    }

                }
                if (c > 3 && lastWall > 0)
                {
                    if (wallType > 0 || lastRightWall > 0)
                    {
                        DrawFarFacingWallAdj(surface, c, 8, w.Event & 127, lastWall);

                    }
                }
                c++;
                lastWall = wallType;
                lastRightWall = getRightWall(w, facing);
            }
            c = 0;
            foreach (var w in fpsFarMapView)
            {
                if (c > 0 && c < 6)
                {
                    var wallType = getRightWall(w, facing);
                    if (c >= 3)
                    {
                        if (wallType > 0)
                            DrawFarRightWall(surface, c, 8, w.Event & 127, wallType);
                    }
                    wallType = getLeftWall(w, facing);
                    if (c <= 3)
                    {
                        if (wallType > 0)
                            DrawFarLeftWall(surface, c, 8, w.Event & 127, wallType);
                    }

                }
                c++;
            }
            c = 0;
            foreach (var w in fpsMidMapView)
            {
                var wallType = getOppositeWall(w, facing);
                if (wallType > 0)
                    DrawMidFacingWall(surface, c, 8, w.Event & 127, wallType);                    
                c++;
            }
            c = 0;
            foreach (var w in fpsMidMapView)
            {
                if (c > 0 && c < 5)
                {
                    var wallType = getLeftWall(w, facing);
                    if (wallType > 0 && c < 3)
                        DrawMidLeftWall(surface, c, 8, w.Event & 127, wallType);
                    wallType = getRightWall(w, facing);
                    if (wallType > 0 && c > 1)
                        DrawMidRightWall(surface, c, 8, w.Event & 127, wallType);
                }
                c++;
            }

            c = 0;
            foreach (var w in fpsNearMapView)
            {
                var wallType = getOppositeWall(w, facing);
                if (wallType > 0)
                    DrawNearFacingWall(surface, c, 8, w.Event & 127, wallType);
                c++;
            }
            c = 0;
            foreach (var w in fpsNearMapView)
            {
                if (c == 1)
                {
                    var wallType = getRightWall(w, facing);
                    if (wallType > 0)
                        DrawNearRightWall(surface, c, 8, w.Event & 127, wallType);
                    wallType = getLeftWall(w, facing);
                    if (wallType > 0)
                        DrawNearLeftWall(surface, c, 8, w.Event & 127, wallType);
                }
                c++;
            }
            DrawViewPort(surface, c, 8, 127);
            var screenSurface = Graphics.FromImage(fpview);
            var r =new Rectangle(0, 0, viewPortWidth*4, viewPortHeight*4);
            screenSurface.DrawImage(cache_bm, r);
            
        }

        private int getOppositeWall(GeoWallRecord w, string facing)
        {
            int wallType = 0;
            if (facing == "s")
                wallType = w.South;
            if (facing == "n")
                wallType = w.North;
            if (facing == "e")
                wallType = w.East;
            if (facing == "w")
                wallType = w.West;
            return wallType;
        }
        private int getRightWall(GeoWallRecord w, string facing)
        {
            int wallType = 0;
            if (facing == "e")
                wallType = w.South;
            if (facing == "w")
                wallType = w.North;
            if (facing == "n")
                wallType = w.East;
            if (facing == "s")
                wallType = w.West;
            return wallType;
        }
        private int getLeftWall(GeoWallRecord w, string facing)
        {
            int wallType = 0;
            if (facing == "w")
                wallType = w.South;
            if (facing == "e")
                wallType = w.North;
            if (facing == "s")
                wallType = w.East;
            if (facing == "n")
                wallType = w.West;
            return wallType;
        }

        private static GeoWallRecord getXYMap(int x, int y, IEnumerable<GeoWallRecord> wallRecords)
        {
            foreach (var w in wallRecords)
            {
                if (w.Row == y && w.Column == x)
                {
                    return w;
                }
            }
            // we're off the edge of the map, so return an empty/dummy wallrecord
            return new GeoWallRecord
            {
                Row = y,
                Column = x,
                North = 0,
                East = 0,
                South = 0,
                West = 0,
                Event = 0,
                Door = 0,
            };
        }
        private static List<GeoWallRecord> getMapRow(int xstart, int ystart, int xend, IEnumerable<GeoWallRecord> wallRecords)
        {
            var mapView = new List<GeoWallRecord>();
            var y = ystart;
            for (var x = xstart; x <= xend; x++)
            {
                mapView.Add(getXYMap(x, y, wallRecords));
            }
            return mapView;
        }
        private static List<GeoWallRecord> getMapCol(int ystart, int xstart, int yend, IEnumerable<GeoWallRecord> wallRecords)
        {
            var mapView = new List<GeoWallRecord>();
            var x = xstart;
            for (var y = ystart; y <= yend; y++)
            {
                mapView.Add(getXYMap(x, y, wallRecords));
            }
            return mapView;
        }
        private void DrawMap(System.Drawing.Image town, IEnumerable<GeoWallRecord> wallRecords, FpViewPos currentFPViewPos)
        {
            var surface = Graphics.FromImage(town);
            surface.Clear(Color.White);
            DrawGutter(surface);
            DrawRooms(wallRecords, surface);
            DrawGrid(surface);
            DrawViewCursor(surface, currentFPViewPos.cursorX, currentFPViewPos.cursorY, currentFPViewPos.facing);
        }

        /// <summary>
        /// Draw a light grid over the entire map
        /// </summary>
        /// <param name="surface"></param>
        private void DrawViewPort(Graphics surface, int x, int eight = 8, int e = 0)
        {

            var wallWidth = 7 * eight;
            var wallHeight = 11 * eight;
            var pen = new Pen(Color.DodgerBlue);
            surface.DrawRectangle(pen, (int)(1.5*eight)+100 + (int)(0.5 * wallWidth), 100 - eight, wallWidth * 2 -(int)(3*eight), wallHeight);
        }
        /// 
/*        private int getWallSetIdxById(int id) {
            int i = 0;
            foreach (var b in _goldBoxFile._daxWallDefFile._blockIds)
            {
                if (b == id) return i;
                i++;
            }
            return -1;
        }*/
        private Bitmap get_wallbm(int wt, int wv)
        {
            Bitmap bm;
            var pink = 0XFFFF52FF;
            //var pink = 0XFFFF5252;
            Color transparentColor  =  Color.FromArgb((int)pink);

            // get current block_id
            var current_dax_id = selected_dax_id;
            if (tab.SelectedIndex > -1)
            {
                current_dax_id = _goldBoxFile.GetMaps()[tab.SelectedIndex].DaxId;
            }
            // get wallset idx by block_id
            for(int i = 0; i < 3; i++) {
                int wallset_id = current_dax_id;
                
                if (_goldBoxFile.wallsetMapping.ContainsKey(current_dax_id))
                    wallset_id = _goldBoxFile.wallsetMapping[current_dax_id][i];
                if (wallset_id == 255 || wallset_id == 127) { wallset_id = current_dax_id; }
                if (_goldBoxFile.wallsetBitmaps.ContainsKey(wallset_id))
                {

                    int wsLen;
                    wsLen = _goldBoxFile.wallsetBitmaps[wallset_id].Count / 10;
                    if (wt <= wsLen)
                    {
                        bm = _goldBoxFile.wallsetBitmaps[wallset_id][wv + ((wt - 1) * 10)];
                        bm.MakeTransparent(transparentColor);
                        return bm;
                    }
                    wt -= wsLen;
                }
                }
            return new Bitmap(1, 1);
        }
            
        private void DrawNearRightWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {
            var wallWidth = 7 * eight;
            var wallHeight = 8 * eight;
            var point = new PointF(((x+1) * wallWidth)-(int)(5*eight), -eight);
            var pen = new Pen(Color.DodgerBlue);
            surface.DrawImage(get_wallbm(wallType, 8), point);
        }
        private void DrawNearLeftWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {
            var wallWidth = 7 * eight;
            var wallHeight = 8 * eight;
            var point = new PointF( x * wallWidth -(int)(7.0*eight), -eight);
            var pen = new Pen(Color.DodgerBlue);
            surface.DrawImage(get_wallbm(wallType, 7), point);
        }
        private void DrawNearFacingWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {

            var wallWidth = 7*eight;
            var wallHeight = 8*eight;
            var point = new PointF((x * wallWidth) - (int)(5.0 * eight), 0);

            surface.DrawImage(get_wallbm(wallType, 6), point);


        }
        private void DrawMidFacingWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {
            var wallWidth = 3*8;
            var wallHeight = 4*8;
            var point = new PointF((x * wallWidth) - (int)(eight * 2.0),  (int)(2.0 * eight));
            surface.DrawImage(get_wallbm(wallType, 3), point);
        }
        private void DrawMidLeftWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {
            var wallWidth = 3 * 8;
            var wallHeight = 4 * 8;
            var point = new PointF((x * wallWidth) - (int)(eight * 4) , 0 );
            surface.DrawImage(get_wallbm(wallType, 4), point);
        }
        private void DrawMidRightWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1)
        {
            var wallWidth = 3 * 8;
            var wallHeight = 4 * 8;
            var point = new PointF( (x * wallWidth) + (int)(eight * 1), 0);
            surface.DrawImage(get_wallbm(wallType, 5), point);
        }
        private void DrawFarRightWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1, bool doubleWall = true)
        {
            var wallWidth = 2 * 8;
            var wallHeight = 1 * 8;
            var offset = eight;
            if (x == 3)
                offset = 0;
            var point = new PointF(offset +  (x * wallWidth) , 0 + (int)(2.0 * eight));
            surface.DrawImage(get_wallbm(wallType, 2), point);
        }
        private void DrawFarLeftWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1, bool doubleWall = true)
        {
            var wallWidth = 2 * 8;
            var wallHeight = 1 * 8;
            var offset = -eight;
            if (x == 3)
                offset = 0;
            var point = new PointF(offset + (x * wallWidth) - (int)(eight * 2.0),  (int)(2.0 * eight));
            surface.DrawImage(get_wallbm(wallType, 1), point);
        }

        private void DrawFarFacingWall(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1, bool doubleWall = true)
        {
            var wallWidth = 2 * 8;
            var wallHeight = 1 * 8;
            var point = new PointF( (x * wallWidth) - (int)(eight * 1.0),  (int)(3.0 * eight));
            surface.DrawImage(get_wallbm(wallType, 0), point);
        }
        private void DrawFarFacingWallAdj(Graphics surface, int x, int eight = 8, int e = 0, int wallType = 1, bool doubleWall = true)
        {
            var wallWidth = 2 * 8;
            var wallHeight = 1 * 8;
            var point = new PointF( (x * wallWidth) - (int)(eight * 2.0), (int)(3.0 * eight));
            surface.DrawImage(get_wallbm(wallType, 9), point);
        }
        private static void DrawViewCursor(Graphics surface, int xpos, int ypos, string facing)
        {
            var pen = new Pen(Color.Red);
            var x = xpos * RoomSize + GutterSize + RoomSize/2;
            var y = ypos * RoomSize + GutterSize + RoomSize / 2;
            if (facing == "n")
            {
                surface.DrawLine(pen, x - 5, y, x, y - 5);
                surface.DrawLine(pen, x + 5, y, x, y - 5);
            }
            if (facing == "s")
            {
                surface.DrawLine(pen, x-5, y, x, y+5);
                surface.DrawLine(pen, x+5, y, x, y+5);
            }
            if (facing == "w")
            {
                surface.DrawLine(pen, x, y-5, x-5, y);
                surface.DrawLine(pen, x, y+5, x-5, y);
            }
            if (facing == "e")
            {
                surface.DrawLine(pen, x, y - 5, x + 5, y);
                surface.DrawLine(pen, x, y + 5, x + 5, y);
            }
        }
        private static void DrawGrid(Graphics surface)
        {
            var pen = new Pen(Color.DodgerBlue);

            for (int row = 0; row < 18; row++)
            {
                var x1 = 0;
                var y1 = row * RoomSize;
                var x2 = BaseMapSize + GutterSize;
                var y2 = row * RoomSize;
                surface.DrawLine(pen, x1, y1, x2, y2);

                for (int col = 0; col < 18; col++)
                {
                    x1 = col * RoomSize;
                    y1 = row;
                    x2 = col * RoomSize;
                    y2 = row + BaseMapSize + GutterSize;
                    surface.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        /// <summary>
        /// Loop through and render each room separately to control the layers
        /// </summary>
        /// <param name="wallRecords"></param>
        /// <param name="surface"></param>
        private static void DrawRooms(IEnumerable<GeoWallRecord> wallRecords, Graphics surface)
        {
            foreach (var wallRecord in wallRecords)
            {
                DrawRoom(surface, wallRecord);
            }
        }

        /// <summary>
        /// Draw a gutter along the left and top showing the grid numbers
        /// </summary>
        /// <param name="surface"></param>
        private static void DrawGutter(Graphics surface)
        {
            var font = new Font("Courier New", 14);
            var brush = new SolidBrush(Color.FromArgb(85, 85, 85));
            var point = new PointF(0, 0);
            var rectSize = new SizeF(RoomSize, RoomSize);
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            surface.TextRenderingHint = TextRenderingHint.AntiAlias;

            for (int row = 0; row < 16; row++)
            {
                point.X = 0;
                point.Y = row * GutterSize + GutterSize;
                surface.DrawString(row.ToString(), font, brush, new RectangleF(point, rectSize), format);
            }

            for (var col = 0; col < 16; col++)
            {
                point.X = col * GutterSize + GutterSize;
                point.Y = 0;
                surface.DrawString(col.ToString(), font, brush, new RectangleF(point, rectSize), format);
            }
        }

        /// <summary>
        /// Draws a single room on the town map
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="wallRecord"></param>
        private static void DrawRoom(Graphics surface, GeoWallRecord wallRecord)
        {
            var whiteBrush = new SolidBrush(Color.White);
            var yellowBrush = new SolidBrush(Color.Yellow);
            var redBrush = new SolidBrush(Color.Red);
            var grayBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
            var wallColor = Color.FromArgb(170, 170, 170);
            var wallBrush = new SolidBrush(wallColor);
            var eventBrush = new SolidBrush(Color.ForestGreen);

            var x = wallRecord.Column * RoomSize + GutterSize;
            var y = wallRecord.Row * RoomSize + GutterSize;

            DrawRoomBase(surface, y, grayBrush, x);
            DrawRoomEvent(surface, wallRecord, y, eventBrush, x);
            DrawRoomWalls(surface, wallRecord, WallThickness, y, wallBrush, x);
            DrawRoomDoors(surface, wallRecord, yellowBrush, x, redBrush, whiteBrush, y);
        }

        /// <summary>
        /// Draw each of the room doors. This goes over top of the base layer and
        /// is offset from the base. It will draw doors normal (white), locked (yellow),
        /// or wizard or spell locked (red)
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="wallRecord"></param>
        /// <param name="yellowBrush"></param>
        /// <param name="x"></param>
        /// <param name="redBrush"></param>
        /// <param name="whiteBrush"></param>
        /// <param name="y"></param>
        private static void DrawRoomDoors(Graphics surface, GeoWallRecord wallRecord, SolidBrush yellowBrush, int x,
                                          SolidBrush redBrush, SolidBrush whiteBrush, int y)
        {
            var doorX = 0;
            var doorY = 0;
            var doorW = 0;
            var doorH = 0;

            var northDoor = wallRecord.Door & 3;
            var eastDoor = ((wallRecord.Door >> 2) & 3);
            var southDoor = ((wallRecord.Door >> 4) & 3);
            var westDoor = ((wallRecord.Door >> 6) & 3);

            var doorBrushes = new[] { null, whiteBrush, yellowBrush, redBrush };

            if (northDoor > 0)
            {
                doorX = x + (WallThickness * 2);
                doorY = y;
                doorW = RoomSize - (WallThickness * 4);
                doorH = WallThickness * 2;
                surface.FillRectangle(doorBrushes[northDoor], doorX, doorY, doorW, doorH);
            }
            if (eastDoor > 0)
            {
                doorX = x + RoomSize - (WallThickness * 2);
                doorY = y + (WallThickness * 2);
                doorW = WallThickness * 2;
                doorH = RoomSize - (WallThickness * 4);
                surface.FillRectangle(doorBrushes[eastDoor], doorX, doorY, doorW, doorH);
            }
            if (southDoor > 0)
            {
                doorX = x + (WallThickness * 2);
                doorY = y + RoomSize - (WallThickness * 2);
                doorW = RoomSize - (WallThickness * 4);
                doorH = WallThickness * 2;
                surface.FillRectangle(doorBrushes[southDoor], doorX, doorY, doorW, doorH);
            }

            if (westDoor <= 0) return;

            // West                
            doorX = x;
            doorY = y + (WallThickness * 2);
            doorW = WallThickness * 2;
            doorH = RoomSize - (WallThickness * 4);
            surface.FillRectangle(doorBrushes[westDoor], doorX, doorY, doorW, doorH);
        }

        /// <summary>
        /// Draw the four walls surrounding the room
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="wallRecord"></param>
        /// <param name="wallThickness"></param>
        /// <param name="y"></param>
        /// <param name="wallBrush"></param>
        /// <param name="x"></param>
        private static void DrawRoomWalls(Graphics surface, GeoWallRecord wallRecord, int wallThickness, int y, Brush wallBrush, int x)
        {
            if (wallRecord.North != 0)
            {
                surface.FillRectangle(wallBrush, x, y, RoomSize, wallThickness);
            }
            if (wallRecord.East != 0)
            {
                surface.FillRectangle(wallBrush, x + RoomSize - wallThickness, y, wallThickness, RoomSize);
            }
            if (wallRecord.South != 0)
            {
                surface.FillRectangle(wallBrush, x, y + RoomSize - wallThickness, RoomSize, wallThickness);
            }
            if (wallRecord.West != 0)
            {
                surface.FillRectangle(wallBrush, x, y, wallThickness, RoomSize);
            }
        }

        /// <summary>
        /// Draw an indicator that this room has an event associated with it
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="wallRecord"></param>
        /// <param name="y"></param>
        /// <param name="eventBrush"></param>
        /// <param name="x"></param>
        private static void DrawRoomEvent(Graphics surface, GeoWallRecord wallRecord, int y, Brush eventBrush, int x)
        {
            var font = new Font("Courier New", 12);
            var brush = new SolidBrush(Color.FromArgb(0, 0, 0));
            var point = new PointF(x, y);
            var rectSize = new SizeF(RoomSize, RoomSize);

            if (wallRecord.Event > 0)
            {
                surface.FillRectangle(eventBrush, x, y, RoomSize, RoomSize);
                if ((wallRecord.Event & 127) > 0)
                    surface.DrawString((wallRecord.Event & 127).ToString(), font, brush, point);
            }
        }

        /// <summary>
        /// This is the base colour for each room and is drawn first
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="y"></param>
        /// <param name="grayBrush"></param>
        /// <param name="x"></param>
        private static void DrawRoomBase(Graphics surface, int y, Brush grayBrush, int x)
        {
            surface.FillRectangle(grayBrush, x, y, RoomSize, RoomSize);
        }
    }
}