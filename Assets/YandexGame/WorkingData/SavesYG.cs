
using System.Collections.Generic;
using static Game;

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

        public AchievementsData achievementsData;
        public BoostData boostData;
        public FortuneData fortuneData;
        public GameData gameData;
        public PlotData plotData;
        public ProgressBarData progressBarData;
        public SettingsData settingsData;
        public SkinCoinData skinCoinData;
        public SpawnDownData spawnDownData;
        public StatsData statsData;
        public TimerData timerData;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {

        }
    }
}
