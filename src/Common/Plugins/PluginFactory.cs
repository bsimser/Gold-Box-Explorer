using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoldBoxExplorer.Lib.Plugins.Character;
using GoldBoxExplorer.Lib.Plugins.Dax;
using GoldBoxExplorer.Lib.Plugins.DaxEcl;
using GoldBoxExplorer.Lib.Plugins.GameData;
using GoldBoxExplorer.Lib.Plugins.Geo;
using GoldBoxExplorer.Lib.Plugins.GeoDax;
using GoldBoxExplorer.Lib.Plugins.Glb;
using GoldBoxExplorer.Lib.Plugins.Hex;
using GoldBoxExplorer.Lib.Plugins.Image;
using GoldBoxExplorer.Lib.Plugins.Items;
using GoldBoxExplorer.Lib.Plugins.SavedGames;
using GoldBoxExplorer.Lib.Plugins.Tbl;
using GoldBoxExplorer.Lib.Plugins.Text;
using GoldBoxExplorer.Lib.Plugins.Vault;

namespace GoldBoxExplorer.Lib.Plugins
{
    public static class PluginFactory
    {
        // TODO create list via IoC (any class implementing IPlugin?)
        private static readonly IList<IPlugin> Plugins = new List<IPlugin>
            {
                new DaxEclPlugin(),
                new GeoDaxPlugin(),
                new DaxImagePlugin(),
                new DaxWallDefPlugin(),
                new FruaCharacterPlugin(),
                new FruaGameDataPlugin(),
                new FruaGeoPlugin(),
                new FruaGlbPlugin(),
                new FruaItemPlugin(),
                new FruaSavedGamePlugin(),
                new FruaTblPlugin(),
                //new FruaVaultPlugin(), // chokes on gold box vault files, disabled until I understand what's wrong with it TODO
                new ImagePlugin(),
                new TextFilePlugin(),
            };

        public static IPlugin CreateUsing(PluginParameter args)
        {
            var info = new FileInfo(args.Filename);
            if (info.Length == 0) return null;
            
            foreach (var plugin in Plugins.Where(plugin => plugin.IsSatisifedBy(args.Filename)))
            {
                return plugin.CreateUsing(args);
            }

            // default plugin if we can't recognize the others
            return new HexPlugin().CreateUsing(args);
        }
    }
}