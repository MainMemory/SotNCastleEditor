using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SotNData;

namespace SotNCastleRandomizer
{
	static class Program
	{
		static void Main(string[] args)
		{
			var mapInfo = CastleMap.Load(@"d:\documents\github\sotncastleeditor\SotNCastleEditor\zones.json");
			var rand = new Random();
			foreach (var zone in mapInfo.Zones.Where(a => a.Map == Maps.NormalCastle && a.ID != Zones.CastleEntrance1st))
			{
				var roomslist = new List<RoomDoors>(zone.Rooms.Length);
				var teleports = new List<(ZoneRoom teleport, ZoneRoom connect, Point offset)>();
				foreach (ZoneRoom room in zone.Rooms)
				{
					room.GenerateRegionInfo();
					if (room.Tiles != null)
						roomslist.Add(new RoomDoors(room, room.Doors.ToList()));
					else if (room.Teleport.HasValue)
					{
						ZoneRoom connect = null;
						Point offset = Point.Empty;
						foreach (var r2 in roomslist.Where(a => a.room.Bounds.Contains(room.X - 1, room.Y)))
						{
							connect = r2.room;
							offset = new Point(r2.room.Width, room.Y - r2.room.Y);
							var d2 = r2.doors.Find(a => a.Location.X == r2.room.Width - 1 && a.Location.Y == room.Y - r2.room.Y && a.Side == Sides.Right);
							if (d2 != null)
							{
								r2.doors.Remove(d2);
								goto addtele;
							}
						}
						foreach (var r2 in roomslist.Where(a => a.room.Bounds.Contains(room.X, room.Y - 1)))
						{
							connect = r2.room;
							offset = new Point(room.X - r2.room.X, r2.room.Height);
							var d2 = r2.doors.Find(a => a.Location.X == room.X - r2.room.X && a.Location.Y == r2.room.Height - 1 && a.Side == Sides.Bottom);
							if (d2 != null)
							{
								r2.doors.Remove(d2);
								goto addtele;
							}
						}
						foreach (var r2 in roomslist.Where(a => a.room.Bounds.Contains(room.X + 1, room.Y)))
						{
							connect = r2.room;
							offset = new Point(-1, room.Y - r2.room.Y);
							var d2 = r2.doors.Find(a => a.Location.X == 0 && a.Location.Y == room.Y - r2.room.Y && a.Side == Sides.Left);
							if (d2 != null)
							{
								r2.doors.Remove(d2);
								goto addtele;
							}
						}
						foreach (var r2 in roomslist.Where(a => a.room.Bounds.Contains(room.X, room.Y + 1)))
						{
							connect = r2.room;
							offset = new Point(room.X - r2.room.X, -1);
							var d2 = r2.doors.Find(a => a.Location.X == room.X - r2.room.X && a.Location.Y == 0 && a.Side == Sides.Top);
							if (d2 != null)
							{
								r2.doors.Remove(d2);
								goto addtele;
							}
						}
						addtele:
						teleports.Add((room, connect, offset));
					}
				}
				retryzone:
				var roomqueue = new List<RoomDoors>(roomslist);
				var opendoors = new List<RoomDoors>();
				while (roomqueue.Count > 0)
				{
					var tiles = new MapTile[400, 400];
					var room = roomqueue.Pop(rand.Next(roomqueue.Count));
					room.room.Location = Point.Empty;
					for (int y = 0; y < room.room.Height; y++)
						for (int x = 0; x < room.room.Width; x++)
							tiles[x + 200, y + 200] = room.room.Tiles[y][x];
					var usedrooms = new List<ZoneRoom>() { room.room };
					room = new RoomDoors(room.room, new List<Door>(room.doors));
					var door = room.doors.Pop(rand.Next(room.doors.Count));
					if (room.doors.Count != 0)
						opendoors.Add(room);
					do
					{
						var roompool = roomqueue.SelectMany(a => a.doors.Where(c => c.Side == (door.Side ^ Sides.Right)).Select(b => (room: a, door: b))).ToList();
						while (true)
						{
							if (roompool.Count == 0)
							{
								Console.WriteLine("Generation failed in {0}! Rooms left: {1}", zone.Name, roomqueue.Count);
								goto retryzone;
							}
							var (r2, d2) = roompool.Pop(rand.Next(roompool.Count));
							r2.room.X = room.room.X + door.Location.X - d2.Location.X;
							r2.room.Y = room.room.Y + door.Location.Y - d2.Location.Y;
							switch (d2.Side)
							{
								case Sides.Left:
									++r2.room.X;
									break;
								case Sides.Top:
									++r2.room.Y;
									break;
								case Sides.Right:
									--r2.room.X;
									break;
								case Sides.Bottom:
									--r2.room.Y;
									break;
							}
							bool overlap = false;
							for (int y = 0; y < r2.room.Height; y++)
							{
								for (int x = 0; x < r2.room.Width; x++)
									if (tiles[r2.room.X + x + 200, r2.room.Y + y + 200] != null && r2.room.Tiles[y][x] != null)
									{
										overlap = true;
										break;
									}
								if (overlap)
									break;
							}
							if (overlap)
								continue;
							var dlist = new List<Door>(r2.doors);
							dlist.Remove(d2);
							foreach (var d3 in r2.doors)
							{
								if (d3 == d2)
									continue;
								var x = r2.room.X + d3.Location.X + 200;
								var y = r2.room.Y + d3.Location.Y + 200;
								switch (d3.Side)
								{
									case Sides.Left:
										--x;
										break;
									case Sides.Top:
										--y;
										break;
									case Sides.Right:
										++x;
										break;
									case Sides.Bottom:
										++y;
										break;
								}
								if (tiles[x, y] != null)
								{
									switch (d3.Side)
									{
										case Sides.Left:
											if (tiles[x, y].Right == TileTypes.Empty)
												overlap = true;
											break;
										case Sides.Top:
											if (tiles[x, y].Bottom == TileTypes.Empty)
												overlap = true;
											break;
										case Sides.Right:
											if (tiles[x, y].Left == TileTypes.Empty)
												overlap = true;
											break;
										case Sides.Bottom:
											if (tiles[x, y].Top == TileTypes.Empty)
												overlap = true;
											break;
									}
									if (overlap)
										break;
									foreach (var r3 in opendoors.Where(a => a.room.Bounds.Contains(x - 200, y - 200)))
										r3.doors.RemoveAll(a => a.Side == (d3.Side ^ Sides.Right) && a.Location.X == x - 200 - r3.room.X && a.Location.Y == y - 200 - r3.room.Y);
									opendoors.RemoveAll(a => a.doors.Count == 0);
								}
							}
							if (overlap)
								continue;
							usedrooms.Add(r2.room);
							roomqueue.Remove(r2);
							for (int y = 0; y < r2.room.Height; y++)
								for (int x = 0; x < r2.room.Width; x++)
									tiles[r2.room.X + x + 200, r2.room.Y + y + 200] = r2.room.Tiles[y][x];
							if (dlist.Count > 0)
							{
								room = new RoomDoors(r2.room, dlist);
								door = dlist.Pop(rand.Next(dlist.Count));
								if (dlist.Count > 0)
									opendoors.Add(room);
							}
							else if (opendoors.Count > 0)
							{
								room = opendoors[rand.Next(opendoors.Count)];
								door = room.doors.Pop(rand.Next(room.doors.Count));
								if (room.doors.Count == 0)
									opendoors.Remove(room);
							}
							else
								room = null;
							break;
						}
					} while (room != null);
				}
				Console.WriteLine("Successfully randomized {0}!", zone.Name);
			}
			Console.WriteLine("Done!");
			Console.ReadKey(true);
		}

		static T Pop<T>(this List<T> list, int i)
		{
			var item = list[i];
			list.RemoveAt(i);
			return item;
		}
	}

	class RoomDoors
	{
		public ZoneRoom room;
		public List<Door> doors;

		public RoomDoors(ZoneRoom room, List<Door> doors)
		{
			this.room = room;
			this.doors = doors;
		}
	}
}
