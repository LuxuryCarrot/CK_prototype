using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControlChanger : MonoBehaviour
{
    PlayerController player;
    string justEatItem;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponent<PlayerController>();
    }


    public void ItemEattingCheck()
    {
        if (GameManager.Instance.isItemEatting)
        {
            switch (player.stat.curWeaponProperty)
            {
                case ElementalProperty.Fire:
                    justEatItem = "Trans_Fire";
                    break;
                case ElementalProperty.Water:
                    justEatItem = "Trans_Water";
                    break;
                case ElementalProperty.Grass:
                    justEatItem = "Trans_Grass";
                    break;
            }

            //컨트롤러 교체
            transform.GetComponent<Animator>().runtimeAnimatorController = player.propertyAnimController[(int)player.stat.curWeaponProperty - 1]; ;
            GameManager.Instance.isItemEatting = false;         //다먹었으므로
        }
    }


}
