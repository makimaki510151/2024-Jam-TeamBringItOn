using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[")]
    private Transform playerTransform = null;

    [SerializeField, Tooltip("�ŏ��̈ʒu")]
    private float startPoint = 0;

    [SerializeField, Tooltip("�Ō�̈ʒu")]
    private float goalPoint = 0;

    [SerializeField, Tooltip("�}�b�v�̒���")]
    private float mapRange = 0;

    private float height = 0;
    private Vector2 start;
    private Vector2 goal;

    Transform myTransform;

    void Start()
    {
        myTransform = transform;
        height = myTransform.position.y;
    }

    void Update()
    {
        start = new Vector2(startPoint + playerTransform.position.x, height);
        goal = new Vector2(goalPoint + playerTransform.position.x, height);
        myTransform.position = Vector2.Lerp(start, goal, playerTransform.position.x / mapRange);
    }
}
