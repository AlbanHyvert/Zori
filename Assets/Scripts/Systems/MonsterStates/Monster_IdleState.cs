
using System.Collections;
using UnityEngine;

public class Monster_IdleState : ControllerState
{
    private MonsterController _controller = null;

    public override void Enter()
    {
        _controller = Controller.mController;

        _controller.MeshRenderer.material.color = Color.white;

        Controller.mController.Rigidbody.isKinematic = false;
        Controller.mController.Rigidbody.useGravity = true;

        WaitBeforeSwitchState();
    }

    private void WaitBeforeSwitchState()
    {
        //Check for Behaviour
        int rdmChance = Random.Range(0, 100);

        _controller.MeshRenderer.material.color = Color.white;

        if (rdmChance <= 80)
        {
            Controller.SetState(new Monster_WalkState());

            return;
        }
        else if (rdmChance > 80)
        {
            _controller.MeshRenderer.material.color = Color.black;

            WaitBeforeSwitchState();
        }
    }

    public override void Exit() 
    {
        
    }
}