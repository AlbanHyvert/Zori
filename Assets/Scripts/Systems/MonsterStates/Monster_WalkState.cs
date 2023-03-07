using UnityEngine;

public class Monster_WalkState : ControllerState
{
    private MonsterController _controller = null;
    private Vector3 _nextPosition = Vector3.zero;

    private float _speed = 0;
    private PlayerController _player = null;

    public override void Enter()
    {
        _player = Player.Instance.GetComponent<PlayerController>();

        _controller = Controller.mController;

        _controller.MeshRenderer.material.color = Color.blue;

        _nextPosition = GenerateRandomPosition();

        GameManager.Instance.OnUpdateMonster += Tick;
    }

    public override void Tick(float time)
    {
        if(!_controller) return;

        //Check for Behaviour

        CheckForPlayer();

        if(Vector3.Distance(_controller.transform.position, _nextPosition) < 0.1f)
        {
            _nextPosition = GenerateRandomPosition();
        }

        // Gradually increase the movement speed up to the maximum speed
        if (_speed < _controller.Settings.MaxWalkVelocity)
        {
            _speed += _controller.Settings.WalkSpeed * (_controller.Settings.Acceleration * time);
        }
        
        SetRotation();

        _controller.transform.position = Vector3.MoveTowards(_controller.transform.position, _nextPosition, _speed * time);
    }

    private void SetRotation()
    {
        // Get the direction from the current object to the target object
        Vector3 direction = _nextPosition - _controller.transform.position;

        // Set the y component of the direction vector to 0
        direction.y = 0;

        // Create a Quaternion that looks in the modified direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Apply the rotation to the object's transform, but only on the y-axis
        _controller.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    private void CheckForPlayer()
    {
        // Calculate the start and end points of the cone vision
        Vector3 direction = _player.transform.position - _controller.transform.position;
        float angle = Vector3.Angle(direction, _controller.transform.forward);

        // Check if the player is within the cone vision using raycasting
        if(angle < _controller.Settings.FieldOfView)
        {
            RaycastHit hit;
            if (Physics.Raycast(_controller.transform.position, direction.normalized, out hit, _controller.Settings.MaxViewDistance))
            {
                PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();

                if (player == null) return;

                if (player.IsChased) return;

                player.SetIsChased(true);

                // The player is within the cone vision
                _controller.SetTarget(player.transform);

                _controller.SetState(new Monster_ChaseState());
            }
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        if(_controller.mController == null) return Vector3.zero;

        Bounds area = _controller.mController.Area;
        MonsterController monster = _controller.mController;

        Vector3 randomPos = area.center + Random.insideUnitSphere * area.extents.magnitude;
        randomPos.y = monster.transform.position.y;

        return randomPos;
    }

    public override void Exit() 
    {
        GameManager.Instance.OnUpdateMonster -= Tick;
    }
}