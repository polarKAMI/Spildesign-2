using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ChracterStatAmmoModifier : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        playerattack2 ammo = character.GetComponent<playerattack2>();
        if (ammo != null)
        {
            ammo.AddAmmo((int)val);
        }
    }

   
}
