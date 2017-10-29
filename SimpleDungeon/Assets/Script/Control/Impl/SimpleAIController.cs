using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIController : AIController {

    Character character;
    AIAction curAction = AIAction.IDEL;
    
    Character target;

    void Update(){
        if(receiver != null){
            character = (Character)receiver;
        }

        if(!character.GetCharacterInfo().IsDead ()){
            ActionPlanning();
            DoAction();
        }
    }

    protected void ActionPlanning(){
        if(curAction == AIAction.IDEL)
            curAction = AIAction.SEARCH;
    }

    protected void DoAction(){
        if(curAction == AIAction.IDEL){
            Idel();
        }
        else if(curAction == AIAction.SEARCH){
            Search();
        }
        else if(curAction == AIAction.ATTACK){
            Attack();
        }
    }

    protected void Idel(){
        character.releaseA();
        character.releaseD();
        character.releaseW();
        character.releaseS();
        character.releaseJ();
        character.releaseK();
        character.releaseL();
    }

    protected void Search(){
        if(character.isMoving){
            if(character.isBlocking){
                if(character.faceDir == (int)World.Direction.LEFT){
                    character.releaseA();
                    character.pressD();
                }
                else{
                    character.releaseD();
                    character.pressA();
                }
            }
        }
        else{
            character.releaseD();
            character.pressA();
        }
    }
    protected void Attack(){
        character.Turn(character.DirectionTo(target));
        character.pressJ();
    }

    protected void Move(){

    }
}
