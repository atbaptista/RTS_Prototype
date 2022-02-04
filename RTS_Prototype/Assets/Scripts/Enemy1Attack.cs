using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Attack : IState
{
    Enemy1 enemy1;

    public Enemy1Attack(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
