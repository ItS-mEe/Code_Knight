using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftCard : Card
{
    public override void execute(){
        player.turnLeft();
    }
}
