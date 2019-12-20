using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Tank202
/// Copyright 2017, Rip Jaw Productions
/// </summary>

public class HUD : MonoBehaviour
{
    // Enumerations
    internal enum ExposureType
    {
        /// <summary>
        /// Currently not exposed
        /// </summary>
        None,

        /// <summary>
        /// Exposed for a specific time with transitions
        /// </summary>
        Display,

        /// <summary>
        /// Exposed without transitioning into group. When hidden transitions out
        /// </summary>
        Peek,

        /// <summary>
        /// Exposed with transitions. Needs to be manually hidden
        /// </summary>
        Show
    }

    // Classes
    /// <summary>
    /// Contains variables required for HUD group transitions
    /// </summary>
    [Serializable]
    public class HUDGroup
    {
        public CanvasGroup alphaHandler;

        /// <summary>
        /// Don't change this variable
        /// </summary>
        internal ExposureType exposureType;

        /// <summary>
        /// Don't change this variable
        /// </summary>
        internal float timeOut;
    }

    // Script parameters(Instance)
    [Header("Parameters")]

    [Tooltip("Time for group to be previewed(Transitions included)")]
    [Range(1, 20)]
    public float displayTime = 5;

    [Tooltip("Time it takes for alpha channel to change completely")]
    [Range(0, 5)]
    public float transitionTime = 1;

    [Header("Title and comment")]
    public Text title;
    public HUDGroup titleGroup;

    [Space]
    public Text comment;
    public HUDGroup commentGroup;

    [Header("Level and experience")]
    public Text levelText;
    public HUDGroup levelGroup;

    [Space]
    public Text experienceText;
    public Slider experienceSlider;
    public HUDGroup experienceGroup;
    int? experienceRequirement;
    int? experience;
    int? level;

    [Header("Player status")]
    public Text healthText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public HUDGroup playerStatusGroup;
    float? maxHealth;
    float? maxStamina;
    float? health;
    float? stamina;

    // Instance methods
    void Start()
    {
        SetAlpha(titleGroup, 0);
        SetAlpha(commentGroup, 0);
        SetAlpha(levelGroup, 0);
        SetAlpha(experienceGroup, 0);
        SetAlpha(playerStatusGroup, 0);

        instance = this;

        OnInitializedEvent(new EventArgs());
    }

    void SetAlpha(HUDGroup group, float value)
    {
        group.alphaHandler.alpha = value;
    }

    IEnumerator DisplayGroup(HUDGroup group, float delay = 0)
    {
        // Checks if coroutine is already running
        if (group.exposureType != ExposureType.Display)
        {
            // Delay fade in
            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            // No other coroutine - this coroutine will do transitions
            group.exposureType = ExposureType.Display;
            group.timeOut = Time.time + displayTime;

            while (group.timeOut > Time.time)
            {
                if (group.timeOut - transitionTime > Time.time)
                {
                    // It is the beginning or midle of the transition so we increase group visibility
                    group.alphaHandler.alpha += Time.deltaTime * transitionTime;
                }
                else
                {
                    // The end of transition so we decrease group visibility
                    group.alphaHandler.alpha -= Time.deltaTime * transitionTime;
                }

                group.alphaHandler.alpha = Mathf.Clamp01(group.alphaHandler.alpha);
                yield return null;
            }

            // When all is finished hide group and reset timer indicating that transition coroutine is finished
            group.alphaHandler.alpha = 0;
            group.timeOut = 0;
            group.exposureType = ExposureType.None;
        }
        else
        {
            // Other coroutine detected so we just add more showing time
            group.timeOut = Time.time + displayTime;
        }

        yield break;
    }

    IEnumerator PeekGroup(HUDGroup group)
    {
        // Checks if coroutine is already running
        if (group.exposureType == ExposureType.None)
        {
            // No other coroutine - this coroutine will do transitions
            group.exposureType = ExposureType.Peek;
            group.timeOut = float.PositiveInfinity;

            while (group.timeOut > Time.time)
            {
                if (group.timeOut - transitionTime > Time.time)
                {
                    group.alphaHandler.alpha = 1;
                }
                else
                {
                    // The end of transition so we decrease group visibility
                    group.alphaHandler.alpha -= Time.deltaTime * transitionTime;
                    group.alphaHandler.alpha = Mathf.Clamp01(group.alphaHandler.alpha);
                }

                yield return null;
            }

            // When all is finished hide group and reset timer indicating that transition coroutine is finished
            group.alphaHandler.alpha = 0;
            group.timeOut = 0;
            group.exposureType = ExposureType.None;
        }

        yield break;
    }

