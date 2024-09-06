using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("�f�t�H���g�̈ړ����x")]
    private float defaultSpeed = 10.0f;
    [SerializeField, Tooltip("�m�b�N�o�b�N��(��)")]
    private float knockbackLeft = 10.0f;
    [SerializeField, Tooltip("�m�b�N�o�b�N��(��)")]
    private float knockbackUp = 5.0f;
    [SerializeField, Tooltip("���G����")]
    private float invincibleTime = 0.5f;
    private float invincibleTimer = 0;
    [SerializeField, Tooltip("�ő�ړ����x")]
    private float maxSpeed = 20f;

    // �擾���Ă���o�t�A�C�e���̐�
    [SerializeField]
    private float speedBuffItemCount = 0;
    [SerializeField, Tooltip("�ʏ�̃W�����v��")]
    private float normalJumpPower = 8.0f;
    [SerializeField, Tooltip("�p���B�̃W�����v��")]
    private float parryJumpPower = 5.0f;

    // �v���C���[�̈ʒu
    public enum PlayCharacter
    {
        Water,
        Fire
    }
    [SerializeField]
    private PlayCharacter character = PlayCharacter.Water;

    [SerializeField, Tooltip("�p���B����")]
    private float parryTime = 0.25f;
    private float parryTimer = 0;

    [SerializeField, Tooltip("���G���̐F")]
    private Color transparentColor = new(1, 1, 1, 0.25f);

    [SerializeField, Tooltip("�ڒn���Ă���Ƃ݂Ȃ�����")]
    private float isGroundTimer = 0.5f;
    private float isGroundTime = 0;

    [SerializeField, Tooltip("�A�j���[�^�[")]
    private Animator myAnimator = null;

    [SerializeField]
    private float skateboardBuffPower = 1.3f;
    private float skateboardBuffContainer = 1;
    [SerializeField]
    private float skateboardTime = 1.0f;
    private float skateboardTimer = 0;

    [Header("���֌W")]
    [SerializeField]
    private float seWaterParryVol = 1.0f;
    [SerializeField]
    private AudioClip seWaterParryClip = null;

    [SerializeField]
    private float seFireParryVol = 1.0f;
    [SerializeField]
    private AudioClip seFireParryClip = null;

    [SerializeField]
    private float seJumpVol = 1.0f;
    [SerializeField]
    private AudioClip seJumpClip = null;


    static readonly int isParryId = Animator.StringToHash("isParry");
    static readonly int isDamageId = Animator.StringToHash("isDamage");

    private bool isJump = false;
    private bool isParry = false;
    private bool isParryHit = false;        // �p���B������������t���O���I��
    private bool isParryCancel = false;     // �p���B�ł��Ȃ��Ȃ�t���O���I��
    private bool isGround = false;
    private bool isTransparent = false;     // �_�ŗp

    private Vector2 tempVector2 = new(0, 0);
    private Vector2 vector2zero = Vector2.zero;
    private Color colorWhite = Color.white;
    private float deltaTime;
    private Enemy.EnemyAiType aiType;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;
    private SpriteRenderer mySpriteRenderer;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // �ڒn���Ă���Ȃ�t���O���I��
            if (isGround)
            {
                isJump = true;
            }
            else
            {
                // �ڒn���Ă��炸�A�p���B���\�Ńp���B���łȂ���΁A�p���B���s��
                if (!isParry && !isParryCancel)
                {
                    Debug.Log("����");
                    isParry = true;
                    parryTimer = parryTime;
                    myAnimator.SetTrigger(isParryId);
                }
            }
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MainGameRoot.Instance.StockEnemyShot(character);
        }
    }

    void Start()
    {
        // �R���|�[�l���g�擾
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        isGroundTime = isGroundTimer;
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        // �ړ�����
        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100)))* skateboardBuffContainer * Time.deltaTime;
        myRigidbody2D.velocity += tempVector2;

        // �W�����v�t���O���I���Ȃ�A�W�����v����
        if (isJump)
        {
            AudioControl.Instance.SetSEVol(seJumpVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seJumpClip, myTransform);
            myRigidbody2D.AddForce(myTransform.up * normalJumpPower, ForceMode2D.Impulse);
            isJump = false;
        }

        tempVector2 = myRigidbody2D.velocity;

        // �ړ����x���ő�l�𒴂��Ȃ��悤�ɂ���
        if (tempVector2.x > maxSpeed* skateboardBuffContainer)
        {
            tempVector2.x = maxSpeed;
            myRigidbody2D.velocity = tempVector2;
        }

        // �ڒn���Ă��Ă��㏸���������Ă��Ȃ���΁A�ڒn���Ă��邩���肷��
        if(!isGround && tempVector2.y < 0.1f && tempVector2.y > -0.1f)
        {
            // �w�肵�����Ԃ��o������A�ڒn���Ă���Ƃ���
            isGroundTime -= deltaTime;
            if(isGroundTime <= 0)
            {
                isGround = true;
            }
        }
        else
        {
            isGroundTime = isGroundTimer;
        }

        // �p���B��ԂȂ�A���Ԃ��v������
        if (isParry)
        {
            parryTimer -= deltaTime;

            // �w�肵�����Ԃ��o������A�p���B��Ԃ���������
            if (parryTimer < 0)
            {
                isParry = false;

                // �ڒn���Ă��Ȃ��p���B���������Ă��Ȃ��Ȃ�A�p���B�ł��Ȃ��悤�ɂ���
                if (!isGround && !isParryHit)
                {
                    isParryCancel = true;
                }
            }
        }

        // ���G���Ȃ玞�Ԃ��v������
        if (invincibleTimer > 0)
        {
            invincibleTimer -= deltaTime;

            // �_�ŏ���
            isTransparent = !isTransparent;
            if (isTransparent)
            {
                mySpriteRenderer.color = transparentColor;
            }
            else
            {
                mySpriteRenderer.color = colorWhite;
            }

            // �w�肵�����Ԃ��o������A�J���[�����ɖ߂�
            if (invincibleTimer <= 0)
            {
                mySpriteRenderer.color = colorWhite;
            }
        }

        if(skateboardTimer > 0)
        {
            skateboardTimer -= deltaTime;
            if(skateboardTimer <= 0)
            {
                skateboardTimer = 0;
                skateboardBuffContainer = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �n�ʂɃq�b�g
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
            isParryCancel = false;
            isParryHit = false;
            myAnimator.SetBool(isDamageId, false);
        }
        // �G�Ƀq�b�g
        else if (collision.CompareTag("Enemy"))
        {
            // �p���B���Ȃ�A�p���B�������s��
            if (isParry)
            {
                switch (character)
                {
                    case PlayCharacter.Water:
                        AudioControl.Instance.SetSEVol(seWaterParryVol*MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seWaterParryClip,myTransform);
                        break;
                    case PlayCharacter.Fire:
                        AudioControl.Instance.SetSEVol(seFireParryVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seFireParryClip,myTransform);
                        break;
                }

                isParryHit = true;
                collision.GetComponent<Enemy>().StockMove(character);
                if (skateboardTimer <= 0)
                {
                    tempVector2 = myRigidbody2D.velocity;
                    tempVector2.y = 0;
                    myRigidbody2D.velocity = tempVector2;
                    myRigidbody2D.AddForce(myTransform.up * parryJumpPower, ForceMode2D.Impulse);
                }

                // �G�t�F�N�g����
                
            }
            // ���G���ԂłȂ��Ȃ�A�m�b�N�o�b�N�������s��
            else if (invincibleTimer <= 0)
            {
                myRigidbody2D.velocity = vector2zero;
                myRigidbody2D.AddForce(-myTransform.right * knockbackLeft, ForceMode2D.Impulse);
                myRigidbody2D.AddForce(myTransform.up * knockbackUp, ForceMode2D.Impulse);
                invincibleTimer = invincibleTime;
                myAnimator.SetBool(isDamageId, true);

                aiType = collision.GetComponent<Enemy>().AiType;
                // �^�R�ɓ���������A�^�R�X�~�𔭎˂�����
                if (aiType == Enemy.EnemyAiType.Octopus)
                {
                    collision.GetComponent<Enemy>().HitOctopus(character);
                }
                // �e�ɓ���������A���̒e������
                else if(aiType == Enemy.EnemyAiType.Bullet)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        // �S�[���Ƀq�b�g
        else if (collision.CompareTag("Goal"))
        {
            MainGameRoot.Instance.GoalPlayer(character);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    public void BuffUp(int buffPower)
    {
        speedBuffItemCount += buffPower;
    }
    public void SkateboardTime()
    {
        skateboardTimer = skateboardTime;
        skateboardBuffContainer = skateboardBuffPower;
        isParry = true;
        parryTimer = skateboardTime;
    }
    public void Reverse()
    {

    }
}
