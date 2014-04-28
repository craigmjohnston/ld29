using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public enum BuildingType {
    None, House, ForestersLodge, GuardTower, Villa
}

public enum TimeOfDay {
    None, Dawn, Morning, Noon, Afternoon, Dusk, Night
}

[Serializable]
public class BuildingCost {
    public BuildingType buildingType;
    public int food;
    public int wood;
    public int gold;
}

[Serializable]
public class TimeColour {
    public TimeOfDay timeOfDay;
    public Color backgroundColour;
    public Color fogColour;
    public Color ambientColour;
    public bool fog;
    public float fogDensity;
    public Color textColour;
}

public class GameValues : MonoBehaviour {
    private static GameValues instance;

    public static GameValues Instance {
        get {
            if (instance == null) {
                instance = new GameValues();
            }
            return instance;
        }
    }

    public float gameStartTime;
    public static float GameStartTime {
        get { return Instance.gameStartTime; }
    }

    public int food;
    public static int Food {
        get { return Instance.food; }
        set { Instance.food = value; }
    }

    public int wood;
    public static int Wood {
        get { return Instance.wood; }
        set { Instance.wood = value; }
    }

    public int gold;
    public static int Gold {
        get { return Instance.gold; }
        set { Instance.gold = value; }
    }

    public Building currentBuilding;
    public static Building CurrentBuilding {
        get { return Instance.currentBuilding; }
        set { Instance.currentBuilding = value; }
    }

    public BuildingCost[] buildingCosts;
    public static BuildingCost[] BuildingCosts {
        get { return Instance.buildingCosts; }
    }

    public float currentTime = 0;
    public static float CurrentTime {
        get { return Instance.currentTime; }
    }

    private int numberOfVillagers = 0;
    public static int NumberOfVillagers {
        get { return Instance.numberOfVillagers; }
        set {
            if (value < NumberOfVillagers) {
                Instance.daysWithoutDeaths = 0;
                Instance.deaths += NumberOfVillagers - value;
            }
            Instance.numberOfVillagers = value; 
        }
    }

    private int numberOfNobles = 0;
    public static int NumberOfNobles {
        get { return Instance.numberOfNobles; }
        set {
            if (value < NumberOfNobles) {
                Instance.daysWithoutDeaths = 0;
                Instance.deaths += NumberOfNobles - value;
            }
            Instance.numberOfNobles = value;
        }
    }

    public int TotalNumberOfVillagers {
        get { return NumberOfVillagers + NumberOfNobles; }
    }

    private int deaths = 0;
    private int days = 0;

    public float lengthOfHour;
    public int dailyFoodPerPerson;
    public int dailyFoodPerNoble;
    public int dailyGoldPerVillager;
    public int dailyGoldPerNoble;
    public int daysWithoutDeaths;

    private int villagersToDie = 0;
    public static int VillagersToDie {
        get { return Instance.villagersToDie; }
        set { Instance.villagersToDie = value; }
    }

    private int noblesToDie = 0;
    public static int NoblesToDie {
        get { return Instance.noblesToDie; }
        set { Instance.noblesToDie = value; }
    }

    public static TimeOfDay CurrentTimeOfDay {
        get {
            float hours = CurrentTime*24;
            if (hours >= 5 && hours < 8) return TimeOfDay.Dawn;
            if (hours >= 8 && hours < 12) return TimeOfDay.Morning;
            if (hours >= 12 && hours < 14) return TimeOfDay.Noon;
            if (hours >= 14 && hours < 20) return TimeOfDay.Afternoon;
            if (hours >= 20 && hours < 22) return TimeOfDay.Dusk;
            if (hours >= 22 || hours < 5) return TimeOfDay.Night;
            return TimeOfDay.None;
        }
    }

    private float timeSinceLastPhase;
    public float TimeSinceLastPhase {
        get {
            if (StartOfPhase > currentTime) {
                return (currentTime + 1) - StartOfPhase;
            }
            return currentTime - StartOfPhase;
        }
    }

