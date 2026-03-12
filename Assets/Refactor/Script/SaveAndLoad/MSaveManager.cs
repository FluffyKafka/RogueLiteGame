using AudioSystem;
using InventorySystem;
using SkillSystem;
using StatsSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace SaveSystem
{
    public interface IInitSaveManager
    {
        public void Init(ISaveStats _stats, ISaveInventory _inventory, ISaveSkill _skill, ISaveAduio _audio);
    }
    internal class MSaveManager : MonoBehaviour, IInitSaveManager
    {
        [SerializeField] protected string filename;
        [SerializeField] protected bool isEncrptData;
        [SerializeField] protected string code;
        [SerializeField] protected DGameData defaultGameData;

        [Header("Test")]
        [SerializeField] protected DGameData gameData;
        protected TFileDataHandler fileDataHandler;

        protected ISaveStats stats;
        protected ISaveStats.DStatsData statsData = new();

        protected ISaveInventory inventory;
        protected ISaveInventory.DInventoryData inventoryData = new();

        protected ISaveSkill skill;
        protected ISaveSkill.DSkillSaveData skillData = new();

        protected ISaveAduio audioSystem;
        protected ISaveAduio.DAudioSaveData audioData = new();

        public void Init(ISaveStats _stats, ISaveInventory _inventory, ISaveSkill _skill, ISaveAduio _audio)
        {
            stats = _stats;
            inventory = _inventory;
            skill = _skill;
            audioSystem = _audio;
        }

        private void Start()
        {
            fileDataHandler = new TFileDataHandler(Application.persistentDataPath, filename, isEncrptData, code);

            LoadGame();
        }

        [ContextMenu("Delete Save File")]
        protected void DeleteSaveData()
        {
            fileDataHandler = new TFileDataHandler(Application.persistentDataPath, filename, isEncrptData, code);
            fileDataHandler.Delete();
        }

        protected void NewGame()
        {
            gameData = new DGameData();
            fileDataHandler.Save(gameData);
        }

        protected void LoadGame()
        {
            gameData = fileDataHandler.Load();
            if(gameData == null)
            {
                gameData = defaultGameData;
            }
            SeperateGameData();

            stats.Load(statsData);
            inventory.Load(inventoryData);
            skill.Load(skillData);
            audioSystem.Load(audioData);
        }

        [ContextMenu("Save Game --Test")]
        protected void SaveGame()
        {
            stats.Save(ref statsData);
            inventory.Save(ref inventoryData);
            skill.Save(ref skillData);
            audioSystem.Save(ref audioData);

            GenerateGameData();
            fileDataHandler.Save(gameData);
        }

        protected void GenerateGameData()
        {
            gameData.HP = statsData.hp;

            gameData.equipment.CopyFrom(inventoryData.equipment);
            gameData.equipmentStash.CopyFrom(inventoryData.equipmentStash);
            gameData.itemStash = inventoryData.itemStash;

            gameData.skillTree.CopyFrom(skillData.skillUnlock);

            gameData.bgmVolume = audioData.bgmVolume;
            gameData.sfxVolume = audioData.sfxVolume;
            gameData.envVolume = audioData.envVolume;
        }
        protected void SeperateGameData()
        {
            statsData.hp = gameData.HP;

            inventoryData.equipment = gameData.equipment.ToDictionary();
            inventoryData.equipmentStash = gameData.equipmentStash.ToDictionary();
            inventoryData.itemStash = gameData.itemStash;

            skillData.skillUnlock = gameData.skillTree.ToDictionary();

            audioData.bgmVolume = gameData.bgmVolume;
            audioData.sfxVolume = gameData.sfxVolume;
            audioData.envVolume = gameData.envVolume;
        }

        protected bool HaveSaveData()
        {
            if (fileDataHandler.Load() != null)
            {
                return true;
            }
            return false;
        }
    }                    

    [System.Serializable]
    public class DGameData
    {
        public float HP;
        public int currency;
        public int coin;
        public bool isNewGame;

        public List<string> itemStash;
        public DSerializableDictionary<string, float> equipmentStash;
        public DSerializableDictionary<string, float> equipment;

        public DSerializableDictionary<string, bool> skillTree;

        public bool isPlayerRemainingExist;
        public string playerRemainingSceneName;
        public int playerLeftCurrency;

        public bool isPlayerHealthBarActive;

        public float bgmVolume;
        public float sfxVolume;
        public float envVolume;

        public string currentSceneName;

        public DGameData()
        {
            isNewGame = true;

            HP = -1;

            currency = 0;
            coin = 0;

            itemStash = new List<string>();
            equipmentStash = new DSerializableDictionary<string, float>();
            equipment = new DSerializableDictionary<string, float>();

            skillTree = new DSerializableDictionary<string, bool>();

            isPlayerRemainingExist = false;
            playerRemainingSceneName = "";
            playerLeftCurrency = 0;

            bgmVolume = 0.3688f;
            sfxVolume = 0.3688f;
            envVolume = 0.3688f;

            isPlayerHealthBarActive = false;
            currentSceneName = string.Empty;
        }
    }

    [System.Serializable]
    public class DSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            this.Clear();

            Assert.IsTrue(keys.Count == values.Count);

            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }

        public void CopyFrom(Dictionary<TKey, TValue> source)
        {
            if (source == null) return;

            this.Clear();
            foreach (var kvp in source)
            {
                this.Add(kvp.Key, kvp.Value);
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(this);
        }
    }

    public class TFileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        private bool isEncryptData = false;
        private string codeWord;

        public TFileDataHandler(string _dataDirPath, string _dataFileName, bool _isEncryptData, string _codeWord)
        {
            this.dataDirPath = _dataDirPath;
            this.dataFileName = _dataFileName;
            this.isEncryptData = _isEncryptData;
            this.codeWord = _codeWord;
        }

        public void Save(DGameData _data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//按照要求创建目录（若已经存在则什么也不做）

                string dataToStore = JsonUtility.ToJson(_data, true);//将数据类转换为string(仅public和[SerializeField]可被序列化，后面的bool指示文件是否被格式化以提高可读性)

                if (isEncryptData)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))//开启文件(若文件已经存在则将其覆盖)
                {
                    using (StreamWriter writer = new StreamWriter(stream))//创建写入器
                    {
                        writer.Write(dataToStore);//写入
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        public DGameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            DGameData loadData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (isEncryptData)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadData = JsonUtility.FromJson<DGameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadData;
        }

        public void Delete()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private string EncryptDecrypt(string _data)
        {
            string modifiedData = "";

            for (int i = 0; i < _data.Length; i++)
            {
                modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);//异或两次变回原值
            }

            return modifiedData;
        }
    }
}