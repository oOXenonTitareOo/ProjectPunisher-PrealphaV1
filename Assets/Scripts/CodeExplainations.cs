/*using UnityEngine; // Using the General Unity Engine library.
using UnityEngine.InputSystem; // Using Unity Engine's input library to handle keys, inputs. Mouse, keypresses and gamepads.

public class PlayerMovement : MonoBehaviour // Class and Class Name. Class name should generally be the file's name. In coding classes can be multiple. But by default creating a MonoBehaviour script in Unity creates a default class.
{
    public float moveSpeed = 5f; // Public means visible from outside. Can be seen and used in other script files or it is visible and tweakable in Unity Editor. Private on the other hand means that it can be only seen and used in this file only. Float means fraction number, like 0.4 instead of round number: 4. Because we used a float variable, we need to always use "f" at the end of the quantity. Movespeed is the variable's name. Can be litearlly anything in any language. Can be emojis or unicode characters too.
    public float sprintSpeed = 30f;
    public float jumpForce = 5f;
    public float groundDistance = 0.4f;
    private float currentSpeed;
    public Transform groundCheck; // Every GameObject has a Transform component. A GameObject is an object inside the game. Can be 3D or 2D. The Transform of the GameObject is the collection of parameters of size, position, rotation. Without a Transform the object cannot physically exist in the 3D space. It is it's "body".
    public LayerMask groundMask; // LayerMasks in Unity are layers. You can create layers. "Ground" "Air" "Space" "Bushes" and so on. With layers you can tweak what collides or interacts with what. This way you can make dropped items not fall through ground.
    public LayerMask droppedItemMask;
    private Rigidbody rb; // A Rigidbody is Rigid because it's solid, and it's a body because it is not 2D but 3D. Has a physical solid body that you can tweak. This is a "skeleton". Without it the objects are just models, textures. Everything can go through them. Also a rigidbody has gravity and weight. A collider does not.
    private Vector2 moveInput; // A Vector2 is a Vector with 2 dimensions. A Vector3 is the same but 3 dimensions. 2 dimensions = x, y. 3 dimensions = x, y, z. A Vector2 in this example is a collection of 2 numbers. X,Y. This marks our way of moving the mouse on a flat table.
    private bool isGrounded; // A Bool is a yes-no switch. Can be 0 or 1 in binary. In coding, it's true or false.
    private bool isSprinting = false;
    void Start() // A Void is a function. Can be private or public
    {
        rb = GetComponent<Rigidbody>(); // This line tells Unity to search for The Rigidbody. Go find it and use it. 
    }

    void Update()
    {
        CheckGround(); // Here we can see a function call. Already made functions can be called in one another. Here in the Update we called CheckGround. Unity has a custom, pre-made invisible function called Update. Update is a virtual clock called tick. The engine checks everything many times per second. Here in this case we called the Check Ground to check if the player jumped or not. Checking is done every tick. So every second we check a lot of times if player is jumped or not. With update, the tickrate is dependent on fps. So if you have 60 = 60 times a second. With 144? 144 times. FixedUpdate does not depend on fps tho.
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnJump()
    {
        if (isGrounded) // An if statement. If x happens, y happens. A railway's switch sort of. Here the isGrounded means if the player did not jump yet but pressed jump, add a force to make it jumüp.
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse); // ForceMode.Impluse is a way of telling Unity to use the mentioned and declared force that we did infront of this to do it instantly. We said: Apply all force in a blink instead of waiting or adding it over time. The opposite of Impulse is .Force
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | droppedItemMask); // With Physics.CheckSphere we make an invisible, virtual Sphere. We defy it's center, and radius aka how bit it is. If the ground is in the bubble / sphere we are on the ground.
    }

    void OnMovement(InputValue value)  // Here InputValue value is just telling Unity that Movement should be values.
    {
        moveInput = value.Get<Vector2>(); // We said first: "we are going to use values for movement" than to get the actual value, we said: Make a 2 dimensional vector and get it's values. Make those values the "value".
    }

    void OnSprint()
    {
        isSprinting = true;
    }

    void OnSprintRelease()
    {
        isSprinting = false;
    }

    void MovePlayer()
    {
        if(isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else if(!isSprinting)
        {
            currentSpeed = moveSpeed;
        }

        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y; // We use * to multiply. We can do other things, but with multiplying the numbers we can use smaller numbers.
        direction.Normalize(); // We are shrinking the direction vector's value down to 1. Regardless of it's actual or original value. Without this, the direction would be exponentional, so bigger and bigger with more deviation from start.
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);
    }
}*/