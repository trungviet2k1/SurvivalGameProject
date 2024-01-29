using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public UnityEvent OnDayPass = new();

    public int dayInGame = 1;
    public int yearInGame = 0;
    public TextMeshProUGUI dayUI;

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public Season currentSeason = Season.Spring;
    private int dayPerSeason = 2;
    private int dayInCurrentSeason = 1;

    public enum DayOfWeak
    {
        Monday,
        Tuesday,
        Wednesday,
        Thurday,
        Friday,
        Saturday,
        Sunday
    }

    public DayOfWeak currentDayOfWeak = DayOfWeak.Monday;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayInCurrentSeason += 1;

        currentDayOfWeak = (DayOfWeak)(((int)currentDayOfWeak + 1) % 7);

        if (dayInCurrentSeason > dayPerSeason)
        {
            dayInCurrentSeason = 1;
            currentSeason = GetNextSeason();
        }

        UpdateUI();
        OnDayPass.Invoke();
    }

    private Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeason;
        int nextSeasonIndex = (currentSeasonIndex + 1) % 4;
        if (nextSeasonIndex == 0)
        {
            yearInGame += 1;
        }

        return (Season)nextSeasonIndex;
    }

    private void UpdateUI()
    {
        dayUI.text = $"{currentDayOfWeak} {dayInGame}, {currentSeason}";
    }
}