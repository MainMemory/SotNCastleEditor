using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using SotNData;

namespace SotNCastleEditor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		readonly Brush[] areaColors =
		{
			new SolidBrush(Color.FromArgb(128, 0, 0, 255)),
			new SolidBrush(Color.FromArgb(128, 0, 255, 0)),
			new SolidBrush(Color.FromArgb(128, 255, 0, 0)),
			new SolidBrush(Color.FromArgb(128, 0, 255, 255)),
			new SolidBrush(Color.FromArgb(128, 255, 0, 255)),
			new SolidBrush(Color.FromArgb(128, 255, 255, 0)),
			new SolidBrush(Color.FromArgb(128, 0, 0, 128)),
			new SolidBrush(Color.FromArgb(128, 0, 128, 0)),
			new SolidBrush(Color.FromArgb(128, 128, 0, 0)),
			new SolidBrush(Color.FromArgb(128, 0, 128, 128)),
			new SolidBrush(Color.FromArgb(128, 128, 0, 128)),
			new SolidBrush(Color.FromArgb(128, 128, 128, 0)),
			new SolidBrush(Color.FromArgb(128, 0, 128, 255)),
			new SolidBrush(Color.FromArgb(128, 128, 0, 255)),
			new SolidBrush(Color.FromArgb(128, 0, 255, 128)),
			new SolidBrush(Color.FromArgb(128, 128, 255, 0)),
			new SolidBrush(Color.FromArgb(128, 255, 0, 128)),
			new SolidBrush(Color.FromArgb(128, 255, 128, 0)),
			new SolidBrush(Color.FromArgb(128, 255, 255, 128)),
			new SolidBrush(Color.FromArgb(128, 255, 128, 255)),
			new SolidBrush(Color.FromArgb(128, 128, 255, 255)),
			new SolidBrush(Color.FromArgb(128, 128, 128, 255)),
			new SolidBrush(Color.FromArgb(128, 128, 255, 128)),
			new SolidBrush(Color.FromArgb(128, 255, 128, 128)),
		};
		readonly Brush darkenBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
		readonly Brush highlightBrush = new SolidBrush(Color.FromArgb(64, Color.White));
		readonly Brush bossTeleBrush = new SolidBrush(Color.FromArgb(192, Color.White));
		readonly Brush selectBrush = new SolidBrush(Color.FromArgb(64, Color.Blue));
		readonly Pen borderPen = new Pen(Color.White, 4);
		readonly System.Drawing.Imaging.ImageAttributes imageTrans = new System.Drawing.Imaging.ImageAttributes();
		readonly Bitmap alucard = new Bitmap("Alucard.png");
		readonly Brush backBrush = new TextureBrush(new Bitmap("back.png"));
		readonly string baseFilePath = Path.Combine(Application.StartupPath, "zones.json");

		string fileName;
		CastleMap mapInfo;
		Dictionary<Zones, CastleZone> zoneDict;
		Dictionary<Maps, List<CastleZone>> mapDict;
		double zoomLevel = 1;
		ZoneRoom selectedRoom, highlightRoom;
		Point? movePoint;
		Size moveOffset;
		Point selectedTile;
		Point menuLoc;
		Point? teleLoc;
		bool suppressEvents;

		private void Form1_Load(object sender, EventArgs e)
		{
			imageTrans.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix() { Matrix33 = 0.75f });
			NewFile();
			zoomLevelSelector.SelectedIndex = zoomLevelSelector.FindStringExact("100%");
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewFile();
		}

		private void NewFile()
		{
			OpenFile(null);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog() { DefaultExt = "caspat", Filter = "Castle Files|*.caspat;*.castle|All Files|*.*", RestoreDirectory = true })
				if (ofd.ShowDialog(this) == DialogResult.OK)
					OpenFile(ofd.FileName);
		}

		private void OpenFile(string filename)
		{
			if (filename != null)
			{
				if (Path.GetExtension(filename).Equals(".caspat", StringComparison.OrdinalIgnoreCase))
				{
					mapInfo = CastleMap.Load(baseFilePath);
					MapPatch.Load(filename).Apply(mapInfo);
				}
				else
					mapInfo = CastleMap.Load(filename);
			}
			else
				mapInfo = CastleMap.Load(baseFilePath);
			zoneDict = new Dictionary<Zones, CastleZone>();
			mapDict = new Dictionary<Maps, List<CastleZone>>();
			layoutAreaSelect.BeginUpdate();
			layoutAreaSelect.Items.Clear();
			teleportAreaSelect.BeginUpdate();
			teleportAreaSelect.Items.Clear();
			teleportPrevTSSelect.BeginUpdate();
			teleportPrevTSSelect.Items.Clear();
			bossTeleAreaSelect.BeginUpdate();
			bossTeleAreaSelect.Items.Clear();
			foreach (var zone in mapInfo.Zones)
			{
				layoutAreaSelect.Items.Add(zone.Name);
				teleportAreaSelect.Items.Add(zone.Name);
				teleportPrevTSSelect.Items.Add(zone.Name);
				bossTeleAreaSelect.Items.Add(zone.Name);
				zoneDict.Add(zone.ID, zone);
				if (!mapDict.ContainsKey(zone.Map))
					mapDict.Add(zone.Map, new List<CastleZone>());
				mapDict[zone.Map].Add(zone);
				string imgpath = Path.Combine(Application.StartupPath, "RoomImg", zone.ID.ToString());
				for (int i = 0; i < zone.Rooms.Length; i++)
				{
					var ip = Path.Combine(imgpath, $"{i}.png");
					Bitmap bmp = null;
					if (File.Exists(ip))
						bmp = new Bitmap(ip);
					zone.Rooms[i].SetGraphicsInfo(bmp, zone.Map >= Maps.ReverseCastle);
				}
			}
			layoutAreaSelect.EndUpdate();
			teleportAreaSelect.EndUpdate();
			teleportPrevTSSelect.EndUpdate();
			bossTeleAreaSelect.EndUpdate();
			foreach (var set in mapInfo.MatchingRooms)
			{
				var rooms = set.Select(a => zoneDict[a.Zone].Rooms[a.Room]).ToArray();
				foreach (var rm in rooms)
					rm.MatchingRooms = rooms;
			}
			teleportListBox.Items.Clear();
			teleportListBox.Items.AddRange(mapInfo.TeleportDests.Select(a => $"{a.Zone} {a.Room} {a.X} {a.Y}").ToArray());
			bossTeleListBox.Items.Clear();
			bossTeleListBox.Items.AddRange(mapInfo.BossTeleports.Select(a => $"{a.Zone} {a.Room} {a.X} {a.Y}").ToArray());
			fileName = filename;
			layoutAreaSelect.SelectedIndex = 0;
			teleportListBox.SelectedIndex = 0;
			bossTeleListBox.SelectedIndex = 0;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (fileName == null)
				SaveAs();
			else
				Save(fileName);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveAs();
		}

		private void SaveAs()
		{
			using (var sfd = new SaveFileDialog() { DefaultExt = "caspat", Filter = "Castle Files|*.caspat;*.castle|All Files|*.*", RestoreDirectory = true })
			{
				if (fileName != null)
				{
					sfd.InitialDirectory = Path.GetDirectoryName(fileName);
					sfd.FileName = Path.GetFileName(fileName);
				}
				if (sfd.ShowDialog(this) == DialogResult.OK)
					Save(sfd.FileName);
			}
		}

		private void Save(string filename)
		{
			if (Path.GetExtension(filename).Equals(".caspat", StringComparison.OrdinalIgnoreCase))
				MapPatch.Create(CastleMap.Load(baseFilePath), mapInfo).Save(filename);
			else
				mapInfo.Save(filename);
			fileName = filename;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void zoomLevelSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (zoomLevelSelector.SelectedIndex != -1)
			{
				zoomLevel = int.Parse(((string)zoomLevelSelector.SelectedItem).TrimEnd('%')) / 100.0;
				layoutPanel.Size = new Size((int)(16384 * zoomLevel), (int)(16384 * zoomLevel));
				viewToolStripMenuItem.DropDown.Close();
			}
		}

		private void ViewOption_CheckedChanged(object sender, EventArgs e) => DrawLayout();

		private void layoutPanel_Paint(object sender, PaintEventArgs e)
		{
			DrawLayout(e.Graphics);
		}

		private void DrawLayout()
		{
			using (var gfx = layoutPanel.CreateGraphics())
				DrawLayout(gfx);
		}

		private void DrawLayout(Graphics g2)
		{
			Rectangle rect = new Rectangle((int)Math.Floor(-layoutContainerPanel.AutoScrollPosition.X / zoomLevel / 256), (int)Math.Floor(-layoutContainerPanel.AutoScrollPosition.Y / zoomLevel / 256), (int)Math.Ceiling(layoutContainerPanel.ClientSize.Width / zoomLevel / 256) + 1, (int)Math.Ceiling(layoutContainerPanel.ClientSize.Height / zoomLevel / 256) + 1);
			using (var bmp = new Bitmap(layoutContainerPanel.ClientSize.Width, layoutContainerPanel.ClientSize.Height))
			using (var gfx = Graphics.FromImage(bmp))
			{
				gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
				gfx.TranslateTransform(layoutContainerPanel.AutoScrollPosition.X, layoutContainerPanel.AutoScrollPosition.Y);
				gfx.FillRectangle(backBrush, -layoutContainerPanel.AutoScrollPosition.X, -layoutContainerPanel.AutoScrollPosition.Y, layoutContainerPanel.ClientSize.Width, layoutContainerPanel.ClientSize.Height);
				gfx.ScaleTransform((float)zoomLevel, (float)zoomLevel);
				var baseMat = gfx.Transform;
				CastleZone curZone = mapInfo.Zones[layoutAreaSelect.SelectedIndex];
				List<CastleZone> zones = new List<CastleZone>();
				if (showOtherAreasToolStripMenuItem.Checked)
				{
					if (curZone.Map == Maps.NormalBoss)
						zones.AddRange(mapDict[Maps.NormalCastle]);
					else if (curZone.Map == Maps.ReverseBoss)
						zones.AddRange(mapDict[Maps.ReverseCastle]);
					zones.AddRange(mapDict[curZone.Map]);
					zones.Remove(curZone);
				}
				zones.Add(curZone);
				foreach (var zone in zones)
				{
					if (zone == curZone)
						gfx.FillRectangle(darkenBrush, rect.X * 256, rect.Y * 256, rect.Width * 256, rect.Height * 256);
					var brush = areaColors[Array.IndexOf(mapInfo.Zones, zone) % areaColors.Length];
					for (int i = zone.Rooms.Length - 1; i >= 0; i--)
					{
						var room = zone.Rooms[i];
						var bnd = room.Bounds;
						if (showGraphicsToolStripMenuItem.Checked && !clipGraphicsToMapToolStripMenuItem.Checked)
						{
							bnd.Location += room.LayoutOffset;
							if (room.Image != null)
							{
								bnd.Width = room.Image.Width / 256;
								bnd.Height = room.Image.Height / 256;
							}
						}
						if (rect.IntersectsWith(bnd))
						{
							gfx.TranslateTransform(room.Bounds.X * 256, room.Bounds.Y * 256);
							if (showGraphicsToolStripMenuItem.Checked && room.Image != null)
							{
								if (clipGraphicsToMapToolStripMenuItem.Checked)
									gfx.Clip = room.Region;
								gfx.DrawImage(room.Image, room.LayoutOffset.Width * 256, room.LayoutOffset.Height * 256, room.Image.Width, room.Image.Height);
								if (clipGraphicsToMapToolStripMenuItem.Checked)
									gfx.ResetClip();
								if (showBorderToolStripMenuItem.Checked)
									gfx.DrawPath(borderPen, room.Border);
							}
							else
							{
								gfx.FillRegion(brush, room.Region);
								gfx.DrawPath(borderPen, room.Border);
							}
							gfx.Transform = baseMat;
						}
					}
				}
				if (showBossTeleportsToolStripMenuItem.Checked)
					foreach (var tele in mapInfo.BossTeleports.Where(a => zoneDict.ContainsKey(a.Zone) && zones.Contains(zoneDict[a.Zone])))
					{
						var zone = zoneDict[tele.Zone];
						if (tele.Room < zone.Rooms.Length)
						{
							var room = zone.Rooms[tele.Room];
							gfx.FillRectangle(bossTeleBrush, (room.X + room.LayoutOffsetX + tele.X) * 256, (room.Y + room.LayoutOffsetY + tele.Y) * 256, 256, 256);
						}
					}
				if (showTeleportDestsToolStripMenuItem.Checked)
				{
					foreach (var tele in mapInfo.TeleportDests)
					{
						var zid = tele.Zone;
						if (curZone.Map >= Maps.ReverseCastle)
							zid ^= Zones.BlackMarbleGallery;
						if (zoneDict.TryGetValue(zid, out var zone) && zones.Contains(zone) && tele.Room < zone.Rooms.Length)
						{
							var room = zone.Rooms[tele.Room];
							var bnd = room.Bounds;
							bnd.Location += room.LayoutOffset;
							if (room.Image != null)
							{
								bnd.Width = room.Image.Width / 256;
								bnd.Height = room.Image.Height / 256;
							}
							int x, y;
							if (curZone.Map >= Maps.ReverseCastle)
							{
								x = bnd.Right * 256 - tele.X - alucard.Width / 2;
								y = bnd.Bottom * 256 - tele.Y - alucard.Height / 2;
							}
							else
							{
								x = bnd.Left * 256 + tele.X - alucard.Width / 2;
								y = bnd.Top * 256 + tele.Y - alucard.Height / 2;
							}
							gfx.DrawImage(alucard, x, y, alucard.Width, alucard.Height);
						}
					}
					if (selectedRoom != null && selectedRoom.Teleport.HasValue)
					{
						var tele = mapInfo.TeleportDests[selectedRoom.Teleport.Value];
						var zid = tele.Zone;
						if (curZone.Map >= Maps.ReverseCastle)
							zid ^= Zones.BlackMarbleGallery;
						if (zoneDict.TryGetValue(zid, out var zone) && tele.Room < zone.Rooms.Length)
						{
							var room = zone.Rooms[tele.Room];
							var bnd = room.Bounds;
							bnd.Location += room.LayoutOffset;
							if (room.Image != null)
							{
								bnd.Width = room.Image.Width / 256;
								bnd.Height = room.Image.Height / 256;
							}
							int x, y;
							if (curZone.Map >= Maps.ReverseCastle)
							{
								x = bnd.Right * 256 - tele.X;
								y = bnd.Bottom * 256 - tele.Y;
							}
							else
							{
								x = bnd.Left * 256 + tele.X;
								y = bnd.Top * 256 + tele.Y;
							}
							gfx.DrawLine(Pens.Lime, selectedRoom.X * 256 + selectedRoom.Width * 256 / 2, selectedRoom.Y * 256 + selectedRoom.Height * 256 / 2, x, y);
						}
					}
				}
				if (showGridToolStripMenuItem.Checked)
				{
					for (int x = rect.Left; x < rect.Right; x++)
						gfx.DrawLine(Pens.Red, x * 256, rect.Top * 256, x * 256, rect.Bottom * 256);
					for (int y = rect.Top; y < rect.Bottom; y++)
						gfx.DrawLine(Pens.Red, rect.Left * 256, y * 256, rect.Right * 256, y * 256);
				}
				if (selectedRoom != null)
				{
					if (movePoint.HasValue)
						gfx.TranslateTransform((movePoint.Value.X + moveOffset.Width) * 256, (movePoint.Value.Y + moveOffset.Height) * 256);
					else
						gfx.TranslateTransform(selectedRoom.Bounds.X * 256, selectedRoom.Bounds.Y * 256);
					gfx.FillRegion(selectBrush, selectedRoom.Region);
					gfx.Transform = baseMat;
				}
				if (highlightRoom != null)
				{
					gfx.TranslateTransform(highlightRoom.Bounds.X * 256, highlightRoom.Bounds.Y * 256);
					gfx.FillRegion(highlightBrush, highlightRoom.Region);
					gfx.Transform = baseMat;
				}
				g2.CompositingMode = CompositingMode.SourceCopy;
				g2.CompositingQuality = CompositingQuality.HighSpeed;
				g2.InterpolationMode = InterpolationMode.NearestNeighbor;
				g2.SmoothingMode = SmoothingMode.HighSpeed;
				g2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
				g2.DrawImage(bmp, -layoutContainerPanel.AutoScrollPosition.X, -layoutContainerPanel.AutoScrollPosition.Y, bmp.Width, bmp.Height);
			}
		}

		private void layoutAreaSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (layoutAreaSelect.SelectedIndex != -1)
			{
				selectedRoom = null;
				splitContainer1.Panel2.Enabled = false;
				DrawLayout();
			}
		}

		private void layoutPanel_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
				return;
			var chunkLoc = new Point((int)(e.X / zoomLevel) / 256, (int)(e.Y / zoomLevel) / 256);
			selectedRoom = mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms.FirstOrDefault(a => a.Bounds.Contains(chunkLoc) && (!clipGraphicsToMapToolStripMenuItem.Checked || a.Tiles == null || a.Tiles[chunkLoc.Y - a.Bounds.Y][chunkLoc.X - a.Bounds.X] != null));
			if (selectedRoom != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						movePoint = chunkLoc;
						moveOffset = new Size(selectedRoom.X - chunkLoc.X, selectedRoom.Y - chunkLoc.Y);
						break;
					case MouseButtons.Right:
						editTeleportDestToolStripMenuItem.Enabled = selectedRoom.Teleport.HasValue;
						moveTeleportDestHereToolStripMenuItem.Enabled = selectedRoom.Tiles != null;
						editBossTeleportToolStripMenuItem.Enabled = mapInfo.BossTeleports.Any(a => a.Zone == (Zones)layoutAreaSelect.SelectedIndex
																				 && a.Room == Array.IndexOf(mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms, selectedRoom)
																				 && a.X == chunkLoc.X - selectedRoom.X - selectedRoom.LayoutOffsetX
																				 && a.Y == chunkLoc.Y - selectedRoom.Y - selectedRoom.LayoutOffsetY);
						menuLoc = new Point((int)(e.X / zoomLevel), (int)(e.Y / zoomLevel));
						contextMenuStrip1.Show(layoutPanel, e.Location);
						break;
				}
				highlightRoom = null;
				DrawLayout();
				roomTilePanel.Size = new Size(selectedRoom.Width * 32, selectedRoom.Height * 32);
				selectedTile = Point.Empty;
				if (selectedRoom.Tiles == null)
					splitContainer1.Panel2.Enabled = false;
				else
				{
					splitContainer1.Panel2.Enabled = true;
					SelectedTileChanged();
				}
			}
			else
				splitContainer1.Panel2.Enabled = false;
		}

		private void layoutPanel_MouseMove(object sender, MouseEventArgs e)
		{
			var chunkLoc = new Point((int)(e.X / zoomLevel) / 256, (int)(e.Y / zoomLevel) / 256);
			if (e.Button == MouseButtons.Left && movePoint.HasValue)
			{
				if (movePoint.Value != chunkLoc)
				{
					movePoint = chunkLoc;
					DrawLayout();
				}
			}
			else
			{
				var rm = mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms.FirstOrDefault(a => a.Bounds.Contains(chunkLoc) && (!clipGraphicsToMapToolStripMenuItem.Checked || a.Tiles == null || a.Tiles[chunkLoc.Y - a.Bounds.Y][chunkLoc.X - a.Bounds.X] != null));
				if (highlightRoom != rm)
				{
					layoutPanel.Cursor = rm == null ? Cursors.Default : Cursors.SizeAll;
					highlightRoom = rm;
					DrawLayout();
				}
			}
		}

		private void layoutPanel_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || selectedRoom == null)
				return;
			var chunkLoc = new Point((int)(e.X / zoomLevel) / 256, (int)(e.Y / zoomLevel) / 256);
			var offset = new Size(chunkLoc.X - selectedRoom.X + moveOffset.Width, chunkLoc.Y - selectedRoom.Y + moveOffset.Height);
			if (offset.IsEmpty)
				return;
			ZoneRoom[] rooms = { selectedRoom };
			if (moveWholeAreaButton.Checked)
			{
				rooms = mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms;
				foreach (var rm in rooms)
				{
					if (rm.X + offset.Width < 0)
						offset.Width = -rm.X;
					if (rm.Y + offset.Height < 0)
						offset.Height = -rm.Y;
					if (rm.X + rm.Width + offset.Width > 64)
						offset.Width = 64 - (rm.X + rm.Width);
					if (rm.Y + rm.Height + offset.Height > 64)
						offset.Height = 64 - (rm.Y + rm.Height);
				}
			}
			if (syncIdenticalRoomsToolStripMenuItem.Checked)
				rooms = rooms.SelectMany(a => a.MatchingRooms).ToArray();
			if (selectedRoom.Reverse)
				offset = new Size(-offset.Width, -offset.Height);
			foreach (var rm in rooms)
				if (rm.Reverse)
					rm.Location -= offset;
				else
					rm.Location += offset;
			movePoint = null;
			DrawLayout();
		}

		private void editTeleportDestToolStripMenuItem_Click(object sender, EventArgs e)
		{
			teleportListBox.SelectedIndex = selectedRoom.Teleport.Value;
			tabControl1.SelectedIndex = 1;
		}

		private void moveTeleportDestHereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tele = mapInfo.TeleportDests[teleportListBox.SelectedIndex];
			int x = menuLoc.X;
			int y = menuLoc.Y;
			var bnd = selectedRoom.Bounds;
			bnd.Location += selectedRoom.LayoutOffset;
			if (selectedRoom.Image != null)
			{
				bnd.Width = selectedRoom.Image.Width / 256;
				bnd.Height = selectedRoom.Image.Height / 256;
			}
			if (selectedRoom.Reverse)
			{
				x = bnd.Right * 256 - x;
				y = bnd.Bottom * 256 - y;
			}
			else
			{
				x -= bnd.Left * 256;
				y -= bnd.Top * 256;
			}
			tele.Zone = (Zones)layoutAreaSelect.SelectedIndex;
			if (selectedRoom.Reverse)
				tele.Zone ^= Zones.BlackMarbleGallery;
			tele.Room = (short)Array.IndexOf(mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms, selectedRoom);
			tele.X = (short)x;
			tele.Y = (short)y;
			SelectedTeleportChanged();
		}

		private void editBossTeleportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bossTeleListBox.SelectedIndex = Array.FindIndex(mapInfo.BossTeleports, a => a.Zone == (Zones)layoutAreaSelect.SelectedIndex
																	 && a.Room == Array.IndexOf(mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms, selectedRoom)
																	 && a.X == menuLoc.X / 256 - selectedRoom.X - selectedRoom.LayoutOffsetX
																	 && a.Y == menuLoc.Y / 256 - selectedRoom.Y - selectedRoom.LayoutOffsetY);
			tabControl1.SelectedIndex = 2;
		}

		private void roomTilePanel_Paint(object sender, PaintEventArgs e)
		{
			DrawRoomTiles(e.Graphics);
		}

		private void SelectedTileChanged()
		{
			DrawRoomTiles();
			suppressEvents = true;
			MapTile mapTile = selectedRoom.Tiles[selectedTile.Y][selectedTile.X];
			if (mapTile == null)
			{
				tileTypeSelector.SelectedIndex = 0;
				tileLeftSelector.SelectedIndex = 0;
				tileLeftSelector.Enabled = false;
				tileTopSelector.SelectedIndex = 0;
				tileTopSelector.Enabled = false;
				tileRightSelector.SelectedIndex = 0;
				tileRightSelector.Enabled = false;
				tileBottomSelector.SelectedIndex = 0;
				tileBottomSelector.Enabled = false;
			}
			else
			{
				tileTypeSelector.SelectedIndex = (int)mapTile.Type;
				tileLeftSelector.SelectedIndex = (int)mapTile.Left;
				tileLeftSelector.Enabled = true;
				tileTopSelector.SelectedIndex = (int)mapTile.Top;
				tileTopSelector.Enabled = true;
				tileRightSelector.SelectedIndex = (int)mapTile.Right;
				tileRightSelector.Enabled = true;
				tileBottomSelector.SelectedIndex = (int)mapTile.Bottom;
				tileBottomSelector.Enabled = true;
			}
			suppressEvents = false;
		}

		private void DrawRoomTiles()
		{
			using (Graphics gfx = roomTilePanel.CreateGraphics())
				DrawRoomTiles(gfx);
		}

		private void DrawRoomTiles(Graphics gfx)
		{
			gfx.Clear(roomTilePanel.BackColor);
			if (selectedRoom == null || selectedRoom.Tiles == null)
				return;
			var mapbmp = new BitmapBits(selectedRoom.Width * 4 + 1, selectedRoom.Height * 4 + 1);
			for (int y = 0; y < selectedRoom.Height; y++)
				for (int x = 0; x < selectedRoom.Width; x++)
					if (selectedRoom.Tiles[y][x] != null)
					{
						int px = x * 4;
						int py = y * 4;
						MapTile exits = selectedRoom.Tiles[y][x];
						mapbmp.FillRectangle((byte)exits.Type, px + 1, py + 1, 3, 3);
						if (exits.Left == TileTypes.Room)
							mapbmp.DrawLine((byte)exits.Type, px, py + 1, px, py + 3);
						else
							mapbmp[px, py + 2] = (byte)exits.Left;
						if (exits.Top == TileTypes.Room)
							mapbmp.DrawLine((byte)exits.Type, px + 1, py, px + 3, py);
						else
							mapbmp[px + 2, py] = (byte)exits.Top;
						if (exits.Left == TileTypes.Room && exits.Top == TileTypes.Room && selectedRoom.Tiles[y - 1][x - 1] != null && selectedRoom.Tiles[y - 1][x - 1].Right == TileTypes.Room && selectedRoom.Tiles[y - 1][x - 1].Bottom == TileTypes.Room)
							mapbmp[px, py] = (byte)exits.Type;
						if (exits.Right != TileTypes.Room)
							mapbmp[px + 4, py + 2] = (byte)exits.Right;
						if (exits.Bottom != TileTypes.Room)
							mapbmp[px + 2, py + 4] = (byte)exits.Bottom;
					}
			gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
			using (var bmp = mapbmp.ToBitmap4bpp(Color.White, Color.Blue, Color.DarkBlue, Color.Black, Color.Red, Color.Yellow, Color.Lime))
				gfx.DrawImage(bmp, 1, 1, selectedRoom.Width * 32, selectedRoom.Height * 32);
			gfx.DrawRectangle(Pens.Lime, selectedTile.X * 32, selectedTile.Y * 32, 32, 32);
		}

		private void roomTilePanel_MouseClick(object sender, MouseEventArgs e)
		{
			selectedTile = new Point(e.X / 32, e.Y / 32);
			SelectedTileChanged();
		}

		private void tileTypeSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppressEvents) return;
			if (tileTypeSelector.SelectedIndex == 0)
			{
				selectedRoom.Tiles[selectedTile.Y][selectedTile.X] = null;
				suppressEvents = true;
				tileLeftSelector.SelectedIndex = 0;
				tileLeftSelector.Enabled = false;
				tileTopSelector.SelectedIndex = 0;
				tileTopSelector.Enabled = false;
				tileRightSelector.SelectedIndex = 0;
				tileRightSelector.Enabled = false;
				tileBottomSelector.SelectedIndex = 0;
				tileBottomSelector.Enabled = false;
				suppressEvents = false;
			}
			else
			{
				if (selectedRoom.Tiles[selectedTile.Y][selectedTile.X] == null)
					selectedRoom.Tiles[selectedTile.Y][selectedTile.X] = new MapTile();
				selectedRoom.Tiles[selectedTile.Y][selectedTile.X].Type = (TileTypes)tileTypeSelector.SelectedIndex;
			}
			selectedRoom.GenerateRegionInfo();
			DrawLayout();
			DrawRoomTiles();
		}

		private void tileLeftSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppressEvents) return;
			selectedRoom.Tiles[selectedTile.Y][selectedTile.X].Left = (TileTypes)tileLeftSelector.SelectedIndex;
			selectedRoom.GenerateRegionInfo();
			DrawLayout();
			DrawRoomTiles();
		}

		private void tileTopSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppressEvents) return;
			selectedRoom.Tiles[selectedTile.Y][selectedTile.X].Top = (TileTypes)tileTopSelector.SelectedIndex;
			selectedRoom.GenerateRegionInfo();
			DrawLayout();
			DrawRoomTiles();
		}

		private void tileRightSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppressEvents) return;
			selectedRoom.Tiles[selectedTile.Y][selectedTile.X].Right = (TileTypes)tileRightSelector.SelectedIndex;
			selectedRoom.GenerateRegionInfo();
			DrawLayout();
			DrawRoomTiles();
		}

		private void tileBottomSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppressEvents) return;
			selectedRoom.Tiles[selectedTile.Y][selectedTile.X].Bottom = (TileTypes)tileBottomSelector.SelectedIndex;
			selectedRoom.GenerateRegionInfo();
			DrawLayout();
			DrawRoomTiles();
		}

		private void TeleportRoomChanged()
		{
			var tele = mapInfo.TeleportDests[teleportListBox.SelectedIndex];
			var zone = GetTeleZone(tele);
			if (zone != null && tele.Room < zone.Rooms.Length)
			{
				var room = zone.Rooms[tele.Room];
				if (room.Image != null)
					teleportPreview.Size = room.Image.Size;
				else
					teleportPreview.Size = new Size(room.Width * 256, room.Height * 256);
			}
			else
				teleportPreview.Size = new Size(256, 256);
			DrawTeleportRoom();
		}

		private void DrawTeleportRoom()
		{
			using (Graphics gfx = teleportPreview.CreateGraphics())
				DrawTeleportRoom(gfx);
		}

		private void DrawTeleportRoom(Graphics g2)
		{
			var tele = mapInfo.TeleportDests[teleportListBox.SelectedIndex];
			Bitmap roomImg = null;
			var zone = GetTeleZone(tele);
			if (zone != null && tele.Room < zone.Rooms.Length)
				roomImg = zone.Rooms[tele.Room].Image;
			using (Bitmap bmp = new Bitmap(teleportPreview.Width, teleportPreview.Height))
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				if (roomImg != null)
					gfx.DrawImage(roomImg, 0, 0, roomImg.Width, roomImg.Height);
				else
				{
					gfx.Clear(Color.White);
					gfx.DrawLine(Pens.Red, 0, 0, teleportPreview.Width, teleportPreview.Height);
					gfx.DrawLine(Pens.Red, 0, teleportPreview.Height, teleportPreview.Width, 0);
				}
				int x = tele.X - alucard.Width / 2;
				int y = tele.Y - alucard.Height / 2;
				if (zone?.Map >= Maps.ReverseCastle)
				{
					x = teleportPreview.Width - tele.X - alucard.Width / 2;
					y = teleportPreview.Height - tele.Y - alucard.Height / 2;
				}
				gfx.DrawImage(alucard, x, y, alucard.Width, alucard.Height);
				if (teleLoc.HasValue)
					gfx.DrawImage(alucard, new Rectangle(teleLoc.Value.X - alucard.Width / 2, teleLoc.Value.Y - alucard.Height / 2, alucard.Width, alucard.Height), 0, 0, alucard.Width, alucard.Height, GraphicsUnit.Pixel, imageTrans);
				g2.DrawImage(bmp, 0, 0, teleportPreview.Width, teleportPreview.Height);
			}
		}

		private CastleZone GetTeleZone(TeleportDest tele)
		{
			Zones zid = tele.Zone;
			if (showReverseCastleCheckBox.Checked)
				zid ^= Zones.BlackMarbleGallery;
			if (!zoneDict.TryGetValue(zid, out CastleZone zone))
				zoneDict.TryGetValue(zid ^ Zones.BlackMarbleGallery, out zone);
			return zone;
		}

		private void SelectedTeleportChanged()
		{
			if (teleportListBox.SelectedIndex == -1) return;
			var tele = mapInfo.TeleportDests[teleportListBox.SelectedIndex];
			suppressEvents = true;
			teleportAreaSelect.SelectedIndex = Array.FindIndex(mapInfo.Zones, a => a.ID == tele.Zone);
			teleportRoomSelect.Value = tele.Room;
			teleportPrevTSSelect.SelectedIndex = Array.FindIndex(mapInfo.Zones, a => a.ID == tele.PreviousTileset);
			teleportXCoord.Value = tele.X;
			teleportYCoord.Value = tele.Y;
			suppressEvents = false;
			TeleportRoomChanged();
		}

		private void teleportListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectedTeleportChanged();
		}

		private void teleportAreaSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.TeleportDests[teleportListBox.SelectedIndex].Zone = mapInfo.Zones[teleportAreaSelect.SelectedIndex].ID;
			TeleportRoomChanged();
		}

		private void teleportRoomSelect_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.TeleportDests[teleportListBox.SelectedIndex].Room = (short)teleportRoomSelect.Value;
			TeleportRoomChanged();
		}

		private void teleportPrevTSSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.TeleportDests[teleportListBox.SelectedIndex].PreviousTileset = mapInfo.Zones[teleportPrevTSSelect.SelectedIndex].ID;
		}

		private void teleportXCoord_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.TeleportDests[teleportListBox.SelectedIndex].X = (short)teleportXCoord.Value;
			DrawTeleportRoom();
		}

		private void teleportYCoord_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.TeleportDests[teleportListBox.SelectedIndex].Y = (short)teleportYCoord.Value;
			DrawTeleportRoom();
		}

		private void showReverseCastleCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			TeleportRoomChanged();
		}

		private void teleportPreview_Paint(object sender, PaintEventArgs e)
		{
			DrawTeleportRoom(e.Graphics);
		}

		private void teleportPreview_MouseMove(object sender, MouseEventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1)
				return;
			teleLoc = e.Location;
			DrawTeleportRoom();
		}

		private void teleportPreview_MouseLeave(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1)
				return;
			teleLoc = null;
			DrawTeleportRoom();
		}

		private void teleportPreview_MouseClick(object sender, MouseEventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1)
				return;
			var tele = mapInfo.TeleportDests[teleportListBox.SelectedIndex];
			int x = e.X;
			int y = e.Y;
			if (GetTeleZone(tele)?.Map >= Maps.ReverseCastle)
			{
				x = teleportPreview.Width - e.X;
				y = teleportPreview.Height - e.Y;
			}
			tele.X = (short)x;
			tele.Y = (short)y;
			suppressEvents = true;
			teleportXCoord.Value = x;
			teleportYCoord.Value = y;
			suppressEvents = false;
			teleLoc = null;
			DrawTeleportRoom();
		}

		private void BossTeleRoomChanged()
		{
			var tele = mapInfo.BossTeleports[bossTeleListBox.SelectedIndex];
			var zone = GetTeleZone(tele);
			if (zone != null && tele.Room < zone.Rooms.Length)
			{
				var room = zone.Rooms[tele.Room];
				if (room.Image != null)
					bossTelePreview.Size = room.Image.Size;
				else
					bossTelePreview.Size = new Size(room.Width * 256, room.Height * 256);
			}
			else
				bossTelePreview.Size = new Size(256, 256);
			DrawBossTeleRoom();
		}

		private void DrawBossTeleRoom()
		{
			using (Graphics gfx = bossTelePreview.CreateGraphics())
				DrawBossTeleRoom(gfx);
		}

		private void DrawBossTeleRoom(Graphics g2)
		{
			var tele = mapInfo.BossTeleports[bossTeleListBox.SelectedIndex];
			Bitmap roomImg = null;
			var zone = GetTeleZone(tele);
			if (zone != null && tele.Room < zone.Rooms.Length)
				roomImg = zone.Rooms[tele.Room].Image;
			using (Bitmap bmp = new Bitmap(bossTelePreview.Width, bossTelePreview.Height))
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				if (roomImg != null)
					gfx.DrawImage(roomImg, 0, 0, roomImg.Width, roomImg.Height);
				else
				{
					gfx.Clear(Color.White);
					gfx.DrawLine(Pens.Red, 0, 0, bossTelePreview.Width, bossTelePreview.Height);
					gfx.DrawLine(Pens.Red, 0, bossTelePreview.Height, bossTelePreview.Width, 0);
				}
				gfx.FillRectangle(bossTeleBrush, tele.X * 256, tele.Y * 256, 256, 256);
				g2.DrawImage(bmp, 0, 0, bossTelePreview.Width, bossTelePreview.Height);
			}
		}

		private CastleZone GetTeleZone(BossTeleport tele)
		{
			zoneDict.TryGetValue(tele.Zone, out CastleZone zone);
			return zone;
		}

		private void SelectedBossTeleChanged()
		{
			if (bossTeleListBox.SelectedIndex == -1) return;
			var tele = mapInfo.BossTeleports[bossTeleListBox.SelectedIndex];
			suppressEvents = true;
			bossTeleAreaSelect.SelectedIndex = Array.FindIndex(mapInfo.Zones, a => a.ID == tele.Zone);
			bossTeleRoomSelect.Value = tele.Room;
			bossTeleXCoord.Value = tele.X;
			bossTeleYCoord.Value = tele.Y;
			bossTeleBossSelect.Value = tele.BossID;
			suppressEvents = false;
			BossTeleRoomChanged();
		}

		private void bossTeleListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectedBossTeleChanged();
		}

		private void bossTeleAreaSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (bossTeleAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].Zone = mapInfo.Zones[bossTeleAreaSelect.SelectedIndex].ID;
			BossTeleRoomChanged();
		}

		private void bossTeleRoomSelect_ValueChanged(object sender, EventArgs e)
		{
			if (bossTeleAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].Room = (short)bossTeleRoomSelect.Value;
			BossTeleRoomChanged();
		}

		private void bossTeleXCoord_ValueChanged(object sender, EventArgs e)
		{
			if (bossTeleAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].X = (short)bossTeleXCoord.Value;
			DrawBossTeleRoom();
		}

		private void bossTeleYCoord_ValueChanged(object sender, EventArgs e)
		{
			if (bossTeleAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].Y = (short)bossTeleYCoord.Value;
			DrawBossTeleRoom();
		}

		private void bossTeleBossSelect_ValueChanged(object sender, EventArgs e)
		{
			if (bossTeleAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].BossID = (int)bossTeleBossSelect.Value;
		}

		private void bossTeleViewDest_Click(object sender, EventArgs e)
		{
			teleportListBox.SelectedIndex = mapInfo.BossTeleports[bossTeleListBox.SelectedIndex].DestID;
			tabControl1.SelectedIndex = 1;
		}

		private void bossTelePreview_Paint(object sender, PaintEventArgs e)
		{
			DrawBossTeleRoom(e.Graphics);
		}
	}
}
