﻿using Assets.Scripts.GameCore;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.GeneralLocalization.Modles;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.UI.Controls;
using Assets.Scripts.UI.Panels;
using HarmonyLib;
using ModHelper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Assets.Scripts.PeroTools.Nice.Datas;
using System.Reflection.Emit;
using System.IO;
using Ionic.Zip;

namespace MuseDashCustomAlbumMod
{
    public static class CustomAlbum
    {
        public static readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            { "ChineseT", "自定義谱面" },
            { "ChineseS", "自定义谱面" },
            { "English", "Custom Albums" },
            { "Korean", "Custom Albums" },
            { "Japanese", "Custom Albums" },
        };
        public static readonly int MusicPackgeUid = 999;
        public static readonly string JsonName = $"ALBUM{MusicPackgeUid + 1}";
        public static readonly string MusicPackge = $"music_package_{MusicPackgeUid}";

        public static readonly string AlbumPackPath = "Custom_Albums";
        public static readonly string AlbumPackExt = "mdm";

        public static Dictionary<string, CustomAlbumInfo> Albums = new Dictionary<string, CustomAlbumInfo>();
        public static void DoPatching()
        {
            var harmony = new Harmony("com.github.mo10.customalbum");

            StageUIPatch.DoPatching(harmony);
            DataPatch.DoPathcing(harmony);
            ExtraPatch.DoPatching(harmony);

            LoadCustomAlbums();
        }

        public static void LoadCustomAlbums()
        {

            if (!Directory.Exists(AlbumPackPath))
            {
                // Create custom album path
                Directory.CreateDirectory(AlbumPackPath);
            }

            foreach (var file in Directory.GetFiles(AlbumPackPath, $"*.{AlbumPackExt}"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                var albumInfo = CustomAlbumInfo.LoadFromFile(file);
                if (albumInfo != null)
                {
                    ModLogger.Debug($"Loaded {albumInfo}");
                    Albums.Add(fileName, albumInfo);
                }
            }

        }
    }
}