    IEnumerator ShowGroup(HUDGroup group)
    {
        // Checks if coroutine is already running
        if (group.exposureType == ExposureType.None)
        {
            // No other coroutine - this coroutine will do transitions
            group.exposureType = ExposureType.Show;
            group.timeOut = float.PositiveInfinity;

            while (group.timeOut > Time.time)
            {
                if (group.timeOut - transitionTime > Time.time)
                {
                    // It is the beginning or midle of the transition so we increase group visibility
                    group.alphaHandler.alpha += Time.deltaTime * transitionTime;
                }
                else
                {
                    // The end of transition so we decrease group visibility
                    group.alphaHandler.alpha -= Time.deltaTime * transitionTime;
                }

                group.alphaHandler.alpha = Mathf.Clamp01(group.alphaHandler.alpha);
                yield return null;
            }

            // When all is finished hide group and reset timer indicating that transition coroutine is finished
            group.alphaHandler.alpha = 0;
            group.timeOut = 0;
            group.exposureType = ExposureType.None;
        }

        yield break;
    }

    void HideGroup(HUDGroup group, float delay)
    {
        // Checks if coroutine is already running
        if (group.exposureType == ExposureType.Show)
        {
            group.timeOut = delay + transitionTime;
        }
    }

    void _SetTitle(string title, string comment)
    {
        if (title != null)
        {
            this.title.text = title;
        }
        if (comment != null)
        {
            this.comment.text = comment;
        }
    }

    void _SetExperience(int? experienceRequirement, int? experience, int? level)
    {
        this.experienceRequirement = experienceRequirement ?? this.experienceRequirement;
        this.experience = experience ?? this.experience;
        this.level = level ?? this.level;
    }

    void _SetExperience(int? experienceGain)
    {
        if(experience.HasValue)
        {
            experience += experienceGain ?? 0;
        }
        else
        {
            experience = experienceGain ?? experience;
        }
    }

    void _SetPlayerStatusUI()
    {
        if (maxHealth.HasValue && health.HasValue)
        {
            healthSlider.value = health.Value / maxHealth.Value;
        }
        if(maxStamina.HasValue && stamina.HasValue)
        {
            staminaSlider.value = stamina.Value / maxStamina.Value;
        }
    }

    void _SetPlayerStatusMax(float? maxHealth, float? maxStamina)
    {
        this.maxHealth = maxHealth ?? this.maxHealth;
        this.maxStamina = maxStamina ?? this.maxStamina;
        _SetPlayerStatusUI();
    }

    void _SetPlayerStatus(float? health, float? stamina)
    {
        if (health.HasValue)
        {
            this.health = health;
            healthText.text = Math.Round(health.Value).ToString();
        }
        if(stamina.HasValue)
        {
            this.stamina = stamina;
        }
        _SetPlayerStatusUI();
    }

    void _DisplayTitle(string title, string comment)
    {
        _SetTitle(title, comment);
        StartCoroutine(DisplayGroup(titleGroup));
        StartCoroutine(DisplayGroup(commentGroup, 0.5f));
    }

    void _DisplayExperience(int? experience, int? experienceRequirement = null, int? currentExperience = null, int? level = null)
    {
        _SetExperience(experienceRequirement, currentExperience, level);
        _SetExperience(experience);
        StartCoroutine(DisplayGroup(levelGroup));
        StartCoroutine(DisplayGroup(experienceGroup));
    }

    void _DisplayPlayerStatus(float? health, float? stamina, float? maxHealth, float? maxStamina)
    {
        _SetPlayerStatus(health, stamina);
        _SetPlayerStatusMax(maxHealth, maxStamina);
        StartCoroutine(DisplayGroup(playerStatusGroup));
    }

    void _PeekTitle(string title, string comment)
    {
        _SetTitle(title, comment);
        StartCoroutine(PeekGroup(titleGroup));
        StartCoroutine(PeekGroup(commentGroup));
    }

    void _PeekExperience()
    {
        StartCoroutine(PeekGroup(experienceGroup));
        StartCoroutine(PeekGroup(levelGroup));
    }

