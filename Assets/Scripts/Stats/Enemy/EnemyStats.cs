using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField]
    private int maxHealth, level = 0, xpAwarded = 0;
    private int currentHealth;
    [SerializeField]
    GUIStyle healthBarStyle;
    [SerializeField]
    GameObject scrollingBattleText;
    public GameObject testRespawn;
    Texture2D healthTexture;
    float halfHeight;
    GameObject healthbar = null;

    private void Die()
    {
        Destroy(healthbar);
        FindObjectOfType<PlayerStats>().AddXp(xpAwarded, level);
        // This guy spreads like cancer...
        Instantiate(testRespawn);
        // TODO: This will need to be removed once animations are made (if, ever, at any point.)
        Destroy(this.gameObject);
    }

    public void TakeDamage(int value, bool isCrit)
    {
        if (healthbar == null)
            healthbar = FindObjectOfType<GameVariables>().CreateHealthbar(this.gameObject);
        ScrollingCombatText battleText = Instantiate(scrollingBattleText).GetComponent<ScrollingCombatText>();
        battleText.Create(gameObject.transform, value.ToString(), isCrit);
        currentHealth -= value;
        if (currentHealth <= 0)
            Die();
    }

    public Vector2 GetHealthBarPos()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        return new Vector2(pos.x - 50, pos.y + halfHeight + (Screen.height / 15));
    }

    public float GetHealthBarSize()
    {
        return (1.0f * currentHealth / maxHealth);
    }

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        level = 0; // Forced scaling
        if (level == 0)
        {
            level = GameVariables.highestPlayerLevel;
            int adjustedMaxHealth = maxHealth + (int)((maxHealth * .1f) * level);
            currentHealth = adjustedMaxHealth;
        }
        halfHeight = gameObject.GetComponent<MeshRenderer>().bounds.extents.y;
        healthTexture = new Texture2D(1, 1);
        healthTexture.SetPixel(0,0,Color.red);
        healthTexture.Apply();
        for (int i = transform.childCount-1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
