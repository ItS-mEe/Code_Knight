using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCard : Card
{
    public override void execute(){
        player.attack();
    }
}
