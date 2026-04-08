using UnityEngine;
using UnityEngine.InputSystem;

// Secara otomatis menambahkan Rigidbody2D ke objek jika belum ada
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f; // Menggunakan gaya dorong untuk lompat 2D

    [Header("Ground Check")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private void Awake()
    {
        // Inisialisasi input dan komponen
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        
        // Mendaftar event lompat
        controls.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        // 1. Membaca Input (Dilakukan di Update agar responsif)
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        // 2. Ground Check versi 2D menggunakan OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundMask);
    }

    // FixedUpdate digunakan untuk semua perhitungan yang melibatkan Rigidbody (Fisika)
    private void FixedUpdate()
    {
        // 3. Pergerakan Kanan/Kiri
        // Kita hanya mengubah kecepatan sumbu X, kecepatan sumbu Y dibiarkan alami (untuk gravitasi/lompat)
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // 4. Menerapkan gaya lompat dengan mengubah kecepatan vertikal
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}