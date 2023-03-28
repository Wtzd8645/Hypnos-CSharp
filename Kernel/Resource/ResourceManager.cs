using System;
using System.Collections.Generic;
using System.IO;

namespace Blanketmen.Hypnos
{
    public sealed class ResourceManager
    {
        #region Singleton
        public static ResourceManager Instance { get; private set; }

        public static void CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new ResourceManager();
            }
        }

        public static void ReleaseInstance()
        {
            Instance = null;
        }

        private ResourceManager() { }
        #endregion

        public static string SaveFileExt { get; private set; }
        public static string BackupFileExt { get; private set; }
        public static string PersistentDataPath { get; private set; }

        private readonly Dictionary<int, DataArchiver> dataArchivers = new Dictionary<int, DataArchiver>(3);

        public void Initialize(ResourceConfig config)
        {
            SaveFileExt = config.saveFileExt;
            BackupFileExt = config.backupFileExt;
            PersistentDataPath = config.persistentDataPath;
            for (int i = 0; i < config.dataArchiverConfigs.Length; ++i)
            {
                DataArchiverConfig archiverConfig = config.dataArchiverConfigs[i];
                DataArchiver archiver = new DataArchiver(
                    CoreUtil.CreateSerializer(archiverConfig.serializer),
                    CoreUtil.CreateCompressor(archiverConfig.compressor),
                    CoreUtil.CreateEncryptor(archiverConfig.encryptor));
                dataArchivers.Add(archiverConfig.id, archiver);
            }
        }

        public void SaveArchive<T>(int archiverId, T obj, string filePath)
        {
            dataArchivers.TryGetValue(archiverId, out DataArchiver archiver);
            if (archiver == null)
            {
                Kernel.LogError($"[ResourceManager] DataArchiver is null. Id: {archiverId}");
                return;
            }

            filePath = Path.Combine(PersistentDataPath, filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            BackUp(filePath);

            try
            {
                archiver.Save(obj, filePath + SaveFileExt);
            }
            catch (Exception e)
            {
                Kernel.LogError($"[ResourceManager] Save file failed. Exception: {e.Message}");
                Restore(filePath);
            }
        }

        private void BackUp(string filePath)
        {
            try
            {
                File.Copy(filePath + SaveFileExt, filePath + BackupFileExt, true);
            }
            catch (FileNotFoundException)
            {
                // NOTE: No backup for the first save.
                return;
            }
            catch (Exception e)
            {
                Kernel.LogError($"[ResourceManager] Back up file failed. Exception: {e.Message}");
            }
        }

        private void Restore(string filePath)
        {
            try
            {
                File.Copy(filePath + BackupFileExt, filePath + SaveFileExt, true);
            }
            catch (Exception e)
            {
                Kernel.LogError($"[ResourceManager] Restore file failed. Exception: {e.Message}");
            }
        }

        public T LoadArchive<T>(int archiverId, string filePath)
        {
            dataArchivers.TryGetValue(archiverId, out DataArchiver archiver);
            if (archiver == null)
            {
                Kernel.LogError($"[ResourceManager] DataArchiver is null. Id: {archiverId}");
                return default;
            }

            filePath = Path.Combine(PersistentDataPath, filePath);
            try
            {
                return archiver.Load<T>(filePath + SaveFileExt);
            }
            catch (Exception e)
            {
                Kernel.LogError($"[ResourceManager] Load file failed, use backup instead. Exception: {e.Message}");
                return LoadBackup<T>(archiver, filePath);
            }
        }

        private T LoadBackup<T>(DataArchiver archiver, string filePath)
        {
            try
            {
                return archiver.Load<T>(filePath + BackupFileExt);
            }
            catch (Exception e)
            {
                Kernel.LogError($"[ResourceManager] Load backup file failed. Exception: {e.Message}");
                return default;
            }
        }
    }
}