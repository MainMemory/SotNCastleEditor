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

namespace SotNCastleEditor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		#region ROM Data
		public static byte[][][] fgametiles;
		public static byte[][][] fgamepalettes;
		readonly int teltbloff = Util.AdjustOffset(0xAE444);

		readonly ZoneInfo[] stageFiles =
		{
			new ZoneInfo("Marble Gallery", Zones.MarbleGallery, Maps.NormalCastle, 0x048f9a38, 0x488F688),
			new ZoneInfo("Outer Wall", Zones.OuterWall, Maps.NormalCastle, 0x049d18b8, 0x4967E38),
			new ZoneInfo("Long Library", Zones.LongLibrary, Maps.NormalCastle, 0x047a1ae8, 0x473C0B8),
			new ZoneInfo("Catacombs", Zones.Catacombs, Maps.NormalCastle, 0x0448f938, 0x4511EC8),
			new ZoneInfo("Olrox's Quarters", Zones.OlroxsQuarters, Maps.NormalCastle, 0x04aa0438, 0x4A369B8),
			new ZoneInfo("Abandoned Mine", Zones.AbandonedMine, Maps.NormalCastle, 0x045e8ae8, 0x462BDD8),
			new ZoneInfo("Royal Chapel", Zones.RoyalChapel, Maps.NormalCastle, 0x04675f08, 0x46F1658),
			new ZoneInfo("Castle Entrance", Zones.CastleEntrance, Maps.NormalCastle, 0x053f4708, 0x538BEE8),
			new ZoneInfo("Castle Center", Zones.CastleCenter, Maps.NormalCastle, 0x0455bff8, 0x459E9B8),
			new ZoneInfo("Underground Caverns", Zones.UndergroundCaverns, Maps.NormalCastle, 0x04c307e8, 0x4BCC018),
			new ZoneInfo("Colosseum", Zones.Colosseum, Maps.NormalCastle, 0x043c2018, 0x4445808),
			new ZoneInfo("Castle Keep", Zones.CastleKeep, Maps.NormalCastle, 0x0560e7b8, 0x55BF3D8),
			new ZoneInfo("Alchemy Laboratory", Zones.AlchemyLaboratory, Maps.NormalCastle, 0x054b0c88, 0x5454E88),
			new ZoneInfo("Clock Tower", Zones.ClockTower, Maps.NormalCastle, 0x055724b8, 0x5508108),
			new ZoneInfo("Warp Rooms", Zones.WarpRooms, Maps.NormalCastle, 0x05883408, 0x5819058),
			new ZoneInfo("Nightmare", Zones.Nightmare, Maps.NormalBoss, 0x05af2478, 0x5A889F8),
			new ZoneInfo("Cerberus", Zones.Cerberus, Maps.NormalBoss, 0x066b32f8, 0x66574F8),
			new ZoneInfo("Richter", Zones.Richter, Maps.NormalBoss, 0x063aa448, 0x6342E88),
			new ZoneInfo("Hippogryph", Zones.Hippogryph, Maps.NormalBoss, 0x06304e48, 0x62A9048),
			new ZoneInfo("Doppleganger10", Zones.Doppleganger10, Maps.NormalBoss, 0x06246d38, 0x61E1C38),
			new ZoneInfo("Scylla", Zones.Scylla, Maps.NormalBoss, 0x061a60b8, 0x613C638),
			new ZoneInfo("Werewolf & Minotaur", Zones.Werewolf_Minotaur, Maps.NormalBoss, 0x060fca68, 0x60A83D8),
			new ZoneInfo("Legion", Zones.Legion, Maps.NormalBoss, 0x0606dab8, 0x6004968),
			new ZoneInfo("Olrox", Zones.Olrox, Maps.NormalBoss, 0x05fa9dc8, 0x5F40C78),
			new ZoneInfo("Final Stage: Bloodlines", Zones.FinalStageBloodlines, Maps.Prologue, 0x0533efc8, 0x52D70D8),
			new ZoneInfo("Black Marble Gallery", Zones.BlackMarbleGallery, Maps.ReverseCastle, 0x04f84a28, 0x4F1B8D8),
			new ZoneInfo("Reverse Outer Wall", Zones.ReverseOuterWall, Maps.ReverseCastle, 0x0504f558, 0x4FE6D38),
			new ZoneInfo("Forbidden Library", Zones.ForbiddenLibrary, Maps.ReverseCastle, 0x04ee2218, 0x4E851B8),
			new ZoneInfo("Floating Catacombs", Zones.FloatingCatacombs, Maps.ReverseCastle, 0x04cfa0b8, 0x4C9F518),
			new ZoneInfo("Death Wing's Lair", Zones.DeathWingsLair, Maps.ReverseCastle, 0x050f7948, 0x5090CB8),
			new ZoneInfo("Cave", Zones.Cave, Maps.ReverseCastle, 0x04da4968, 0x4D48B68),
			new ZoneInfo("Anti-Chapel", Zones.AntiChapel, Maps.ReverseCastle, 0x04e31458, 0x4DD68B8),
			new ZoneInfo("Reverse Entrance", Zones.ReverseEntrance, Maps.ReverseCastle, 0x051ac758, 0x5150958),
			new ZoneInfo("Reverse Castle Center", Zones.ReverseCastleCenter, Maps.ReverseCastle, 0x056bd9e8, 0x5654898),
			new ZoneInfo("Reverse Caverns", Zones.ReverseCaverns, Maps.ReverseCastle, 0x0526a868, 0x5202978),
			new ZoneInfo("Reverse Colosseum", Zones.ReverseColosseum, Maps.ReverseCastle, 0x057509e8, 0x56F2728),
			new ZoneInfo("Reverse Castle Keep", Zones.ReverseKeep, Maps.ReverseCastle, 0x057df998, 0x57933A8),
			new ZoneInfo("Necromancy Laboratory", Zones.NecromancyLaboratory, Maps.ReverseCastle, 0x05902278, 0x589B5E8),
			new ZoneInfo("Reverse Clock Tower", Zones.ReverseClockTower, Maps.ReverseCastle, 0x059bb0d8, 0x5951F88),
			new ZoneInfo("Reverse Warp Rooms", Zones.ReverseWarpRooms, Maps.ReverseCastle, 0x05a6e358, 0x5A05208),
			new ZoneInfo("Galamoth", Zones.Galamoth, Maps.ReverseBoss, 0x06a5f2e8, 0x69FA1E8),
			new ZoneInfo("Akmodan II", Zones.AkmodanII, Maps.ReverseBoss, 0x069d1598, 0x6968448),
			new ZoneInfo("Dracula", Zones.Dracula, Maps.ReverseBoss, 0x0692b668, 0x68C2E48),
			new ZoneInfo("Doppleganger40", Zones.Doppleganger40, Maps.ReverseBoss, 0x06861468, 0x67FC368),
			new ZoneInfo("Creature", Zones.Creature, Maps.ReverseBoss, 0x067cfff8, 0x6768108),
			new ZoneInfo("Medusa", Zones.Medusa, Maps.ReverseBoss, 0x067422a8, 0x66DC878),
			new ZoneInfo("Death", Zones.Death, Maps.ReverseBoss, 0x06620c28, 0x65B8408),
			new ZoneInfo("Beezlebub", Zones.Beezlebub, Maps.ReverseBoss, 0x06590a18, 0x65281F8),
			new ZoneInfo("Trio", Zones.Trio, Maps.ReverseBoss, 0x064705f8, 0x6408708),
			new ZoneInfo("Castle Entrance First Visit", Zones.CastleEntrance1st, Maps.NormalCastle, 0x04b665e8, 0x4AFCB68)
		};

		readonly Zones[][] matchingZones =
		{
			new[] { Zones.MarbleGallery, Zones.BlackMarbleGallery },
			new[] { Zones.OuterWall, Zones.ReverseOuterWall, Zones.Doppleganger10, Zones.Creature },
			new[] { Zones.LongLibrary, Zones.ForbiddenLibrary },
			new[] { Zones.Catacombs, Zones.FloatingCatacombs, Zones.Legion, Zones.Galamoth },
			new[] { Zones.OlroxsQuarters, Zones.DeathWingsLair, Zones.Olrox, Zones.AkmodanII },
			new[] { Zones.AbandonedMine, Zones.Cave, Zones.Cerberus, Zones.Death },
			new[] { Zones.RoyalChapel, Zones.AntiChapel, Zones.Hippogryph, Zones.Medusa },
			new[] { Zones.CastleEntrance, Zones.CastleEntrance1st, Zones.ReverseEntrance },
			new[] { Zones.CastleCenter, Zones.ReverseCastleCenter, Zones.Dracula },
			new[] { Zones.UndergroundCaverns, Zones.ReverseCaverns, Zones.Scylla, Zones.Doppleganger40 },
			new[] { Zones.Colosseum, Zones.ReverseColosseum, Zones.Werewolf_Minotaur, Zones.Trio },
			new[] { Zones.CastleKeep, Zones.ReverseKeep, Zones.Richter },
			new[] { Zones.AlchemyLaboratory, Zones.NecromancyLaboratory, Zones.Beezlebub },
			new[] { Zones.ClockTower, Zones.ReverseClockTower },
			new[] { Zones.WarpRooms, Zones.ReverseWarpRooms }
		};
		#endregion

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
		readonly Brush selectBrush = new SolidBrush(Color.FromArgb(64, Color.Blue));
		readonly Pen borderPen = new Pen(Color.White, 4);
		readonly System.Drawing.Imaging.ImageAttributes imageTrans = new System.Drawing.Imaging.ImageAttributes();
		readonly Bitmap alucard = new Bitmap("Alucard.png");
		readonly Brush backBrush = new TextureBrush(new Bitmap("back.png"));
		readonly string baseFilePath = Path.Combine(Application.StartupPath, "zones.json");

		string fileName;
		MapInfo mapInfo;
		Dictionary<Zones, ZoneInfo> zoneDict;
		Dictionary<Maps, List<ZoneInfo>> mapDict;
		double zoomLevel = 1;
		ZoneRoom selectedRoom, highlightRoom;
		Point? movePoint;
		Size moveOffset;
		Point selectedTile;
		Point menuLoc;
		Point? teleLoc;
		bool suppressEvents;

		#region Load ROM Data
		private void LoadDataFromBin()
		{
			mapInfo = new MapInfo() { Zones = stageFiles };
			using (var fs = File.OpenRead(@"D:\CD\PS1\Castlevania - Symphony of the Night (Track 1).bin"))
			using (MemoryStream ms = new MemoryStream((int)(fs.Length / 2352 * 2048)))
			{
				fs.Seek(0x18, SeekOrigin.Begin);
				byte[] sector = new byte[2048];
				while (fs.Position < fs.Length)
				{
					fs.Read(sector, 0, 2048);
					ms.Write(sector, 0, 2048);
					fs.Seek(304, SeekOrigin.Current);
				}
				(fgametiles, fgamepalettes) = LoadGraphicsFile(ms, Util.AdjustOffset(0x38294B8), true);
				foreach (var zone in stageFiles)
					zone.LoadData(ms);
				mapInfo.Teleports = new TeleportDest[stageFiles.SelectMany(a => a.Rooms).Select(b => b.Teleport).Max().Value];
				ms.Seek(teltbloff, SeekOrigin.Begin);
				var br = new BinaryReader(ms);
				for (int i = 0; i < mapInfo.Teleports.Length; i++)
					mapInfo.Teleports[i] = new TeleportDest() { X = br.ReadInt16(), Y = br.ReadInt16(), Room = (short)(br.ReadInt16() / 8), PreviousTileset = (Zones)br.ReadInt16(), Zone = (Zones)br.ReadInt16() };
			}
			var roomlist = new List<MapRoom>();
			using (var bmp = new Bitmap(@"D:\CD\PS1\SOTN\F_MAP.BIN.png"))
			{
				var bmpd = bmp.LockBits(new Rectangle(0, 0, 256, 256), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
				var pix = new byte[bmpd.Stride * bmpd.Height];
				System.Runtime.InteropServices.Marshal.Copy(bmpd.Scan0, pix, 0, pix.Length);
				bmp.UnlockBits(bmpd);
				var maptiles = new MapTile[64, 64];
				const int leftpx = 256 - 1;
				const int toppx = -256 + 1;
				const int rightpx = 256 + 3;
				const int bottompx = (256 * 3) + 1;
				int tilepx = 256 + 1;
				for (int y = 0; y < 64; y++)
				{
					for (int x = 0; x < 64; x++)
					{
						var type = (TileTypes)pix[tilepx];
						if (type != TileTypes.Empty)
						{
							MapTile tile = new MapTile() { Type = type };
							var exit = (TileTypes)pix[tilepx + leftpx];
							if (exit != TileTypes.Empty && pix[tilepx + leftpx - 256] != 0)
								exit = TileTypes.Room;
							tile.Left = exit;
							exit = (TileTypes)pix[tilepx + toppx];
							if (exit != TileTypes.Empty && pix[tilepx + toppx - 1] != 0)
								exit = TileTypes.Room;
							tile.Top = exit;
							exit = (TileTypes)pix[tilepx + rightpx];
							if (exit != TileTypes.Empty && pix[tilepx + rightpx - 256] != 0)
								exit = TileTypes.Room;
							tile.Right = exit;
							exit = (TileTypes)pix[tilepx + bottompx];
							if (exit != TileTypes.Empty && pix[tilepx + bottompx - 1] != 0)
								exit = TileTypes.Room;
							tile.Bottom = exit;
							maptiles[x, y] = tile;
						}
						tilepx += 4;
					}
					tilepx += 256 * 3;
				}
				for (int y = 0; y < 64; y++)
					for (int x = 0; x < 64; x++)
					{
						var tile = maptiles[x, y];
						if (tile != null)
						{
							MapRoom room = new MapRoom();
							List<Point> points = new List<Point>() { new Point(x, y) };
							for (int i = 0; i < points.Count; i++)
							{
								MapTile tile2 = maptiles[points[i].X, points[i].Y];
								if (tile2.Left == TileTypes.Room)
								{
									var pt = new Point(points[i].X - 1, points[i].Y);
									if (!points.Contains(pt))
										points.Add(pt);
								}
								if (tile2.Top == TileTypes.Room)
								{
									var pt = new Point(points[i].X, points[i].Y - 1);
									if (!points.Contains(pt))
										points.Add(pt);
								}
								if (tile2.Right == TileTypes.Room)
								{
									var pt = new Point(points[i].X + 1, points[i].Y);
									if (!points.Contains(pt))
										points.Add(pt);
								}
								if (tile2.Bottom == TileTypes.Room)
								{
									var pt = new Point(points[i].X, points[i].Y + 1);
									if (!points.Contains(pt))
										points.Add(pt);
								}
							}
							room.Bounds = Rectangle.FromLTRB(points.Min(a => a.X), points.Min(a => a.Y), points.Max(a => a.X) + 1, points.Max(a => a.Y) + 1);
							room.Tiles = new MapTile[room.Bounds.Width, room.Bounds.Height];
							foreach (var pt in points)
							{
								room.Tiles[pt.X - room.Bounds.X, pt.Y - room.Bounds.Y] = maptiles[pt.X, pt.Y];
								maptiles[pt.X, pt.Y] = null;
							}
							roomlist.Add(room);
						}
					}
			}
			roomlist.Sort((a, b) => -(a.Bounds.Width * a.Bounds.Height).CompareTo(b.Bounds.Width * b.Bounds.Height));
			foreach (var zone in stageFiles)
				foreach (var room in zone.Rooms)
				{
					if (room.Teleport.HasValue)
					{
						room.Bounds = room.LayoutBounds;
						continue;
					}
					var bnd = room.LayoutBounds;
					if (zone.Map >= Maps.ReverseCastle)
						bnd = new Rectangle(0x40 - bnd.Right, 0x40 - bnd.Bottom, bnd.Width, bnd.Height);
					var map = roomlist.FirstOrDefault(a => bnd.Contains(a.Bounds));
					if (map != null)
					{
						if (zone.Map >= Maps.ReverseCastle)
							room.Bounds = new Rectangle(0x40 - map.Bounds.Right, 0x40 - map.Bounds.Bottom, map.Bounds.Width, map.Bounds.Height);
						else
							room.Bounds = map.Bounds;
						room.Tiles = new MapTile[map.Bounds.Height][];
						for (int y = 0; y < map.Bounds.Height; y++)
						{
							room.Tiles[y] = new MapTile[map.Bounds.Width];
							for (int x = 0; x < map.Bounds.Width; x++)
								if (zone.Map >= Maps.ReverseCastle)
									room.Tiles[y][x] = map.Tiles[map.Bounds.Width - x - 1, map.Bounds.Height - y - 1]?.Flip();
								else
									room.Tiles[y][x] = map.Tiles[x, y];
						}
					}
					else
						room.Bounds = room.LayoutBounds;
					room.LayoutOffset = (Size)(room.LayoutBounds.Location - (Size)room.Bounds.Location);
				}
			var matchroomlist = new List<RoomIndex[]>();
			foreach (var set in matchingZones)
			{
				var setlist = new List<List<(RoomIndex ridx, ZoneRoom room)>>();
				foreach (var zid in set)
				{
					var zone = stageFiles.Single(b => b.ID == zid);
					for (short i = 0; i < zone.Rooms.Length; i++)
					{
						var bnd = zone.Map >= Maps.ReverseCastle ? new Rectangle(0x40 - zone.Rooms[i].Bounds.Right, 0x40 - zone.Rooms[i].Bounds.Bottom, zone.Rooms[i].Bounds.Width, zone.Rooms[i].Bounds.Height) : zone.Rooms[i].Bounds;
						bool found = false;
						foreach (var lst in setlist)
							if (bnd == lst[0].room.Bounds)
							{
								lst.Add((new RoomIndex() { Zone = zid, Room = i }, zone.Rooms[i]));
								found = true;
								break;
							}
						if (!found)
							setlist.Add(new List<(RoomIndex ridx, ZoneRoom room)>() { (new RoomIndex() { Zone = zid, Room = i }, zone.Rooms[i]) });
					}
				}
				matchroomlist.AddRange(setlist.Where(c => c.Count > 1).Select(a => a.Select(b => b.ridx).ToArray()));
			}
			mapInfo.MatchingRooms = matchroomlist.ToArray();
			foreach (var zone in stageFiles)
			{
				MakeMap(zone.Name.Replace(':', '-'), areaColors[0], zone.Rooms.Select(a => a.LayoutBounds).ToArray());
				MakeMap(zone.Name.Replace(':', '-') + " Map", areaColors[0], zone.Rooms.Select(a => a.Bounds).ToArray());
			}
			GenerateMapBin(mapInfo, @"D:\CD\PS1\SOTN\F_MAP2.BIN.png");
			File.WriteAllText("zones.json", JsonConvert.SerializeObject(mapInfo, Formatting.Indented));
		}

		private static void GenerateMapBin(MapInfo info, string filename)
		{
			BitmapBits mapbmp = new BitmapBits(256, 256);
			foreach (var room in info.Zones.Where(a => a.Map == Maps.NormalCastle).SelectMany(a => a.Rooms).Where(a => a.Tiles != null))
			{
				for (int y = 0; y < room.Bounds.Height; y++)
					for (int x = 0; x < room.Bounds.Width; x++)
						if (room.Tiles[y][x] != null)
						{
							int px = (room.Bounds.X + x) * 4;
							int py = (room.Bounds.Y + y) * 4;
							MapTile exits = room.Tiles[y][x];
							mapbmp.FillRectangle((byte)exits.Type, px + 1, py + 1, 3, 3);
							if (exits.Left == TileTypes.Room)
								mapbmp.DrawLine((byte)exits.Type, px, py + 1, px, py + 3);
							else
								mapbmp[px, py + 2] = (byte)exits.Left;
							if (exits.Top == TileTypes.Room)
								mapbmp.DrawLine((byte)exits.Type, px + 1, py, px + 3, py);
							else
								mapbmp[px + 2, py] = (byte)exits.Top;
							if (exits.Left == TileTypes.Room && exits.Top == TileTypes.Room && room.Tiles[y - 1][x - 1] != null && room.Tiles[y - 1][x - 1].Right == TileTypes.Room && room.Tiles[y - 1][x - 1].Bottom == TileTypes.Room)
								mapbmp[px, py] = (byte)exits.Type;
							if (px < 252 && exits.Right != TileTypes.Room)
								mapbmp[px + 4, py + 2] = (byte)exits.Right;
							if (py < 252 && exits.Bottom != TileTypes.Room)
								mapbmp[px + 2, py + 4] = (byte)exits.Bottom;
						}
			}
			mapbmp.ReplaceColor((byte)TileTypes.Secret, (byte)TileTypes.Empty);
			using (var bmp = mapbmp.ToBitmap4bpp(Color.Magenta, Color.Blue, Color.DarkBlue, Color.Black, Color.Red, Color.Yellow))
				bmp.Save(filename);
		}

		private void MakeMap(string mapfn, Brush brush, Rectangle[] rooms)
		{
			using (Bitmap bmp = new Bitmap(1300, 1300))
			{
				using (Graphics gfx = Graphics.FromImage(bmp))
					for (int j = 0; j < rooms.Length; j++)
						if (!rooms[j].IsEmpty)
						{
							Rectangle tmp = new Rectangle(rooms[j].X * 20, rooms[j].Y * 20, rooms[j].Width * 20, rooms[j].Height * 20);
							gfx.FillRectangle(brush, tmp);
							gfx.DrawRectangle(Pens.White, tmp);
							gfx.DrawString(j.ToString(), Font, Brushes.Black, tmp.X + 1, tmp.Y + 1);
						}
				bmp.Save($@"D:\CD\PS1\SOTN\maps\{mapfn}.png");
			}
		}

		public static (byte[][][] tiles, byte[][][] colors) LoadGraphicsFile(Stream stream, int offset, bool isfgame)
		{
			BinaryReader br = new BinaryReader(stream);
			stream.Seek(offset, SeekOrigin.Begin);
			byte[][][] tiles = new byte[8][][];
			for (int ts = 0; ts < 8; ts++)
			{
				tiles[ts] = new byte[256][];
				BitmapBits tsbmp = new BitmapBits(256, 256);
				for (int y = 0; y < 2; y++)
					for (int x = 0; x < 2; x++)
					{
						BitmapBits tmp = new BitmapBits(128, 128);
						for (int i = 0; i < tmp.Bits.Length; i += 2)
						{
							byte px = br.ReadByte();
							tmp.Bits[i] = (byte)(px & 0xF);
							tmp.Bits[i + 1] = (byte)(px >> 4);
						}
						tsbmp.DrawBitmap(tmp, x * 128, y * 128);
					}
				int ti = 0;
				for (int y = 0; y < 256; y += 16)
					for (int x = 0; x < 256; x += 16)
						tiles[ts][ti++] = tsbmp.GetSection(x, y, 16, 16).Bits;
			}
			byte[][][] colors = new byte[256][][];
			for (int pn = 0; pn < 256; pn++)
				colors[pn] = new byte[16][];
			if (isfgame)
			{
				for (int pn = 0; pn < 256; pn++)
					for (int pi = 0; pi < 16; pi++)
						colors[pn][pi] = ABGR1555ToARGB8888(br.ReadUInt16());
			}
			else
			{
				stream.Seek(offset, SeekOrigin.Begin);
				for (int ts = 0; ts < 4; ts++)
				{
					stream.Seek(0x5C00, SeekOrigin.Current);
					for (int pn = 0; pn < 32; pn++)
						for (int pi = 0; pi < 16; pi++)
							colors[(ts * 4) + (pn / 2 * 16) + (pn % 2)][pi] = ABGR1555ToARGB8888(br.ReadUInt16());
					stream.Seek(0x1C00, SeekOrigin.Current);
					for (int pn = 0; pn < 32; pn++)
						for (int pi = 0; pi < 16; pi++)
							colors[(ts * 4) + (pn / 2 * 16) + (pn % 2) + 2][pi] = ABGR1555ToARGB8888(br.ReadUInt16());
				}
			}
			return (tiles, colors);
		}

		private static byte[] ABGR1555ToARGB8888(ushort c)
		{
			int b = (c >> 10) & 0x1F;
			int g = (c >> 5) & 0x1F;
			int r = c & 0x1F;
			return BitConverter.GetBytes(Color.FromArgb((c & 0x8000) == 0x8000 ? 0x80 : 0xFF, r << 3 | r >> 2, g << 3 | g >> 2, b << 3 | b >> 2).ToArgb());
		}
		#endregion

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
			using (var ofd = new OpenFileDialog() { DefaultExt = "caspat", Filter = "Castle Files|*.castle;*.caspat|All Files|*.*", RestoreDirectory = true })
				if (ofd.ShowDialog(this) == DialogResult.OK)
					OpenFile(ofd.FileName);
		}

		private void OpenFile(string filename)
		{
			if (filename != null)
			{
				if (Path.GetExtension(filename).Equals(".caspat", StringComparison.OrdinalIgnoreCase))
				{
					mapInfo = MapInfo.Load(baseFilePath);
					MapPatch.Load(filename).Apply(mapInfo);
				}
				else
					mapInfo = MapInfo.Load(filename);
			}
			else
				mapInfo = MapInfo.Load(baseFilePath);
			zoneDict = new Dictionary<Zones, ZoneInfo>();
			mapDict = new Dictionary<Maps, List<ZoneInfo>>();
			layoutAreaSelect.BeginUpdate();
			layoutAreaSelect.Items.Clear();
			teleportAreaSelect.BeginUpdate();
			teleportAreaSelect.Items.Clear();
			teleportPrevTSSelect.BeginUpdate();
			teleportPrevTSSelect.Items.Clear();
			foreach (var zone in mapInfo.Zones)
			{
				layoutAreaSelect.Items.Add(zone.Name);
				teleportAreaSelect.Items.Add(zone.Name);
				teleportPrevTSSelect.Items.Add(zone.Name);
				zoneDict.Add(zone.ID, zone);
				if (!mapDict.ContainsKey(zone.Map))
					mapDict.Add(zone.Map, new List<ZoneInfo>());
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
			foreach (var set in mapInfo.MatchingRooms)
			{
				var rooms = set.Select(a => zoneDict[a.Zone].Rooms[a.Room]).ToArray();
				foreach (var rm in rooms)
					rm.MatchingRooms = rooms;
			}
			teleportListBox.Items.Clear();
			teleportListBox.Items.AddRange(mapInfo.Teleports.Select(a => $"{a.Zone} {a.Room} {a.X} {a.Y}").ToArray());
			fileName = filename;
			layoutAreaSelect.SelectedIndex = 0;
			teleportListBox.SelectedIndex = 0;
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
			using (var sfd = new SaveFileDialog() { DefaultExt = "caspat", Filter = "Castle Files|*.castle;*.caspat|All Files|*.*", RestoreDirectory = true })
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
				MapPatch.Create(MapInfo.Load(baseFilePath), mapInfo).Save(filename);
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
				ZoneInfo curZone = mapInfo.Zones[layoutAreaSelect.SelectedIndex];
				List<ZoneInfo> zones = new List<ZoneInfo>();
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
				if (showTeleportDestsToolStripMenuItem.Checked)
				{
					foreach (var tele in mapInfo.Teleports)
					{
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
						var tele = mapInfo.Teleports[selectedRoom.Teleport.Value];
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
			selectedRoom = mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms.FirstOrDefault(a => a.Bounds.Contains(chunkLoc) && (a.Tiles == null || a.Tiles[chunkLoc.Y - a.Bounds.Y][chunkLoc.X - a.Bounds.X] != null));
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
				var rm = mapInfo.Zones[layoutAreaSelect.SelectedIndex].Rooms.FirstOrDefault(a => a.Bounds.Contains(chunkLoc) && (a.Tiles == null || a.Tiles[chunkLoc.Y - a.Bounds.Y][chunkLoc.X - a.Bounds.X] != null));
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
			var tele = mapInfo.Teleports[teleportListBox.SelectedIndex];
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
			var tele = mapInfo.Teleports[teleportListBox.SelectedIndex];
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
			var tele = mapInfo.Teleports[teleportListBox.SelectedIndex];
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

		private ZoneInfo GetTeleZone(TeleportDest tele)
		{
			Zones zid = tele.Zone;
			if (showReverseCastleCheckBox.Checked)
				zid ^= Zones.BlackMarbleGallery;
			if (!zoneDict.TryGetValue(zid, out ZoneInfo zone))
				zoneDict.TryGetValue(zid ^ Zones.BlackMarbleGallery, out zone);
			return zone;
		}

		private void SelectedTeleportChanged()
		{
			if (teleportListBox.SelectedIndex == -1) return;
			var tele = mapInfo.Teleports[teleportListBox.SelectedIndex];
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
			mapInfo.Teleports[teleportListBox.SelectedIndex].Zone = mapInfo.Zones[teleportAreaSelect.SelectedIndex].ID;
			TeleportRoomChanged();
		}

		private void teleportRoomSelect_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.Teleports[teleportListBox.SelectedIndex].Room = (short)teleportRoomSelect.Value;
			TeleportRoomChanged();
		}

		private void teleportPrevTSSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.Teleports[teleportListBox.SelectedIndex].PreviousTileset = mapInfo.Zones[teleportPrevTSSelect.SelectedIndex].ID;
		}

		private void teleportXCoord_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.Teleports[teleportListBox.SelectedIndex].X = (short)teleportXCoord.Value;
			DrawTeleportRoom();
		}

		private void teleportYCoord_ValueChanged(object sender, EventArgs e)
		{
			if (teleportAreaSelect.SelectedIndex == -1 || suppressEvents)
				return;
			mapInfo.Teleports[teleportListBox.SelectedIndex].Y = (short)teleportYCoord.Value;
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
			var tele = mapInfo.Teleports[teleportListBox.SelectedIndex];
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
	}

	public class MapInfo
	{
		public ZoneInfo[] Zones { get; set; }
		public TeleportDest[] Teleports { get; set; }
		public RoomIndex[][] MatchingRooms { get; set; }

		public static MapInfo Load(string filename) => JsonConvert.DeserializeObject<MapInfo>(File.ReadAllText(filename));

		public void Save(string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
	}

	public class ZoneInfo
	{
		public string Name { get; set; }
		public Zones ID { get; set; }
		public Maps Map { get; set; }
		public int Offset { get; set; }
		public ZoneRoom[] Rooms { get; set; }
		private int tileOff;
		private byte[][][] stgtiles;
		private byte[][][] stgpal;

		public ZoneInfo() { }

		public ZoneInfo(string name, Zones zone, Maps map, int offset, int tileOff)
		{
			Name = name;
			ID = zone;
			Map = map;
			Offset = offset;
			this.tileOff = tileOff;
		}

		#region ROM Data Loading
		public void LoadData(Stream fileStream)
		{
			Rooms = ReadLayout(fileStream, Util.AdjustOffset(Offset));
		}

		private void GenerateMaps(Stream fileStream)
		{
			(stgtiles, stgpal) = MainForm.LoadGraphicsFile(fileStream, Util.AdjustOffset(tileOff), false);
			var layers = ReadLayers(fileStream);
			Directory.CreateDirectory($@"D:\CD\PS1\SOTN\layers\{ID}");
			for (int i = 0; i < layers.Count; i++)
			{
				var (fg, bg) = layers[i];
				if (bg != null && bg.Image != null)
					bg.Image.Save($@"D:\CD\PS1\SOTN\layers\{ID}\{i}_bg.png");
				if (fg != null && fg.Image != null)
					fg.Image.Save($@"D:\CD\PS1\SOTN\layers\{ID}\{i}_fg.png");
			}
			Directory.CreateDirectory($@"D:\CD\PS1\SOTN\rooms\{ID}");
			for (int i = 0; i < Rooms.Length; i++)
			{
				var room = Rooms[i];
				if (room.LayoutIndex.HasValue)
				{
					var (fg, bg) = layers[room.LayoutIndex.Value];
					int width = room.LayoutBounds.Width * 256;
					int height = room.LayoutBounds.Height * 256;
					using (var bmp = new Bitmap(width, height))
					{
						using (var gfx = Graphics.FromImage(bmp))
						{
							gfx.Clear(Color.Black);
							if (bg != null && bg.Image != null)
							{
								using (var brush = new TextureBrush(bg.Image))
									gfx.FillRectangle(brush, 0, 0, width, height);
							}
							if (fg != null && fg.Image != null)
							{
								using (var brush = new TextureBrush(fg.Image))
									gfx.FillRectangle(brush, 0, 0, width, height);
							}
						}
						bmp.Save($@"D:\CD\PS1\SOTN\rooms\{ID}\{i}.png");
					}
				}
			}
		}

		private static ZoneRoom[] ReadLayout(Stream fileStream, int fileBase)
		{
			BinaryReader br = new BinaryReader(fileStream);
			fileStream.Seek(fileBase + 0x10, SeekOrigin.Begin);
			fileStream.Seek(br.ReadPointer(fileBase), SeekOrigin.Begin);
			List<ZoneRoom> rooms = new List<ZoneRoom>();
			byte l = br.ReadByte();
			while (l != 0x40)
			{
				ZoneRoom rm = new ZoneRoom();
				rm.LayoutBounds = Rectangle.FromLTRB(l, br.ReadByte(), br.ReadByte() + 1, br.ReadByte() + 1);
				byte tele = br.ReadByte();
				if (br.ReadByte() == 0xFF)
					rm.Teleport = tele;
				else
					rm.LayoutIndex = tele;
				rooms.Add(rm);
				fileStream.Seek(2, SeekOrigin.Current);
				l = br.ReadByte();
			}
			return rooms.ToArray();
		}

		private List<(Layer fg, Layer bg)> ReadLayers(Stream fileStream)
		{
			BinaryReader br = new BinaryReader(fileStream);
			int fileBase = Util.AdjustOffset(Offset);
			int roomCount = Rooms.Where(a => a.LayoutIndex.HasValue).Max(a => a.LayoutIndex.Value) + 1;
			fileStream.Seek(fileBase + 0x20, SeekOrigin.Begin);
			int ptroff = br.ReadPointer(fileBase);
			var layouts = new List<(Layer fg, Layer bg)>();
			var layercache = new Dictionary<uint, Layer>();
			for (int i = 0; i < roomCount; i++)
			{
				fileStream.Seek(ptroff, SeekOrigin.Begin);
				uint fgptr = br.ReadUInt32();
				uint bgptr = br.ReadUInt32();
				if (!layercache.TryGetValue(fgptr, out Layer fg))
					if (fgptr > 0x80180000 && fgptr < 0xA0000000)
					{
						fileStream.Seek((int)(fgptr - 0x80180000) + fileBase, SeekOrigin.Begin);
						fg = new Layer(fileStream, fileBase, stgtiles, stgpal);
						layercache.Add(fgptr, fg);
					}
				if (!layercache.TryGetValue(bgptr, out Layer bg))
					if (bgptr > 0x80180000 && bgptr < 0xA0000000)
					{
						fileStream.Seek((int)(bgptr - 0x80180000) + fileBase, SeekOrigin.Begin);
						bg = new Layer(fileStream, fileBase, stgtiles, stgpal);
						layercache.Add(bgptr, bg);
					}
				layouts.Add((fg, bg));
				ptroff += 8;
			}
			return layouts;
		}
		#endregion
	}

	public class ZoneRoom
	{
		[JsonIgnore]
		public Rectangle LayoutBounds { get; set; }
		[JsonIgnore]
		public int? LayoutIndex { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		[JsonIgnore]
		public Rectangle Bounds
		{
			get => new Rectangle(X, Y, Width, Height);
			set
			{
				X = value.X;
				Y = value.Y;
				Width = value.Width;
				Height = value.Height;
			}
		}
		[JsonIgnore]
		public Point Location
		{
			get => new Point(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		[JsonIgnore]
		public Size Size
		{
			get => new Size(Width, Height);
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}
		public int LayoutOffsetX { get; set; }
		public int LayoutOffsetY { get; set; }
		[JsonIgnore]
		public Size LayoutOffset
		{
			get => new Size(LayoutOffsetX, LayoutOffsetY);
			set
			{
				LayoutOffsetX = value.Width;
				LayoutOffsetY = value.Height;
			}
		}
		public int? Teleport { get; set; }
		public MapTile[][] Tiles { get; set; }
		[JsonIgnore]
		public ZoneRoom[] MatchingRooms { get; set; }
		[JsonIgnore]
		public Bitmap Image { get; private set; }
		[JsonIgnore]
		public Region Region { get; private set; }
		[JsonIgnore]
		public GraphicsPath Border { get; private set; }
		[JsonIgnore]
		public Door[] Doors { get; private set; }
		[JsonIgnore]
		public bool Reverse { get; private set; }

		public void SetGraphicsInfo(Bitmap img, bool reverse)
		{
			Image = img;
			Reverse = reverse;
			GenerateRegionInfo();
		}

		public void GenerateRegionInfo()
		{
			Region = new Region(new Rectangle(0, 0, Width * 256, Height * 256));
			if (Tiles != null)
				for (int y = 0; y < Height; y++)
					for (int x = 0; x < Width; x++)
						if (Tiles[y][x] == null)
							Region.Exclude(new Rectangle(x * 256, y * 256, 256, 256));
			Border = new GraphicsPath();
			var doors = new List<Door>();
			if (Tiles != null)
			{
				int? startlt;
				int? startrb;
				for (int y = 0; y < Height; y++)
				{
					startlt = null;
					startrb = null;
					for (int x = 0; x < Width; x++)
						if (Tiles[y][x] != null)
						{
							switch (Tiles[y][x].Top)
							{
								case TileTypes.Empty:
									if (!startlt.HasValue)
										startlt = x * 256;
									break;
								case TileTypes.Room:
									if (startlt.HasValue)
									{
										Border.AddLine(startlt.Value, y * 256 + 2, x * 256, y * 256 + 2);
										Border.StartFigure();
										startlt = null;
									}
									break;
								default:
									if (!startlt.HasValue)
										startlt = x * 256;
									Border.AddLine(startlt.Value, y * 256 + 2, x * 256 + 64, y * 256 + 2);
									Border.StartFigure();
									startlt = x * 256 + 192;
									doors.Add(new Door(x, y, Sides.Top));
									break;
							}
							switch (Tiles[y][x].Bottom)
							{
								case TileTypes.Empty:
									if (!startrb.HasValue)
										startrb = x * 256;
									break;
								case TileTypes.Room:
									if (startrb.HasValue)
									{
										Border.AddLine(startrb.Value, y * 256 + 254, x * 256, y * 256 + 254);
										Border.StartFigure();
										startrb = null;
									}
									break;
								default:
									if (!startrb.HasValue)
										startrb = x * 256;
									Border.AddLine(startrb.Value, y * 256 + 254, x * 256 + 64, y * 256 + 254);
									Border.StartFigure();
									startrb = x * 256 + 192;
									doors.Add(new Door(x, y, Sides.Bottom));
									break;
							}
						}
						else
						{
							if (startlt.HasValue)
							{
								Border.AddLine(startlt.Value, y * 256 + 2, x * 256, y * 256 + 2);
								Border.StartFigure();
								startlt = null;
							}
							if (startrb.HasValue)
							{
								Border.AddLine(startrb.Value, y * 256 + 254, x * 256, y * 256 + 254);
								Border.StartFigure();
								startrb = null;
							}
						}
					if (startlt.HasValue)
					{
						Border.AddLine(startlt.Value, y * 256 + 2, Width * 256, y * 256 + 2);
						Border.StartFigure();
					}
					if (startrb.HasValue)
					{
						Border.AddLine(startrb.Value, y * 256 + 254, Width * 256, y * 256 + 254);
						Border.StartFigure();
					}
				}
				for (int x = 0; x < Width; x++)
				{
					startlt = null;
					startrb = null;
					for (int y = 0; y < Height; y++)
						if (Tiles[y][x] != null)
						{
							switch (Tiles[y][x].Left)
							{
								case TileTypes.Empty:
									if (!startlt.HasValue)
										startlt = y * 256;
									break;
								case TileTypes.Room:
									if (startlt.HasValue)
									{
										Border.AddLine(x * 256 + 2, startlt.Value, x * 256 + 2, y * 256);
										Border.StartFigure();
										startlt = null;
									}
									break;
								default:
									if (!startlt.HasValue)
										startlt = y * 256;
									Border.AddLine(x * 256 + 2, startlt.Value, x * 256 + 2, y * 256 + 64);
									Border.StartFigure();
									startlt = y * 256 + 192;
									doors.Add(new Door(x, y, Sides.Left));
									break;
							}
							switch (Tiles[y][x].Right)
							{
								case TileTypes.Empty:
									if (!startrb.HasValue)
										startrb = y * 256;
									break;
								case TileTypes.Room:
									if (startrb.HasValue)
									{
										Border.AddLine(x * 256 + 254, startrb.Value, x * 256 + 254, y * 256);
										Border.StartFigure();
										startrb = null;
									}
									break;
								default:
									if (!startrb.HasValue)
										startrb = y * 256;
									Border.AddLine(x * 256 + 254, startrb.Value, x * 256 + 254, y * 256 + 64);
									Border.StartFigure();
									startrb = y * 256 + 192;
									doors.Add(new Door(x, y, Sides.Right));
									break;
							}
						}
						else
						{
							if (startlt.HasValue)
							{
								Border.AddLine(x * 256 + 2, startlt.Value, x * 256 + 2, y * 256);
								Border.StartFigure();
								startlt = null;
							}
							if (startrb.HasValue)
							{
								Border.AddLine(x * 256 + 254, startrb.Value, x * 256 + 254, y * 256);
								Border.StartFigure();
								startrb = null;
							}
						}
					if (startlt.HasValue)
					{
						Border.AddLine(x * 256 + 2, startlt.Value, x * 256 + 2, Height * 256);
						Border.StartFigure();
					}
					if (startrb.HasValue)
					{
						Border.AddLine(x * 256 + 254, startrb.Value, x * 256 + 254, Height * 256);
						Border.StartFigure();
					}
				}
			}
			else
				Border.AddRectangle(new Rectangle(2, 2, Width - 2, Height - 2));
			Doors = doors.ToArray();
		}
	}

	public class MapTile
	{
		public TileTypes Type { get; set; }
		public TileTypes Left { get; set; }
		public TileTypes Top { get; set; }
		public TileTypes Right { get; set; }
		public TileTypes Bottom { get; set; }

		public MapTile Flip()
		{
			return new MapTile()
			{
				Type = Type,
				Left = Right,
				Top = Bottom,
				Right = Left,
				Bottom = Top
			};
		}
	}

	public class MapRoom
	{
		public Rectangle Bounds { get; set; }
		public MapTile[,] Tiles { get; set; }
	}

	public class RoomIndex
	{
		public Zones Zone { get; set; }
		public short Room { get; set; }
	}

	public class TeleportDest : RoomIndex
	{
		public short X { get; set; }
		public short Y { get; set; }
		public Zones PreviousTileset { get; set; }
	}

	public class MapPatch
	{
		public Dictionary<Zones, Dictionary<int, RoomPatch>> RoomLocs { get; set; }
		public Dictionary<int, TeleportPatch> Teleports { get; set; }

		public static MapPatch Load(string filename) => JsonConvert.DeserializeObject<MapPatch>(File.ReadAllText(filename));

		public void Save(string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));

		public static MapPatch Create(MapInfo original, MapInfo modified)
		{
			var result = new MapPatch()
			{
				RoomLocs = new Dictionary<Zones, Dictionary<int, RoomPatch>>(),
				Teleports = new Dictionary<int, TeleportPatch>()
			};
			for (int zn = 0; zn < modified.Zones.Length; zn++)
			{
				var zmod = modified.Zones[zn];
				var zorg = Array.Find(original.Zones, a => a.ID == zmod.ID);
				for (int rn = 0; rn < zmod.Rooms.Length; rn++)
				{
					var rpatch = RoomPatch.Create(zorg.Rooms[rn], zmod.Rooms[rn]);
					if (rpatch is null)
						continue;
					if (!result.RoomLocs.ContainsKey(zmod.ID))
						result.RoomLocs.Add(zmod.ID, new Dictionary<int, RoomPatch>());
					result.RoomLocs[zmod.ID].Add(rn, rpatch);
				}
			}
			for (int tn = 0; tn < modified.Teleports.Length; tn++)
			{
				var tpatch = TeleportPatch.Create(original.Teleports[tn], modified.Teleports[tn]);
				if (tpatch != null)
					result.Teleports.Add(tn, tpatch);
			}
			return result;
		}

		public void Apply(MapInfo mapInfo)
		{
			foreach (var zpatch in RoomLocs)
			{
				var zone = Array.Find(mapInfo.Zones, a => a.ID == zpatch.Key);
				foreach (var rpatch in zpatch.Value)
					rpatch.Value.Apply(zone.Rooms[rpatch.Key]);
			}
			foreach (var tpatch in Teleports)
				tpatch.Value.Apply(mapInfo.Teleports[tpatch.Key]);
		}
	}

	public class RoomPatch
	{
		public int? X { get; set; }
		public int? Y { get; set; }
		public TilePatch[] Tiles { get; set; }

		public static RoomPatch Create(ZoneRoom original, ZoneRoom modified)
		{
			var result = new RoomPatch();
			if (modified.X != original.X || modified.Y != original.Y)
			{
				result.X = modified.X;
				result.Y = modified.Y;
			}
			List<TilePatch> tiles = new List<TilePatch>();
			for (int y = 0; y < modified.Height; y++)
				for (int x = 0; x < modified.Width; x++)
				{
					var tp = TilePatch.Create(original.Tiles[y][x], modified.Tiles[y][x], x, y);
					if (tp != null)
						tiles.Add(tp);
				}
			if (!result.X.HasValue && tiles.Count == 0)
				return null;
			result.Tiles = tiles.ToArray();
			return result;
		}

		public void Apply(ZoneRoom room)
		{
			if (X.HasValue)
				room.X = X.Value;
			if (Y.HasValue)
				room.Y = Y.Value;
			foreach (var tpatch in Tiles)
				tpatch.Apply(ref room.Tiles[tpatch.Y][tpatch.X]);
		}
	}

	public class TilePatch
	{
		public int X { get; set; }
		public int Y { get; set; }
		public TileTypes? Type { get; set; }
		public TileTypes? Left { get; set; }
		public TileTypes? Top { get; set; }
		public TileTypes? Right { get; set; }
		public TileTypes? Bottom { get; set; }

		public static TilePatch Create(MapTile original, MapTile modified, int x, int y)
		{
			var result = new TilePatch
			{
				X = x,
				Y = y
			};
			if (modified is null)
			{
				if (original is null)
					return null;
				return result;
			}
			if (original is null)
			{
				result.Type = modified.Type;
				result.Left = modified.Left;
				result.Top = modified.Top;
				result.Right = modified.Right;
				result.Bottom = modified.Bottom;
				return result;
			}
			bool changed = false;
			if (modified.Type != original.Type)
			{
				changed = true;
				result.Type = modified.Type;
			}
			if (modified.Left != original.Left)
			{
				changed = true;
				result.Left = modified.Left;
			}
			if (modified.Top != original.Top)
			{
				changed = true;
				result.Top = modified.Top;
			}
			if (modified.Right != original.Right)
			{
				changed = true;
				result.Right = modified.Right;
			}
			if (modified.Bottom != original.Bottom)
			{
				changed = true;
				result.Bottom = modified.Bottom;
			}
			if (changed)
				return result;
			return null;
		}

		public void Apply(ref MapTile mapTile)
		{
			if (!Type.HasValue)
			{
				mapTile = null;
				return;
			}
			if (mapTile == null)
				mapTile = new MapTile();
			if (Type.HasValue)
				mapTile.Type = Type.Value;
			if (Left.HasValue)
				mapTile.Left = Left.Value;
			if (Top.HasValue)
				mapTile.Top = Top.Value;
			if (Right.HasValue)
				mapTile.Right = Right.Value;
			if (Bottom.HasValue)
				mapTile.Bottom = Bottom.Value;
		}
	}

	public class TeleportPatch
	{
		public Zones? Zone { get; set; }
		public short? Room { get; set; }
		public short? X { get; set; }
		public short? Y { get; set; }
		public Zones? PreviousTileset { get; set; }

		public static TeleportPatch Create(TeleportDest original, TeleportDest modified)
		{
			var result = new TeleportPatch();
			bool changed = false;
			if (modified.Zone != original.Zone)
			{
				changed = true;
				result.Zone = modified.Zone;
			}
			if (modified.Room != original.Room)
			{
				changed = true;
				result.Room = modified.Room;
			}
			if (modified.X != original.X || modified.Y != original.Y)
			{
				changed = true;
				result.X = modified.X;
				result.Y = modified.Y;
			}
			if (modified.PreviousTileset != original.PreviousTileset)
			{
				changed = true;
				result.PreviousTileset = modified.PreviousTileset;
			}
			if (changed)
				return result;
			return null;
		}

		public void Apply(TeleportDest tele)
		{
			if (Zone.HasValue)
				tele.Zone = Zone.Value;
			if (Room.HasValue)
				tele.Room = Room.Value;
			if (X.HasValue)
				tele.X = X.Value;
			if (Y.HasValue)
				tele.Y = Y.Value;
			if (PreviousTileset.HasValue)
				tele.PreviousTileset = PreviousTileset.Value;
		}
	}

	public class Door
	{
		public Point Location { get; }
		public Sides Side { get; }

		public Door(Point loc, Sides dir)
		{
			Location = loc;
			Side = dir;
		}

		public Door(int x, int y, Sides dir) : this(new Point(x, y), dir) { }
	}

	public class Layer
	{
		public Rectangle Bounds { get; set; }
		public Bitmap Image { get; set; }
		static Dictionary<int, TileDef[]> tileDefCache = new Dictionary<int, TileDef[]>();

		public Layer(Stream stream, int fileBase, byte[][][] stgtiles, byte[][][] stgpal)
		{
			BinaryReader br = new BinaryReader(stream);
			uint layoutoff = br.ReadUInt32();
			if (layoutoff == 0)
				return;
			int defsoff = br.ReadPointer(fileBase);
			uint loadflags = br.ReadUInt32();
			Bounds = Rectangle.FromLTRB((int)(loadflags & 0x3F), (int)((loadflags >> 6) & 0x3F), (int)((loadflags >> 12) & 0x3F) + 1, (int)((loadflags >> 18) & 0x3F) + 1);
			loadflags >>= 24;
			ushort zindex = br.ReadUInt16();
			ushort drawflags = br.ReadUInt16();
			ushort[,] layout = new ushort[Bounds.Width * 16, Bounds.Height * 16];
			stream.Seek((int)(layoutoff - 0x80180000) + fileBase, SeekOrigin.Begin);
			for (int y = 0; y < Bounds.Height * 16; y++)
				for (int x = 0; x < Bounds.Width * 16; x++)
					layout[x, y] = br.ReadUInt16();
			if (!tileDefCache.TryGetValue(defsoff, out TileDef[] tileDefs))
			{
				stream.Seek(defsoff, SeekOrigin.Begin);
				tileDefs = TileDef.Load(stream, fileBase);
			}
			Image = new Bitmap(Bounds.Width * 256, Bounds.Height * 256);
			var bmpd = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var pix = new byte[bmpd.Stride * bmpd.Height];
			for (int y = 0; y < layout.GetLength(1); y++)
				for (int x = 0; x < layout.GetLength(0); x++)
				{
					var td = tileDefs[layout[x, y]];
					var ti = td.Tile;
					if ((loadflags & 0x20) == 0x20)
						ti &= 0xE7;
					byte[] tile;
					if (td.Tileset >= 0x10)
						tile = MainForm.fgametiles[td.Tileset - 0x10][ti];
					else
						tile = stgtiles[td.Tileset][ti];
					byte[][] pal;
					if ((drawflags & 0x200) == 0x200)
						pal = MainForm.fgamepalettes[td.Palette];
					else
						pal = stgpal[td.Palette];
					int to = 0;
					for (int ty = 0; ty < 16; ty++)
						for (int tx = 0; tx < 16; tx++)
						{
							byte v = tile[to++];
							if (v != 0)
								pal[v].CopyTo(pix, (((y * 16) + ty) * bmpd.Stride) + ((x * 16) + tx) * 4);
						}
				}
			System.Runtime.InteropServices.Marshal.Copy(pix, 0, bmpd.Scan0, pix.Length);
			Image.UnlockBits(bmpd);
		}
	}

	public struct TileDef
	{
		public byte Tileset;
		public byte Tile;
		public byte Palette;

		public TileDef(byte tileset, byte tile, byte palette)
		{
			Tileset = tileset;
			Tile = tile;
			Palette = palette;
		}

		public static TileDef[] Load(Stream stream, int fileBase)
		{
			BinaryReader br = new BinaryReader(stream);
			var tsoff = br.ReadPointer(fileBase);
			var tioff = br.ReadPointer(fileBase);
			var paoff = br.ReadPointer(fileBase);
			stream.Seek(tsoff, SeekOrigin.Begin);
			var tsarr = br.ReadBytes(4096);
			stream.Seek(tioff, SeekOrigin.Begin);
			var tiarr = br.ReadBytes(4096);
			stream.Seek(paoff, SeekOrigin.Begin);
			var paarr = br.ReadBytes(4096);
			var result = new TileDef[4096];
			for (int i = 0; i < 4096; i++)
				result[i] = new TileDef(tsarr[i], tiarr[i], paarr[i]);
			return result;
		}
	}

	public enum Zones
	{
		MarbleGallery,
		OuterWall,
		LongLibrary,
		Catacombs,
		OlroxsQuarters,
		AbandonedMine,
		RoyalChapel,
		CastleEntrance,
		CastleCenter,
		UndergroundCaverns,
		Colosseum,
		CastleKeep,
		AlchemyLaboratory,
		ClockTower,
		WarpRooms,
		Nightmare = 0x12,
		Cerberus = 0x16,
		Richter = 0x18,
		Hippogryph,
		Doppleganger10,
		Scylla,
		Werewolf_Minotaur,
		Legion,
		Olrox,
		FinalStageBloodlines,
		BlackMarbleGallery,
		ReverseOuterWall,
		ForbiddenLibrary,
		FloatingCatacombs,
		DeathWingsLair,
		Cave,
		AntiChapel,
		ReverseEntrance,
		ReverseCastleCenter,
		ReverseCaverns,
		ReverseColosseum,
		ReverseKeep,
		NecromancyLaboratory,
		ReverseClockTower,
		ReverseWarpRooms,
		Galamoth = 0x36,
		AkmodanII,
		Dracula,
		Doppleganger40,
		Creature,
		Medusa,
		Death,
		Beezlebub,
		Trio,
		CastleEntrance1st = 0x41
	}

	public enum Maps { Prologue, NormalCastle, NormalBoss, ReverseCastle, ReverseBoss }

	public enum TileTypes { Empty, Normal, Hidden, Secret, Save, Teleport, Room }

	public enum Sides { Left, Top, Right, Bottom }

	public static class Util
	{
		public static int AdjustOffset(int offset) => offset - (offset / 2352 * 304) - 0x18;

		public static int ReadPointer(this BinaryReader br, int fileBase)
		{
			return (int)(br.ReadUInt32() - 0x80180000) + fileBase;
		}
	}
}
