using SotNData;
using System;
using System.IO;
using System.Linq;

namespace SotNCastlePatcher
{
	class Program
	{
		static void Main(string[] args)
		{
			string layoutfilename;
			if (args.Length > 0)
				layoutfilename = args[0];
			else
			{
				Console.Write("Layout: ");
				layoutfilename = Console.ReadLine();
			}
			string romfilename;
			if (args.Length > 1)
				romfilename = args[1];
			else
			{
				Console.Write("ROM: ");
				romfilename = Console.ReadLine();
			}
			if (Directory.Exists(layoutfilename))
			{
				Random rand = new Random();
				var files = Directory.EnumerateFiles(layoutfilename, "*.castle").Concat(Directory.EnumerateFiles(layoutfilename, "*.caspat")).ToArray();
				layoutfilename = files[rand.Next(files.Length)];
			}
			CastleMap map;
			if (Path.GetExtension(layoutfilename).Equals(".caspat", StringComparison.OrdinalIgnoreCase))
			{
				map = CastleMap.Load("zones.json");
				MapPatch.Load(layoutfilename).Apply(map);
			}
			else
				map = CastleMap.Load(layoutfilename);
			using (var fs = File.Open(romfilename, FileMode.Open))
			using (var ds = new DiscImage2352Stream(fs))
			{
				foreach (var stg in Util.StageFiles)
					stg.ReadOffsets(ds);
				map.PatchROM(ds);
			}
		}
	}
}
