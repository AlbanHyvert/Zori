using UnityEngine;

public class Monster_ChaseState : ControllerState
{
    private MonsterController _controller = null;
    private float _speed = 0;

    private float _currentChaseTime = 0f;

    public override void Enter()
    {
        _controller = Controller.mController;

        Debug.Log("In Chase");

        _controller.MeshRenderer.material.color = Color.red;

        if(_controller.Target == null) 
        {
            _controller.SetState(new Monster_IdleState());

            return;
        }

        GameManager.Instance.OnUpdateMonster += Tick;
    }

    public override void Tick(float time)
    {
        if(!_controller) return;

        _currentChaseTime += time;

        if(_currentChaseTime > _controller.Settings.ChaseDuration)
        {
            PlayerController player = _controller.Target.GetComponent<PlayerController>();

            if(player) player.SetIsChased(false);

            _controller.SetState(new Monster_IdleState());
        }

        // Gradually increase the movement speed up to the maximum speed
        if (_speed < _controller.Settings.MaxRunVelocity)
        {
            _speed += _controller.Settings.RunSpeed * (_controller.Settings.Acceleration * time);
        }

        SetRotation();

        _controller.transform.position = Vector3.MoveTowards(_controller.transform.position, _controller.Target.position, _speed * time);
    }

    private void SetRotation()
    {
        // Get the direction from the current object to the target object
        Vector3 direction = _controller.Target.position - _controller.transform.position;

        // Set the y component of the direction vector to 0
        direction.y = 0;

        // Create a Quaternion that looks in the modified direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Apply the rotation to the object's transform, but only on the y-axis
        _controller.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    public override void Exit() 
    {
        GameManager.Instance.OnUpdateMonster -= Tick;
    }
}