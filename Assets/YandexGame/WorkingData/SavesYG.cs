
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int Money;
        public bool Is2XSpeed;
        public bool Is2XMoney;
        public float SpeedBoostTimer;
        public float MoneyBoostTimer;
        public float TempKdRotateRoulette;
        public string petInventoryData;
        public List<ShopItemSaveData> shopItemsData = new List<ShopItemSaveData>();
        public int MoneyRecord;
        public float MusicVolume;
        public float EffectVolume;
        public bool TutorShown;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            MusicVolume = 0.5F;
            EffectVolume = 0.5F;
            openLevels[1] = true;
        }
    }
}
