using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class HUDHandler : MonoBehaviour
{

    public static HUDHandler instance { get; private set; }

    private VisualElement m_Healthbar;
    private VisualElement m_Magicbar;
    private VisualElement m_Lanternbar;
    private VisualElement m_Shieldbar;
    private VisualElement m_Dodgebar;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
     *
     *
     *
     *
     *
     */
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_Magicbar = uiDocument.rootVisualElement.Q<VisualElement>("MagicBar");
        SetMagicValue(1.0f);

        m_Lanternbar = uiDocument.rootVisualElement.Q<VisualElement>("LanternBar");
        SetLanternValue(1.0f);

        m_Shieldbar = uiDocument.rootVisualElement.Q<VisualElement>("ShieldBar");
        SetShieldValue(1.0f);

        m_Dodgebar = uiDocument.rootVisualElement.Q<VisualElement>("DodgeBar");
        SetDodgeValue(1.0f);
    }

    /*
     *
     *
     *
     *
     *
     */
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }


    /*
     *
     *
     *
     *
     *
     */
    public void SetMagicValue(float percentage)
    {
        m_Magicbar.style.width = Length.Percent(100 * percentage);
    }


    /*
     *
     *
     *
     *
     *
     */
    public void SetLanternValue(float percentage)
    {
        // TODO: change this to alter height instead of width.
        // TODO: learn exactly how unity UI Toolkit works
        m_Lanternbar.style.width = Length.Percent(100 * percentage);
    }


    /*
     * 
     * 
     * 
     * 
     * 
     */
    public void SetShieldValue(float percentage)
    {
        m_Shieldbar.style.width = Length.Percent(100 * percentage);
    }

    /*
     * 
     * 
     * 
     * 
     * 
     */
    public void SetDodgeValue(float percentage)
    {
        m_Dodgebar.style.width = Length.Percent(100 * percentage);
    }
}
