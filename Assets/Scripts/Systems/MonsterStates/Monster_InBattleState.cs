using UnityEngine;

public class Monster_InBattleState : ControllerState
{
    public override void Enter()
    {
        Controller.mController.MeshRenderer.material.color = Color.red;

        Controller.mController.Rigidbody.useGravity = false;
        Controller.mController.Rigidbody.isKinematic = true;
    }
}