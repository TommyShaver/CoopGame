using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface IDamageable
{
    // On bombs or landmines we set yRotation on 99 sowe can turn on all hit detection
    public void Damage(int damange , float yRotationHit);
}