    public float LengthOfCurrentPhase {
        get {
            switch (CurrentTimeOfDay) {
                case TimeOfDay.Dawn:
                    return 3 / 24f;
                case TimeOfDay.Morning:
                    return 4 / 24f;
                case TimeOfDay.Noon:
                    return 2 / 24f;
                case TimeOfDay.Afternoon:
                    return 6 / 24f;
                case TimeOfDay.Dusk:
                    return 2 / 24f;
                case TimeOfDay.Night:
                    return 7 / 24f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private float lengthOfCurrentPhase;
    public float TimeUntilNextPhase {
        get { return lengthOfCurrentPhase - TimeSinceLastPhase; }
    }

    public float StartOfPhase {
        get {
            switch (CurrentTimeOfDay) {
                case TimeOfDay.Dawn:
                    return 5f / 24;
                case TimeOfDay.Morning:
                    return 8f / 24;
                case TimeOfDay.Noon:
                    return 12f / 24;
                case TimeOfDay.Afternoon:
                    return 14f / 24;
                case TimeOfDay.Dusk:
                    return 20f / 24;
                case TimeOfDay.Night:
                    return 22f / 24;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public TimeOfDay NextPhase {
        get {
            switch (CurrentTimeOfDay) {
                case TimeOfDay.Dawn:
                    return TimeOfDay.Morning;
                case TimeOfDay.Morning:
                    return TimeOfDay.Noon;
                case TimeOfDay.Noon:
                    return TimeOfDay.Afternoon;
                case TimeOfDay.Afternoon:
                    return TimeOfDay.Dusk;
                case TimeOfDay.Dusk:
                    return TimeOfDay.Night;
                case TimeOfDay.Night:
                    return TimeOfDay.Dawn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public float CurrentHunger {
        get { return numberOfVillagers*dailyFoodPerPerson + numberOfNobles*dailyFoodPerNoble; }
    }

    public dfLabel[] dynamicColourLabels;

    private bool inputDisabled = false;
    public static bool InputDisabled {
        get { return Instance.inputDisabled; }
        set { Instance.inputDisabled = value; }
    }

    public dfLabel infoLabel;
    public static dfLabel InfoLabel {
        get { return Instance.infoLabel; }
    }

    public Color dayColour;
    public Color nightColour;

    public TimeColour[] timeColours;

    private int foodCostOfHouse;

    public dfPanel loseDialog;
    public dfPanel winDialog;
    private bool gameEnded = false;

    void Start() {
        instance = this;
        foodCostOfHouse = buildingCosts.First(b => b.buildingType == BuildingType.House).food;
        lengthOfCurrentPhase = LengthOfCurrentPhase;
        TimeColour timeColour = timeColours.First(c => c.timeOfDay == CurrentTimeOfDay);
        RenderSettings.ambientLight = timeColour.ambientColour;
        RenderSettings.fogColor = timeColour.fogColour;
        RenderSettings.fogDensity = timeColour.fogDensity;
    }

    void Update() {
        if (gameEnded) return;
        TimeOfDay previousTime = CurrentTimeOfDay;
        float relativeDeltaTime = ((1 / lengthOfHour) * Time.deltaTime) / 24f;
        currentTime += relativeDeltaTime;
        // new day
        if (currentTime >= 1) {
            currentTime -= 1;
        }
        ChangeColours();
        timeSinceLastPhase += relativeDeltaTime;
        // new period in day
        if (CurrentTimeOfDay != previousTime) {
            lengthOfCurrentPhase = LengthOfCurrentPhase;
            if (CurrentTimeOfDay == TimeOfDay.Dawn && previousTime != TimeOfDay.Dawn) {
                food -= (dailyFoodPerPerson * numberOfVillagers) + (dailyFoodPerNoble * numberOfNobles);
                if (food < 0) {
                    villagersToDie = (int)Mathf.Min(numberOfVillagers, Mathf.Floor(Mathf.Abs(food) / (float)dailyFoodPerPerson));
                    food += villagersToDie * dailyFoodPerPerson;
                    if (villagersToDie == numberOfVillagers) {
                        noblesToDie = (int)Mathf.Min(numberOfNobles, Mathf.Floor(Mathf.Abs(food) / (float)dailyFoodPerNoble));
                    }
                    food = 0;
                }
                gold += (dailyGoldPerVillager * numberOfVillagers) + (dailyGoldPerNoble * numberOfNobles);
                days += 1;
                daysWithoutDeaths += 1;
            }
        }
        // win condition
        if (daysWithoutDeaths > 3 && numberOfVillagers + numberOfNobles >= 100) {
            winDialog.transform.FindChild("Values Label").GetComponent<dfLabel>().Text = days + " days have passed, and " + deaths + " villagers have perished.";
            winDialog.Show();
            gameEnded = true;
        }
        // lose condition
        if (food < foodCostOfHouse && numberOfVillagers == 0 && !FindObjectsOfType<SpawnVillager>().Any()) {
            loseDialog.transform.FindChild("Values Label").GetComponent<dfLabel>().Text = days + " days passed.\r\nAll " + deaths + " villagers perished.";
            loseDialog.Show();
            gameEnded = true;
        }
    }

    protected void ChangeColours() {
        TimeColour timeColour = timeColours.First(c => c.timeOfDay == CurrentTimeOfDay);
        TimeColour nextTimeColour = timeColours.First(c => c.timeOfDay == NextPhase);
        float progress = TimeSinceLastPhase/lengthOfCurrentPhase;
        RenderSettings.ambientLight = Color.Lerp(timeColour.ambientColour, nextTimeColour.ambientColour, Mathfx.Hermite(0f, 1f, progress));
        RenderSettings.fogColor = Color.Lerp(timeColour.fogColour, nextTimeColour.fogColour, Mathfx.Hermite(0f, 1f, progress));
        RenderSettings.fogDensity = Mathfx.Hermite(timeColour.fogDensity, nextTimeColour.fogDensity, progress);
        foreach (dfLabel label in dynamicColourLabels) {
            label.Color = Color.Lerp(timeColour.textColour, nextTimeColour.textColour, Mathfx.Hermite(0f, 1f, progress));
        }
    }

    public static IEnumerator TimeLerp(float target, float length) {
        Instance.gameEnded = true;
        InputDisabled = true;
        float start = Instance.currentTime;
        float time = 0;
        while (time < length) {
            Instance.currentTime = Mathfx.Hermite(start, target, time / length);
            Instance.ChangeColours();
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        Instance.gameEnded = false;
        InputDisabled = false;
    }

    public IEnumerator ColourLerp(TimeColour lerpTo, float time) {
        float progress = 0.01f;
        while (progress < time) {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, lerpTo.backgroundColour, progress/time);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, lerpTo.ambientColour, progress / time);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, lerpTo.fogColour, progress / time);
            progress += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}