using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using FlameUtils;

public class GameManager : MonoBehaviour
{
    #region Controls
    [Header("Controls")]
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;
    public KeyCode Drop = KeyCode.DownArrow;
    #endregion

    #region Attributes
    [Header("Attributes")]
    public float speed = 5f;
    public float spawnSpeed;
    #endregion

    #region References
    [Header("References")]
    public GameObject selectedPiece;
    public GameObject selectedPieceClone;
    public GameObject nextPiece;
    public GameObject[] pieces;
    private Vector2 oldPiecePos;
    public GameObject guide;
    public List<GameObject> piecesList;
    public GameObject effects;
    #endregion

    private bool wasPaused;

    private bool canDrop = true;

    #region GameOver
    [Header("GameOver")]
    public GameObject gameOverScreen;
    public Dictionary<GameObject, GameObject> piecesClone;
    #endregion

    
    [Header ("Ads")]
    private bool adShown = false;

    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("GameManager");
                    _instance = managerObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    #region Initialization
    private void Start()
    {
        piecesClone = new Dictionary<GameObject, GameObject>();
        piecesList = new List<GameObject>();
        SpawnPiece();
    }

    private void OnEnable()
    {
        EventManager.OnPieceUpgraded += UpgradePiece;
        EventManager.OnGameOver += GameOver;
        EventManager.OnCleanBoard += ClearBoardWithDelay;
        EventManager.OnContinueGame += ContinueGame;
    }

    private void OnDisable()
    {
        EventManager.OnPieceUpgraded -= UpgradePiece;
        EventManager.OnGameOver -= GameOver;
        EventManager.OnCleanBoard -= ClearBoardWithDelay;
        EventManager.OnContinueGame -= ContinueGame;
    }
    #endregion

    #region Update
    private void Update()
    {
        HandleMouseInput();
        UpdateGuidePosition();
    }

    private void HandleMouseInput()
    {
        if (selectedPieceClone != null)
        {
            if (Input.GetMouseButton(0))
            {
                HandlePieceMovement();
            }

            if (Input.GetMouseButtonUp(0))
            {
                HandlePieceDrop();
            }
        }
    }

