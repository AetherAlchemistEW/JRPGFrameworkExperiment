using UnityEngine;
using System.Collections;

public class RadialMenuSpawner : MonoBehaviour
{
    public static RadialMenuSpawner ins;
    public RadialMenu menuPrefab;

    void Awake()
    {
        ins = this;
    }

	public RadialMenu SpawnMenu(BattlePlayer obj, int ring, int cat)
    {
        RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
        newMenu.transform.SetParent(transform, false);
        newMenu.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);

        newMenu.SpawnButtons(obj, ring, cat);
        return newMenu;
    }
}
