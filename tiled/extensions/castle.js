var projpath = FileInfo.cleanPath(FileInfo.joinPaths(FileInfo.path(FileInfo.fromNativeSeparators(__filename)), ".."));

var zoneIDs = [
	"MarbleGallery",
	"OuterWall",
	"LongLibrary",
	"Catacombs",
	"OlroxsQuarters",
	"AbandonedMine",
	"RoyalChapel",
	"CastleEntrance",
	"CastleCenter",
	"UndergroundCaverns",
	"Colosseum",
	"CastleKeep",
	"AlchemyLaboratory",
	"ClockTower",
	"WarpRooms",
	null,
	null,
	null,
	"Nightmare",
	null,
	null,
	null,
	"Cerberus",
	null,
	"Richter",
	"Hippogryph",
	"Doppleganger10",
	"Scylla",
	"Werewolf_Minotaur",
	"Legion",
	"Olrox",
	"FinalStageBloodlines",
	"BlackMarbleGallery",
	"ReverseOuterWall",
	"ForbiddenLibrary",
	"FloatingCatacombs",
	"DeathWingsLair",
	"Cave",
	"AntiChapel",
	"ReverseEntrance",
	"ReverseCastleCenter",
	"ReverseCaverns",
	"ReverseColosseum",
	"ReverseKeep",
	"NecromancyLaboratory",
	"ReverseClockTower",
	"ReverseWarpRooms",
	null,
	null,
	null,
	null,
	null,
	null,
	null,
	"Galamoth",
	"AkmodanII",
	"Dracula",
	"Doppleganger40",
	"Creature",
	"Medusa",
	"Death",
	"Beezlebub",
	"Trio",
	null,
	null,
	"CastleEntrance1st"
];

var castleMapFormat = {
    name: "SotN Castle Map",
    extension: "castle",

	//Function for reading from a castle file
	read: function(fileName) {
		var txtfile = new TextFile(fileName, TextFile.ReadOnly);
		var stginf = JSON.parse(txtfile.readAll());
		txtfile.close();

		var tilemap = new TileMap();
		tilemap.setTileSize(256, 256);
		tilemap.setSize(64, 64);

		var aluts = tiled.tilesetFormat("tsx").read(FileInfo.joinPaths(projpath, "Alucard.tsx"));
		var alutile = aluts.tile(0);
		tilemap.addTileset(aluts);

		var tileset = new Tileset("Rooms");
		tileset.objectAlignment = Tileset.TopLeft;
		tilemap.addTileset(tileset);

		var layer = new GroupLayer("Prologue");
		layer.visible = false;
		tilemap.addLayer(layer);
		layer = new GroupLayer("Normal Castle");
		tilemap.addLayer(layer);
		layer = new GroupLayer("Normal Bosses");
		layer.visible = false;
		tilemap.addLayer(layer);
		layer = new GroupLayer("Reverse Castle");
		layer.visible = false;
		tilemap.addLayer(layer);
		layer = new GroupLayer("Reverse Bosses");
		layer.visible = false;
		tilemap.addLayer(layer);

		var rooms = [];

		for (const zone of stginf.Zones) {
			layer = new ObjectGroup(zone.Name);
			layer.className = "Zone";
			layer.setProperty("ID", zone.ID);
			rooms[zone.ID] = [];
			for (var i = 0; i < zone.Rooms.length; ++i) {
				var room = zone.Rooms[i];
				var obj = new MapObject();
				if (room.Teleport != null)
					obj.className = "TeleportRoom";
				else
					obj.className = "Room";
				obj.x = room.X * 256;
				obj.y = room.Y * 256;
				var fn = FileInfo.joinPaths(projpath, "RoomImg", zoneIDs[zone.ID], i + ".png");
				if (File.exists(fn)) {
					var tile = tileset.addTile();
					tile.setImage(new Image(fn));
					obj.tile = tile;
					obj.size = tile.size;
					obj.x += room.LayoutOffsetX * 256;
					obj.y += room.LayoutOffsetY * 256;
				}
				else {
					obj.shape = MapObject.Rectangle;
					obj.width = room.Width * 256;
					obj.height = room.Height * 256;
				}
				layer.addObject(obj);
				rooms[zone.ID][i] = obj;
			}
			tilemap.layerAt(zone.Map).addLayer(layer);
		}

		layer = new ObjectGroup("Teleports");
		tilemap.addLayer(layer);

		for (const tele of stginf.Teleports) {
			var obj = new MapObject();
			obj.className = "TeleportDest";
			obj.tile = alutile;
			obj.size = alutile.size;
			obj.x = tele.X;
			obj.y = tele.Y;
			var zone = rooms[tele.Zone];
			if (zone == null || zone[tele.Room] == null)
				zone = rooms[tele.Zone ^ 0x20];
			if (zone != null) {
				var room = zone[tele.Room];
				if (room != null) {
					obj.x += room.x;
					obj.y += room.y;
					obj.setProperty("Room", room);
				}
			}
			obj.setProperty("PreviousTileset", tele.PreviousTileset);
			layer.addObject(obj);
		}

		for (const zone of stginf.Zones) {
			for (var i = 0; i < zone.Rooms.length; ++i) {
				var room = zone.Rooms[i];
				if (room.Teleport != null && room.Teleport < layer.objectCount)
					rooms[zone.ID][i].setProperty("Destination", layer.objectAt(room.Teleport));
			}
		}

		return tilemap;
	},


	write: function(map, fileName) {
		var txtfile = new TextFile(fileName, TextFile.ReadOnly);
		var stginf = JSON.parse(txtfile.readAll());
		txtfile.close();

		for (var lid = 0; lid < map.layerCount; ++lid) {
			var layer = map.layerAt(lid);
			switch (layer.name)
			{
				case "Foreground High":
					if (layer.isTileLayer && stginf.ForegroundHigh != null)
						writeForegroundLayer(stginf.ForegroundHigh, layer);
					break;
				case "Foreground Low":
					if (layer.isTileLayer && stginf.ForegroundLow != null)
						writeForegroundLayer(stginf.ForegroundLow, layer);
					break;
				case "Objects":
					if (layer.isObjectGroup) {
						var interactables = new Array();
						var items = new Array();
						var enemies = new Array();
						var rings = new Array();
						var start = null;
						for (var oid = 0; oid < layer.objectCount; oid++) {
							var obj = layer.objectAt(oid);
							switch (obj.className)
							{
								case "Interactable":
									interactables.push(obj);
									break;
								case "Item":
									items.push(obj);
									break;
								case "Enemy":
									enemies.push(obj);
									break;
								case "Ring":
									rings.push(obj);
									break;
								case "Player":
									start = obj;
									break;
							}
						}
						if (stginf.Interactables != null)
							writeInteractables(stginf.Interactables, interactables, map.width, map.height);
						if (stginf.Items != null)
							writeItems(stginf.Items, items, map.width, map.height);
						if (stginf.Enemies != null)
							writeEnemies(stginf.Enemies, enemies, map.width, map.height);
						if (stginf.Rings != null)
							writeRings(stginf.Rings, rings, map.width, map.height);
						if (stginf.PlayerStart != null) {
							var data = new Uint16Array(2);
							if (start != null) {
								data[0] = start.x;
								data[1] = start.y;
							}
							var file = new BinaryFile(FileInfo.joinPaths(projpath, FileInfo.fromNativeSeparators(stginf.PlayerStart)), BinaryFile.WriteOnly);
							file.write(data.buffer);
							file.commit();
						}
					}
					break;
			}
		}
	}

}

tiled.registerMapFormat("castle", castleMapFormat);
