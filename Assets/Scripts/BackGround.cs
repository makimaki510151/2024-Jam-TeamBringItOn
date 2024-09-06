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

    [Header("�������[�v")]

    [SerializeField, Tooltip("�������[�v��")]
    private bool isLoop = false;

    [SerializeField]
    private Transform[] Sprites = null;

    [SerializeField, Tooltip("�J����")]
    private Transform playerCamera = null;

    [SerializeField, Tooltip("�w�i�̕�")]
    private float backGroundWidth = 23.13f;

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
        if (!isLoop)
        {
            start = new Vector2(startPoint + playerTransform.position.x, height);
            goal = new Vector2(goalPoint + playerTransform.position.x, height);
            myTransform.position = Vector2.Lerp(start, goal, playerTransform.position.x / mapRange);
        }
        else
        {
            var cameraGridX = Mathf.FloorToInt(playerCamera.position.x / backGroundWidth);

            for (int index = 0; index < Sprites.Length; index++)
            {
                var position = Sprites[index].position;
                position.x = (index - 1 + cameraGridX) * backGroundWidth;
                Sprites[index].position = position;
            }
        }
    }
}