    void _PeekPlayerStatus(float? health = null, float? stamina = null, float? maxHealth = null, float? maxStamina = null)
    {
        _SetPlayerStatus(health, stamina);
        _SetPlayerStatusMax(maxHealth, maxStamina);
        StartCoroutine(PeekGroup(playerStatusGroup));
    }

    void _ShowTitle(string title, string comment)
    {
        _SetTitle(title, comment);
        StartCoroutine(ShowGroup(titleGroup));
        StartCoroutine(ShowGroup(commentGroup));
    }

    void _ShowPlayerStatus(float? health = null, float? stamina = null, float? maxHealth = null, float? maxStamina = null)
    {
        _SetPlayerStatus(health, stamina);
        _SetPlayerStatusMax(maxHealth, maxStamina);
        StartCoroutine(ShowGroup(playerStatusGroup));
    }

    void _HideTitle(float delay)
    {
        HideGroup(titleGroup, delay);
        HideGroup(titleGroup, delay);
    }

    void _HideExperience(float delay)
    {
        HideGroup(levelGroup, delay);
        HideGroup(experienceGroup, delay);
    }

    void _HidePlayerStatus(float delay)
    {
        HideGroup(playerStatusGroup, delay);
    }

    void OnInitializedEvent(EventArgs e)
    {
        if (OnInitialized != null)
        {
            OnInitialized.Invoke(this, e);
            OnInitialized = null;
        }
    }

    // Static variables and events
    static HUD instance;
    static HUD Instance     // Hidden HUD singleton
    {
        get
        {
            Load();
            return instance;
        }
    }

    static bool IsLoading { get; set; }

    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    public static event EventHandler OnInitialized;

    // public Static methods
    /// <summary>
    /// Recommended to call when scene has been loaded to minimize performance impact upon first HUD call
    /// </summary>
    public static void Load()
    {
        if (instance == null && IsLoading == false)
        {
            IsLoading = true;
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);

            // Set HUD scene as active for a moment as to let Start() methods to fire up
            Scene previousActiveScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("HUD"));
            SceneManager.SetActiveScene(previousActiveScene);
        }
        IsLoading = false;
    }

    public static void SyncExperience(int experienceRequirement, int experience, int level)
    {
        if (IsInitialized)
        {
            Instance._SetExperience(experienceRequirement, experience, level);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._SetExperience(experienceRequirement, experience, level);
            });
            Load();
        }
    }

    public static void SetPlayerStatusMax(float maxHealth, float maxStamina)
    {
        if (IsInitialized)
        {
            Instance._SetPlayerStatusMax(maxHealth, maxStamina);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._SetPlayerStatusMax(maxHealth, maxStamina);
            });
            Load();
        }
    }

    public static void SetPlayerHealth(float health)
    {
        if (IsInitialized)
        {
            Instance._SetPlayerStatus(health, null);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._SetPlayerStatus(health, null);
            });
            Load();
        }
    }

    public static void SetPlayerStamina(float stamina)
    {
        if (IsInitialized)
        {
            Instance._SetPlayerStatus(null, stamina);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._SetPlayerStatus(null, stamina);
            });
            Load();
        }
    }

    public static void PeekHUD()
    {
        if (IsInitialized)
        {
            Instance._PeekPlayerStatus();
            Instance._PeekExperience();
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._PeekPlayerStatus();
                Instance._PeekExperience();
            });
            Load();
        }
    }

    public static void DisplayTitle(string title, string comment)
    {
        if (IsInitialized)
        {
            Instance._DisplayTitle(title, comment);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._DisplayTitle(title, comment);
            });
            Load();
        }
    }

    public static void DisplayExperience(int experienceGain = 0)
    {
        if (IsInitialized)
        {
            Instance._DisplayExperience(experienceGain);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._DisplayExperience(experienceGain);
            });
            Load();
        }
    }

    public static void ShowPlayerStatus()
    {
        if (IsInitialized)
        {
            Instance._ShowPlayerStatus();
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._ShowPlayerStatus();
            });
            Load();
        }
    }

    public static void HidePlayerStatus(float delay = 0)
    {
        if (IsInitialized)
        {
            Instance._HidePlayerStatus(delay);
        }
        else
        {
            OnInitialized += ((sender, e) =>
            {
                Instance._HidePlayerStatus(delay);
            });
            Load();
        }
    }
}
