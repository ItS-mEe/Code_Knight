using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightCard : Card
{
    public override void execute(){
        player.turnRight();
    }
}