    private void HandlePieceMovement()
    {
        if (canDrop)
        {


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mousePos.x, selectedPieceClone.transform.position.y, 0f);

            selectedPieceClone.transform.position =
                Vector3.Lerp(selectedPieceClone.transform.position, targetPos, Time.deltaTime * speed);
        }
    }

    private void HandlePieceDrop()
    {
        if (canDrop)
        {
            if (wasPaused)
            {
                wasPaused = false;
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                DropPiece();
            }
        }
    }

    private void UpdateGuidePosition()
    {
        if (canDrop)
        {
            if (selectedPieceClone != null && !EventSystem.current.IsPointerOverGameObject())
            {
                if (guide != null)
                {
                    guide.transform.position = new Vector2(selectedPieceClone.transform.position.x, guide.transform.position.y);
                }
            }
        }
    }
    #endregion

    #region Piece Management
    private void SpawnPiece()
    {
        if (guide != null)
        {
            selectedPiece = PieceGenerator();
            nextPiece = PieceGenerator();
            selectedPieceClone = Instantiate(selectedPiece, transform.position, Quaternion.identity);

            selectedPieceClone.GetComponent<Rigidbody2D>().isKinematic = true;
            selectedPieceClone.GetComponent<Collider2D>().enabled = false;

            nextPiece = PieceGenerator();
        }

        if (selectedPieceClone == null)
        {
            selectedPiece = PieceGenerator();
            nextPiece = PieceGenerator();
            selectedPieceClone = Instantiate(selectedPiece, transform.position, Quaternion.identity);

            selectedPieceClone.GetComponent<Rigidbody2D>().isKinematic = true;
            selectedPieceClone.GetComponent<Collider2D>().enabled = false;

            nextPiece = PieceGenerator();
        }
    }

    private GameObject PieceGenerator()
    {
        int pieceInt = Random.Range(0, pieces.Length);
        return pieces[pieceInt];
    }

    public void MovePiece(GameObject piece, Vector3 direction)
    {
        piece.transform.position += direction * speed * Time.deltaTime;
    }

    private void DropPiece()
    {
        
        selectedPieceClone.GetComponent<Rigidbody2D>().isKinematic = false;
        selectedPieceClone.GetComponent<Collider2D>().enabled = true;
        oldPiecePos = selectedPieceClone.transform.position;
        selectedPieceClone.GetComponent<GamePieceBrain>().canMerge = true;
        selectedPieceClone.GetComponent<GamePieceBrain>().isDropped = true;

        selectedPiece = null;
        selectedPieceClone = null;

        nextPiece = PieceGenerator();

        EventManager.TriggerPieceDropped(selectedPieceClone);

        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnSpeed);

        selectedPieceClone = Instantiate(nextPiece, oldPiecePos, Quaternion.identity);
        selectedPieceClone.GetComponent<GamePieceBrain>().canMerge = true;

        selectedPieceClone.GetComponent<Rigidbody2D>().isKinematic = true;
        selectedPieceClone.GetComponent<Collider2D>().enabled = false;
    }
    #endregion

    #region Piece Upgrading
    private void UpgradePiece(GameObject piece1, GameObject piece2)
    {
        if (piecesClone.ContainsKey(piece1) || piecesClone.ContainsKey(piece2))
        {
            return;
        }

        if (piecesClone.ContainsValue(piece1) || piecesClone.ContainsValue(piece2))
        {
            return;
        }
        else
        {
            piecesClone.Add(piece1, piece2);
            Vector3 upgradePosition = piece1.transform.position;
            if (piece1.GetComponent<GamePieceBrain>().upgradePiece != null && upgradePosition != null)
            {
                GameObject nextObject = Instantiate(piece1.GetComponent<GamePieceBrain>().upgradePiece.piece,
                    CalculateSpawnPos(piece1.transform.position, piece2.transform.position), Quaternion.identity);
                nextObject.GetComponent<GamePieceBrain>().isDropped = true;
                nextObject.GetComponent<GamePieceBrain>().canMerge = false;

                GameObject _fx = Instantiate(effects, nextObject.gameObject.transform.position, Quaternion.identity);
                FlameUtils.Utilities.DestroyAfterTime(_fx, 3f);
            }

            ScoreManager.Instance.AddScore(piece1.GetComponent<GamePieceBrain>().upgradePiece.score);
            Destroy(piece1);
            Destroy(piece2);
        }
    }
    #endregion

    #region Board Management
    private void ClearBoardWithDelay()
    {
        GameObject[] piecesArray = GameObject.FindGameObjectsWithTag("Piece");

        foreach (GameObject piece in piecesArray)
        {
            piecesList.Add(piece);
        }

        ShuffleList(piecesList);

        StartCoroutine(DestroyPiecesWithDelay(piecesList));
    }

    private IEnumerator DestroyPiecesWithDelay(List<GameObject> shuffledPieces)
    {
        canDrop = false;
        foreach (GameObject piece in shuffledPieces)
        {
            if (piece != null && piece.GetComponent<GamePieceBrain>().isDropped)
            {
                if (piece.GetComponent<GamePieceBrain>().piece != null)
                {
                    // Add score
                    ScoreManager.Instance.AddScore(piece.GetComponent<GamePieceBrain>().piece.score);

                    // Instantiate effects
                    GameObject _fx = Instantiate(effects, piece.transform.position, Quaternion.identity);
                    FlameUtils.Utilities.DestroyAfterTime(_fx, 3f);

                    // Delay before destroying the piece
                    yield return new WaitForSeconds(0.1f); // Adjust the delay time as needed

                    // Destroy the piece
                    ScoreManager.Instance.AddScore(piece.GetComponent<GamePieceBrain>().piece.score);
                    EventManager.TriggerPieceRemoved();
                    Destroy(piece);
                }
            }
        }

        // Clear the list after processing all pieces
        piecesList.Clear();
        canDrop = true;

        // Spawn new piece
        if (selectedPieceClone == null)
        {
            SpawnPiece();
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    #endregion

    #region Game Over
    private void GameOver()
    {
        ScoreManager.Instance.GameOver();
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        if (adShown)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (!adShown)
        {
            if (Interstitial.Instance.IsAdLoaded())
            {
                adShown = true;
                Interstitial.Instance.ShowAd();
            }
            else
            {
                adShown = true;
                Interstitial.Instance.LoadAd();
                Interstitial.Instance.ShowAd();
            }   
            
        }
    }
    #endregion

    private void ContinueGame()
    {
        ResumeGame();
        EventManager.TriggerCleanBoard();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        wasPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ReturnToMenu()
    {
                Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    private Vector3 CalculateSpawnPos(Vector3 piece1, Vector3 piece2)
    {
        return new Vector3((piece1.x + piece2.x) / 2, (piece1.y + piece2.y) / 2, 0);
    }
}
