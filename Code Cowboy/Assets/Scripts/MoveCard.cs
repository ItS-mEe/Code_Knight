using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : Card
{
    public override void execute(){
        player.move();
    }
}
