using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace ZBlade
{
#if ZUNE
    public class MenuSongLibraryItem : MenuItemTree
    {
        public MenuSongLibraryItem(String name, MenuItemTree parent)
            : base(name, parent)
        {
            MediaLibrary lib = new MediaLibrary();

            MenuTextItem stop = new MenuTextItem("Stop Music");
            stop.Activated += new EventHandler(stop_onSelected);

			Nodes.Add(stop);

            MenuSongCollectionItem playAll = new MenuSongCollectionItem("Play All", lib.Songs);
			playAll.Activated += new EventHandler(song_onSelected);
			Nodes.Add(playAll);

            foreach (var artist in lib.Artists)
            {
				MenuItemTree artistTree = new MenuItemTree(artist.Name, this);

				playAll = new MenuSongCollectionItem("Play All", artist.Songs);
				playAll.Activated += new EventHandler(song_onSelected);

				artistTree.Nodes.Add(playAll);

				bool allProtected = true;

                foreach (var song in artist.Songs)
                {
					MenuSongItem songItem = new MenuSongItem(song);

					if (!song.IsProtected)
						allProtected = false;

					songItem.Activated += new EventHandler(song_onSelected);
					artistTree.Nodes.Add(songItem);
                }

				if (allProtected)
				{
					playAll.IsEnabled = false;
					artistTree.IsEnabled = false;
				}

                Nodes.Add(artistTree);
            }
        }

        void stop_onSelected(object sender, EventArgs unused)
        {
            MediaPlayer.Stop();
        }

		void song_onSelected(object sender, EventArgs unused)
        {
            MediaPlayer.IsShuffled = true;
            if (sender is MenuSongItem)
                MediaPlayer.Play((sender as MenuSongItem).ChoiceSong);
            else
                MediaPlayer.Play((sender as MenuSongCollectionItem).ChoiceSongs);
        }
    }

    public class MenuSongItem : MenuTextItem
    {
        public Song ChoiceSong { get; set; }

        public MenuSongItem(Song song)
            : base(song.Name)
        {
            ChoiceSong = song;

			//disable DRM songs
			if (song.IsProtected)
				IsEnabled = false;
        }
    }

    public class MenuSongCollectionItem : MenuTextItem
    {
        public SongCollection ChoiceSongs { get; set; }

        public MenuSongCollectionItem(String name, SongCollection songs)
            : base(name)
        {
            ChoiceSongs = songs;
        }
    }

    public enum MediaType
    {
        Song,
        Picture,
        Video,
    }
#endif
}