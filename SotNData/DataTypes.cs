using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace SotNData
{
	public class CastleMap
	{
		public CastleZone[] Zones { get; set; }
		public TeleportDest[] Teleports { set => TeleportDests = value; }
		public TeleportDest[] TeleportDests { get; set; }
		public BossTeleport[] BossTeleports { get; set; }
		public RoomIndex[][] MatchingRooms { get; set; }

		public static CastleMap Load(string filename) => JsonConvert.DeserializeObject<CastleMap>(File.ReadAllText(filename));

		public void Save(string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));

		public void PatchROM(Stream stream)
		{
			foreach (var zone in Zones)
				zone.PatchROM(stream);
			stream.Position = Util.TeleportDestTableOffset;
			foreach (var tele in TeleportDests)
				tele.PatchROM(stream);
			stream.Position = Util.BossTeleportTableOffset;
			foreach (var tele in BossTeleports)
				tele.PatchROM(stream, Zones);
			var bw = new BinaryWriter(stream);
			foreach (var room in Zones.SelectMany(a => a.Rooms).Where(b => b.WarpID.HasValue))
			{
				stream.Position = Util.WarpRoomTableOffset + (room.WarpID.Value * 4);
				bw.Write((short)room.X);
				bw.Write((short)room.Y);
			}
			stream.Position = Util.MapGraphicsOffset;
			bw.Write(Util.GenerateMap(this).GetPixels4bppPSX());
		}
	}

	public class CastleZone
	{
		public string Name { get; set; }
		public Zones ID { get; set; }
		public Maps Map { get; set; }
		public ZoneRoom[] Rooms { get; set; }

		public CastleZone() { }

		public void PatchROM(Stream stream)
		{
			var br = new BinaryReader(stream);
			var bw = new BinaryWriter(stream);
			var zinf = Util.StageFiles.Single(a => a.ID == ID);
			stream.Seek(zinf.RoomListOffset, SeekOrigin.Begin);
			for (int i = 0; i < Rooms.Length; i++)
			{
				ZoneRoom room = Rooms[i];
				var orgloc = Rectangle.FromLTRB(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
				stream.Seek(-4, SeekOrigin.Current);
				int x = room.X + room.LayoutOffsetX;
				int y = room.Y + room.LayoutOffsetY;
				bw.Write((byte)x);
				bw.Write((byte)y);
				bw.Write((byte)(x + orgloc.Width));
				bw.Write((byte)(y + orgloc.Height));
				byte layind = br.ReadByte();
				if (br.ReadByte() != 0xFF)
				{
					var savepos = stream.Position;
					var (fgoff, bgoff) = zinf.LayerOffsets[layind];
					if (fgoff != 0)
					{
						stream.Seek(fgoff + 8, SeekOrigin.Begin);
						uint loadflags = br.ReadUInt32();
						var layloc = Rectangle.FromLTRB((int)(loadflags & 0x3F), (int)((loadflags >> 6) & 0x3F), (int)((loadflags >> 12) & 0x3F), (int)((loadflags >> 18) & 0x3F));
						if (!layloc.Location.IsEmpty)
						{
							loadflags &= 0xFF000000;
							loadflags |= (uint)(x + (orgloc.X - layloc.X) & 0x3F);
							loadflags |= (uint)((y + (orgloc.Y - layloc.Y) & 0x3F) << 6);
							loadflags |= (uint)((x + (orgloc.X - layloc.X) + layloc.Width & 0x3F) << 12);
							loadflags |= (uint)((y + (orgloc.Y - layloc.Y) + layloc.Height & 0x3F) << 18);
							stream.Seek(-4, SeekOrigin.Current);
							bw.Write(loadflags);
						}
					}
					if (bgoff != 0)
					{
						stream.Seek(bgoff + 8, SeekOrigin.Begin);
						uint loadflags = br.ReadUInt32();
						var layloc = Rectangle.FromLTRB((int)(loadflags & 0x3F), (int)((loadflags >> 6) & 0x3F), (int)((loadflags >> 12) & 0x3F), (int)((loadflags >> 18) & 0x3F));
						if (!layloc.Location.IsEmpty)
						{
							loadflags &= 0xFF000000;
							loadflags |= (uint)(x + (orgloc.X - layloc.X) & 0x3F);
							loadflags |= (uint)((y + (orgloc.Y - layloc.Y) & 0x3F) << 6);
							loadflags |= (uint)((x + (orgloc.X - layloc.X) + layloc.Width & 0x3F) << 12);
							loadflags |= (uint)((y + (orgloc.Y - layloc.Y) + layloc.Height & 0x3F) << 18);
							stream.Seek(-4, SeekOrigin.Current);
							bw.Write(loadflags);
						}
					}
					stream.Seek(savepos, SeekOrigin.Begin);
				}
				stream.Seek(2, SeekOrigin.Current);
			}
		}
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
		public int? WarpID { get; set; }
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

		public void PatchROM(Stream stream)
		{
			var bw = new BinaryWriter(stream);
			bw.Write(X);
			bw.Write(Y);
			bw.Write((short)(Room * 8));
			bw.Write((short)PreviousTileset);
			bw.Write((short)Zone);
		}
	}

	public class BossTeleport : RoomIndex
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int BossID { get; set; }
		public int DestID { get; set; }

		public void PatchROM(Stream stream, CastleZone[] zones)
		{
			var bw = new BinaryWriter(stream);
			var x = X;
			var y = Y;
			var z = zones.FirstOrDefault(a => a.ID == Zone);
			if (z != null && Room < z.Rooms.Length)
			{
				var r = z.Rooms[Room];
				x += r.X + r.LayoutOffsetX;
				y += r.Y + r.LayoutOffsetY;
			}
			bw.Write(x);
			bw.Write(y);
			bw.Write((int)Zone);
			bw.Write(BossID);
			bw.Write(DestID);
		}
	}

	public class MapPatch
	{
		public Dictionary<Zones, Dictionary<int, RoomPatch>> RoomLocs { get; set; }
		public Dictionary<int, TeleportDestPatch> Teleports { set => TeleportDests = value; }
		public Dictionary<int, TeleportDestPatch> TeleportDests { get; set; }
		public Dictionary<int, BossTeleportPatch> BossTeleports { get; set; }

		public static MapPatch Load(string filename)
		{
			var result = JsonConvert.DeserializeObject<MapPatch>(File.ReadAllText(filename));
			if (result.RoomLocs == null)
				result.RoomLocs = new Dictionary<Zones, Dictionary<int, RoomPatch>>();
			if (result.TeleportDests == null)
				result.TeleportDests = new Dictionary<int, TeleportDestPatch>();
			if (result.BossTeleports == null)
				result.BossTeleports = new Dictionary<int, BossTeleportPatch>();
			return result;
		}

		public void Save(string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));

		public static MapPatch Create(CastleMap original, CastleMap modified)
		{
			var result = new MapPatch()
			{
				RoomLocs = new Dictionary<Zones, Dictionary<int, RoomPatch>>(),
				TeleportDests = new Dictionary<int, TeleportDestPatch>()
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
			for (int tn = 0; tn < modified.TeleportDests.Length; tn++)
			{
				var tpatch = TeleportDestPatch.Create(original.TeleportDests[tn], modified.TeleportDests[tn]);
				if (tpatch != null)
					result.TeleportDests.Add(tn, tpatch);
			}
			for (int tn = 0; tn < modified.BossTeleports.Length; tn++)
			{
				var tpatch = BossTeleportPatch.Create(original.BossTeleports[tn], modified.BossTeleports[tn]);
				if (tpatch != null)
					result.BossTeleports.Add(tn, tpatch);
			}
			return result;
		}

		public void Apply(CastleMap mapInfo)
		{
			foreach (var zpatch in RoomLocs)
			{
				var zone = Array.Find(mapInfo.Zones, a => a.ID == zpatch.Key);
				foreach (var rpatch in zpatch.Value)
					rpatch.Value.Apply(zone.Rooms[rpatch.Key]);
			}
			foreach (var tpatch in TeleportDests)
				tpatch.Value.Apply(mapInfo.TeleportDests[tpatch.Key]);
			foreach (var tpatch in BossTeleports)
				tpatch.Value.Apply(mapInfo.BossTeleports[tpatch.Key]);
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
			if (modified.Tiles != null)
			{
				for (int y = 0; y < modified.Height; y++)
					for (int x = 0; x < modified.Width; x++)
					{
						var tp = TilePatch.Create(original.Tiles[y][x], modified.Tiles[y][x], x, y);
						if (tp != null)
							tiles.Add(tp);
					}
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

	public class TeleportDestPatch
	{
		public Zones? Zone { get; set; }
		public short? Room { get; set; }
		public short? X { get; set; }
		public short? Y { get; set; }
		public Zones? PreviousTileset { get; set; }

		public static TeleportDestPatch Create(TeleportDest original, TeleportDest modified)
		{
			var result = new TeleportDestPatch();
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

	public class BossTeleportPatch
	{
		public Zones? Zone { get; set; }
		public short? Room { get; set; }
		public int? X { get; set; }
		public int? Y { get; set; }
		public int? BossID { get; set; }
		public int? DestID { get; set; }

		public static BossTeleportPatch Create(BossTeleport original, BossTeleport modified)
		{
			var result = new BossTeleportPatch();
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
			if (modified.BossID != original.BossID)
			{
				changed = true;
				result.BossID = modified.BossID;
			}
			if (modified.DestID != original.DestID)
			{
				changed = true;
				result.DestID = modified.DestID;
			}
			if (changed)
				return result;
			return null;
		}

		public void Apply(BossTeleport tele)
		{
			if (Zone.HasValue)
				tele.Zone = Zone.Value;
			if (Room.HasValue)
				tele.Room = Room.Value;
			if (X.HasValue)
				tele.X = X.Value;
			if (Y.HasValue)
				tele.Y = Y.Value;
			if (BossID.HasValue)
				tele.BossID = BossID.Value;
			if (DestID.HasValue)
				tele.DestID = DestID.Value;
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

	public class ZoneInfo
	{
		public string Name { get; }
		public Zones ID { get; }
		public Maps Map { get; }
		public int DataOffset { get; }
		public int TileOffset { get; }
		public int RoomListOffset { get; private set; }
		public System.Collections.ObjectModel.ReadOnlyCollection<(int fg, int bg)> LayerOffsets { get; private set; }
		private byte[][][] stgtiles;
		private byte[][][] stgpal;

		public ZoneInfo(string name, Zones zone, Maps map, int offset, int tileOff)
		{
			Name = name;
			ID = zone;
			Map = map;
			DataOffset = Util.AdjustOffset(offset);
			TileOffset = Util.AdjustOffset(tileOff);
		}

		public void ReadOffsets(Stream stream)
		{
			BinaryReader br = new BinaryReader(stream);
			stream.Seek(DataOffset + 0x10, SeekOrigin.Begin);
			RoomListOffset = br.ReadPointer(DataOffset);
			stream.Seek(RoomListOffset, SeekOrigin.Begin);
			byte layoutCount = 0;
			byte l = br.ReadByte();
			while (l != 0x40)
			{
				stream.Seek(3, SeekOrigin.Current);
				byte tele = br.ReadByte();
				if (br.ReadByte() != 0xFF)
					layoutCount = Math.Max(tele, layoutCount);
				stream.Seek(2, SeekOrigin.Current);
				l = br.ReadByte();
			}
			++layoutCount;
			stream.Seek(DataOffset + 0x20, SeekOrigin.Begin);
			int ptroff = br.ReadPointer(DataOffset);
			var layouts = new List<(int fg, int bg)>();
			for (int i = 0; i < layoutCount; i++)
			{
				stream.Seek(ptroff, SeekOrigin.Begin);
				uint fgptr = br.ReadUInt32();
				uint bgptr = br.ReadUInt32();
				if (fgptr > 0x80180000 && fgptr < 0xA0000000)
					fgptr = (uint)(fgptr - 0x80180000 + DataOffset);
				if (bgptr > 0x80180000 && bgptr < 0xA0000000)
					bgptr = (uint)(bgptr - 0x80180000 + DataOffset);
				layouts.Add(((int)fgptr, (int)bgptr));
				ptroff += 8;
			}
			LayerOffsets = new System.Collections.ObjectModel.ReadOnlyCollection<(int fg, int bg)>(layouts);
		}

		public CastleZone LoadData(Stream fileStream)
		{
			return new CastleZone()
			{
				ID = ID,
				Map = Map,
				Name = Name,
				Rooms = ReadLayout(fileStream, DataOffset)
			};
		}

		private void GenerateMaps(Stream fileStream, byte[][][] fgametiles, byte[][][] fgamepal, ZoneRoom[] rooms)
		{
			(stgtiles, stgpal) = Util.LoadGraphicsFile(fileStream, TileOffset, false);
			var layers = ReadLayers(fileStream, fgametiles, fgamepal, rooms.Where(a => a.LayoutIndex.HasValue).Max(a => a.LayoutIndex.Value) + 1);
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
			for (int i = 0; i < rooms.Length; i++)
			{
				var room = rooms[i];
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

		private List<(Layer fg, Layer bg)> ReadLayers(Stream fileStream, byte[][][] fgametiles, byte[][][] fgamepal, int roomCount)
		{
			BinaryReader br = new BinaryReader(fileStream);
			fileStream.Seek(DataOffset + 0x20, SeekOrigin.Begin);
			int ptroff = br.ReadPointer(DataOffset);
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
						fileStream.Seek((int)(fgptr - 0x80180000) + DataOffset, SeekOrigin.Begin);
						fg = new Layer(fileStream, DataOffset, stgtiles, stgpal, fgametiles, fgamepal);
						layercache.Add(fgptr, fg);
					}
				if (!layercache.TryGetValue(bgptr, out Layer bg))
					if (bgptr > 0x80180000 && bgptr < 0xA0000000)
					{
						fileStream.Seek((int)(bgptr - 0x80180000) + DataOffset, SeekOrigin.Begin);
						bg = new Layer(fileStream, DataOffset, stgtiles, stgpal, fgametiles, fgamepal);
						layercache.Add(bgptr, bg);
					}
				layouts.Add((fg, bg));
				ptroff += 8;
			}
			return layouts;
		}
	}

	public class Layer
	{
		public Rectangle Bounds { get; set; }
		public Bitmap Image { get; set; }
		static Dictionary<int, TileDef[]> tileDefCache = new Dictionary<int, TileDef[]>();

		public Layer(Stream stream, int fileBase, byte[][][] stgtiles, byte[][][] stgpal, byte[][][] fgametiles, byte[][][] fgamepal)
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
						tile = fgametiles[td.Tileset - 0x10][ti];
					else
						tile = stgtiles[td.Tileset][ti];
					byte[][] pal;
					if ((drawflags & 0x200) == 0x200)
						pal = fgamepal[td.Palette];
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

		public static int MapGraphicsOffset => AdjustOffset(0x1EF8E8);
		public static int TeleportDestTableOffset => AdjustOffset(0xAE444);
		public static int BossTeleportTableOffset => AdjustOffset(0xAEA94);
		public static int WarpRoomTableOffset => AdjustOffset(0x5883A64);

		static readonly ZoneInfo[] stageFiles =
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

		public static System.Collections.ObjectModel.ReadOnlyCollection<ZoneInfo> StageFiles { get; } = new System.Collections.ObjectModel.ReadOnlyCollection<ZoneInfo>(stageFiles);

		static readonly Zones[][] matchingZones =
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

		static readonly Brush areaBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 255));

		public static void LoadDataFromBin()
		{
			var mapInfo = new CastleMap();
			using (var fs = File.OpenRead(@"D:\CD\PS1\Castlevania - Symphony of the Night (Track 1).bin"))
			using (var ms = new DiscImage2352Stream(fs))
			{
				var (fgametiles, fgamepalettes) = LoadGraphicsFile(ms, AdjustOffset(0x38294B8), true);
				var zones = new List<CastleZone>();
				foreach (var zone in stageFiles)
					zones.Add(zone.LoadData(ms));
				mapInfo.Zones = zones.ToArray();
				var bosses = new List<BossTeleport>();
				ms.Seek(BossTeleportTableOffset, SeekOrigin.Begin);
				var br = new BinaryReader(ms);
				var x = br.ReadInt32();
				while (x != 0x80)
				{
					var y = br.ReadInt32();
					var zid = (Zones)br.ReadInt32();
					var zone = zones.Find(a => a.ID == zid);
					short rid = 0;
					if (zone != null)
					{
						rid = (short)Array.FindIndex(zone.Rooms, a => a.LayoutBounds.Contains(x, y));
						if (rid != -1)
						{
							x -= zone.Rooms[rid].LayoutBounds.X;
							y -= zone.Rooms[rid].LayoutBounds.Y;
						}
					}
					bosses.Add(new BossTeleport() { X = x, Y = y, Zone = zid, Room = rid, BossID = br.ReadInt32(), DestID = br.ReadInt32() });
					x = br.ReadInt32();
				}
				mapInfo.BossTeleports = bosses.ToArray();
				mapInfo.TeleportDests = new TeleportDest[zones.SelectMany(a => a.Rooms).Select(b => b.Teleport).Concat(bosses.Select(c => (int?)c.DestID)).Max().Value + 1];
				ms.Seek(TeleportDestTableOffset, SeekOrigin.Begin);
				for (int i = 0; i < mapInfo.TeleportDests.Length; i++)
					mapInfo.TeleportDests[i] = new TeleportDest() { X = br.ReadInt16(), Y = br.ReadInt16(), Room = (short)(br.ReadInt16() / 8), PreviousTileset = (Zones)br.ReadInt16(), Zone = (Zones)br.ReadInt16() };
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
			foreach (var zone in mapInfo.Zones)
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
					var zone = mapInfo.Zones.Single(b => b.ID == zid);
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
			foreach (var zone in mapInfo.Zones)
			{
				MakeLargeMap(zone.Name.Replace(':', '-'), areaBrush, zone.Rooms.Select(a => a.LayoutBounds).ToArray());
				MakeLargeMap(zone.Name.Replace(':', '-') + " Map", areaBrush, zone.Rooms.Select(a => a.Bounds).ToArray());
			}
			var mapbmp = GenerateMap(mapInfo);
			using (var bmp = mapbmp.ToBitmap4bpp(Color.Magenta, Color.Blue, Color.DarkBlue, Color.Black, Color.Red, Color.Yellow))
				bmp.Save(@"D:\CD\PS1\SOTN\F_MAP2.BIN.png");
			File.WriteAllText("zones.json", JsonConvert.SerializeObject(mapInfo, Formatting.Indented));
		}

		public static BitmapBits GenerateMap(CastleMap info)
		{
			TileTypes[,] tiles = new TileTypes[0x40, 0x40];
			TileTypes?[,] hdoors = new TileTypes?[0x40, 0x40];
			TileTypes?[,] vdoors = new TileTypes?[0x40, 0x40];
			foreach (var room in info.Zones.Where(a => a.Map == Maps.NormalCastle).SelectMany(a => a.Rooms).Where(a => a.Tiles != null))
			{
				for (int y = 0; y < room.Bounds.Height; y++)
					for (int x = 0; x < room.Bounds.Width; x++)
						if (room.Tiles[y][x] != null)
						{
							MapTile exits = room.Tiles[y][x];
							int mx = room.X + x;
							int my = room.Y + y;
							if (exits.Type > tiles[mx, my])
								tiles[mx, my] = exits.Type;
							if (hdoors[mx, my].HasValue)
								switch (hdoors[mx, my].Value)
								{
									case TileTypes.Normal:
									case TileTypes.Hidden:
										hdoors[mx, my] = exits.Left;
										break;
								}
							else
								hdoors[mx, my] = exits.Left;
							if (vdoors[mx, my].HasValue)
								switch (vdoors[mx, my].Value)
								{
									case TileTypes.Normal:
									case TileTypes.Hidden:
										vdoors[mx, my] = exits.Top;
										break;
								}
							else
								vdoors[mx, my] = exits.Top;
							if (x < 0x3F)
								if (hdoors[mx + 1, my].HasValue)
									switch (hdoors[mx + 1, my].Value)
									{
										case TileTypes.Normal:
										case TileTypes.Hidden:
											hdoors[mx + 1, my] = exits.Right;
											break;
									}
								else
									hdoors[mx + 1, my] = exits.Right;
							if (y < 0x3F)
								if (vdoors[mx, my + 1].HasValue)
									switch (vdoors[mx, my + 1].Value)
									{
										case TileTypes.Normal:
										case TileTypes.Hidden:
											vdoors[mx, my + 1] = exits.Bottom;
											break;
									}
								else
									vdoors[mx, my + 1] = exits.Bottom;
						}
			}
			BitmapBits mapbmp = new BitmapBits(256, 256);
			for (int y = 0; y < 0x40; y++)
				for (int x = 0; x < 0x40; x++)
					if (tiles[x, y] != TileTypes.Empty)
					{
						int px = x * 4;
						int py = y * 4;
						mapbmp.FillRectangle((byte)tiles[x, y], px + 1, py + 1, 3, 3);
						if (hdoors[x, y].HasValue)
							switch (hdoors[x, y].Value)
							{
								case TileTypes.Hidden:
									mapbmp[px, py + 2] = (byte)TileTypes.Hidden;
									break;
								case TileTypes.Normal:
								case TileTypes.Save:
								case TileTypes.Teleport:
									if (x > 0 && tiles[x - 1, y] == TileTypes.Hidden)
										mapbmp[px, py + 2] = (byte)TileTypes.Hidden;
									else
										mapbmp[px, py + 2] = (byte)TileTypes.Normal;
									break;
								case TileTypes.Room:
									mapbmp.DrawLine((byte)tiles[x, y], px, py + 1, px, py + 3);
									break;
							}
						if (vdoors[x, y].HasValue)
							switch (vdoors[x, y].Value)
							{
								case TileTypes.Hidden:
									mapbmp[px + 2, py] = (byte)TileTypes.Hidden;
									break;
								case TileTypes.Normal:
								case TileTypes.Save:
								case TileTypes.Teleport:
									if (y > 0 && tiles[x, y - 1] == TileTypes.Hidden)
										mapbmp[px + 2, py] = (byte)TileTypes.Hidden;
									else
										mapbmp[px + 2, py] = (byte)TileTypes.Normal;
									break;
								case TileTypes.Room:
									mapbmp.DrawLine((byte)tiles[x, y], px + 1, py, px + 3, py);
									break;
							}
						if (hdoors[x, y] == TileTypes.Room && vdoors[x, y] == TileTypes.Room && x > 0 && y > 0 && tiles[x - 1, y - 1] != TileTypes.Empty && hdoors[x, y - 1] == TileTypes.Room && vdoors[x - 1, y] == TileTypes.Room)
							mapbmp[px, py] = (byte)tiles[x, y];
					}
			return mapbmp;
		}

		private static void MakeLargeMap(string mapfn, Brush brush, Rectangle[] rooms)
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
							gfx.DrawString(j.ToString(), new Font(FontFamily.GenericSansSerif, 12), Brushes.Black, tmp.X + 1, tmp.Y + 1);
						}
				bmp.Save($@"D:\CD\PS1\SOTN\maps\{mapfn}.png");
			}
		}
	}
}
